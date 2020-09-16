using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace ForLabApi.Utility
{
    public class Utility
    {
        private static readonly string DataUsage1_Description = "I have consumption/service data from all of the sites in this forecast, I want enter this data for each sites.";
        private static readonly string DataUsage2_Description = "I have consumption/service data from some of the sites in this forecast, I want to use this data for non reported sites.";
        private static readonly string DataUsage3_Description = "I want to categorize sites in group based on level and testing behaviors and enter consumption/service data for the group.";
        public static string[] Months = new string[] { "january", "february", "march", "april", "may", "june", "july", "august", "september", "october", "november", "december" };
        public static string GetDatausageDescription(string dusage)
        {
            string description = "";
            DataUsageEnum du = (DataUsageEnum)Enum.Parse(typeof(DataUsageEnum), dusage);

            switch (du)
            {
                case DataUsageEnum.DATA_USAGE1:
                    description = DataUsage1_Description;
                    break;
                case DataUsageEnum.DATA_USAGE2:
                    description = DataUsage2_Description;
                    break;
                case DataUsageEnum.DATA_USAGE3:
                    description = DataUsage3_Description;
                    break;
            }

            return description;
        }
        public static bool validDate(string duration, string period)
        {
            string[] s = duration.Split(new char[] { '-' });
            bool hasError = false;

            try
            {
                if (duration.StartsWith("Q") && ((period =="Monthly") || (period == "Bimonthly")))
                {
                    hasError = true;
                }
                else if (s.Length == 1 && ((period =="Monthly") || (period == "Bimonthly") || (period == "Quarterly")))
                {
                    hasError = true;
                }


            }
            catch
            {
                hasError = true;
            }
            return hasError;

        }

        public static string DatetimeToDurationStr(string PeriodEnum, DateTime date)
        {

            //string str=date.ToString("DD/MM/YYYY", CultureInfo.InvariantCulture);
            //date = Convert.ToDateTime(str);
            string CDuration = string.Empty;
            if (PeriodEnum == "Bimonthly" || PeriodEnum == "Monthly")
            {

                CDuration = String.Format("{0}-{1}", Months[date.Month - 1], date.Year);
            }
            else if (PeriodEnum == "Quarterly")
            {
                CDuration = String.Format("Qua{0}-{1}", GetQuarter(date), date.Year);
            }
            else if (PeriodEnum =="Yearly")//b
            {
                CDuration = date.Year.ToString();
            }
            return CDuration;
        }

        public static bool IsDateTime(string datestring)
        {
            DateTime tempDate;

            return DateTime.TryParse(datestring, out tempDate) ? true : false;
        }

        public static DateTime DurationToDateTime(string duration)
        {
            string[] s = duration.Split(new char[] { '-' });
            DateTime d;
            if (s.Length == 1)
                d = new DateTime(int.Parse(duration), 1, 1);
            else if (s[0].StartsWith("Q"))
                d = new DateTime(int.Parse(s[1]), GetQuarter(s[0]), 1);
            else
                d = new DateTime(int.Parse(s[1]), GetMonth(s[0]), 1);

            return d;
        }
        public static int GetQuarter(DateTime d)
        {
            int qua;
            if (d.Month <= 3)
                qua = 1;
            else if (d.Month <= 6)
                qua = 2;
            else if (d.Month <= 9)
                qua = 3;
            else
                qua = 4;

            return qua;
        }
        public static decimal GetAdjustedVolume(decimal reported, int dos, string period, decimal workingdays)
        {
            decimal y = 0;
            if (period == "Monthly")
                y = workingdays;
            if (period == "Bimonthly")
                y = workingdays * 2;
            if (period == "Quarterly")
                y = workingdays * 3;
            if (period == "Yearly")
                y = workingdays * 12;
            if (y - dos != 0)
                return Math.Round(((reported * y) / (y - dos)), 2, MidpointRounding.ToEven);
            else
                return 0;
        }

        public static int GetQuarter(string qtext)
        {
            int q = 0;
            switch (qtext)
            {
                case "Qua1":
                    q = 1;
                    break;
                case "Qua2":
                    q = 4;
                    break;
                case "Qua3":
                    q = 7;
                    break;
                case "Qua4":
                    q = 10;
                    break;
            }
            return q;
        }

        private static int GetMonth(string mname)
        {
            for (int i = 0; i < 12; i++)
            {
                if (Months[i] == mname)
                    return i + 1;
            }
            return 1;
        }
        public static DateTime CorrectDateTime(DateTime durationdt)
        {
            DateTime date = new DateTime();
            if (durationdt.Day > 25)
            {
                if (durationdt.Month == 12)
                    date = new DateTime(durationdt.Year + 1, 1, 01);
                else
                    date = new DateTime(durationdt.Year, durationdt.Month + 1, 01);
            }
            else
            {
                date = durationdt;
            }
            return date;
        }
    }
}
