using OpenQA.Selenium;

namespace AutomationAssignmentNess.Pages.Locators
{
    public class GlobalLocators
    {

        public By Loader => By.XPath("//span[contains(@class, 'spinner') or contains(@class, 'loading')] | //div[contains(@class, 'spinner')]");

    }
}