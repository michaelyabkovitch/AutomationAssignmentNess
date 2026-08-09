using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutomationAssignmentNess.Pages.Locators
{
    public class GlobalLocators
    {

        public By Loader => By.XPath("//span[contains(@class, 'spinner') or contains(@class, 'loading')] | //div[contains(@class, 'spinner')]");

    }
}