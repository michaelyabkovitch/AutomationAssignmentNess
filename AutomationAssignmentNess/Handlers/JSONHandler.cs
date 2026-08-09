using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutomationAssignmentNess.Handlers
{
    public static class JSONHandler
    {

        public static Dictionary<string, string> ConvertJsonToDictionary(string jsonFilePath)
        {
            // Check if the provided file path actually exists in the system
            if (!File.Exists(jsonFilePath))
            {
              
                throw new FileNotFoundException($"JSON file was not found at path: {jsonFilePath}");
            }

            try
            {

                string jsonString = File.ReadAllText(jsonFilePath);

                var stringDictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(jsonString);

                return stringDictionary;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error parsing the JSON file. Please ensure the format is valid and contains only string-convertible values. Details: {ex.Message}");
            }
        }
        public static List<Dictionary<string, string>> ConvertJsonToListOfDictionaries(string jsonFilePath)
        {

            if (!File.Exists(jsonFilePath))
            {
                throw new FileNotFoundException($"JSON file was not found at path: {jsonFilePath}");
            }

            try
            {
                string jsonString = File.ReadAllText(jsonFilePath);
                var listOfDictionaries = JsonConvert.DeserializeObject<List<Dictionary<string, string>>>(jsonString);

                return listOfDictionaries;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error parsing the JSON array file. Details: {ex.Message}");
            }
        }
    }
}
