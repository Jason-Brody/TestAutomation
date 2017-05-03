using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MySelenium {
    public interface IWebPage {
        IWebPage FromPage();

        void Navigate(IWebPage FromPage);

        bool IsNavigate();

        object Parameter { get; set; }

        IWebDriver Driver { get; set; }
    }

    public interface IWebPage<T> where T:IWebPage {


        T FromPage();

        void Navigate(T FromPage);

        bool IsNavigate();

    }
}
