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
*  the automation skips the login phase and performs the purchasing steps as a Guest.
* **Currency:** The tests assume prices are displayed in ILS (as per regional defaults) and parse the text accordingly.
* **Dynamic Variants:** If a product requires picking a size or color before adding it to the cart, the code handles this by randomly selecting an available visible option.

## Reporting

The project uses **Extent Reports** for logging. After execution, an HTML report is generated inside the `Reports` directory in the project root.
It includes step-by-step logs and automatically attaches screenshots for items added to the cart and for any test failures.
