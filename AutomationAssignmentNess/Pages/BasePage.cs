using AutomationAssignmentNess.Actions.SeleniumActaions;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace AutomationAssignmentNess.Pages
{
    public class BasePage
    {
        protected IWebDriver _driver;
        protected WebDriverWait _wait;
        protected SeleniumActions Action;

        public BasePage(IWebDriver driver, WebDriverWait wait)
        {
            _driver = driver;
            _wait = wait;

            Action = new SeleniumActions(driver, wait);
        }
    }
}
