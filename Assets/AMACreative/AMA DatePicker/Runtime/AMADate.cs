namespace AMADatePicker
{
    public struct AMADate
    {
        public int MM;
        public int dd;
        public int yyyy;
        public string Month;
        public Age Age;

        public AMADate(int _MM, int _dd, int _yyyy)
        {
            MM = _MM;
            dd = _dd;
            yyyy = _yyyy;
            Month = Tools.Months[_MM - 1];
            Age = Tools.CalculateAge(_MM, _dd, _yyyy);
        }
    }
}