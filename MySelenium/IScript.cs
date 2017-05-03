using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MySelenium {
    public interface IScript<T> {
        void Run(T parameter);
    }

    public interface IScript {
        void Run(Parameter parameter);
    }

    public abstract class BaseScript<T> : IScript<T>, IScript where T:Parameter {
        public abstract void Run(T parameter);

        void IScript.Run(Parameter parameter) {
            Run((T)parameter);
        }
    }


    public abstract class WebScript<T> : BaseScript<T> where T:Parameter {

        protected IWebDriver driver;

        public WebScript() {
            driver = new ChromeDriver();
            driver.Manage().Window.Maximize();
        }
    }
}
