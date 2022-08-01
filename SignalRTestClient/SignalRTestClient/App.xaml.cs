using System;
using SignalRTestClient.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SignalRTestClient
{
    public partial class App : Application
    {
        public App(DeviceInformation device)
        {
            Information = device;

            InitializeComponent();

            MainPage = new MainPage();
        }

        public DeviceInformation Information { get; }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
