using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MySelenium {
    public class TimeWait {

        public TimeSpan TimeToOut { get; private set; } = TimeSpan.FromSeconds(10);

        public TimeSpan FreQuency { get; private set; } = TimeSpan.FromMilliseconds(50);


        public static TimeWait Get(TimeSpan timeToTimeOut, TimeSpan frequency) {
            return new TimeWait() {
                TimeToOut = timeToTimeOut,
                FreQuency = frequency
            };
        }

        public static TimeWait Get(TimeSpan timeToTimeOut) {
            return new TimeWait() {
                TimeToOut = timeToTimeOut
            };
        }

        public static TimeWait Get(double secondsToTimeOut) {
            return Get(secondsToTimeOut, 100);
        }

        public static TimeWait Get(double secondsToTimeOut,double Milliseconds) {
            return new TimeWait() {
                TimeToOut = TimeSpan.FromSeconds(secondsToTimeOut),
                FreQuency = TimeSpan.FromMilliseconds(Milliseconds)
            };
        }


        public static TimeWait Default {
            get {
                return new TimeWait();
            }
        }

        public void Until(Func<bool> condition) {
            until(null, condition);
        }

        public void RunUntil(Action action, Func<bool> condition) {
            until(action, condition);
        }

        public T GetUntil<T>(Func<T> GetMethod, Func<T, bool> condition) {
            return until(GetMethod, condition);
        }

        public T Get<T>(Func<T> GetMethod) {
            return until(GetMethod, t => t != null);
        }

       

        private void until(Action action, Func<bool> condition) {
            var ts = new CancellationTokenSource();
            CancellationToken ct = ts.Token;
            var tempTime = TimeToOut;
            var task = Task.Factory.StartNew( () => {
                while (true) {
                    action?.Invoke();
                    Task.Delay(FreQuency.Milliseconds).Wait();
                    if (condition() || ct.IsCancellationRequested)
                        return;
                }
            }, ct);
            while (true) {
                if (task.IsCompleted) {
                    return;
                }
                Task.Delay(FreQuency.Milliseconds).Wait();
                tempTime = tempTime.Subtract(FreQuency);
                if (tempTime <= FreQuency) {
                    ts.Cancel();
                    throw new TimeoutException();
                }                
            }
        }

        private T until<T>(Func<T> getMethod, Func<T, bool> condition) {
            var tempTime = TimeToOut;
            var ts = new CancellationTokenSource();
            CancellationToken ct = ts.Token;
            var task = Task.Factory.StartNew(() => {
                while (true) {
                    T item = getMethod();
                    Task.Delay(FreQuency.Milliseconds).Wait();
                    if (item != null) {
                        if (condition == null || condition?.Invoke(item) == true)
                            return item;
                    }
                    if (ct.IsCancellationRequested)
                        return default(T);
                }
            },ct);


            while (true) {
                if (task.IsCompleted) {
                    return task.Result;
                }
                tempTime = tempTime.Subtract(FreQuency);
                Task.Delay(FreQuency.Milliseconds).Wait();
                if (tempTime <= FreQuency) {
                    if (task.IsCompleted) {
                        return task.Result;
                    } else {
                        ts.Cancel();
                        throw new TimeoutException();
                    }
                }
            }
        }







    }



}
