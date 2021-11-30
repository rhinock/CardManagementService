namespace CMS
{
    public class Expire
    {
        public int Month { get; set; }
        public int Year { get; set; }

        public Expire()
        {
            Month = 1;
            Year = 2021;
        }

        public Expire(int month, int year)
        {
            Month = month;
            Year = year;
        }
    }
}
