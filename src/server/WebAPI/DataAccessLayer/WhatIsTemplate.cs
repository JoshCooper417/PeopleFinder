using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace WebAPI.DataAccessLayer
{
    public class WhatIsTemplate : ITemplate
    {
        private static string key1Lookup = "מה ה";
        private static Regex key2Lookup = new Regex(" של ");

        private string lookupType { get; set; } // E.g., מספר
        private string lookupValue { get; set; } // E.g., מספר

        public Regex MatchingRegex()
        {
            return new Regex(@"מה ה.* של .+");
        }

        public DbRequest MakeDbRequest(string input, bool shouldShowAll)
        {
            // input comes in as "מה המספר של עומר"

            input = input.Replace(key1Lookup, "");
            // Now input is "מספר של עומר"

            var inputSplitOnKey2 = key2Lookup.Split(input, 2);
            // inputSplitOnKey2 is ["עומר" ,"מספר" ]
            if (inputSplitOnKey2.Length != 2)
            {
                return null;
            }

            lookupType = inputSplitOnKey2[0];

            lookupValue = inputSplitOnKey2[1].TrimEnd('?');
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

    }
}