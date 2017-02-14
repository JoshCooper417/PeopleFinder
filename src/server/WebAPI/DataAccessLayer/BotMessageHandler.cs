using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebAPI.DataAccessLayer
{
    public class BotMessageHandler
    {
        private string input;

        public BotMessageHandler(string inputMessage) {
            this.input = inputMessage;
        }

        public IEnumerable<object> GetReply()
        {
            return new object[] {
                new {
                    message = input + input,
                    is_bot = true
                }
            };
        }
    }
}