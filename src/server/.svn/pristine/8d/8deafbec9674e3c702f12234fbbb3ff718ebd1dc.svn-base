using WebAPI.DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web;

namespace WebAPI.Controllers
{
    public class ValuesController : ApiController
    {
        // GET api/values
        public IEnumerable<object> Get([FromUri]Request request)
        {
            switch (request.Type)
            {
                case "query":
                    return new InputHandler().GetPersons(
                        request.Data, request.ShowAll);
                case "initialmetadata":
                    return InitialMetadataGenerator.GetInitialMetadata();
                case "adminaddtag":
                    return AdminTagAdder.AddTag(request.Data, request.TagForAnyone);
                case "addtag":
                    var new_tag =
                        TagAdder.AddTagForPerson(request.MisparIshi, request.Data);
                    return new object[] { new { new_tag } };
            }
            return new object[] { };
        }

        // The following exist only for debugging purposes.
        // GET api/values/name
        public IEnumerable<object> Get(string value, string showAll)
        {
            switch (showAll)
            {
                case "galwantsdata":
                    return LogDataForGal.GetLogDataForGal(value);
                case "0":
                    return new InputHandler().GetPersons(
                        value, false);
                case "1":
                    return new InputHandler().GetPersons(
                        value, true);
            }
            return new object[] { };
        }

        // POST api/values
        public void Post([FromBody]Request request)
        {
            switch (request.Type)
            {
                case "whatido":
                    WhatIDoAdder.AddWhatIDo(
                        request.MisparIshi, request.Data);
                    return;
                case "log":
                    Logger.Log(request.Logs);
                    return;
                case "deletetag":
                    TagAdder.DeleteTagForPerson(request.MisparIshi, request.Data);
                    return;
            }
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

    public class Request
    {
        public string Type { get; set; }
        public string MisparIshi { get; set; }
        public string Data { get; set; }
        public LogData Logs { get; set; }
        public bool ShowAll { get; set; }
        public bool TagForAnyone { get; set; }
    }
}