using System.Collections.Generic;
using System.IO;
using NUnit.Framework;
using AutomationAssignmentNess.Handlers;

namespace AutomationAssignmentNess.Tests.TestHelpers
{
    /// <summary>
    /// A dedicated static class responsible for providing external test data to NUnit test fixtures.
    /// </summary>
    public static class SearchDataProviders
    {
        /// <summary>
        /// Reads the SearchItems.json array and yields test cases for data-driven testing.
        /// </summary>
        public static IEnumerable<TestCaseData> ProvideSearchData()
        {
            // Retrieve the absolute path of the base execution directory securely, independent of NUnit's test context
            string baseDirectory = System.AppDomain.CurrentDomain.BaseDirectory;

            // Construct the explicit path to the JSON file
            string jsonPath = Path.Combine(baseDirectory, "Files", "SearchItems.json");

            // Convert the JSON array into a structured list of dictionaries
            var searchDataList = JSONHandler.ConvertJsonToListOfDictionaries(jsonPath);

            // Iterate through the list and yield each dictionary as an isolated NUnit test case
            foreach (var data in searchDataList)
            {
                string itemName = data.ContainsKey("ITEMNAME") ? data["ITEMNAME"] : "UnknownItem";
                yield return new TestCaseData(data).SetName($"SearchFlow_ForItem_{itemName}");
            }
        }
    }
}