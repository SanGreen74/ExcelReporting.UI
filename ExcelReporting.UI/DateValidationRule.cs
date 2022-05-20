using System;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Windows.Controls;

namespace ExcelReporting.UI
{
    public class DateValidationRule : ValidationRule
    {
        private readonly Regex regex = new Regex(@"^\d{2}.\d{2}.\d{4}", RegexOptions.Compiled);
        
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            var stringValue = Convert.ToString(value);
            if (string.IsNullOrEmpty(stringValue))
            {
                return new ValidationResult(false, "Значение не может быть пустым");
            }

            if (!regex.IsMatch(stringValue))
            {
                return new ValidationResult(false, "Значение должно иметь формат \'12.08.1998\'");
            }
            
            return ValidationResult.ValidResult;
        }
    }
}