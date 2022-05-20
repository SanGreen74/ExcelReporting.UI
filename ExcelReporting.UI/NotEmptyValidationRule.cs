using System;
using System.Globalization;
using System.Windows.Controls;

namespace ExcelReporting.UI
{
    public class NotEmptyValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            if (value == null)
            {
                return new ValidationResult(false, "Значение не может быть пустым");
            }

            var stringValue = Convert.ToString(value);
            return string.IsNullOrEmpty(stringValue)
                ? new ValidationResult(false, "Значение не может быть пустым")
                : ValidationResult.ValidResult;
        }
    }
}