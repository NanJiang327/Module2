using Today.Services;
using Today.View;
using Microsoft.Azure.Mobile;
using Microsoft.Azure.Mobile.Analytics;
using Microsoft.Azure.Mobile.Crashes;

using Xamarin.Forms;

namespace Today
{
    public partial class App : Application
    {
        static ItemDB database;

        public App()
        {
            // The root page of your application
            MainPage = new NavigationPage(new ItemListPage())
            {
                BarTextColor = Color.White,
                BarBackgroundColor = Color.FromHex("#F2C500")
            };

        }

        public static ItemDB Database
        {
            get
            {
                if (database == null)
                {
                    database = ItemDB.DefaultManager;
                }
                return database;
            }
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
