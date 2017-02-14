using System;
using System.Collections.Generic;
using System.Data.Objects;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using WebAPI.Models;

namespace WebAPI.DataAccessLayer
{
    public class PersonRetriever
    {

        private static Regex isNumberRegex = new Regex(@"^\d+$");
        private static int numberToShowInFirstTry = 15;

        private static Stopwatch stopwatch = new Stopwatch();

        public IEnumerable<object> GetPerson(string input, bool shouldShowAll)
        {
            stopwatch.Start();
            var dataContext = new PersonDataContext();
            dataContext.Log = new DebugWriter();

            // Look at strings in descending size order in the hope that the strings with the fewest
            // results will come up first making each subsequent query pool smaller.
            var substrings = input.Split(' ').OrderByDescending(s => s.Length);
            IEnumerable<Person> matchingPersons = new List<Person>();
            var alreadyChecked = false;

            foreach (var substring in substrings)
            {
                if (alreadyChecked && matchingPersons.Count() == 0)
                {
                    // If here that means we already have no matches, so no reason to do anything else.
                    break;
                }

                var isNumber = isNumberRegex.IsMatch(substring);

                matchingPersons =
                    (alreadyChecked ? matchingPersons : dataContext.Persons).Where(person =>
                        isNumber ?
                            person.Mobile.Contains(substring)
                            || person.MisparIshi.Contains(substring)
                            || person.WorkPhone.Contains(substring)
                                : person.GivenName.Contains(substring)
                                || person.JobTitle.Contains(substring)
                                || person.LongWorkTitle.Contains(substring)
                                || person.AlternateName.Contains(substring)
                                || person.Department.Contains(substring)
                                || person.Company.Contains(substring)
                                || person.Location.Contains(substring)
                        );

                alreadyChecked = true;
            }

            var numberToTake = shouldShowAll ? int.MaxValue : numberToShowInFirstTry;
            var listWasCutOff = matchingPersons.Count() > numberToTake;

            var returnObject = new List<object>();
            returnObject.Add(createMetadataObject(listWasCutOff));

            matchingPersons.OrderBy(person => person.GivenName).Take(numberToTake)
                .ToList().ForEach(person => { returnObject.Add(createPerson(person)); });

            stopwatch.Stop();
            Debug.WriteLine(stopwatch.Elapsed);
            stopwatch.Reset();

            return returnObject;
        }


        private object createPerson(Person person)
        {
            return new
            {
                name = person.GivenName,
                office = person.Location,
                mail = person.Mail,
                mobile = person.Mobile,
                job_title = person.JobTitle,
                work_phone = person.WorkPhone
            };
        }

        private object createMetadataObject(bool listWasCutOff)
        {
            var shouldShowSeeMore = listWasCutOff;
            return new { shouldShowSeeMore };
        }
    }

    class DebugWriter : System.IO.TextWriter
    {
        public override void Write(string value)
        {
            Debug.Write(value);
        }

        public override void Write(char[] buffer, int index, int count)
        {
            Debug.Write(new String(buffer, index, count));
        }

        public override System.Text.Encoding Encoding
        {
            get { return System.Text.Encoding.Default; }
        }
    }
}