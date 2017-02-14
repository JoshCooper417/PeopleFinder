using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace WebAPI.DataAccessLayer
{
    public class EnglishToHebrew
    {
        private static Dictionary<char, char> hebrewEnglishDict =
            createHebrewToEnglishDicionary();

        public static string maybeConvertToHebrew(string input) {
            if (!Regex.IsMatch(input, @"^[\.,a-zA-Z0-9]")) {
                return input;
            }
            var updatedChars = input.ToCharArray().Select(
                c => hebrewEnglishDict.ContainsKey(c) ? hebrewEnglishDict[c] : c);
            return new String(updatedChars.ToArray());
        }

        private static Dictionary<char, char> createHebrewToEnglishDicionary()
        {
            var dictionary = new Dictionary<char, char>();
            dictionary.Add('q', '/');
            dictionary.Add('w', '\'');
            dictionary.Add('e', 'ק');
            dictionary.Add('r', 'ר');
            dictionary.Add('t', 'א');
            dictionary.Add('y', 'ט');
            dictionary.Add('u', 'ו');
            dictionary.Add('i', 'ן');
            dictionary.Add('o', 'ם');
            dictionary.Add('p', 'פ');
            dictionary.Add('a', 'ש');
            dictionary.Add('s', 'ד');
            dictionary.Add('d', 'ג');
            dictionary.Add('f', 'כ');
            dictionary.Add('g', 'ע');
            dictionary.Add('h', 'י');
            dictionary.Add('j', 'ח');
            dictionary.Add('k', 'ל');
            dictionary.Add('l', 'ך');
            dictionary.Add(';', 'ף');
            dictionary.Add('z', 'ז');
            dictionary.Add('x', 'ס');
            dictionary.Add('c', 'ב');
            dictionary.Add('v', 'ה');
            dictionary.Add('b', 'נ');
            dictionary.Add('n', 'מ');
            dictionary.Add('m', 'צ');
            dictionary.Add(',', 'ת');
            dictionary.Add('.', 'ץ');
            dictionary.Add('\'', ',');
            dictionary.Add('Q', '/');
            dictionary.Add('W', '\'');
            dictionary.Add('E', 'ק');
            dictionary.Add('R', 'ר');
            dictionary.Add('T', 'א');
            dictionary.Add('Y', 'ט');
            dictionary.Add('U', 'ו');
            dictionary.Add('I', 'ן');
            dictionary.Add('O', 'ם');
            dictionary.Add('P', 'פ');
            dictionary.Add('A', 'ש');
            dictionary.Add('S', 'ד');
            dictionary.Add('D', 'ג');
            dictionary.Add('F', 'כ');
            dictionary.Add('G', 'ע');
            dictionary.Add('H', 'י');
            dictionary.Add('J', 'ח');
            dictionary.Add('K', 'ל');
            dictionary.Add('L', 'ך');
            dictionary.Add('Z', 'ז');
            dictionary.Add('X', 'ס');
            dictionary.Add('C', 'ב');
            dictionary.Add('V', 'ה');
            dictionary.Add('B', 'נ');
            dictionary.Add('N', 'מ');
            dictionary.Add('M', 'צ');
            return dictionary;
        }
    }
}