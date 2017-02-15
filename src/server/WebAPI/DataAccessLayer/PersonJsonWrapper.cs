using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebAPI.Models;

namespace WebAPI.DataAccessLayer
{
    // Converts a PersonFromDbWrapper object to a C# object that will be sent as
    // as JSON to the client.
    public class PersonJsonWrapper
    {
        public object JsonFromClient { get; set; }
        
        // These fields are exposed to enable sorting by them.
        public char Mail { get; set; }
        public string Name { get; set; }
        public bool IsMe { get; set; }

        private PersonFromDbWrapper person;

        public PersonJsonWrapper(PersonFromDbWrapper person, string lookupField)
        {
            this.person = person;

            this.IsMe = this.getIsMe();
            this.Name = person.GivenName;
            // Sort by the first character of the mail so we have all the
            // S emails before the M emails.
            this.Mail = person.Mail == null ? ' ' : person.Mail.FirstOrDefault();

            this.JsonFromClient = toJsonObject(person, lookupField);
        }

        private object toJsonObject(PersonFromDbWrapper person, string lookupField)
        {
            // TODO(Josh): Some of these fields ar eonly here for
            // backwards-compatibility.
            return new
            {
                mispar_ishi = person.MisparIshi,
                name = this.getDisplayName(),
                mail = person.Mail,
                mobile = person.Mobile,
                long_work_title = person.LongWorkTitle,
                job_title = person.JobTitle,
                work_phone = person.WorkPhone,
                picture = this.getPictureString(),
                darga = person.Darga,
                birthday = this.getBirthdayDisplayString(),
                is_birthday_today = this.isBirthdayToday(),
                top_row = this.createTopRowJson(lookupField),
                bottom_row = this.createBottomRowJson(lookupField),
                tags = this.getTags(),
                is_me = this.IsMe,
                what_i_do = person.WhatIDo
                    
            };
        }

        private IEnumerable<object> createTopRowJson(string lookupField)
        {
            var objects = new List<object>();

            List<string> fields = new List<string>();

            // always add display name 
            objects.maybeAddDisplayFieldObject(FieldsAliases.DISPLAY_NAME, this.getDisplayName());

            if (lookupField != null && lookupField != "")
            {
                fields = FieldsAliases.getJsonFields(lookupField);

                foreach (string field in fields)
                {
                    switch (field)
                    {
                        case FieldsAliases.BIRTHDAY:
                            {
                                objects.maybeAddDisplayFieldObject(field, this.getBirthdayDisplayString());
                                break;
                            };
                        case FieldsAliases.MISPAR_ISHI:
                            {
                                objects.maybeAddDisplayFieldObject(field, person.MisparIshi);
                                break;
                            }
                        case FieldsAliases.FIRST_NAME:
                            {
                                objects.maybeAddDisplayFieldObject(field, person.GivenName);
                                break;
                            };
                        case FieldsAliases.LAST_NAME:
                            {
                                objects.maybeAddDisplayFieldObject(field, person.Surname);
                                break;
                            };
                        case FieldsAliases.JOB:
                            {
                                objects.maybeAddDisplayFieldObject(field, person.LongWorkTitle);
                                break;
                            };
                        case FieldsAliases.MOBILE_PHONE:
                            {
                                objects.maybeAddDisplayFieldObject(field, person.Mobile);
                                break;
                            };
                        case FieldsAliases.WORK_PHONE:
                            {
                                objects.maybeAddDisplayFieldObject(field, person.WorkPhone);
                                break;
                            };
                        case FieldsAliases.RANK:
                            {
                                objects.maybeAddDisplayFieldObject(field, person.Darga);
                                break;
                            };
                        case FieldsAliases.END_OF_SERVICE:
                            {
                                var endOfServiceDisplayString = "";
                                if (person.EndOfService.HasValue)
                                {
                                    var endOfService = person.EndOfService.Value;
                                    endOfServiceDisplayString = endOfService.ToString("dd/MM/yyyy");
                                }
                                objects.maybeAddDisplayFieldObject(FieldsAliases.END_OF_SERVICE, endOfServiceDisplayString);
                                break;
                            };
                        case FieldsAliases.SEX:
                            {
                                objects.maybeAddDisplayFieldObject(field, person.Sex);
                                break;
                            };
                        case FieldsAliases.MAIL:
                            {
                                objects.maybeAddDisplayFieldObject(field, person.Mail);
                                break;
                            };
                        case FieldsAliases.FAX:
                            {
                                objects.maybeAddDisplayFieldObject(field, person.Fax);
                                break;
                            };
                    }
                }

            }

            // if only defaults, set this records
            if (objects.Count <= 1)
            {
                objects.maybeAddDisplayFieldObject(FieldsAliases.MOBILE_PHONE, person.Mobile);
            }

            // always add job unless already added
            if (!fields.Contains(FieldsAliases.JOB))
            {
                objects.maybeAddDisplayFieldObject(FieldsAliases.JOB, person.LongWorkTitle);
            }

            return objects;
        }

        private IEnumerable<object> createBottomRowJson(string lookupField)
        {
            var objects = new List<object>();

            if (lookupField != null && lookupField != "")
            {
                List<string> fields = FieldsAliases.getJsonFields(lookupField);

                if (!fields.Contains(FieldsAliases.MISPAR_ISHI))
                {
                    objects.maybeAddDisplayFieldObject(FieldsAliases.MISPAR_ISHI, person.MisparIshi);
                }

                if (!fields.Contains(FieldsAliases.FIRST_NAME))
                {
                    objects.maybeAddDisplayFieldObject(FieldsAliases.FIRST_NAME, person.GivenName);
                }

                if (!fields.Contains(FieldsAliases.LAST_NAME))
                {
                    objects.maybeAddDisplayFieldObject(FieldsAliases.LAST_NAME, person.Surname);
                }

                if (!fields.Contains(FieldsAliases.MOBILE_PHONE))
                {
                    objects.maybeAddDisplayFieldObject(FieldsAliases.MOBILE_PHONE, person.Mobile);
                }

                if (!fields.Contains(FieldsAliases.WORK_PHONE))
                {
                    objects.maybeAddDisplayFieldObject(FieldsAliases.WORK_PHONE, person.WorkPhone);
                }

                if (!fields.Contains(FieldsAliases.BIRTHDAY))
                {
                    objects.maybeAddDisplayFieldObject(FieldsAliases.BIRTHDAY, this.getBirthdayDisplayString());
                }

                if (!fields.Contains(FieldsAliases.RANK))
                {
                    objects.maybeAddDisplayFieldObject(FieldsAliases.RANK, person.Darga);
                }

                if (!fields.Contains(FieldsAliases.END_OF_SERVICE))
                {
                    var endOfServiceDisplayString = "";
                    if (person.EndOfService.HasValue)
                    {
                        var endOfService = person.EndOfService.Value;
                        endOfServiceDisplayString = endOfService.ToString("dd/MM/yyyy");
                    }
                    objects.maybeAddDisplayFieldObject(FieldsAliases.END_OF_SERVICE, endOfServiceDisplayString);
                }

                if (!fields.Contains(FieldsAliases.SEX))
                {
                    objects.maybeAddDisplayFieldObject(FieldsAliases.SEX, person.Sex);
                }

                if (!fields.Contains(FieldsAliases.MAIL))
                {
                    objects.maybeAddDisplayFieldObject(FieldsAliases.MAIL, person.Mail);
                }

                if (!fields.Contains(FieldsAliases.FAX))
                {
                    objects.maybeAddDisplayFieldObject(FieldsAliases.FAX, person.Fax);
                }
            }
            else
            {
                objects.maybeAddDisplayFieldObject(FieldsAliases.BIRTHDAY, this.getBirthdayDisplayString());
                objects.maybeAddDisplayFieldObject(FieldsAliases.WORK_PHONE, person.WorkPhone);
                objects.maybeAddDisplayFieldObject(FieldsAliases.RANK, person.Darga);
                objects.maybeAddDisplayFieldObject(FieldsAliases.SEX, person.Sex);
                objects.maybeAddDisplayFieldObject(FieldsAliases.MAIL, person.Mail);
                objects.maybeAddDisplayFieldObject(FieldsAliases.FAX, person.Fax);
                objects.maybeAddDisplayFieldObject(FieldsAliases.MISPAR_ISHI, person.MisparIshi);
            }

            return objects;
        }

        // The additional relevant fields that don't appear as part of the top or
        // bottom rows.
        private object createOtherFieldsJson()
        {
            return new
            {
                mail = person.Mail,
                picture = this.getPictureString(),
                is_birthday_today = this.isBirthdayToday(),
            };
        }

        private string getBirthdayDisplayString()
        {
            return person.BirthdayDisplayString;
        }

        private bool isBirthdayToday()
        {
            var today = DateTime.Today;
            // TODO(:Josh): Extract this because it is shared with the queryable extension class.
            var todayDispalayString = String.Format("{0}.{1}",
                today.Day.ToString("D2"),
                today.Month.ToString("D2"));
            var personBirthday = person.BirthdayDisplayString;
            return personBirthday != null
                && person.BirthdayDisplayString.Equals(todayDispalayString);
        }

        private string getPictureString()
        {
            return person.Picture != null ?
                Convert.ToBase64String(person.Picture.ToArray()) : "";
        }

        private List<object> getTags()
        {
            var factors = getFactors(person.Tags);
            return factors
                .Where(factor => TagToPrimeDictionary.PRIME_TO_TAG.ContainsKey(factor))
                .Select(factor => {
                    return TagToPrimeDictionary.PRIME_TO_TAG[factor].ToJson();
                })
                .ToList();
        }

        private IEnumerable<long> getFactors(long number)
        {
            var factors = new HashSet<long>();
            var i = 0;
            while (number > 1 && i < TagToPrimeDictionary.EXISTING_TAG_PRIMES.Count())
            {
                var currentPrime =
                    TagToPrimeDictionary.EXISTING_TAG_PRIMES.ElementAt(i);
                if (number % currentPrime == 0)
                {
                    number /= currentPrime;
                    factors.Add(currentPrime);
                    continue;
                }
                i++;
            }
            return factors.AsEnumerable();

        }

        private String getDisplayName() {
            return String.Format("{0} {1}", person.GivenName, person.Surname);
        }

        private bool getIsMe()
        {
            return CurrentMisparIshi.GetCurrentMisparIshi().Equals(person.MisparIshi);
        }
    }

    public static class JsonExtensions
    {
        public static void maybeAddDisplayFieldObject(
            this List<object> list,
            string name,
            string value)
        {
            if (value == null || value.Length == 0)
            {
                list.Add(new { name, value = "N/A" });
            } else
            {
                list.Add(new { name, value });
            }
        }

        public static object ToJson(this TagPrime tagPrime)
        {
            return new
            {
                tag = tagPrime.Tag,
                non_admins_can_edit = tagPrime.AllowNonAdminsToAdd
            };
        }
    
    }
}