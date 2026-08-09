using AventStack.ExtentReports;
using AventStack.ExtentReports.Reporter;

namespace AutomationAssignmentNess.Handlers
{
    public static class ExtentReportHandler
    {
        private static ExtentReports _extent;

        private static ExtentTest _test;

        public static void InitReport(string reportPath)
        {
            var sparkReporter = new ExtentSparkReporter(reportPath);

            _extent = new ExtentReports();

            _extent.AttachReporter(sparkReporter);

            _extent.AddSystemInfo("Environment", "QA Automation Test");
            _extent.AddSystemInfo("Author", "A True C# Master");
        }

        public static void CreateTest(string testName)
        {
            // אומרים למנוע: "היי, התחיל טסט חדש עם השם הזה, תתחיל לעקוב אחריו"
            _test = _extent.CreateTest(testName);
        }


        // הדפסת הודעת מידע רגילה (צבע כחול בדו"ח)
        public static void LogInfo(string message)
        {
            // מוסיף שורת לוג מסוג Info לטסט הנוכחי
            _test.Log(Status.Info, message);
        }

        // הדפסת הודעת הצלחה (צבע ירוק בדו"ח)
        public static void LogPass(string message)
        {
            // מוסיף שורת לוג מסוג Pass לטסט הנוכחי
            _test.Log(Status.Pass, message);
        }

        // הדפסת הודעת כישלון (צבע אדום בדו"ח) - מקבלת הודעה ונתיב אופציונלי לתמונה
        public static void LogFail(string message, string screenshotPath = null)
        {
            // בודק אם לא העברנו לו נתיב לתמונה
            if (string.IsNullOrEmpty(screenshotPath))
            {
                // פשוט רושם שהטסט נכשל עם ההודעה
                _test.Log(Status.Fail, message);
            }
            else
            {
                // אם יש לנו תמונה, אנחנו בונים אותה ומצרפים אותה ישר לתוך הלוג של ה-Fail!
                var mediaModel = MediaEntityBuilder.CreateScreenCaptureFromPath(screenshotPath).Build();
                _test.Log(Status.Fail, message, mediaModel);
            }
        }

        // פונקציה חובה! היא זו שבאמת כותבת את הכל לקובץ ה-HTML הפיזי בסוף
        public static void Flush()
        {
            // אם לא תקרא לפונקציה הזו בסוף הריצה, קובץ ה-HTML שלך יהיה ריק לחלוטין.
            _extent.Flush();
        }
    }
}
