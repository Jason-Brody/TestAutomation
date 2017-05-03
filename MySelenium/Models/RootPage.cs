using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;

namespace MySelenium.Models {
    public class RootPage : IWebPage {

        public object Parameter { get; set; } = null;
        public IWebDriver Driver { get; set; } = null;

        public bool IsNavigate() {
            return true;
        }

        public void Navigate(IWebPage FromPage) {
            
        }

        IWebPage IWebPage.FromPage() {
            throw new NotImplementedException();
        }
    }
}

