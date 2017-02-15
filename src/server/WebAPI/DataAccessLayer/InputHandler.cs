using System;
using System.Collections.Generic;
using System.Data.Objects;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using WebAPI.Models;

namespace WebAPI.DataAccessLayer
{
    public class InputHandler
    {
        // (1) Validate the query and return an empty list if the query is invalid.
        // (2) Convert the input query to a SQL query.
        // (3) Post-process each SQL output to convert it to the JSON the client
        //     expects.
        // (4) Return a list containing a Metadata object and the list of JSON
        //     objects for each person.
        public IEnumerable<object> GetPersons(string input, bool shouldShowAll)
        {
            var translatedInput = EnglishToHebrew.maybeConvertToHebrew(input);

            DbRequest dbRequest = null;
            ITemplate templateToUse = null;
            foreach (ITemplate template in Templates.CreateTemplateList())
            {
                if (!template.MatchingRegex().IsMatch(translatedInput))
                {
                    continue;
                }
                dbRequest = template.MakeDbRequest(translatedInput, shouldShowAll);
                if (dbRequest == null)
                {
                    continue;
                }
                templateToUse = template;
                break;
            }

            if (dbRequest == null || templateToUse == null) {
                return new object[] { };
            }

            var returnObjects = createMatchingPersonsList(dbRequest, templateToUse);
            var metadataObject =
                createMetadataObject(templateToUse, returnObjects, dbRequest, input, translatedInput);
            returnObjects.Insert(0, metadataObject);

            return returnObjects;
        }

        // Returns null if we should not make a request.
        private DbRequest createDbRequest(string input, bool shouldShowAll)
        {
            return new DbRequest(input, shouldShowAll);
        }

        private List<object> createMatchingPersonsList(DbRequest dbRequest, ITemplate templateToUse)
        {
            // Post process the selected entities; adjusting and renaming some fields.
            var personJsonsFromDb = DbReader.GetPersonsFromDb(dbRequest)
                .Select(person => new PersonJsonWrapper(person, templateToUse.getLookupField()))
                .ToList();

            var processedPersonJsons = templateToUse.ProcessPersonJsons(personJsonsFromDb);

            return personJsonsFromDb
                .OrderByDescending(person => person.IsMe)
                .ThenByDescending(person => person.Mail)
                .ThenBy(person => person.Name)
                .Select(person => person.JsonFromClient)
                .ToList();
        }


        private object createMetadataObject(ITemplate template,
            IEnumerable<object> persons,
            DbRequest dbRequest, string originalInput, string translatedInput)
        { 
            // TODO(josh): This is a cheating heuristic that is occasionally
            // incorrect.
            // Fix - Request one more number than you're displaying. 
            var listWasCutOff =
                !dbRequest.ShouldShowAll
                && persons.Count() == dbRequest.NumberToTake;

            return new {
                //query = template.MetdataDisplayValue(),
                templateData = template.AddMetadata(),
                shouldShowSeeMore = listWasCutOff,
                isAdmin = CurrentMisparIshi.IsAdmin(),
                originalInput = originalInput,
                translatedInput = translatedInput,
                nonAdminsCanAddTags = false
            };
        }
    }
}