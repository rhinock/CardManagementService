using System.ComponentModel.DataAnnotations;

namespace GatewayService.Attributes
{
    public class PanValidationAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            return CheckLuhn(value?.ToString() ?? "");
        }

        /// <summary>
        /// Returns true if given card number is valid
        /// </summary>
        /// <param name="cardNo"></param>
        /// <returns></returns>
        static bool CheckLuhn(string cardNo)
        {
            int nDigits = cardNo.Length;

            int nSum = 0;
            bool isSecond = false;

            for (int i = nDigits - 1; i >= 0; i--)
            {
                int d = cardNo[i] - '0';

                if (isSecond == true)
                    d = d * 2;

                // We add two digits to handle
                // cases that make two digits
                // after doubling

                nSum += d / 10;
                nSum += d % 10;

                isSecond = !isSecond;
            }

            return (nSum % 10 == 0);
        }
    }
}
