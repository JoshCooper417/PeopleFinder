using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PeopleDbFiller
{
    class Program
    {
        static Random rnd = new Random();

        static List<string> GIVEN_NAMES = new List<string> {
            "ג'וש",
            "עומר",
            "חי",
            "מאי",
            "יניב",
            "שרון",
            "אליס",
            "שון",
            "אד",
            "לקסה",
            "עדי",
            "אדם",
            "אוהב",
            "אופיר"
        };
        static List<string> FAMILY_NAMES = new List<string> {
            "קופר",
            "יובל",
            "כהן",
            "ריבלין",
            "זימגר",
            "אורט",
            "דוידוביץ'",
            "דיננברג",
            "קרול",
            "לב",
            "ארז",
            "הרצוג",
            "ברבי",
            "ארליך"
        };
        static List<string> JOBS = new List<string> {
            "תוא\"ר/חדשנות",
            "תקשוב/לוטם",
            "כללי/דוברות",
        };
        static List<string> DARGAS = new List<string> {
            "רב\"ט",
            "סרן"
        };

        public static void Main(string[] args)
        {
            var numberAdded = 0;
            var totalSubmits = 0;

            var dataContext = new PersonDataContext();
            dataContext.Log = new DebugWriter();
            dataContext.Persons.DeleteAllOnSubmit(dataContext.Persons.ToList());
            dataContext.SubmitChanges();

            string path = @"../../people.txt";
            string[] lines = System.IO.File.ReadAllLines(path);

            foreach (string line in lines)
            {
                var l = line.Split(',');
                var misparIshi = l[0].Trim();
                var givenName = l[1].Trim();
                var familyName = l[2].Trim();
                var job = l[3].Trim();
                var min = l[4].Trim();

                // random darga
                int r = rnd.Next(DARGAS.Count);
                var darga = DARGAS[r];

                // random phone number
                var thilit = new List<char>() { '4', '2', '0' };
                r = rnd.Next(thilit.Count);
                var randThilit = thilit[r];

                int _min = 1000000;
                int _max = 9999999;
                var randNum = rnd.Next(_min, _max).ToString();

                var workPhone = $"05{randThilit}{randNum}";

                var person = new Person();
                person.WorkPhone = workPhone;
                person.Mobile = workPhone;
                person.MisparIshi = misparIshi.ToString();
                person.Mail = misparIshi + "@iaf.il";
                person.GivenName = givenName;
                person.Surname = familyName;
                person.JobTitle = job;
                person.JobTitleSearchable = job.Replace("\"", "");
                person.Darga = darga;
                person.Tags = 1;
                dataContext.Persons.InsertOnSubmit(person);
                if (++numberAdded > 30)
                {
                    Console.WriteLine("Submitting #" + ++totalSubmits);
                    numberAdded = 0;
                    dataContext.SubmitChanges();
                }
            }

            //var misparIshi = 1000001;
            //var numberAdded = 0;
            //var totalSubmits = 0;
            //GIVEN_NAMES.ForEach(givenName => {
            //    FAMILY_NAMES.ForEach(familyName =>
            //    {
            //        JOBS.ForEach(job => {
            //            DARGAS.ForEach(darga => {
            //                var person = new Person();
            //                person.MisparIshi = misparIshi.ToString();
            //                person.Mail = misparIshi + "@iaf.il";
            //                person.GivenName = givenName;
            //                person.Surname = familyName;
            //                person.JobTitle = job;
            //                person.JobTitleSearchable = job.Replace("\"", "");
            //                person.Darga = darga;
            //                person.Tags = 1;
            //                dataContext.Persons.InsertOnSubmit(person);
            //                misparIshi++;
            //                if (++numberAdded > 400) {
            //                    Console.WriteLine("Submitting #" + ++totalSubmits);
            //                    numberAdded = 0;
            //                    dataContext.SubmitChanges();
            //                }
            //            });
            //        });
            //    });
            //});
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
