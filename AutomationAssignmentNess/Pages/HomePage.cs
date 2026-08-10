using AutomationAssignmentNess.Handlers;
using AutomationAssignmentNess.Pages.Locators;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AutomationAssignmentNess.Pages
{

    public class HomePage : BasePage
    {
        HomePageLocators homePageLoctors = new HomePageLocators();

        public HomePage(IWebDriver driver, WebDriverWait wait) : base(driver, wait)
        {
        }


        public bool ClickLogin()
        {
            return Action.Click(homePageLoctors.LogInButtn, "Log in");
        }

        private bool MakeSearch(string item)
        {
            Action.SendText(homePageLoctors.SearchBar, item, "Search bar");
            return Action.Click(homePageLoctors.Searchbtn, "search btn ");

        }
        private bool SetMinVal(string minVal)
        {

            return Action.SendText(homePageLoctors.MinVal, minVal, "minVal");
        }

        private bool SetMaxVal(string maxVal)
        {

            return Action.SendText(homePageLoctors.MaxVal, maxVal, "maxVal");
        }
        private bool ClickSubmitPriceRange()
        {

            return Action.Click(homePageLoctors.SubmitPriceRange, "SubmitPriceRange");
        }

        /// <summary>
        /// Parses the search results container to extract product URLs, filtering them based on a maximum price threshold.
        /// It cleans and evaluates price strings (handling 'ILS' currency and price ranges) and safely ignores 
        /// items with missing data or unparsable formats.
        /// </summary>
        /// <param name="limit">The maximum number of valid product URLs to extract.</param>
        /// <param name="maxPrice">The maximum allowed price. Items exceeding this value are skipped.</param>
        /// <returns>A list containing the URLs of the products that fall within the price limit.</returns>
        private List<string> GetSearchResults(int limit, string maxPrice)
        {
            try
            {
                double targetMaxPrice = double.Parse(maxPrice);
                List<string> validProductUrls = new List<string>();

                IWebElement resultsContainer = Action.WaitForElementToBeAvailable(homePageLoctors.SearchResulsts, "SearchResulsts");
                var productCards = resultsContainer.FindElements(By.XPath(".//li[contains(@class, 's-card')]"));

                foreach (var card in productCards)
                {
                    if (validProductUrls.Count >= limit)
                    {
                        break;
                    }

                    try
                    {
                        IWebElement priceElement = card.FindElement(By.XPath(".//span[contains(@class,'s-card__price')]"));
                        string rawPriceText = priceElement.Text;

                        string cleanPriceText = rawPriceText.Replace("ILS", "").Trim();
                        cleanPriceText = cleanPriceText.Split(new string[] { " to ", "to" }, StringSplitOptions.None)[0].Trim();

                        double actualPrice = double.Parse(cleanPriceText);

                        if (actualPrice <= targetMaxPrice)
                        {
                            IWebElement linkElement = card.FindElement(By.XPath(".//*[contains(@class,'su-card-container__header')]//a[contains(@class, 's-card__link')]"));
                            string url = linkElement.GetAttribute("href");

                            if (!validProductUrls.Contains(url))
                            {
                                validProductUrls.Add(url);
                            }
                        }
                    }
                    catch (NoSuchElementException)
                    {
                        continue;
                    }
                    catch (FormatException)
                    {
                        continue;
                    }
                }
                return validProductUrls;
            }
            catch (Exception ex)
            {
                ExtentReportHandler.LogFail($"Failed to extract product links. Details: {ex.Message}");
                throw new Exception($"Failed to extract product links from the search results. -> {ex.Message}");
            }
        }

        public List<string> SearchItemsByNameUnderPrice(string itemName, string maxPrice, string minPrice, int limit)
        {
            MakeSearch(itemName);
            SetMinVal(minPrice);
            SetMaxVal(maxPrice);
            ClickSubmitPriceRange();

            List<string> itemUrls = GetSearchResults(limit, maxPrice);

            while (itemUrls.Count < limit)
            {
                List<string> temp = Paging(itemUrls, limit, maxPrice);

                if (temp == null)
                {
                    break;
                }
                itemUrls = temp;
            }

            ExtentReportHandler.LogInfo($"Found {itemUrls.Count} items.");
            return itemUrls;
        }

        /// <summary>
        /// Navigates to the next page of search results to fetch additional items when the current page 
        /// does not yield enough products to reach the desired limit. Ensures a stable DOM state by waiting 
        /// for the old results container to become stale before extracting new URLs.
        /// </summary>
        /// <param name="currentList">The current list of collected product URLs.</param>
        /// <param name="limit">The total target number of product URLs to collect.</param>
        /// <param name="maxPrice">The maximum allowed price for filtering the new results.</param>
        /// <returns>A combined list of previous and newly fetched product URLs, or null if no further pages are available.</returns>
        private List<string> Paging(List<string> currentList, int limit, string maxPrice)
        {
            IWebElement oldResultsContainer = Action.WaitForElementToBeAvailable(homePageLoctors.SearchResulsts, "oldResultsContainer");

            if (!Action.Click(homePageLoctors.NextPageBtn, "next page btn"))
            {
                ExtentReportHandler.LogInfo("No more next page buttons found.");
                return null;
            }
            // Wait strictly for the OLD container to become "Stale" (disconnected from the DOM)
            _wait.Until(driver =>
            {
                try
                {
                    bool isStillThere = oldResultsContainer.Displayed;
                    return false;
                }
                catch (StaleElementReferenceException)
                {
                    return true;
                }
            });
            Action.WaitForElementToBeAvailable(homePageLoctors.SearchResulsts, "SearchResulsts");

            int currentSize = currentList.Count;
            int delta = limit - currentSize;

            List<string> newList = GetSearchResults(delta, maxPrice);

            return currentList.Concat(newList).ToList();
        }


        public void GoToCart()
        {
            Action.Click(homePageLoctors.CartButton, "CartButton");
        }





    }

}
