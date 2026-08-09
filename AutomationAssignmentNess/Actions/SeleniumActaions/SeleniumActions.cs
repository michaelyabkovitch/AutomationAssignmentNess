using AutomationAssignmentNess.Handlers;
using AutomationAssignmentNess.Pages.Locators;
using OpenQA.Selenium;
using OpenQA.Selenium.BiDi.BrowsingContext;
using OpenQA.Selenium.Support.UI;
using System;
using System.IO;


namespace AutomationAssignmentNess.Actions.SeleniumActaions
{
    public class SeleniumActions
    {
        private IWebDriver _driver;
        private WebDriverWait _wait;
        private GlobalLocators _globalLocators;
        

        public SeleniumActions(IWebDriver driver, WebDriverWait wait)
        {
            _driver = driver;
            _wait = wait;
        }

        public bool Click(By locator, string elementName)
        {
            try
            {
                ExtentReportHandler.LogInfo($"Waiting for element '{elementName}' to be clickable.");
                ScrollToElement(locator, elementName);

                // Wait until the element is both displayed and enabled 
                IWebElement element = _wait.Until(d =>
                {
                    IWebElement el = d.FindElement(locator);
                    if (el.Displayed && el.Enabled)
                    {
                        return el;
                    }

                    return null;
                });

                element.Click();

                ExtentReportHandler.LogPass($"Successfully clicked on element '{elementName}'.");

                return true;
            }
            catch (WebDriverTimeoutException ex)
            {
                ExtentReportHandler.LogFail($"Timeout while waiting for element '{elementName}' to be clickable. -> {ex.Message}");

                return false;
            }
            catch (Exception ex)
            {
                ExtentReportHandler.LogFail($"An error occurred while attempting to click on element '{elementName}'. -> {ex.Message}");

                return false;
            }
        }

        public bool SendText(By locator, string text, string elementName)
        {
            try
            {
                ExtentReportHandler.LogInfo($"Waiting for element '{elementName}' to be ready for text input.");
                ScrollToElement(locator, elementName);

                // Wait until the element is both displayed and enabled 
                IWebElement element = _wait.Until(d =>
                {
                    IWebElement el = d.FindElement(locator);
                    if (el.Displayed && el.Enabled)
                    {
                        return el;
                    }

                    return null;
                });

                element.Clear();

                element.SendKeys(text);

                ExtentReportHandler.LogPass($"Successfully entered text '{text}' into element '{elementName}'.");

                return true;
            }
            catch (WebDriverTimeoutException ex)
            {
                ExtentReportHandler.LogFail($"Timeout while waiting for element '{elementName}' to be ready for input. -> {ex.Message}");

                return false;
            }
            catch (Exception ex)
            {
                ExtentReportHandler.LogFail($"An error occurred while attempting to send keys to element '{elementName}'. -> {ex.Message}");

                return false;
            }
        }

        public bool ScrollToElement(By locator, string elementName)
        {
            try
            {
                ExtentReportHandler.LogInfo($"Attempting to scroll to element '{elementName}'.");

                IWebElement element = _driver.FindElement(locator);
                IJavaScriptExecutor js = (IJavaScriptExecutor)_driver;
                js.ExecuteScript("arguments[0].scrollIntoView({behavior: 'smooth', block: 'center'});", element);
                System.Threading.Thread.Sleep(500);

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public IWebElement WaitForElementToBeAvailable(By locator, string elementName)
        {
            try
            {
                IWebElement element = _wait.Until(driver =>
                {
                    try
                    {
                        var el = driver.FindElement(locator);

                        if (el.Displayed && el.Enabled)
                        {
                            return el; 
                        }

                        return null; 
                    }
                    catch (NoSuchElementException)
                    {
                        return null;
                    }
                    catch (StaleElementReferenceException)
                    {
                        return null;
                    }
                });

                ExtentReportHandler.LogPass($"Element '{elementName}' is available.");
                return element;
            }
            catch (WebDriverTimeoutException)
            {
                ExtentReportHandler.LogFail($"Timeout: The element '{elementName}' did not become available within the expected time.");

                throw new Exception($"Failed to locate or interact with element: {elementName}");
            }
        }

        public SeleniumActions NewTab()
        {
            _driver.SwitchTo().NewWindow(WindowType.Tab);
            ExtentReportHandler.LogInfo($"open New TAB");
            return this;
        }

        public SeleniumActions Navigate(string url)
        {
            _driver.Navigate().GoToUrl(url);
            ExtentReportHandler.LogInfo($"navigate to {url}");

            return this;
        }

        public void TakeScreenshot(string directoryPath, string currentUrl)
        {
            ITakesScreenshot screenshotDriver = (ITakesScreenshot)_driver;

            Screenshot screenshot = screenshotDriver.GetScreenshot();

            string timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
            string fileName = $"AddToCart_{timestamp}.png";

            string fullPath = Path.Combine(directoryPath, fileName);

            screenshot.SaveAsFile(fullPath);

            // Log the success to the report
            ExtentReportHandler.LogInfo($"Screenshot saved successfully for {currentUrl} at: {fullPath}");
        }

        public void WhitForSpinner()
        {

            try
            {
                WebDriverWait shortWait = new WebDriverWait(_driver, TimeSpan.FromSeconds(2));
                shortWait.Until(d => d.FindElement(_globalLocators.Loader).Displayed);
            }
            catch (WebDriverTimeoutException)
            {
            }

            _wait.Until(d =>
            {
                var elements = d.FindElements(_globalLocators.Loader);
                return elements.Count == 0 || !elements[0].Displayed;
            });

        }



    }
}
