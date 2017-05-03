using MySelenium;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KbWebAutomation.Pages {
    public static class NavigationUtil {
        public static void NavigateTo<T, T1>(IWebDriver driver, BasePage<T, T1> page, T1 parameter) where T : class,IWebPage {
            NavigateTo<T>(driver, page, parameter);
        }

        public static void NavigateTo<T>(IWebDriver driver, BasePage<T> page, object parameter) where T :class, IWebPage {
            Stack<IWebPage> pages = new Stack<IWebPage>();
            IWebPage myPage = page;
            do {
                pages.Push(myPage);
                myPage = myPage.FromPage();
            } while (myPage != null);

            while (pages.Count > 0) {
                var p = pages.Pop();
                var tempPage = p.FromPage();
                if (tempPage != null) {
                    tempPage.Parameter = parameter;
                    tempPage.Driver = driver;
                }
                driver.WaitPageLoad();
                p.Parameter = parameter;
                p.Driver = driver;
                p.Navigate(tempPage);
                TimeWait.Default.Until(() => switchPage(driver, p));
            }
        }


        private static bool switchPage(IWebDriver driver, IWebPage page) {

            if (page.FromPage() == null)
                return true;
            if (driver.WindowHandles.Count == 1)
                return true;
            foreach (var window in driver.WindowHandles) {

                driver.SwitchTo().Window(window);
                if (page.IsNavigate()) {
                    return true;
                }
            }
            return false;

        }


    }
}
