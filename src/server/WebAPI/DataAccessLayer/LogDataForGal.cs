using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebAPI.Models;

namespace WebAPI.DataAccessLayer
{
    public class LogDataForGal
    {
        public static IEnumerable<object> GetLogDataForGal(string today)
        {
            if (!CurrentMisparIshi.IsAdmin())
            {
                return new object[] { new { message = "Only for admins" } };
            }
            return today.Equals("today") ?
                GetTodaysLogDataForGal() : GetAllLogDataForGal();
        }
        
        private static IEnumerable<object> GetAllLogDataForGal()
        {
            return new LogDataContext().Logs
                .Where(log => log.GivenName != null)
                .GroupBy(log => log.GivenName)
                .Select(log => new
                {
                    Name = log.Key,
                    Count = log.Count()
                })
                .OrderByDescending(group => group.Count)
                .ToList();
        }
        
        public static IEnumerable<object> GetTodaysLogDataForGal()
        {
            return new LogDataContext().Logs
                .Where(log => log.GivenName != null
                    && log.TimeCreated.HasValue
                    && log.TimeCreated.Value.Date == DateTime.Today)
                .GroupBy(log => log.GivenName)
                .Select(log => new
                {
                    Name = log.Key,
                    Count = log.Count()
                })
                .OrderByDescending(group => group.Count)
                .ToList();
        }
    }
}