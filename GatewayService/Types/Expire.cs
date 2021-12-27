using System;
using System.Linq;
using System.ComponentModel.DataAnnotations;

namespace GatewayService.Types
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

        public static implicit operator Expire(string value)
        {
            if(value == null)
            {
                return null;
            }

            int[] parts = value.Split('/').Select(x => int.Parse(x)).ToArray();

            if (parts.Length != 2)
                throw new ArgumentException("Only Month and Year should be defined");

            if (parts[1] < DateTime.Now.Year)
                throw new ArgumentException("Year shouldn't be less than current year");

            if (parts[0] < 1 || parts[0] > 12)
                throw new ArgumentException("Month shouldn't be less than current month");

            return new Expire { Month = parts[0], Year = parts[1] };
        }

        public override string ToString()
        {
            return $"{Month}/{Year}";
        }
    }
}
