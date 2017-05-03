using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MySelenium {

    public abstract class BasePage<T, T1> : BasePage<T> where T:class,IWebPage {
        public new T1 Parameter { get => (T1)base.Parameter; set => base.Parameter = value; }
    }


    public abstract class BasePage<T> : IWebPage<T>,IWebPage where T:class,IWebPage {

        public IWebDriver Driver { get; set; }

        public abstract T FromPage();
        public object Parameter { get; set; }
        

        public abstract bool IsNavigate();

        public abstract void Navigate(T FromPage);

        protected T1 checkNull<T1>(T1 obj) where T1:class {
            if (obj == null) {
                throw new ArgumentNullException(typeof(T1).Name);
            }
            return obj;
        }

        public void Navigate(IWebPage FromPage) {
            Navigate((T)FromPage);
        }

        IWebPage IWebPage.FromPage() {
            T fromPage = FromPage();
            return fromPage;
        }
    }



    




}
