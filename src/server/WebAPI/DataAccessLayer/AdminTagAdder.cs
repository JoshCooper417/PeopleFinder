using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebAPI.Models;

namespace WebAPI.DataAccessLayer
{
    public class AdminTagAdder
    {
        public static IEnumerable<object> AddTag(string tagToAdd, bool isTagForAnyone)
        {           
            if (!CurrentMisparIshi.IsAdmin())
            {
                return createResponseObject(
                    "You're not an admin, what are you doing here??");
            }

            if (tagToAdd.Contains(" "))
            {
                return createResponseObject(
                    "אסור להוסיף תגים עם רווחים");
            }
            
            
            var nextPrime = TagToPrimeDictionary.GetNextPrime();
            if (nextPrime == -1)
            {
                return createResponseObject(
                    "יש 10,000 תגים ויותר מזה לא נתמך");
            }

            
            var dataContext = new PersonDataContext();
            var alreadyExistingTags =
                dataContext.TagPrimes.Where(tag => tag.Tag.Equals(tagToAdd));
            if (alreadyExistingTags.Count() > 0)
            {
                return createResponseObject(
                    String.Format("התג {0} כבר קיים", tagToAdd));
            }

            var newTagPrime = new TagPrime();
            newTagPrime.PrimeId = nextPrime;
            newTagPrime.Tag = tagToAdd;
            newTagPrime.AllowNonAdminsToAdd = isTagForAnyone;
            
            dataContext.TagPrimes.InsertOnSubmit(newTagPrime);
            dataContext.SubmitChanges();

            TagToPrimeDictionary.ResetTagToPrimeDictionaries();
            
            return createResponseObject(
                String.Format("התג {0} התווסף בהצלחה", tagToAdd));
        }

        private static IEnumerable<object> createResponseObject(string response) {
            return new object[] { new  { response } };
        }
    }
}