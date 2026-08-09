using AutomationAssignmentNess.Handlers;
using AutomationAssignmentNess.Pages.Locators;
using AutomationAssignmentNess.Utilities;
using AventStack.ExtentReports;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;


namespace AutomationAssignmentNess.Pages
{
    public class CartPage : BasePage
    {

        CartPageLocators cartPageLocators = new CartPageLocators();

        public CartPage(IWebDriver driver, WebDriverWait wait) : base(driver, wait)
        {
        }

        private Dictionary<string, string> GetPrices()
        {
            Dictionary<string, string> prices = new Dictionary<string, string>();

            prices.Add("ITEM_TOTAL", HelpUtilities.CleanPriceILS(Action.WaitForElementToBeAvailable(cartPageLocators.ItemTotal, "ItemTotal").Text));
            prices.Add("SHIPPING", HelpUtilities.CleanPriceILS(Action.WaitForElementToBeAvailable(cartPageLocators.ShippungTotal, "ShippungTotal").Text));
            prices.Add("SUBTOTAL", HelpUtilities.CleanPriceILS(Action.WaitForElementToBeAvailable(cartPageLocators.Subtotal, "Subtotal").Text));

            return prices;


        }

        public void AssertCartTotalNotExceeds(double budgetPerItem, int itemsCount)
        {
            Dictionary<string, string> prices = new Dictionary<string, string>();

            prices = GetPrices();

            Double maxMoneyIwantToSpend = budgetPerItem * itemsCount;

            ExtentReportHandler.LogInfo($"SUBTOTAL = {prices["SUBTOTAL"]}");
            ExtentReportHandler.LogInfo($"maxMoneyIwantToSpend = { maxMoneyIwantToSpend}");
            Assert.That(Convert.ToDouble(prices["SUBTOTAL"]) <= maxMoneyIwantToSpend, Is.True);

        }




    }
}
