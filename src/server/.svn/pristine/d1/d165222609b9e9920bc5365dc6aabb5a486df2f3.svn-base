using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebAPI.Models;

namespace WebAPI.DataAccessLayer
{
    public class TagAdder
    {
        public static object AddTagForPerson(string misparIshi, string tag)
        {
            return new PersonAndTag(misparIshi, tag).Add();
        }

        public static bool DeleteTagForPerson(string misparIshi, string tag)
        {
            return new PersonAndTag(misparIshi, tag).Delete();
        }
    }

    class PersonAndTag
    {
        private PersonDataContext dataContext;
        private Person personFromDb;
        private long tagPrime;
        private bool isValid;

        public PersonAndTag(string misparIshi, string tag)
        {
            if (!CurrentMisparIshi.IsCurrentUserOrAdmin(misparIshi)
                || !TagToPrimeDictionary.TAG_TO_PRIME.ContainsKey(tag))
            {
                this.isValid = false;
                return;
            }
            this.tagPrime = TagToPrimeDictionary.TAG_TO_PRIME[tag];

            this.dataContext = new PersonDataContext();

            var personsFromDb = dataContext.Persons
                .Where(person => person.MisparIshi.Equals(misparIshi))
                .ToList();
            if (personsFromDb.Count() != 1)
            {
                this.isValid = false;
                return;
            }

            this.personFromDb = personsFromDb.First();

            this.isValid = true;
        }

        public object Add()
        {
            if (!this.isValid || tagAlreadyExists())
            {
                return false;
            }
            this.personFromDb.Tags *= tagPrime;
            this.dataContext.SubmitChanges();
            return TagToPrimeDictionary.PRIME_TO_TAG[tagPrime].ToJson();
        }

        public bool Delete()
        {
            if (!this.isValid || !tagAlreadyExists())
            {
                return false;
            }
            this.personFromDb.Tags /= tagPrime;
            this.dataContext.SubmitChanges();
            return true;
        }

        private bool tagAlreadyExists()
        {
            return this.personFromDb.Tags % tagPrime == 0;
        }
    }
}