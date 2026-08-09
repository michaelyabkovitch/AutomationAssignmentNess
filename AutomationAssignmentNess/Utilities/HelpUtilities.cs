
using AutomationAssignmentNess.Pages.Locators;
using System;
using System.IO;


namespace AutomationAssignmentNess.Utilities
{
    public static class HelpUtilities
    {
        public static bool CheckAndCreateFolder(string folderPath)
        {
            try
            {
                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }

                return true;
            }
            catch
            {
                return false;
            }
        }

        public static string CleanPriceILS(string price)
        {
            string cleanPriceText = price.Replace("ILS", "").Trim();
            return cleanPriceText = cleanPriceText.Split(new string[] { " to ", "to" }, StringSplitOptions.None)[0].Trim();
        }


    }
}
