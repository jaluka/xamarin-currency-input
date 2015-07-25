using System;

using UIKit;
using ReactiveUI;
using System.Diagnostics;
using cashcore;

namespace cashinput
{
    public partial class ViewController : UIViewController, IViewFor<MyViewModel>
    {
        public ViewController(IntPtr handle)
            : base(handle)
        {
        }

        public MyViewModel ViewModel { get; set;}

        object IViewFor.ViewModel
        { 
            get
            {
                return this.ViewModel;
            }

            set
            {
                this.ViewModel = (MyViewModel)value;
            }
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            // Perform any additional setup after loading the view, typically from a nib.

            this.ViewModel = new MyViewModel();

            this.Bind(this.ViewModel, x => x.Bedrag, x => x.txf_bedrag1.Amount);
            this.Bind(this.ViewModel, x => x.Bedrag, x => x.txf_bedrag2.Amount);
            //this.Bind(this.ViewModel, vm => vm.Bedrag, v => v.lbl_result.Text);

            this.ViewModel.WhenAnyValue(x => x.Bedrag).Subscribe((am) =>
                {
                    Debug.WriteLine("W00t: {0}", am);
                });
        }

        public override void DidReceiveMemoryWarning()
        {
            base.DidReceiveMemoryWarning();
            // Release any cached data, images, etc that aren't in use.
        }
    }
}

