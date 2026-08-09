using OpenQA.Selenium;

namespace AutomationAssignmentNess.Pages.Locators
{
    public class ProdactpageLocators
    {

        public By AddToCard => By.Id("atcBtn_btn_1");
        public By ColorSelect => By.XPath("//button[.//span[text()='Color:' or text()='Colour:']]");
        public By SizeSelect => By.XPath("//button[.//span[contains(text(), 'Size')]]");
        public By SeeInCartButton => By.XPath("//span[@class='ux-call-to-action__text' and text()='See in cart']");
        public By DropdownOptions => By.XPath("//div[@role='option' and not(@aria-disabled='true')]");

    }
}
