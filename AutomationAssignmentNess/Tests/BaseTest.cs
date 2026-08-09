using AutomationAssignmentNess.Handlers;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.IO;

namespace AutomationAssignmentNess.Tests
{
    [TestFixture]
    public class BaseTest
    {
        protected IWebDriver _driver;
        protected WebDriverWait _wait;
        protected DriverHandler _driverHandler;
        protected Dictionary<string, string> webSiteLink;
        protected Dictionary<string, string> userData;
        protected List<Dictionary<string, string>> itemData;


        [OneTimeSetUp]
        public void Setup()
        {
            string workingDirectory = TestContext.CurrentContext.WorkDirectory;
            string projectDirectory = Directory.GetParent(workingDirectory).Parent.FullName;
            string reportsFolder = Path.Combine(projectDirectory, "Reports");

            if (!Directory.Exists(reportsFolder))
            {
                Directory.CreateDirectory(reportsFolder);
            }

            string reportFileName = $"ExtentReport_{DateTime.Now:yyyy_MM_dd_HH_mm_ss}.html";
            string fullReportPath = Path.Combine(reportsFolder, reportFileName);
            ExtentReportHandler.InitReport(fullReportPath);

            _driverHandler = new DriverHandler();

            _driver = _driverHandler.DriverCreation();
            _driver.Manage().Window.Maximize();
            _wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));



        }

        [SetUp]
        public void SetupBeforeTest()
        {
            _driver.Manage().Cookies.DeleteAllCookies();
            string currentTestName = TestContext.CurrentContext.Test.Name;
            ExtentReportHandler.CreateTest(currentTestName);
            webSiteLink = JSONHandler.ConvertJsonToDictionary(Path.Combine(NUnit.Framework.TestContext.CurrentContext.TestDirectory, "Files", "links.json"));
            userData = JSONHandler.ConvertJsonToDictionary(Path.Combine(NUnit.Framework.TestContext.CurrentContext.TestDirectory, "Files", "LoginUserData.json"));
            //itemData = JSONHandler.ConvertJsonToListOfDictionaries(Path.Combine(NUnit.Framework.TestContext.CurrentContext.TestDirectory, "Files", "SearchItems.json"));
        }

        [TearDown]
        public void CleanUpAfterTest()
        {
            var testStatus = TestContext.CurrentContext.Result.Outcome.Status;

            if (testStatus == NUnit.Framework.Interfaces.TestStatus.Failed)
            {
                string screenshotPath = $"error_{Guid.NewGuid()}.png";
                ((ITakesScreenshot)_driver).GetScreenshot().SaveAsFile(screenshotPath);

                string errorMessage = TestContext.CurrentContext.Result.Message;
                ExtentReportHandler.LogFail($"Test Fail : {errorMessage}", screenshotPath);
            }
            else if (testStatus == NUnit.Framework.Interfaces.TestStatus.Passed)
            {

                ExtentReportHandler.LogPass("Test Pass.");
            }
        }

        [OneTimeTearDown]
        public void GlobalTearDown()
        {

            if (_driver != null)
            {

                _driver.Quit();
                _driver.Dispose();

            }
            ExtentReportHandler.Flush();


        }
    }
}
