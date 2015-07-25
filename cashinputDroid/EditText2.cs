using System;
using Android.Widget;
using Android.Content;
using Android.Text;
using cashCore;
using System.ComponentModel;
using System.Diagnostics;

namespace cashinputDroid
{
    public class MyWatcher : Java.Lang.Object, ITextWatcher
    {
        private CurrencyEditText currencyEdit;

        public void OnTextChanged(Java.Lang.ICharSequence s, int start, int before, int count)
        {
            //Debug.WriteLine("OnTextChanged {0} {1}", s, start);
        }

        public MyWatcher(CurrencyEditText currencyEdit)
        {
            this.currencyEdit = currencyEdit;
        }

        public void AfterTextChanged(IEditable s)
        {
            //Debug.WriteLine("AfterTextChanged {0}", s);
            this.currencyEdit.Reparse();
        }

        public void BeforeTextChanged(Java.Lang.ICharSequence s, int start, int count, int after)
        {
            //Debug.WriteLine("BeforeTextChanged {0} {1}", s, start);
        }
    }

    public class CurrencyFilter : Java.Lang.Object, IInputFilter, ICurrencyInputFilter
    {
        private CurrencyEditText currencyEdit;

        public CurrencyFilter(CurrencyEditText currencyEdit)
        {
            this.currencyEdit = currencyEdit;
        }

        public Java.Lang.ICharSequence FilterFormatted(Java.Lang.ICharSequence source, int start, int end, ISpanned dest, int dstart, int dend)
        {
            var replacement = source.ToString();
            var ok = this.MayInsertTextAt(this.currencyEdit.Text, replacement, dstart, this.currencyEdit.DecimalPlaces);
            if (ok)
            {
                return source;
            }
            else
            {
                return new Java.Lang.String(string.Empty);
            }
        }
    }

    public class CurrencyEditText : EditText, INotifyPropertyChanged, ICurrencyControlMixin
    {
        public event PropertyChangedEventHandler PropertyChanged;
    
        private int decimalPlaces;

        private decimal amount;

        public CurrencyEditText(Context context, global::Android.Util.IAttributeSet attrs, int defStyle)
            : base(context, attrs, defStyle)
        {
            Init2();
        }

        public CurrencyEditText(Context context, global::Android.Util.IAttributeSet attrs)
            : base(context, attrs)
        {
            Init2();
        }

        public CurrencyEditText(Context context)
            : base(context)
        {
            Init2();
        }

        private CurrencyFilter inputFilter;

        private void Init2()
        {
            DecimalPlaces = 2;
            this.inputFilter = new CurrencyFilter(this);
            //new Android.Text.filter

            this.AddTextChangedListener(new MyWatcher(this));

            this.FocusChange += CurrencyEditText_FocusChange;
        }

        void CurrencyEditText_FocusChange (object sender, FocusChangeEventArgs e)
        {
            if (e.HasFocus)
            {
                this.FormatAsDecimal();
                this.SetFilters(new IInputFilter[] { inputFilter });
            }
            else
            {
                this.SetFilters(new IInputFilter[] { });
                this.FormatAsCurrency();
            }
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
                this.Hint = nul.ToString(this.GetCurrencyFormatString());
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

            System.Diagnostics.Debug.WriteLine(string.Format("{0}", this.amount));
        }
    }
}

