using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace WebAPI.DataAccessLayer
{
    public class TillWhenTemplate : ITemplate
    {
        private static string key1Lookup = "עד מתי ";

        private string lookupField; // E.g., מספר
        private string lookupValue; // E.g., מספר

        public string getLookupValue()
        {
            return lookupValue;
        }

        public string getLookupField()
        {
            return lookupField;
        }

        public Regex MatchingRegex()
        {
            return new Regex(@"עד מתי .+");
        }

        public DbRequest MakeDbRequest(string input, bool shouldShowAll)
        {
            // input comes in as "עד מתי שמעון"
            // Now input is "שמעון"
            input = input.Replace(key1Lookup, "");

            lookupField = "תאריך שחרור";
            
            // queryParts is ["שמעון" ]
            string[] queryParts = input.Split(' ');

            // build the lookup value from all query parts except of the last one
            string value = "";
            for (int i = 0; i < queryParts.Length; i++)
            {
                value += ' ' + queryParts[i];
            }
            value.TrimStart(' ');

            lookupValue = value;

            var dbRequest = new DbRequest(lookupValue, shouldShowAll);
            return dbRequest.IsValid ? dbRequest : null;
        }

        public List<PersonJsonWrapper> ProcessPersonJsons(List<PersonJsonWrapper> personJsons)
        {
            return personJsons;
        }

        public object AddMetadata()
        {
            return new
            {
                query = "יש צחוקים ויש חלאס"
            };
        }
    }
}
