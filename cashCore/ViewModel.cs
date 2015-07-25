using System;
using ReactiveUI;

namespace cashcore
{
    public class MyViewModel : ReactiveObject
    {
        public decimal bedrag;
        public MyViewModel()
        {
            this.bedrag = 42;
        }

        public decimal Bedrag
        {
            get
            {
                return this.bedrag;
            }

            set
            {
                this.RaiseAndSetIfChanged(ref this.bedrag, value);
            }
        }
    }
}

