using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace WebAPI.DataAccessLayer
{
    public class ActionToTemplate : ITemplate
    {
        // חייג אל
        private static string[] noiseWordsLookup = new string[2] { "חייג", "אל"};

        private string lookupValue;
        private string lookupField;

        public string getLookupValue()
        {
            return lookupValue;
        }

        public string getLookupField()
        {
            return lookupField;
        }

        public DbRequest MakeDbRequest(string input, bool shouldShowAll)
        {
            // input comes in as "חייג אל עומר"

            foreach (string lookup in noiseWordsLookup)
            {
                input = input.Replace(lookup, "");
            }
            lookupValue = input;
            var dbRequest = new DbRequest(lookupValue, shouldShowAll);
            return dbRequest.IsValid ? dbRequest : null;
        }

        public Regex MatchingRegex()
        {
            return new Regex(@"חייג אל .*");
        }

        public object AddMetadata()
        {
            return new
            {
                query = lookupValue,
                action = "send"
            };
        }

        public List<PersonJsonWrapper> ProcessPersonJsons(List<PersonJsonWrapper> personJsons)
        {
            return personJsons;
        }

    }
}