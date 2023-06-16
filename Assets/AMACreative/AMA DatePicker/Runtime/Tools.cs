using System;
using System.Text;

namespace AMADatePicker
{
    public enum TextTypeOptions { Text, TMPText }
    public enum LayoutOptions { Vertical, Horizontal, Compact }
    public enum MonthOptions { Words, Numbers }
    public enum LabelPositionOptions { Top, Left, Right, Bottom }
    //public enum Languages { English, Urdu, Arabic }

    public static class Tools
    {
        public readonly static string[] Months = { "JANUARY", "FEBRUARY", "MARCH",
                                          "APRIL", "MAY", "JUNE",
                                          "JULY", "AUGUST", "SEPTEMBER",
                                          "OCTOBER", "NOVEMBER", "DECEMBER" };

        public readonly static int[] _MonthWithOnlyThirties = { 4, 6, 9, 11 };

        public static Age CalculateAge(int MM, int dd, int yyyy)
        {
            StringBuilder _DoBRaw = new StringBuilder();
            _DoBRaw.Append(yyyy);
            _DoBRaw.Append(",");
            _DoBRaw.Append(MM);
            _DoBRaw.Append(",");
            _DoBRaw.Append(dd);

            DateTime _DoB = Convert.ToDateTime(_DoBRaw.ToString());
            DateTime _Now = DateTime.Now;

            if (_DoB.Ticks > _Now.Ticks)
            {
                Age _EmptyAge = new Age();
                return _EmptyAge;
            }

            int _Years = new DateTime(_Now.Subtract(_DoB).Ticks).Year - 1;

            DateTime _PastYearDate = _DoB.AddYears(_Years);
            int _Months = 0;

            for (int i = 1; i <= 12; i++)
            {
                if (_PastYearDate.AddMonths(i) == _Now)
                {
                    _Months = i;
                    break;
                }
                else if (_PastYearDate.AddMonths(i) >= _Now)
                {
                    _Months = i - 1;
                    break;
                }
            }

            int _Days = _Now.Subtract(_PastYearDate.AddMonths(_Months)).Days;
            int _Hours = _Now.Subtract(_PastYearDate).Hours;
            int _Minutes = _Now.Subtract(_PastYearDate).Minutes;
            int _Seconds = _Now.Subtract(_PastYearDate).Seconds;

            return new Age(_Years, _Months, _Days, _Hours, _Minutes, _Seconds);
        }

        public static (bool MM, bool dd, bool yyyy) ValidateDate(int MM, int dd, int yyyy)
        {
            bool _MMCorrect = true;
            bool _ddCorrect = true;
            bool _yyyyCorrect = true;

            // CHECKING YEAR

            if (yyyy < 0) { _yyyyCorrect = false; }

            // CHECKING MONTH

            if (MM < 1 || MM > 12) { _MMCorrect = false; }

            // CHECKING DAY

            if (dd < 1 || dd > 31) { _ddCorrect = false; }

            if (MM == 4 || MM == 6 || MM == 9 || MM == 11)
            {
                if (dd > 30) { _ddCorrect = false; }
            }

            if (MM == 2 && yyyy % 4 == 0)
            {
                if (dd > 29) { _ddCorrect = false; }
            }
            else if (MM == 2 && yyyy % 4 != 0)
            {
                if (dd > 28) { _ddCorrect = false; }
            }

            return (_MMCorrect, _ddCorrect, _yyyyCorrect);
        }

        public static (int MM, int dd, int yyyy) ValidateAndFixDate(int MM, int dd, int yyyy)
        {
            // CHECKING MONTH

            if (MM < 1) { MM = 1; }
            else if (MM > 12) { MM = 12; }

            // CHECKING DAY

            if (dd < 1) { dd = 1; }
            else if (dd > 31) { dd = 31; }
            
            if (MM == 4 || MM == 6 || MM == 9 || MM == 11)
            {
                if (dd > 30) { dd = 30; }
            }

            if (MM == 2 && yyyy % 4 == 0)
            {
                if (dd > 29) { dd = 29; }
            }
            else if (MM == 2 && yyyy % 4 != 0)
            {
                if (dd > 28) { dd = 28; }
            }

            return (MM, dd, yyyy);
        }
    }
}