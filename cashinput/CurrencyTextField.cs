// This file has been autogenerated from a class added in the UI designer.

namespace cashinput
{
    using System;
    
    using Foundation;
    using UIKit;
    using System.ComponentModel;
    using cashCore;
    
    public partial class CurrencyTextField : UITextField, INotifyPropertyChanged, ICurrencyInputFilter, ICurrencyControlMixin
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private int decimalPlaces;
        
        private decimal amount;
        
        public CurrencyTextField (IntPtr handle) : base (handle)
        {
        }

        public override void AwakeFromNib()
        {
            base.AwakeFromNib();

            DecimalPlaces = 2;

            this.ShouldChangeCharacters = checkShouldChange;
            this.EditingDidBegin += CurrencyTextField_EditingDidBegin;
            this.EditingChanged += AmountChangedDuringEditing;
            this.EditingDidEnd += AmountEditingEnded;
        }

        bool checkShouldChange(UITextField txField, NSRange y, string replacementString)
        {
            return this.MayInsertTextAt(txField.Text, replacementString, (int)y.Location, this.DecimalPlaces);
        }

        public int DecimalPlaces
        {
            get
            {
                return this.decimalPlaces;
            }

            set
            {
                this.decimalPlaces = value;
                decimal nul = 0;
                this.Placeholder = nul.ToString(this.GetCurrencyFormatString());;
            }
        }

        public decimal Amount
        {
            get
            {
                return amount;
            }

            set
            {
                SetInternal(value);
                this.FormatAsCurrency();
            }
        }

        public void SetInternal(decimal v)
        {
            if (amount != v)
            {
                amount = v;
                if (this.PropertyChanged != null)
                {
                    this.PropertyChanged(this, new PropertyChangedEventArgs("Amount"));
                }
            }
        }
            
        void CurrencyTextField_EditingDidBegin(object sender, EventArgs e)
        {
            this.FormatAsDecimal();
        }

        private void AmountChangedDuringEditing(object sender, EventArgs e)
        {
            this.Reparse();
        }

        private void AmountEditingEnded(object sender, EventArgs e)
        {
            this.FormatAsCurrency();
        }
    }
}
