using WebAPI.DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace WebAPI.Controllers
{
    public class ValuesController : ApiController
    {
        // GET api/values
        public IEnumerable<object> Get()
        {
            return new object[] { };
        }

        // Returns a list of objects. The first object contains metadata about the response.
        // GET api/values/name
        public IEnumerable<object> Get(string value, string showAll)
        {
            if (value.Length < 3)
            {
                return new object[] {};
            }
            PersonRetriever personRetriever = new PersonRetriever();
            var shouldShowAll = showAll.Equals("1");
            return personRetriever.GetPerson(
                EnglishToHebrew.maybeConvertToHebrew(value), shouldShowAll);
        }

        // POST api/values
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        public void Delete(int id)
        {
        }
    }
}