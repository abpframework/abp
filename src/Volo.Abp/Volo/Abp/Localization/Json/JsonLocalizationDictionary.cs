using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Volo.Abp.Localization.Json
{
    /// <summary>
    ///     This class is used to build a localization dictionary from json.
    /// </summary>
    /// <remarks>
    ///     Use static Build methods to create instance of this class.
    /// </remarks>
    public class JsonLocalizationDictionary : LocalizationDictionary
    {
        /// <summary>
        ///     Private constructor.
        /// </summary>
        /// <param name="cultureName">Culture of the dictionary</param>
        private JsonLocalizationDictionary(string cultureName)
            : base(cultureName)
        {
        }

        /// <summary>
        ///     Builds an <see cref="JsonLocalizationDictionary" /> from given file.
        /// </summary>
        /// <param name="filePath">Path of the file</param>
        public static JsonLocalizationDictionary BuildFromFile(string filePath)
        {
            try
            {
                return BuildFromJsonString(File.ReadAllText(filePath));
            }
            catch (Exception ex)
            {
                throw new AbpException("Invalid localization file format! " + filePath, ex);
            }
        }

        /// <summary>
        ///     Builds an <see cref="JsonLocalizationDictionary" /> from given json string.
        /// </summary>
        /// <param name="jsonString">Json string</param>
        public static JsonLocalizationDictionary BuildFromJsonString(string jsonString)
        {
            JsonLocalizationFile jsonFile;
            try
            {
                jsonFile = JsonConvert.DeserializeObject<JsonLocalizationFile>(
                    jsonString,
                    new JsonSerializerSettings
                    {
                        ContractResolver = new CamelCasePropertyNamesContractResolver()
                    });
            }
            catch (JsonException ex)
            {
                throw new AbpException("Can not parse json string. " + ex.Message);
            }

            var cultureCode = jsonFile.Culture;
            if (string.IsNullOrEmpty(cultureCode))
            {
                throw new AbpException("Culture is empty in language json file.");
            }

            var dictionary = new JsonLocalizationDictionary(cultureCode);
            var dublicateNames = new List<string>();
            foreach (var item in jsonFile.Texts)
            {
                if (string.IsNullOrEmpty(item.Key))
                {
                    throw new AbpException("The key is empty in given json string.");
                }

                if (dictionary.Contains(item.Key))
                {
                    dublicateNames.Add(item.Key);
                }

                dictionary[item.Key] = new LocalString(item.Key, item.Value.NormalizeLineEndings());
            }

            if (dublicateNames.Count > 0)
            {
                throw new AbpException(
                    "A dictionary can not contain same key twice. There are some duplicated names: " +
                    dublicateNames.JoinAsString(", "));
            }

            return dictionary;
        }
    }
}