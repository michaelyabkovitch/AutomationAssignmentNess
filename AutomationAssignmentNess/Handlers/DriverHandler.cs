using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutomationAssignmentNess.Handlers
{
    public class DriverHandler
    {
      
        /// <summary>
        /// This function create new Driver instent with no option use 
        /// </summary>
        /// <returns></returns>
        public IWebDriver DriverCreation() {

            IWebDriver driver = new ChromeDriver();

            return driver;

        }
        public void MoveToPage(IWebDriver driver, string url)
        {
            try
            {
           
                driver.Navigate().GoToUrl(url);

            }
            catch (Exception ex)
            {

                Console.WriteLine($"FAILE to navgiat to site : {ex.Message}");

                throw;
            }
        }






    }
}
