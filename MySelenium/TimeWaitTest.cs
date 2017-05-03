using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MySelenium {
    public class TimeWaitTest {
        public static void test() {
            var dt = DateTime.Now;
            int i = 0;

            TimeWait.Default.RunUntil(() => TimeWait.Default.RunUntil(() => Console.WriteLine(i++), () => false), () => {
                Console.WriteLine($"-->{i++}");
                return false;
            });

            

           
           
            Console.ReadLine();
        }
    }
}
