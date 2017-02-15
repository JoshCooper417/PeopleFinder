using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace WebAPI.DataAccessLayer
{
    // Wraps the data needed to make a request to a DB
    public class DbRequest
    {
        private static Regex isNumberRegex = new Regex(@"^\d+-?\d+$");

        private static int NUMBER_TO_SHOW_IN_FIRST_TRY = 5;

        public List<string> StandardInputValues { get; set; }
        public long Tags { get; set; }
        public int NumberToTake { get; set; }
        public bool ShouldShowAll { get; set; }
        public bool IsValid { get; set; }
        public bool IsOnlyNumbers { get; set; }

        public DbRequest(string input, bool shouldShowAll)
        {
            var substrings = InputCleaner.ToCleanInputWords(input);

            this.IsValid = substrings.Count() > 0;
            this.NumberToTake = shouldShowAll ?
                int.MaxValue : NUMBER_TO_SHOW_IN_FIRST_TRY;
            this.ShouldShowAll = shouldShowAll;
            this.IsOnlyNumbers =
                substrings.All(substring => isNumberRegex.IsMatch(substring));

            this.StandardInputValues = new List<string>();
            this.Tags = 1; // Default value.
            substrings.ToList().ForEach(substring => {
                var tagId = maybeGetTagId(substring);
                if (tagId != -1) {
                    this.Tags *= tagId;
                } else {
                    this.StandardInputValues.Add(substring);
                }
            });
        }

        // Returns a tag ID if the string starts with # and we have a tag with this name.
        // Returns -1 otherwise.
        private long maybeGetTagId(string inputWord)
        {
            if (!inputWord.StartsWith("#"))
            {
                return -1;
            }
            var tag = inputWord.Substring(1);
            if (!TagToPrimeDictionary.TAG_TO_PRIME.ContainsKey(tag))
            {
                return -1;
            }
            return TagToPrimeDictionary.TAG_TO_PRIME[tag];
        }
    }
}