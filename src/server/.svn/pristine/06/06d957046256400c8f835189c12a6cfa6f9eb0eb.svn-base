using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebAPI.Models;

namespace WebAPI.DataAccessLayer
{
    public class Logger
    {
        public static void Log(LogData logData)
        {
            if (logData.SessionId == null || logData.LogType == null)
            {
                return;
            }
            
            var dataContext = new LogDataContext();

            var misparIshi = CurrentMisparIshi.GetCurrentMisparIshi();
            if (misparIshi.Equals("6073570") || misparIshi.Equals("8159165"))
            {
                return;
            }

            var log = getLog(dataContext, logData.SessionId, misparIshi);

            switch (logData.LogType.ToLower())
            {
                case "mail":
                    log.MailSent++;
                    break;
                case "request":
                    log.RequestsMade++;
                    if (logData.Query != null)
                    {
                        var request = new Request();
                        request.ID = logData.SessionId;
                        request.Request1 = logData.Query;
                        request.Time = DateTime.Now;
                        dataContext.Requests.InsertOnSubmit(request);
                    }
                    break;
                case "morepressed":
                    log.MorePressed++;
                    break;
                case "seeallpressed":
                    log.SeeAllPressed++;
                    break;
                case "timer":
                    log.Timer += 1;
                    break;
            }
            dataContext.SubmitChanges();
        }

        private static Log getLog(
            LogDataContext dataContext,
            string id,
            string misparIshi)
        {
            var logFromDB =
                dataContext.Logs.Where(log => log.ID.Equals(id)).FirstOrDefault();
            if (logFromDB != null)
            {
                return logFromDB;
            }
            var newLog = new Log();
            newLog.TimeCreated = DateTime.Now;
            newLog.ID = id;
            newLog.MailSent = 0;
            newLog.MorePressed = 0;
            newLog.RequestsMade = 0;
            newLog.SeeAllPressed = 0;
            newLog.Timer = 0;
            newLog.MisparIshi = misparIshi;
            newLog.GivenName = getGivenNameForMisparIshi(misparIshi);
            dataContext.Logs.InsertOnSubmit(newLog);
            return newLog;
        }

        private static string getGivenNameForMisparIshi(string misparIshi)
        {
            var dataContext = new PersonDataContext();
            return dataContext.Persons
                .Where(person => person.MisparIshi.Equals(misparIshi))
                .Select(person => String.Format("{0} {1}",
                    person.GivenName, person.Surname))
                .FirstOrDefault();
        }
    }

    public class LogData
    {
        public string SessionId { get; set; }
        public string LogType { get; set; }
        public string Query { get; set; }
    }
}