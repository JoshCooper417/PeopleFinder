using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebAPI.DataAccessLayer
{
    public class AlertGenerator
    {
        public static IEnumerable<object> GenerateAlert()
        {
            return new object[] { GenerateSingleAlertObject() };
        }
        
        public static object GenerateSingleAlertObject()
        {
            return new { alert = "הנך משתמש/ת בגרסה לא מעודכנת. כדי שהמערכת תעבוד למיטב יכולותיה, נא למחוק את הגרסה הקיימת ולהתקין מחדש לפי ההוראות כאן: http://app0111laka01.iaf/PplFndr" };
        }
    }
}