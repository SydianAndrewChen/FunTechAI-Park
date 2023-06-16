namespace AMADatePicker
{
    public struct Age
    {
        public int Years;
        public int Months;
        public int Days;
        public int Hours;
        public int Minutes;
        public int Seconds;

        public Age(int _Years, int _Months, int _Days, int _Hours, int _Minutes, int _Seconds)
        {
            Years = _Years;
            Months = _Months;
            Days = _Days;
            Hours = _Hours;
            Minutes = _Minutes;
            Seconds = _Seconds;
        }
    }
}