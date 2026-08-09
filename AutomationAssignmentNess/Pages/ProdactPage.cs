using AutomationAssignmentNess.Handlers;
using AutomationAssignmentNess.Pages.Locators;
using AutomationAssignmentNess.Utilities;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AutomationAssignmentNess.Pages
{
    public class ProdactPage : BasePage
    {
        ProdactpageLocators prodactpageElments= new ProdactpageLocators();
        public ProdactPage(IWebDriver driver, WebDriverWait wait) : base(driver, wait)
        {
        }


        public void AddItemsToCart(List<string> productUrls)
        {
          
            string mainWindow = _driver.CurrentWindowHandle;
            string screenshotsDir = Path.Combine(Directory.GetParent(NUnit.Framework.TestContext.CurrentContext.WorkDirectory).Parent.FullName, "Reports", "Screenshots");

            HelpUtilities.CheckAndCreateFolder(screenshotsDir);

            Random randomGenerator = new Random();

            foreach (string url in productUrls)
            {
                try
                {
                    Action.NewTab().Navigate(url);

                    if (_driver.FindElements(prodactpageElments.ColorSelect).Count > 0)
                    {
                        Action.Click(prodactpageElments.ColorSelect, "Color Dropdown");

                        SelectRandomOption(randomGenerator);
                    }

                    if (_driver.FindElements(prodactpageElments.SizeSelect).Count > 0)
                    {
                        Action.Click(prodactpageElments.SizeSelect, "Size Dropdown");
                        SelectRandomOption(randomGenerator);
                    }

                    Action.Click(prodactpageElments.AddToCard, "Add To Cart Button");
                    Action.WaitForElementToBeAvailable(prodactpageElments.SeeInCartButton, "SeeInCartButton");
                    Action.TakeScreenshot(screenshotsDir, url);
                }
                catch (Exception ex)
                {

                    ExtentReportHandler.LogFail($"Failed to process product: {url}. Details: {ex.Message}");
                }
                finally
                {
                    _driver.Close();

                    _driver.SwitchTo().Window(mainWindow);
                }
            }
        }

        private void SelectRandomOption(Random rnd)
        {
            var allOptions = _wait.Until(d => d.FindElements(prodactpageElments.DropdownOptions));
            var visibleOptions = allOptions.Where(opt => opt.Displayed && opt.Enabled).ToList();

            if (visibleOptions.Count > 0)
            {
                int randomIndex = rnd.Next(0, visibleOptions.Count);
                IWebElement selectedOption = visibleOptions[randomIndex];

                selectedOption.Click();

                try
                {
                    _wait.Until(d =>
                    {
                        try
                        {
                            return !selectedOption.Displayed;
                        }
                        catch (StaleElementReferenceException)
                        {
                            return true;
                        }
                    });
                }
                catch (WebDriverTimeoutException)
                {
                    ExtentReportHandler.LogInfo("Waiting for dropdown to close timed out, continuing anyway.");
                }
            }
            else
            {
                ExtentReportHandler.LogInfo("Opened dropdown, but no selectable options were visible on screen.");
            }
        }



    }
}
