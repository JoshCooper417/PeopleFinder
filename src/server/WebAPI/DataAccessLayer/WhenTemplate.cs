using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace WebAPI.DataAccessLayer
{
    public class WhenTemplate : ITemplate
    {
        private static string key1Lookup = "מתי ";

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
            return new Regex(@"מתי .+ .+");
        }

        public DbRequest MakeDbRequest(string input, bool shouldShowAll)
        {
            // input comes in as "מתי עומר נולד"
            // Now input is "עומר נולד"
            input = input.Replace(key1Lookup, "");
            // queryParts is ["עומר" ,"נולד" ]
            string[] queryParts = input.Split(' ');

            // try to find a relevant lookup field based on the last query parts
            List<string> fields = null;
            int fieldIndex = queryParts.Length - 1;
            for (int i = queryParts.Length - 1; i > 0; i--)
            {
                fields = FieldsAliases.getJsonFields(queryParts[i]);

                if (fields != null && fields.Count > 0)
                {
                    // the recieved field is relevant for this template
                    if (fields[0] == FieldsAliases.BIRTHDAY ||
                       fields[0] == FieldsAliases.END_OF_SERVICE)
                    {
                        lookupField = queryParts[i];
                        fieldIndex = i;
                    }
                }
            }

            // if a field has been found, set the db request
            if (lookupField != null)
            {
                string value = "";
                // build the lookup value from all query parts except of the last one
                for (int i = 0; i < fieldIndex; i++)
                {
                    value += " " + queryParts[i];
                }

                lookupValue = value;

                var dbRequest = new DbRequest(lookupValue, shouldShowAll);
                return dbRequest.IsValid ? dbRequest : null;
            }
            else
            {
                return null;
            }
        }

        public List<PersonJsonWrapper> ProcessPersonJsons(List<PersonJsonWrapper> personJsons)
        {
            return personJsons;
        }

        public object AddMetadata()
        {
            return new
            {
                query = "זה התאריך בו " + lookupValue + " " + lookupField + ":"
            };
        }
    }
}
