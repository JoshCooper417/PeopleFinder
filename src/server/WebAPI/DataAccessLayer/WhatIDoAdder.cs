using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebAPI.Models;

namespace WebAPI.DataAccessLayer
{
    public class WhatIDoAdder
    {
        public static void AddWhatIDo(string misparIshi, string value)
        {
            var dataContext = new PersonDataContext();

            var personsFromDb = dataContext.Persons
                .Where(person => person.MisparIshi.Equals(misparIshi))
                .ToList();
            if (personsFromDb.Count() != 1)
            {
                return;
            }

            var personFromDb = personsFromDb.First();
            
            if (!CurrentMisparIshi.IsCurrentUserOrAdmin(
                personFromDb.MisparIshi))
            {
                return;
            }

            personFromDb.WhatIDo = value;
            dataContext.SubmitChanges();
        }
    }

    
}