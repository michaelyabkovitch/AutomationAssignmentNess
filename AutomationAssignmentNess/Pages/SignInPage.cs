using AutomationAssignmentNess.Pages.Locators;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutomationAssignmentNess.Pages
{
    public class SignInPage : BasePage  
    {
        LogInScreenLocators logInScreenLocators = new LogInScreenLocators();
        public SignInPage(IWebDriver driver, WebDriverWait wait) : base(driver, wait)
        {


        }


        
        private bool EnterUserId(string userId ,string elementName)
        {
            return Action.SendText(logInScreenLocators.UserId, userId, elementName);

        }

        private bool ClickContinue()
        {
            return Action.Click(logInScreenLocators.ContinueBtn, "ContinueBtn");
        }

        public bool PerformFullLogin(string userId,string passowrd ="") 
        {
            if (!EnterUserId(userId, "User ID Field")) return false;
            if (!ClickContinue()) return false;

            return true;


        }
        




    }
}
