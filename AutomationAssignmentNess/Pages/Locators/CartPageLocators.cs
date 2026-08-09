using OpenQA.Selenium;

namespace AutomationAssignmentNess.Pages.Locators
{
    public class CartPageLocators
    {
        public By ItemTotal => By.XPath("//div[@data-test-id='ITEM_TOTAL']");
        public By ShippungTotal => By.XPath("//div[@data-test-id='SHIPPING']");
        public By Subtotal => By.XPath("//div[@data-test-id='SUBTOTAL']");



    }
}
