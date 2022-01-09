using CMS.Types;
using System;
using System.ComponentModel.DataAnnotations;

namespace CMS.Attributes
{
    public class ExpireValidationAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            Expire expire = value as Expire;

            if (expire.Year < DateTime.Now.Year)
                return false;
            
            if (expire.Month < DateTime.Now.Month)
                return false;

            return true;
        }
    }
}
