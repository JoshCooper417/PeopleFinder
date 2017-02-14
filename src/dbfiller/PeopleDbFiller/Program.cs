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
            var dataContext = new PersonDataContext();
            dataContext.Log = new DebugWriter();
            dataContext.Persons.DeleteAllOnSubmit(dataContext.Persons.ToList());
            dataContext.SubmitChanges();

            var misparIshi = 1000001;
            var numberAdded = 0;
            var totalSubmits = 0;
            GIVEN_NAMES.ForEach(givenName => {
                FAMILY_NAMES.ForEach(familyName =>
                {
                    JOBS.ForEach(job => {
                        DARGAS.ForEach(darga => {
                            var person = new Person();
                            person.MisparIshi = misparIshi.ToString();
                            person.Mail = misparIshi + "@iaf.il";
                            person.GivenName = givenName;
                            person.Surname = familyName;
                            person.JobTitle = job;
                            person.JobTitleSearchable = job.Replace("\"", "");
                            person.Darga = darga;
                            person.Tags = 1;
                            dataContext.Persons.InsertOnSubmit(person);
                            misparIshi++;
                            if (++numberAdded > 400) {
                                Console.WriteLine("Submitting #" + ++totalSubmits);
                                numberAdded = 0;
                                dataContext.SubmitChanges();
                            }
                        });
                    });
                });
            });
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
