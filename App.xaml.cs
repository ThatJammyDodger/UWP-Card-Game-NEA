using System;
using Programming_Project.Services;
using Windows.UI.Xaml.Controls;
using Windows.ApplicationModel.Activation;
using Windows.UI.Xaml;


namespace Programming_Project
{
    public sealed partial class App : Application
    {
        private Lazy<ActivationService> _activationService;

        private ActivationService ActivationService
        {
            get { return _activationService.Value; }
        }

        public App()
        {
            InitializeComponent();
            UnhandledException += OnAppUnhandledException;

            // Deferred execution until used. Check https://docs.microsoft.com/dotnet/api/system.lazy-1 for further info on Lazy<T> class.
            _activationService = new Lazy<ActivationService>(CreateActivationService);
        }

        protected override async void OnLaunched(LaunchActivatedEventArgs args)
        {
            if (!args.PrelaunchActivated)
            {
                await ActivationService.ActivateAsync(args);
            }
        }

        protected override async void OnActivated(IActivatedEventArgs args)
        {
            await ActivationService.ActivateAsync(args);
        }

        private void OnAppUnhandledException(object sender, Windows.UI.Xaml.UnhandledExceptionEventArgs e)
        {
            Uhoh();

            Application.Current.Exit();
        }
        public static async void Uhoh(string arg1 = "nobody cares", string arg2 = "except the sad person who just lost all progress on a sad card game lol.")
        {
            ContentDialog ERROR = new ContentDialog()
            {
                Title = "ERRORRRRRRRRRRRRRRRRR ARRGGHH",
                Content = $"WE'RE ALL DOOMED!!\n\n Exception Details:\n{arg1} - {arg2}",
                CloseButtonText = "yikes"
            };

            await ERROR.ShowAsync();
        }


        private ActivationService CreateActivationService()
        {
            return new ActivationService(this, typeof(Views.SigninPage), new Lazy<UIElement>(CreateShell));
        }

        private UIElement CreateShell()
        {
            return new Views.ShellPage();
        }
    }
}
