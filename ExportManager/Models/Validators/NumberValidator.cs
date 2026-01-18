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
                if(!(Convert.ToDecimal(number) >= 0m))
                    return "Value cannot be negative.";
            }
            catch
            {
            }
            return null;
        }
    }
}
