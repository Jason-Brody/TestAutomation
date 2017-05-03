using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Remote;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Newtonsoft.Json;
namespace MySelenium {

    public class HighlightConfig {
        public static bool NeedHighlight { get; set; } = false;

        public static int HighlightSeconds { get; set; } = 1;
    }


    public static class SeleniumExtension {

       


        static SeleniumExtension() {
            highlightScript = LoadJs("MySelenium.JsLib.Highlight.js");
            mouseOverScript = LoadJs("MySelenium.JsLib.MouseOver.js");
         
        }

        public static void SaveAsJson(this object obj,string fileName) {
            var str = JsonConvert.SerializeObject(obj);
            File.WriteAllLines(fileName, new string[] { str });
        }

        public static void SaveAsXml<T>(this T obj,string fileName) {
            XmlSerializer xs = new XmlSerializer(typeof(T));
            using(FileStream fs = new FileStream(fileName, FileMode.Create)) {
                xs.Serialize(fs, obj);
            }
        }

        public static void SaveAsXml(this object obj,Type tp, string fileName) {
            XmlSerializer xs = new XmlSerializer(tp);
            using (FileStream fs = new FileStream(fileName, FileMode.Create)) {
                xs.Serialize(fs, obj);
            }
        }

        public static T ReadFromXml<T>(string fileName) {
            XmlSerializer xs = new XmlSerializer(typeof(T));
            using(FileStream fs = new FileStream(fileName, FileMode.Open)) {
                return (T)xs.Deserialize(fs);
            }
        }




        private static string LoadJs(string resourceName) {
            var assembly = Assembly.GetExecutingAssembly();
            var names = assembly.GetManifestResourceNames();
            using (Stream stream = assembly.GetManifestResourceStream(resourceName))

            using (StreamReader reader = new StreamReader(stream)) {
                string result = reader.ReadToEnd();
                return result;
            }
        }

        #region GetClickableElement
        public static IWebElement GetClickableElement(this IWebDriver driver,By by,TimeSpan? ts=null) {
            if (ts == null) {
                ts = TimeSpan.FromSeconds(10);
            }
            WebDriverWait wait = new WebDriverWait(driver, ts.Value);
            return wait.Until(ExpectedConditions.ElementToBeClickable(by));
        }
        #endregion


        public static void ExistRun<T, T1, T2,T3>(this ValueTuple<T, T1, T2,T3> obj, Action<T, T1, T2,T3> action) {

            if (obj.Item1 == null || obj.Item2 == null || obj.Item3 == null || obj.Item4 ==null) {
                return;
            }

            action(obj.Item1, obj.Item2, obj.Item3,obj.Item4);
        }

        public static void ExistRun<T, T1,T2>(this ValueTuple<T, T1,T2> obj, Action<T, T1,T2> action) {

            if (obj.Item1 == null || obj.Item2 == null || obj.Item3 == null) {
                return;
            }

            action(obj.Item1, obj.Item2,obj.Item3);
        }

        public static void ExistRun<T, T1>(this ValueTuple<T, T1> obj, Action<T, T1> action) {
            if (obj.Item1 == null || obj.Item2 == null) {
                return;
            }

            action(obj.Item1, obj.Item2);
        }

        public static void ExistRun<T>(this T obj, Action<T> action) {
            if (obj != null) {
                action(obj);
            }
        }

        public static void ExistRun(this object obj,Action action) {
            if (obj != null)
                action();
        }

        public static void WaitPageLoad(this IWebDriver driver,TimeSpan? ts=null) {
            if (ts == null) {
                ts = TimeSpan.FromSeconds(20);
            }

            TimeWait.Get(ts.Value).Until(() => {
                var result = (driver as IJavaScriptExecutor).ExecuteScript("return document.readyState");
                return result.ToString() == "complete";
            });

            
           
        }

       

        public static void JClick(this IWebElement element) {
           
            RemoteWebElement e1 = element as RemoteWebElement;
            (e1.WrappedDriver as IJavaScriptExecutor).ExecuteScript("var tempE = arguments[0];tempE.click()", e1);
            Task.Delay(100).Wait();
        }


        public static void ForeMouseOver(this IWebElement element) {
            var before = getStatus();

            TimeWait.Default.RunUntil(() => { element.MouseOver(); }, () => {
                var after = getStatus();

                if (before.Item1 != after.Item1 ||
                 before.Item2 != after.Item2 ||
                 before.Item3 != after.Item3
                 )
                    return true;
                return false;
            });




            ValueTuple<string, int, string> getStatus()
            {
                var url = ((RemoteWebElement)element).WrappedDriver.Url;
                var windowCount = ((RemoteWebElement)element).WrappedDriver.WindowHandles.Count;
                var html = ((RemoteWebElement)element).WrappedDriver.PageSource;
                return new ValueTuple<string, int, string>(url, windowCount, html);
            }
        }


        public static void ForceClick(this IWebElement element) {

            var before = getStatus();

            TimeWait.Default.RunUntil(element.Click, () => {
                var after = getStatus();
            
                if (before.Item1 != after.Item1 ||
                 before.Item2 != after.Item2 ||
                 before.Item3 != after.Item3 
                 )
                    return true;
                return false;
            });

           


            ValueTuple<string,int,string> getStatus()
            {
                var url = ((RemoteWebElement)element).WrappedDriver.Url;
                var windowCount = ((RemoteWebElement)element).WrappedDriver.WindowHandles.Count;
                var html = ((RemoteWebElement)element).WrappedDriver.PageSource;
                return new ValueTuple<string, int, string>( url, windowCount, html);
            }

        }

        #region GetVisualElement
       

        public static IWebElement GetVisualElement(this IWebElement element, By by, TimeSpan? ts=null) {

            var driver = (element as RemoteWebElement).WrappedDriver;

            return GetVisualElement(driver,by,ts);
        }

        public static IWebElement GetVisualElement(this IWebDriver driver, By by, TimeSpan? ts = null) {
            if (ts == null) {
                ts = TimeSpan.FromSeconds(10);
            }

            WebDriverWait wait = new WebDriverWait(driver,ts.Value);
            var e = wait.Until(ExpectedConditions.ElementIsVisible(by));
            if (e != null && HighlightConfig.NeedHighlight) {
                e.Highlight(HighlightConfig.HighlightSeconds);
            }
            return e;


            
            
        }

        
        #endregion


        //#region GetElementUntil
        //public static void UntilExist(Action trigger, Func<bool> condition, TimeWait setting = null) {
           
        //    if (setting == null)
        //        setting = TimeWait.Default;
        //    setting.RunUntil(trigger, condition);
        //}
        //public static T UntilExist<T>(Func<T> method, TimeWait setting = null) {
        //    if (setting == null) {
        //        setting = TimeWait.Default;
        //    }
        //    return setting.Get(method);
        //}

      

        //#endregion



        //#region GetElementLazy
        //public static IWebElement GetElementLazy(this IWebDriver driver, By by, TimeWait setting = null) {
        //    return UntilExist(() => driver.GetElement(by), setting);
        //}

        

        //public static IWebElement GetElementLazy(this IWebElement element, By by, TimeWait setting = null) {
        //    return UntilExist(() => element.GetElement(by), setting);
        //}
        //#endregion



        //#region GetElement without exception

        public static IWebElement GetElement(this IWebDriver driver, By by) {
            return GetElement(driver.FindElement, by);
        }

        public static IWebElement GetElement(this IWebElement element, By by) {
            
            return GetElement(element.FindElement, by);
        }

        public static IWebElement GetElement(Func<By, IWebElement> method, By by) {
            try {
                return method(by);
            } catch {
                return null;
            }
        }
      

        #region ScreenShot
        public static void TakeScreenShot(this IWebDriver driver, string fileLocation) {
            Screenshot ss = ((ITakesScreenshot)driver).GetScreenshot();
            ss.SaveAsFile(fileLocation, ScreenshotImageFormat.Jpeg);
        }
        #endregion

        #region Highlight
        static string highlightScript;

        public static IWebElement Highlight(this IWebElement e, int seconds = 1) {
            if (seconds < 0) {
                return e;
            }
            RemoteWebElement e1 = e as RemoteWebElement;
            (e1.WrappedDriver as IJavaScriptExecutor).ExecuteScript(highlightScript, e, seconds);
            Task.Delay(seconds * 1000).Wait();
            return e;
        }

        public static IWebElement Highlight(this IWebDriver driver, IWebElement e, int seconds = 1) {
            if (seconds < 0) {
                return e;
            }
            ((IJavaScriptExecutor)driver).ExecuteScript(highlightScript, e, seconds);
            Task.Delay(seconds * 1000).Wait();
            return e;
        }


        #endregion


        #region MouseOver
        private static string mouseOverScript;

        static int i = 0;
        public static IWebElement MouseOver(this IWebElement e) {
            Console.WriteLine($"mouse over {++i}");
            RemoteWebElement e1 = e as RemoteWebElement;
            Actions action = new Actions(e1.WrappedDriver);
            action.MoveToElement(e1).Build().Perform();
            Task.Delay(100).Wait();
            return e;

            
        }

        public static IWebElement JMouseOver(this IWebElement e) {
            RemoteWebElement e1 = e as RemoteWebElement;
            (e1.WrappedDriver as IJavaScriptExecutor).ExecuteScript(mouseOverScript, e);
            Task.Delay(300).Wait();
            return e;
        }


        #endregion
    }
}
