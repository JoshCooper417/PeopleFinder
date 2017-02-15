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
        private static List<string> mailWordsLookup = new List<string>() {"mail", "שלח מייל", "שלח דואר"};
        private static List<string> chatWordsLookup = new List<string>() {"chat", "שלח הודעה", "הודע" };
        private static List<string> callWordsLookup = new List<string>() {"call", "התקשר", "חייג", "טלפן" };
        private static List<string> toWordsLookup = new List<string>() { "אל", "ל" };
        private static List<List<string>> actionWordsLookup = new List<List<string>>() { mailWordsLookup, chatWordsLookup, callWordsLookup };
        
        private string lookupValue { get; set; }
        private string lookupAction { get; set; }

        public DbRequest MakeDbRequest(string input, bool shouldShowAll)
        {
            string actionString;
            foreach (List<string> currWords in actionWordsLookup )
            {
                for (int i = 1; (i < currWords.Count) && (string.IsNullOrWhiteSpace(lookupAction)); i++)
                {
                    for (int j = 0; (j < toWordsLookup.Count) && (string.IsNullOrWhiteSpace(lookupAction)); j++)
                    {
                        actionString = currWords[i] + " " + toWordsLookup[j];
                        if (input.Contains(actionString))
                        {
                            lookupAction = currWords[0];
                            input = input.Replace(actionString, "");
                        }
                    }
                }
            }
            lookupValue = input;
            var dbRequest = new DbRequest(lookupValue, shouldShowAll);
            return dbRequest.IsValid ? dbRequest : null;
        }

        public Regex MatchingRegex()
        {
            string regex = @".+ (";
            for (int i = 0; i < toWordsLookup.Count; i++)
            {
                regex += toWordsLookup[i];
                if (i < toWordsLookup.Count - 1)
                    regex += "|";
            }
            regex += @").+";
            return new Regex(regex);
        }

        public object AddMetadata()
        {
            return new
            {
                query = lookupValue,
                action = lookupAction
            };
        }

        public List<PersonJsonWrapper> ProcessPersonJsons(List<PersonJsonWrapper> personJsons)
        {
            return personJsons;
        }
    }
}