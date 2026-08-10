using AutomationAssignmentNess.Handlers;
using AutomationAssignmentNess.Pages.Locators;
using AutomationAssignmentNess.Utilities;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AutomationAssignmentNess.Pages
{
    public class ProductPage : BasePage
    {
        ProdactPageLocators prodactpageElments = new ProdactPageLocators();
        public ProductPage(IWebDriver driver, WebDriverWait wait) : base(driver, wait)
        {
        }

        /// <summary>
        /// Iterates through the provided product URLs, opens each in a new tab, and attempts to add the item to the cart.
        /// The method dynamically detects all available variant dropdowns (e.g., Color, Size) and selects a random option for each.
        /// Note: If the product is an auction item or lacks a standard 'Buy It Now' / 'Add to Cart' button, 
        /// the method will gracefully catch the failure, skip the current item without performing any action, 
        /// and continue to the next URL in the list.
        /// </summary>
        /// <param name="productUrls">A list of product URLs to process.</param>
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
                   
                     
                     
                    int dropdownCount = _driver.FindElements(prodactpageElments.AllVariantDropdowns).Count;

                    for (int i = 0; i < dropdownCount; i++)
                    {
                        
                        By currentDropdownLocator = prodactpageElments.SpecificVariantDropdown(i);
                        var currentDropdown = _wait.Until(d =>
                        {
                            var elements = d.FindElements(prodactpageElments.AllVariantDropdowns);
                            return elements.Count > i ? elements[i] : null;
                        });

                        if (currentDropdown == null) break;

                        Action.ScrollToElement(currentDropdownLocator, $"Variant Dropdown #{i + 1}");

                        _wait.Until(d => currentDropdown.Displayed && currentDropdown.Enabled);
                        currentDropdown.Click();

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

        /// <summary>
        /// Retrieves all available options from the currently open dropdown menu, filters for those that are visible and enabled, 
        /// and randomly selects one to click. After the click, it waits for the dropdown to close (detecting when the selected 
        /// option becomes invisible or stale) to ensure the UI is ready for subsequent actions.
        /// </summary>
        /// <param name="rnd">An instance of the Random class used to generate the random index for selection.</param>
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
                    ExtentReportHandler.LogInfo("Waiting for dropdown to close timed out,continuing anyway.");
                }
            }
            else
            {
                ExtentReportHandler.LogInfo("Opened dropdown,but no selectable options were visible on screen.");
            }
        }



    }
}
