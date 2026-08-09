using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutomationAssignmentNess.Pages.Locators
{
    public class CartPageLocators
    {
        public By ItemTotal => By.XPath("//div[@data-test-id='ITEM_TOTAL']");
        public By ShippungTotal => By.XPath("//div[@data-test-id='SHIPPING']");
        public By Subtotal => By.XPath("//div[@data-test-id='SUBTOTAL']");



    }
}
