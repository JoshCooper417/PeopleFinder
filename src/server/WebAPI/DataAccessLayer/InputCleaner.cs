using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace WebAPI.DataAccessLayer
{
    public class InputCleaner
    {

        // Trims the input from extraneous spaces and verifies validity.
        // Returns null if the input is invalid.
        // Also handles special terms that should be extracted together.
        // TODO(Josh): Clarify the division of responsibilities between
        // this class and DbRequest.cs.
        public static IEnumerable<string> ToCleanInputWords(string input)
        {
            // Trim and eliminate all occurences of consecutive spaces.
            input = input.Trim().ReplaceAll("  ", " ");

            var termsToRemoveInInput = new List<string>();
            TermExtractions.TERMS.Keys.ToList().ForEach(termToRemove =>
            {
                if (!input.Contains(termToRemove))
                {
                    return;
                }
                input = input.Without(termToRemove);
                termsToRemoveInInput.Add(TermExtractions.TERMS[termToRemove]);
            });

            // Remove all words that appear multiple times.
            var separatedWords = input.Split(' ')
                .Union(termsToRemoveInInput)
                .Where(word => word.Length > 0)
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .ToList()
                .MergeNumberWords();

            // Make sure there is at least one word with three characters.
            var hasOneWordWithEnoughCharacters = false;
            separatedWords.ForEach(word =>
            {
                hasOneWordWithEnoughCharacters |= word.Length >= 3;
            });

            return hasOneWordWithEnoughCharacters ? separatedWords : new List<string>();
        }
    }

    public static class Extensions
    {
        private static Regex isNumberRegex = new Regex(@"^\d+$");

        public static string Without(this string baseString, string substring)
        {
            return baseString.ReplaceAll(substring, "");
        }
        
        public static string ReplaceAll(
            this string baseString,
            string toReplace,
            string replaceWith)
        {
            while (baseString.Contains(toReplace))
            {
                baseString = baseString.Replace(toReplace, replaceWith);
            }
            return baseString;
        }

        // For every word in the list at index i such that the word at
        // index i + 1 is an integer and the word at index i is not
        // (e.g., "בח"א" folled by "8"), merge the strings at indices
        // i and i + 1 into one string.
        public static List<string> MergeNumberWords(
            this List<string> strings)
        {
            List<string> unmodifiedWords = new List<string>();
            List<string> mergedWords = new List<string>();
            for (var i = 0; i < strings.Count; i++)
            {
                // If we're always on the last word, add it as unmodified because
                // There is no next word to check.
                if (i == strings.Count - 1
                    // If this is a number or the next one is not a number, we
                    // haven't met the conditions.
                    || isNumberRegex.IsMatch(strings.ElementAt(i))
                    || !isNumberRegex.IsMatch(strings.ElementAt(i + 1))
                    )
                {
                    unmodifiedWords.Add(strings.ElementAt(i));
                    continue;
                }
                
                // If here, string i is not a number and string i + 1 is
                // so instead we add it as a merged word.
                mergedWords.Add(
                    String.Format("{0} {1}",
                    strings.ElementAt(i),
                    strings.ElementAt(i + 1)));
               
                // i + 1 is the number we just added so skip it.
                i++;

            }
            return mergedWords.Union(unmodifiedWords).ToList();
        }
    }
}