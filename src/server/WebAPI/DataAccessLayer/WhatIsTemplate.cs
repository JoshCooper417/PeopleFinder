using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace WebAPI.DataAccessLayer
{
    public class WhatIsTemplate : ITemplate
    {
        private static string key1Lookup = "מה ";
        private static Regex key2Lookup = new Regex(" של ");

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
            return new Regex(@"מה .* של .+");
        }

        public DbRequest MakeDbRequest(string input, bool shouldShowAll)
        {
            input = input.TrimEnd('?');
            // input comes in as "מה המספר של עומר"

            input = input.Replace(key1Lookup, "");
            // Now input is "מספר של עומר"

            var inputSplitOnKey2 = key2Lookup.Split(input, 2);
            // inputSplitOnKey2 is ["עומר" ,"מספר" ]
            if (inputSplitOnKey2.Length != 2)
            {
                return null;
            }

            lookupField= inputSplitOnKey2[0];

            
            lookupValue = inputSplitOnKey2[1].TrimEnd(' ');
            lookupValue = inputSplitOnKey2[1].TrimStart(' ');
            var dbRequest = new DbRequest(lookupValue, shouldShowAll);
            return dbRequest.IsValid ? dbRequest : null;
        }

        public List<PersonJsonWrapper> ProcessPersonJsons(List<PersonJsonWrapper> personJsons)
        {
            return personJsons;
        }

        public string MetdataDisplayValue()
        {
            return lookupValue;
        }

        public object AddMetadata()
        {
            return new
            {
                query = "הנה " + lookupField + " של " + lookupValue + ":"
            };
        }
    }
}