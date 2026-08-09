using AutomationAssignmentNess.Actions.SeleniumActaions;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
