using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.Cli.Configuration;

public class ConfigReader : IConfigReader, ITransientDependency
{
    const string appSettingFileName = "appsettings.json";

    public AbpCliConfig Read(string directory)
    {
        var settingsFilePath = Path.Combine(directory, appSettingFileName);

        if (!File.Exists(settingsFilePath))
        {
            throw new FileNotFoundException($"appsettings file could not be found. Path:{settingsFilePath}");
        }

        var settingsFileContent = File.ReadAllText(settingsFilePath);

        var documentOptions = new JsonDocumentOptions
        {
            CommentHandling = JsonCommentHandling.Skip
        };

        using (var document = JsonDocument.Parse(settingsFileContent, documentOptions))
        {
            if (document.RootElement.TryGetProperty("AbpCli", out var element))
            {
                var configJson = element.GetRawText();
                var options = new JsonSerializerOptions
                {
                    Converters =
                        {
                            new JsonStringEnumConverter()
                        },
                    ReadCommentHandling = JsonCommentHandling.Skip
                };

                return JsonSerializer.Deserialize<AbpCliConfig>(configJson, options);
            }
            else
            {
                return new AbpCliConfig();
            }
        }
    }
}
