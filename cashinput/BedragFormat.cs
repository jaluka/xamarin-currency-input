namespace cashinput
{
    using System;
    using System.Globalization;
    using System.Text.RegularExpressions;

    using System.Globalization;

    public static class CurrencyExtensions
    {
        public static string ToCurrencyString(this decimal amount)
        {
            return amount.ToString("C2");
        }

        public static string ToCurrencyString(this decimal amount, int nrDigits)
        {
            return amount.ToString("C" + nrDigits);
        }

        public static decimal FromCurrencyString(this string textAmount)
        {
            decimal amount;
            decimal.TryParse(textAmount, NumberStyles.Any, new CultureInfo("nl-NL"), out amount);
            return amount;
        }
    }

    public class BedragFormat
    {
        private string bedrag;
        private decimal bedragDecimal;
        private string voorgaandeText;
        private bool endediting;

        public string Formateer(string nieuweText, bool endediting = false)
        {
            if (nieuweText == string.Empty)
            {
                return nieuweText;
            }

            this.endediting = endediting;

            if (string.IsNullOrEmpty(this.voorgaandeText) || nieuweText.Length > this.voorgaandeText.Length)
            {
                this.KarakterToevoegen(nieuweText);
            }
            else if (nieuweText.Length < this.voorgaandeText.Length)
            {
                this.KarakterVerwijderen(nieuweText);
            }

            return this.Parse(nieuweText);
        }

        public decimal GetBedrag()
        {
            decimal waarde;
            return decimal.TryParse(this.bedrag, out waarde) ? waarde : 0;
        }

        public void Clear()
        {
            this.bedrag = string.Empty;
            this.voorgaandeText = string.Empty;
        }

        private void KarakterToevoegen(string text)
        {
            if (!string.IsNullOrEmpty(text))
            {
                var last = text[text.Length - 1];
                this.bedrag += last;
            }
        }

        private void KarakterVerwijderen(string text)
        {
            if (this.bedrag.Length > 0)
            {
                this.bedrag = this.bedrag.Remove(this.bedrag.Length - 1);
                if (this.bedrag.Length == 0)
                {
                    this.bedrag = "0";
                }
            }
        }

        private string Parse(string nieuweText)
        {
            // Punten aan het einde vervangen door punten.
            if (this.bedrag.EndsWith(".") || this.bedrag.EndsWith(","))
            {
                this.bedrag = this.bedrag.TrimEnd('.', ',');
                
                // Alleen komma aan het einde toevoegen als er nog geen komma in het bedrag zat.
                if (!this.bedrag.Contains(","))
                {
                    this.bedrag += ",";
                }
            }

            if (decimal.TryParse(this.bedrag, NumberStyles.Any, new CultureInfo("nl-NL"), out this.bedragDecimal))
            {
                if (this.bedragDecimal < 0)
                {
                    // Bedrag kleiner dan toegestaan.
                    this.Clear();
                    nieuweText = string.Empty;
                }
                else
                {
                    nieuweText = this.FormateerDecimalen();
                }
            }
            else
            {
                this.Clear();
                nieuweText = string.Empty;
            }

            return nieuweText;
        }

        private string FormateerDecimalen()
        {
            int count = BitConverter.GetBytes(decimal.GetBits(this.bedragDecimal)[3])[2];
            if (this.endediting)
            {
                // Bij endediting altijd 2 decimalen tonen.
                this.voorgaandeText = this.bedragDecimal.ToCurrencyString();
                switch (count)
                {
                    case 1:
                        this.bedrag += "0";
                        break;
                    case 0:
                        if (!this.bedrag.EndsWith(",", StringComparison.Ordinal))
                        {
                            this.bedrag += ",";
                        }

                        this.bedrag += "00";
                        break;
                }
            }
            else if (count == 2)
            {
                // Bij endediting altijd 2 decimalen tonen.
                this.voorgaandeText = this.bedragDecimal.ToCurrencyString();
            }
            else if (count == 1)
            {
                this.voorgaandeText = this.bedragDecimal.ToCurrencyString(1);
            }
            else if (count == 0)
            {
                // Check meerdere nullen (0) achterelkaar.
                if (Regex.IsMatch(this.bedrag, "^0*$"))
                {
                    this.bedrag = "0";
                    this.voorgaandeText = "0";
                    this.bedragDecimal = 0;
                }

                this.voorgaandeText = this.bedragDecimal.ToCurrencyString(0);
            }
            else
            {
                this.bedrag = this.bedrag.Remove(this.bedrag.Length - 1);
            }

            this.ForceerKomma();

            return this.voorgaandeText;
        }

        private void ForceerKomma()
        {
            // Forceer komma
            if (this.bedrag.EndsWith(",", StringComparison.Ordinal))
            {
                this.voorgaandeText += ",";
            }
        }
    }
}