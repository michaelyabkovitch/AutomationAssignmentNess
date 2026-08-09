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
