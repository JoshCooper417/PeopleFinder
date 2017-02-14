using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using WebAPI.Models;

namespace WebAPI.DataAccessLayer
{
    public class Timer
    {

        private Stopwatch stopwatch;
        private QueryTime queryTime;

        public Timer(string input, bool shouldShowAll)
        {
            
            stopwatch = new Stopwatch();
            stopwatch.Start();
            
            queryTime = new QueryTime();
            queryTime.Query = input;
            queryTime.ShowAll = shouldShowAll;
            queryTime.Now = DateTime.Now;
        }

        public void Stop()
        {
            stopwatch.Stop();
            queryTime.Elapsed = stopwatch.ElapsedMilliseconds;
            
            var dataContext = new LogDataContext();
            dataContext.QueryTimes.InsertOnSubmit(queryTime);
            dataContext.SubmitChanges();
        }
    }
}