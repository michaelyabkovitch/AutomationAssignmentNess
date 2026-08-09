using OpenQA.Selenium;

namespace AutomationAssignmentNess.Pages.Locators
{
    public class ProdactPageLocators
    {

        public By AddToCard => By.Id("atcBtn_btn_1");
        public By AllVariantDropdowns => By.XPath("//div[contains(@class, 'x-sku')]//button[@aria-haspopup='listbox']");
        public By SeeInCartButton => By.XPath("//span[@class='ux-call-to-action__text' and text()='See in cart']");
        public By DropdownOptions => By.XPath("//div[@role='option' and not(@aria-disabled='true') and @data-sku-value-name]");

        public By SpecificVariantDropdown(int index)
        {
            return By.XPath($"(//div[contains(@class, 'x-sku')]//button[@aria-haspopup='listbox'])[{index}]");
        }

    }
}
