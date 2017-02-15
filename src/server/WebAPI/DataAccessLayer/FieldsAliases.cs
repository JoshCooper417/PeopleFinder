using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebAPI.DataAccessLayer
{
    public class FieldsAliases
    {
        private static Dictionary<string, List<string>> lookupTypesMap = null;

        public const string MISPAR_ISHI = "מספר אישי";
        public const string FIRST_NAME = "שם פרטי";
        public const string DISPLAY_NAME = "שם";
        public const string MOBILE_PHONE = "טלפון נייד";
        public const string WORK_PHONE = "טלפון";
        public const string PICTURE = "תמונה";
        public const string BIRTHDAY = "תאריך לידה";
        public const string TAGS = "תיוגים";
        public const string LAST_NAME = "שם משפחה";
        public const string JOB = "תפקיד";
        public const string RANK = "דרגה";
        public const string SEX = "מין";
        public const string MAIL = "מייל";
        public const string FAX = "פקס";
        public const string END_OF_SERVICE = "תת\"ש";

        /**
         * get the json fields name based on native language aliases
         * */
        public static List<string> getJsonFields(string nativeLangFieldName)
        {
            List<string> jsonFields = new List<string>();

            initFieldsMap();

            // for each field name key, check if there is a match with the native lang phrase
            foreach (string key in lookupTypesMap.Keys)
            {
                List<string> aliases = lookupTypesMap[key];

                foreach (string alias in aliases)
                {
                    if (nativeLangFieldName == alias || nativeLangFieldName == "ה" + alias)
                    {
                        jsonFields.Add(key);
                    }
                }
            }

            return jsonFields;
        }


        private static void initFieldsMap()
        {
            if (lookupTypesMap == null)
            {
                lookupTypesMap = new Dictionary<string, List<string>>();

                lookupTypesMap.Add(MISPAR_ISHI, new List<string>() { "מספר אישי", "מספר האישי", "מספרו האישי", "מספרה האישי" });
                lookupTypesMap.Add(FIRST_NAME, new List<string>() { "שם הפרטי", "שמו הפרטי", "שם פרטי", "שמה הפרטי" });
                lookupTypesMap.Add(DISPLAY_NAME, new List<string>() { "שם", "שמו", "קוראים", "שמה" });
                lookupTypesMap.Add(LAST_NAME, new List<string>() { "שמו המשפחתי", "שם משפחתו", "שם המשפחה", "שם משפחה", "שם משפחתה", "שמה המשפחתי" });
                lookupTypesMap.Add(MOBILE_PHONE, new List<string>() { "טלפון", "נייד", "מספר", "מספר טלפון", "פלאפון", "מספר הטלפון", "מספר הפלאפון", "סלולרי", "מספרו", "מספר הפלאפון", "טלפון נייד", "מספרה", "מיספרה", "מיספרו", "טלפונו", "טלפונה"});
                lookupTypesMap.Add(JOB, new List<string>() { "תפקיד", "תפקידו", "מקצוע", "שיבוץ", "מקצועו", "מקצועה", "תפקידה", "מיקצועו", "מיקצועה", "עובד", "עובדת", "משרת", "משרתת", "משובץ", "משובצת"});
                lookupTypesMap.Add(WORK_PHONE, new List<string>() { "טלפון", "טלפון במשרד", "טלפון המשרדי", "טלפון משרד", "טלפון המטכ\"לי", "מספר הטלפון", "מספר טלפון", "מספרו", "מספר", "מיספר", "מספרה", "מיספרו", "טלפונה", "טלפונו"});
                lookupTypesMap.Add(PICTURE, new List<string>() { "תמונה", "תמונת פרופיל", "תמונתו", "תמונתה", "תמונת הפקופיל"});
                lookupTypesMap.Add(RANK, new List<string>() { "דרגה", "דרגתו", "ותק", "וותק", "דרגתה"});
                lookupTypesMap.Add(BIRTHDAY, new List<string>() {"נברא" , "נבראה", "חוגג", "חוגגת","יום הולדת","יומהולדת", "נולד" , "נולדה","יומולדת", "יום ההולדת", "תאריך הלידה", "תאריך יום ההולדת", "תאריך לידה", "יום הולדתו", "יום הולדתה" });
                lookupTypesMap.Add(TAGS, new List<string>() { "תיוגים", "תגים", "מתויג", "מתוייג", "תג", "תיוג", "האשטאג", "#", "מתוייגת", "מתויגת" });
                lookupTypesMap.Add(MAIL, new List<string>() { "מייל", "כתובת המייל", "אימייל", "email", "דואר האלקטרוני", "דואר אלקטרוני", "דוא\"ל", "כתובת אימייל" });
                lookupTypesMap.Add(FAX, new List<string>() { "פקס", "מספר הפקס", "מספר פקס" });
                lookupTypesMap.Add(SEX, new List<string>() { "מין", "מגדר", "מינו", "מינה", "מגדרו", "מגדרה"});
                lookupTypesMap.Add(END_OF_SERVICE, new List<string>() { "משתחרר", "תאריך השחרור", "תאריך שחרור", "תאריך שחרורו", "משתחררת", "תאריך שחרורה" });
            } 
        }
    }
}