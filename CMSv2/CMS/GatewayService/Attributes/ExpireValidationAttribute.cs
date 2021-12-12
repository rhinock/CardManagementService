using GatewayService.Types;
using System;
using System.ComponentModel.DataAnnotations;

namespace GatewayService.Attributes
{
    public class ExpireValidationAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            Expire expire = value as Expire;

            if (expire.Year < DateTime.Now.Year)
                return false;
            
            if (expire.Year == DateTime.Now.Year && expire.Month < DateTime.Now.Month)
                return false;

            return true;
        }
    }
}
