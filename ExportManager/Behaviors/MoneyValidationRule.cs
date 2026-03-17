using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace ExportManager.Behaviors
{
    public class MoneyValidationRule: ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            var str = value as string;

            if (string.IsNullOrWhiteSpace(str))
                return new ValidationResult(false, "Value is required.");

            if (!decimal.TryParse(str, out var result))
                return new ValidationResult(false, "Invalid number.");

            if (result < 0)
                return new ValidationResult(false, "Value cannot be negative.");

            return ValidationResult.ValidResult;
        }
    }
}
