using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebAPI.DataAccessLayer
{
    // Responsible for creating the initial metadata object. It will contain a
    // single object with the following fields:
    // is_admin
    // tags_to_add
    // messages
    public class InitialMetadataGenerator
    {
        public static IEnumerable<object> GetInitialMetadata()
        {
            return new object[] {
                new {
                    is_admin = CurrentMisparIshi.IsAdmin(),
                    messages = AlertGenerator.GenerateSingleAlertObject(),
                    tags_to_add = TagGetter.GetTags()
                }
            };
        }
    }
}