using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExportManager.Models.Validators
{
    public class NumberValidator : Validator
    {
        public static string IsPositive(object number) 
        {
            if (number == null)
                return "Invalid value.";
            try
            {
                var decimalValue = Convert.ToDecimal(number);
                if (decimalValue < 0m)
                    return "Value cannot be negative.";
                return null;
            }
            catch
            {
                return "Ivalid number format.";
            }
        }
        public static string IsPercentage(object number)
        {
            if (number == null)
                return "Invalid value.";
            try
            {
                if (!(Convert.ToDecimal(number) >= 0m))
                    return "Value cannot be negative.";
                if (!(Convert.ToDecimal(number) <= 100m))
                    return "Value cannot be higher than 100.";
            }
            catch
            {
            }
            return null;
        }
    }
}
