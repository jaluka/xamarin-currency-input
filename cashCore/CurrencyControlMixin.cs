using System;
using System.Globalization;

namespace cashCore
{
    public interface ICurrencyControlMixin
    {
        string Text {get;set;}
        decimal Amount {get;set;}
        void SetInternal(decimal v);
        int DecimalPlaces {get;}
    }

    public interface ICurrencyInputFilter
    {
    }

    public static class InputFilterExtension
    {
        public static bool MayInsertTextAt(this ICurrencyInputFilter thiz, string currentText, string replacementString, int position, int decimalPlaces)
        {
            // Empty string is always fine:
            if (string.IsNullOrEmpty(replacementString))
            {
                return true;
            }

            // Deny multicharacter input (such as copy-paste input):
            if (replacementString.Length > 1)
            {
                return false;
            }

            const string numericCharacters = "0123456789";
            string decimalCharacter = CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator;

            // Allow a single decimal point:
            if (currentText.Contains(decimalCharacter))
            {
                int dotPosition = currentText.IndexOf(decimalCharacter);

                string validCharacters = numericCharacters;

                if (validCharacters.Contains(replacementString))
                {
                    if (position <= dotPosition)
                    {
                        return true;
                    }
                    else
                    {
                        if (currentText.Length - dotPosition <= decimalPlaces)
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                }
                else
                {
                    return false;
                }
            }
            else
            {
                string validCharacters = numericCharacters + decimalCharacter;

                if (validCharacters.Contains(replacementString))
                {
                    if (replacementString.Equals(decimalCharacter))
                    {
                        if (position < currentText.Length - decimalPlaces)
                        {
                            return false;
                        }
                        else
                        {
                            return true;
                        }
                    }
                    else
                    {
                        return true;
                    }
                }
                else
                {
                    return false;
                }
            }
        }

        public static void Reparse(this ICurrencyControlMixin thiz)
        {
            if (string.IsNullOrEmpty(thiz.Text))
            {
                thiz.SetInternal(0);
            }
            else
            {
                decimal xx;
                if (decimal.TryParse(thiz.Text, System.Globalization.NumberStyles.Currency,
                    CultureInfo.CurrentCulture, out xx))
                {
                    thiz.SetInternal(xx);
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine("Wrong input");
                }
            }
        }

        public static void FormatAsDecimal(this ICurrencyControlMixin thiz)
        {
            if (thiz.Amount == 0m)
            {
                thiz.Text = string.Empty;
            }
            else
            {
                thiz.Text = thiz.Amount.ToString(thiz.GetDecimalFormatString());
            }
        }

        public static void FormatAsCurrency(this ICurrencyControlMixin thiz)
        {
            if (thiz.Amount == 0m)
            {
                thiz.Text = string.Empty;
            }
            else
            {
                thiz.Text = thiz.Amount.ToString(thiz.GetCurrencyFormatString());
            }
        }

        public static string GetDecimalFormatString(this ICurrencyControlMixin thiz)
        {
            return "0." + new string('#', thiz.DecimalPlaces);
        }

        public static string GetCurrencyFormatString(this ICurrencyControlMixin thiz)
        {            
            return string.Format("C{0}", thiz.DecimalPlaces);
        }
    }
}
