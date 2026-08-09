using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutomationAssignmentNess.Pages.Locators
{
    public class HomePageLoctors 
    {
        public By LogInButtn => By.XPath("//a[contains(@href,'signin.ebay.com')]");
        public By SearchBar => By.Id("gh-ac");
        public By Searchbtn => By.Id("gh-search-btn");

        public By MinVal => By.XPath("//input[contains(@id,'beginParamValue-textbox')]");
        public By MaxVal => By.XPath("//input[contains(@id,'endParamValue')]");
        public By SubmitPriceRange => By.XPath("//*[@title='Submit price range']");

        public By SearchResulsts => By.ClassName("srp-results");
        public By NextPageBtn => By.ClassName("pagination__next");
        public By CartButton => By.XPath("//a[contains(@href,'cart.ebay.com')]");




    }
}
