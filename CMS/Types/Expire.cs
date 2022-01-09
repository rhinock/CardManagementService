using System.ComponentModel.DataAnnotations;

namespace CMS.Types
{
    public class Expire
    {
        [Range(1, 12)]
        public int Month { get; set; }
        [Range(2021, 2099)]
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
