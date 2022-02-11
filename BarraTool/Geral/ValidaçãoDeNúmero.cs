using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Windows.Controls;

namespace BarraTool.Geral
{
    
    /*class ValidaçãoDeNúmero : ValidationRule
    {
        
        public double Min { get; set; }
        public double Max { get; set; }
        public string TipoDeValidação { get; set; }

        public ValidaçãoDeNúmero()
        {
        }

        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            double número = 0;
            try
            {
                if (((string)value).Length > 0)
                    número = double.Parse((String)value, cultureInfo);
            }
            catch (Exception e)
            {
                return new ValidationResult(false, $"Caracteres ilegais ou {e.Message}");
            }

            if (TipoDeValidação == "Min")
            {
                if (número < Min)
                {
                    return new ValidationResult(false,
                      $"Por favor, entre um número maior que {Min}.");
                }
            }
            else if (TipoDeValidação == "Max")
            {
                if (número > Max)
                {
                    return new ValidationResult(false,
                      $"Por favor, entre um número menor que {Max}.");
                }
            }
            else
            {
                if ((número < Min) || (número > Max))
                {
                    return new ValidationResult(false,
                      $"Por favor, entre um número entre: {Min}-{Max}.");
                }
            }
            return ValidationResult.ValidResult;
        }
    }*/
}
