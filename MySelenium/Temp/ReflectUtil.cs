using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MySelenium.Temp {
    public class ReflectUtil {



        public static void Reflect(object obj, Func<object, PropertyInfo, bool> isHandlered, Func<object, PropertyInfo, object> subItemGet) {
            Reflect(obj, (o, p) => {

                var isHand = isHandlered(o, p);
                if (!isHand) {
                    object subItem = subItemGet?.Invoke(obj, p);

                    if (subItem == null || subItem.GetType().GetTypeInfo().IsPrimitive || subItem.GetType() == typeof(string))
                        return;
                    Reflect(subItem, isHandlered, subItemGet);
                }
            });
        }

        public static void Reflect(object obj, Action<object, PropertyInfo> propertyHanler) {
            if (obj == null || propertyHanler == null)
                return;
            foreach (var prop in obj.GetType().GetTypeInfo().GetProperties()) {
                propertyHanler.Invoke(obj, prop);
            }
        }
    }
}
