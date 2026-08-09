using AutomationAssignmentNess.Pages;
using AutomationAssignmentNess.Tests;
using AutomationAssignmentNess.Tests.TestHelpers;
using NUnit.Framework;
using System.Collections.Generic;



namespace AutomationAssignmentNess
{
    [TestFixture]
    public class MainTest : BaseTest
    {
        [TestCaseSource(typeof(SearchDataProviders), nameof(SearchDataProviders.ProvideSearchData))]
        public void TestMethod1(Dictionary<string, string> searchItemData)
        {
            var homePage = new HomePage(_driver, _wait);
            var signInPage = new SignInPage(_driver, _wait);
            var prodactPage = new ProductPage(_driver, _wait);
            var cartPage = new CartPage(_driver, _wait);
            string itemName = searchItemData["ITEMNAME"];
            string maxPrice = searchItemData["MAXPRICE"];
            string minPrice = searchItemData["MINPRICE"];
            string limit = searchItemData["LIMIT"];

            _driverHandler.MoveToPage(_driver, webSiteLink["EBAY"]);
            //homePage.ClickLogin();
            //Assert.That(signInPage.PerformFullLogin(userData["USERID"]), Is.True);

            //using this to move to the main page because i can't login to a real user
            _driverHandler.MoveToPage(_driver, webSiteLink["EBAY"]);
            prodactPage.AddItemsToCart(
                homePage.SearchItemsByNameUnderPrice(itemName, maxPrice, minPrice, int.Parse(limit)));
            homePage.GoToCart();
            cartPage.AssertCartTotalNotExceeds(double.Parse(maxPrice), int.Parse(limit));



        }
    }
}
