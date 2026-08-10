# eBay E2E Automation Assignment

This project is a UI automation framework written in C# and Selenium, designed to test an end-to-end purchasing flow on eBay.
It covers searching for items, applying price filters, selecting product variants (like size and color), and validating the total cart price.

## How to Run

1. Open `AutomationAssignmentNess.sln` in Visual Studio 2022.
2. Build the solution (`Ctrl + Shift + B`) to automatically restore all NuGet packages (NUnit, Selenium, ExtentReports, Newtonsoft.Json).
3. Open the NUnit Test Explorer and run `TestMethod1`.

## Architecture Overview

* **Page Object Model (POM):** The framework separates UI locators from business logic. Locators are kept in dedicated classes (e.g., `HomePageLocators`), while the logic sits in page classes (e.g., `HomePage`).
* **Data-Driven:** Test inputs (item names, min/max price, item limit) are read dynamically from `Files/SearchItems.json`.
* **Selenium Wrapper:** Standard Selenium actions are wrapped in a custom `SeleniumActions` class to handle dynamic waits, scrolling to elements before clicking, and reducing test flakiness.

## Notes & Assumptions

* **Login:** eBay has aggressive bot detection and Captchas that frequently block automated logins. To keep the test stable and focus on the core flow,
* the automation skips the login phase and performs the purchasing steps as a Guest.
* **Auction/Bid Items:** If the automation selects an item that cannot be added directly to the cart (e.g., requires placing a bid instead of "Buy It Now"),
*  the system is designed to gracefully skip it and move to the next available item link.
* **Dynamic Variants & UI Inconsistencies:** If a product requires picking a size or color before adding it to the cart, the code handles this by randomly selecting an available visible option. However, due to extreme inconsistency in eBay's UI across different product categories (dropdown names and options constantly change), there might be edge cases where selection fails.
*  The automation does not account for every possible unexpected UI variation.
* **Price Calculation & Cart Total:** The requested logic was to calculate the expected total based on (Number of items * Max price). However, the framework extracts the actual price directly from the cart's 'Subtotal'.
*  Since the Subtotal often includes shipping fees, tests might fail if the final sum exceeds the strict expected mathematical limit.
* **Currency:** The tests assume prices are displayed in ILS (as per regional defaults) and parse the text accordingly.

## Reporting

The project uses **Extent Reports** for logging. After execution, an HTML report is generated inside the `Reports` directory in the project root.
It includes step-by-step logs and automatically attaches screenshots for items added to the cart and for any test failures.
