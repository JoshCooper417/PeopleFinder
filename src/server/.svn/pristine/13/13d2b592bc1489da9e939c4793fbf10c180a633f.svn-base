using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebAPI.DataAccessLayer
{
    public class TermExtractions
    {
        public static string BIRTHDAY = "BIRTHDAY";

        // Defines a list of substrings that we replace with our own values. This is
        // used to support EasterEggs like "birthdays" and phrases that have multiple
        // words that should be kept together.
        public static IDictionary<string, string> TERMS =
            new Dictionary<string, string> (StringComparer.OrdinalIgnoreCase) {
            { "יום הולדת", BIRTHDAY },
            { "ימי הולדת", BIRTHDAY },
            { "יומולדת", BIRTHDAY },
            { "birthday", BIRTHDAY },
            { "נןראיגשט", BIRTHDAY }
        };
    }
}