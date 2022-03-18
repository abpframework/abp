using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Validation.StringValues;

namespace Volo.Abp.FeatureManagement.JsonConverters;

public class NewtonsoftStringValueTypeJsonConverter : JsonConverter, ITransientDependency
{
    public override bool CanWrite => false;

    protected readonly AbpFeatureManagementApplicationContractsOptions Options;

    public NewtonsoftStringValueTypeJsonConverter(IOptions<AbpFeatureManagementApplicationContractsOptions> options)
    {
        Options = options.Value;
    }
    
    public override bool CanConvert(Type objectType)
    {
        return objectType == typeof(IStringValueType);
    }

    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
    {
        throw new NotImplementedException("This method should not be called to write (since CanWrite is false).");
    }

    public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
    {
        if (reader.TokenType != JsonToken.StartObject)
        {
            return null;
        }

        var jsonObject = JObject.Load(reader);

        var stringValue = CreateStringValueTypeByName(jsonObject, jsonObject["name"].ToString());
        foreach (var o in serializer.Deserialize<Dictionary<string, object>>(
            new JsonTextReader(new StringReader(jsonObject["properties"].ToString()))))
        {
            stringValue[o.Key] = o.Value;
        }

        stringValue.Validator = CreateValueValidatorByName(jsonObject["validator"], jsonObject["validator"]["name"].ToString());
        foreach (var o in serializer.Deserialize<Dictionary<string, object>>(
            new JsonTextReader(new StringReader(jsonObject["validator"]["properties"].ToString()))))
        {
            stringValue.Validator[o.Key] = o.Value;
        }

        return stringValue;
    }

    protected virtual IStringValueType CreateStringValueTypeByName(JObject jObject, string name)
    {
        if (name == "SelectionStringValueType")
        {
            var selectionStringValueType = new SelectionStringValueType();
            if (jObject["itemSource"].HasValues)
            {
                selectionStringValueType.ItemSource = new StaticSelectionStringValueItemSource(jObject["itemSource"]["items"]
                    .Select(item => new LocalizableSelectionStringValueItem()
                    {
                        Value = item["value"].ToString(),
                        DisplayText = new LocalizableStringInfo(item["displayText"]["resourceName"].ToString(), item["displayText"]["name"].ToString())
                    }).ToArray());
            }

            return selectionStringValueType;
        }

        return name switch
        {
            "FreeTextStringValueType" => new FreeTextStringValueType(),
            "ToggleStringValueType" => new ToggleStringValueType(),
            _ => throw new ArgumentException($"{nameof(IStringValueType)} named {name} was not found!")
        };
    }

    protected virtual IValueValidator CreateValueValidatorByName(JToken jObject, string name)
    {
        foreach (var factory in Options.ValueValidatorFactory.Where(factory => factory.CanCreate(name)))
        {
            return factory.Create();
        }

        throw new ArgumentException($"{nameof(IValueValidator)} named {name} was cannot be created!");
    }
}
