using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;

namespace Volo.Abp.AgentTools;

/// <summary>
/// The redaction rule for tool arguments in logs and call records: values of password-like names (nested too) become
/// <see cref="RedactedValue"/>, also inside a string that holds a JSON object or array. One rule for every caller (the invoker,
/// hosts that record their own tool calls).
/// </summary>
public static class AgentToolArgumentRedactor
{
    public const string RedactedValue = "***";

    // Strings that look like JSON are parsed within these bounds and become RedactedValue beyond them.
    private const int MaxJsonStringLength = 64 * 1024;
    private const int MaxJsonStringNesting = 4; // strings in the JSON of strings
    private const int MaxJsonStringDepth = 64; // the default of System.Text.Json
    private const int MaxFailedJsonStrings = 1000; // per call; each one costs an exception

    // Matched against argument names (nested too) lower-cased, without '_' and '-'.
    private static readonly string[] SensitiveNameParts =
    [
        "authorization", "cookie", "bearer", "session", "token", "secret", "password", "passwd", "passphrase", "pwd", "apikey",
        "accesskey", "privatekey", "credential", "connectionstring", "clientsecret"
        // Not "auth": it would match "author".
    ];

    public static bool IsSensitiveName(string name) => IsSensitiveName(name, null);

    /// <summary>
    /// The built-in rule plus <paramref name="additionalNameParts"/> (e.g. <see cref="AbpAgentToolOptions.AdditionalSensitiveArgumentNames"/>),
    /// matched the same way: both sides lower-cased, without '_' and '-', as substrings.
    /// </summary>
    public static bool IsSensitiveName(string name, IEnumerable<string>? additionalNameParts)
    {
        var normalized = Normalize(name);
        return SensitiveNameParts.Any(normalized.Contains)
               || additionalNameParts?.Any(p => Normalize(p) is { Length: > 0 } part && normalized.Contains(part)) == true;
    }

    private static string Normalize(string name) => name.Replace("_", "").Replace("-", "").ToLowerInvariant();

    /// <summary>
    /// Arguments as compact JSON with sensitive values replaced; duplicate names are all kept. <paramref name="isSensitiveName"/>
    /// defaults to <see cref="IsSensitiveName(string)"/>. A string that holds a JSON object or array (comments and trailing commas
    /// allowed) is redacted by the same rule, strings inside it too, and stays as it is when nothing in it is sensitive. One that
    /// starts and ends like JSON but is too long, nested too deep or not valid text, or comes after too many that are no JSON, becomes
    /// <see cref="RedactedValue"/>. Serialization exceptions propagate; the caller decides what to record instead.
    /// </summary>
    public static string ToRedactedJson(
        IDictionary<string, object?> arguments,
        JsonSerializerOptions jsonOptions,
        Func<string, bool>? isSensitiveName = null)
    {
        var root = JsonSerializer.SerializeToElement(arguments, jsonOptions);
        using var stream = new MemoryStream();
        using (var writer = new Utf8JsonWriter(stream))
        {
            new Redaction(isSensitiveName ?? IsSensitiveName).Write(writer, root);
        }

        return Encoding.UTF8.GetString(stream.ToArray());
    }

    private sealed class Redaction(Func<string, bool> isSensitiveName)
    {
        private static readonly JsonReaderOptions StringScanOptions = new()
        {
            CommentHandling = JsonCommentHandling.Skip,
            AllowTrailingCommas = true,
            MaxDepth = MaxJsonStringLength // any
        };

        private static readonly JsonDocumentOptions StringReadOptions = new()
        {
            CommentHandling = JsonCommentHandling.Skip,
            AllowTrailingCommas = true,
            MaxDepth = MaxJsonStringDepth
        };

        // The outermost writer escapes the text again.
        private static readonly JsonWriterOptions StringWriteOptions = new() { Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping };

        private int _stringNesting;
        private int _failedStrings;

        /// <summary>True when something was redacted. Not through JsonNode: it accepts no duplicate names.</summary>
        public bool Write(Utf8JsonWriter writer, JsonElement element)
        {
            var redacted = false;
            switch (element.ValueKind)
            {
                case JsonValueKind.Object:
                    writer.WriteStartObject();
                    foreach (var property in element.EnumerateObject())
                    {
                        writer.WritePropertyName(property.Name);
                        if (isSensitiveName(property.Name))
                        {
                            writer.WriteStringValue(RedactedValue);
                            redacted = true;
                        }
                        else
                        {
                            redacted |= Write(writer, property.Value);
                        }
                    }

                    writer.WriteEndObject();
                    break;
                case JsonValueKind.Array:
                    writer.WriteStartArray();
                    foreach (var item in element.EnumerateArray())
                    {
                        redacted |= Write(writer, item);
                    }

                    writer.WriteEndArray();
                    break;
                case JsonValueKind.String when RedactString(element.GetString()!) is { } text:
                    writer.WriteStringValue(text);
                    redacted = true;
                    break;
                default:
                    if (_stringNesting == 0)
                    {
                        element.WriteTo(writer);
                    }
                    else
                    {
                        // As the string has it, escapes included.
                        writer.WriteRawValue(element.GetRawText(), skipInputValidation: true);
                    }

                    break;
            }

            return redacted;
        }

        /// <summary>Null when the text stays as it is.</summary>
        private string? RedactString(string text)
        {
            var json = text.AsMemory().Trim();
            if (json.Length < 2 || json.Span[0] is not ('{' or '[') || json.Span[^1] is not ('}' or ']'))
            {
                return null;
            }

            if (json.Length > MaxJsonStringLength || _stringNesting == MaxJsonStringNesting || _failedStrings == MaxFailedJsonStrings)
            {
                return RedactedValue;
            }

            _stringNesting++;
            try
            {
                var utf8 = new byte[Encoding.UTF8.GetByteCount(json.Span)];
                Encoding.UTF8.GetBytes(json.Span, utf8);
                if (IsTooDeep(utf8))
                {
                    return RedactedValue;
                }

                using var document = JsonDocument.Parse(utf8, StringReadOptions);
                using var stream = new MemoryStream();
                bool redacted;
                using (var writer = new Utf8JsonWriter(stream, StringWriteOptions))
                {
                    redacted = Write(writer, document.RootElement);
                }

                return redacted ? Encoding.UTF8.GetString(stream.ToArray()) : null;
            }
            catch (JsonException)
            {
                _failedStrings++;
                return null; // not JSON
            }
            catch (InvalidOperationException)
            {
                _failedStrings++;
                return RedactedValue; // a name or string in it has a lone surrogate
            }
            finally
            {
                _stringNesting--;
            }
        }

        /// <summary>Throws a JsonException for invalid JSON of any depth. The reader takes linear time, however deep the JSON is.</summary>
        private static bool IsTooDeep(ReadOnlySpan<byte> utf8)
        {
            var isTooDeep = false;
            var reader = new Utf8JsonReader(utf8, StringScanOptions);
            while (reader.Read())
            {
                isTooDeep |= reader.CurrentDepth >= MaxJsonStringDepth && reader.TokenType is JsonTokenType.StartObject or JsonTokenType.StartArray;
            }

            return isTooDeep;
        }
    }
}
