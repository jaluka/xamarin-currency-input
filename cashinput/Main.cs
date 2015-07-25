using UIKit;
using System.Globalization;

namespace cashinput
{
    public class Application
    {
        // This is the main entry point of the application.
        static void Main(string[] args)
        {
            // appl
            //CultureInfo.CurrentCulture = new CultureInfo("nl-NL");
            // if you want to use a different Application Delegate class from "AppDelegate"
            // you can specify it here.
            UIApplication.Main(args, null, "AppDelegate");
        }
    }
}
