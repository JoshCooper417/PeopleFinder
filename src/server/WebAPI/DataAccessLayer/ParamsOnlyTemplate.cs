﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace WebAPI.DataAccessLayer
{
    public class ParamsOnlyTemplate : ITemplate
    {
        private string input;

        public Regex MatchingRegex()
        {
            return new Regex(@".*");
        }

        public DbRequest MakeDbRequest(string input, bool shouldShowAll)
        {
            this.input = input;
            return new DbRequest(input, shouldShowAll);
        }

        public List<PersonJsonWrapper> ProcessPersonJsons(List<PersonJsonWrapper> personJsons)
        {
            return personJsons;
        }

        public string MetdataDisplayValue()
        {
            return input;
        }

    }
}