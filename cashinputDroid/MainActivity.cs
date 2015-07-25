using System;

using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using ReactiveUI;

namespace cashinputDroid
{
    [Activity(Label = "cashinputDroid", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : ReactiveActivity
    {
        int count = 1;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

            // Get our button from the layout resource,
            // and attach an event to it
            Button button = FindViewById<Button>(Resource.Id.myButton2);

            CurrencyEditText bedrag1 = FindViewById<CurrencyEditText>(Resource.Id.myBedrag);
            CurrencyEditText bedrag2 = FindViewById<CurrencyEditText>(Resource.Id.myBedrag2);

            bedrag1.Amount = 133;
            bedrag2.Amount = 133;

            // bedrag1.add

            // this.Bind
            button.Click += delegate
            {
                button.Text = string.Format("{0} clicks!", count++);
            };
        }
    }
}


