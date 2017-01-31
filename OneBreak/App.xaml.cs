using OneBreak.Pages;
using OneBreak.ViewModels;
using System;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.ApplicationModel.Resources;
using Windows.ApplicationModel.Resources.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace OneBreak
{
    sealed partial class App : Application
    {
        public static NewsViewModel NewsViewModel { get; } = new NewsViewModel();

        public App()
        {
            this.InitializeComponent();
            this.Suspending += OnSuspending;
            this.Resuming += OnResuming;
        }

        private void OnResuming(object sender, object e)
        {
            if (NewsViewModel == null) return;

            NewsViewModel.LoadNews();
        }

        /// <summary>
        /// Invoked when the application is launched normally by the end user.  Other entry points
        /// will be used such as when the application is launched to open a specific file.
        /// </summary>
        /// <param name="e">Details about the launch request and process.</param>
        protected override void OnLaunched(LaunchActivatedEventArgs e)
        {
            Frame rootFrame = Window.Current.Content as Frame;

            // Do not repeat app initialization when the Window already has content,
            // just ensure that the window is active
            if (rootFrame == null)
            {
                // Create a Frame to act as the navigation context and navigate to the first page
                rootFrame = new Frame();

                rootFrame.NavigationFailed += OnNavigationFailed;

                if (e.PreviousExecutionState == ApplicationExecutionState.Terminated)
                {
                    //TODO: Load state from previously suspended application
                }

                // Place the frame in the current Window
                Window.Current.Content = rootFrame;
            }

            if (e.PrelaunchActivated == false)
            {
                if (rootFrame.Content == null)
                {
                    // When the navigation stack isn't restored navigate to the first page,
                    // configuring the new page by passing required information as a navigation
                    // parameter
                    rootFrame.Navigate(typeof(NewsPage), e.Arguments);
                }
                // Ensure the current window is active
                Window.Current.Activate();
            }
        }

        /// <summary>
        /// Invoked when Navigation to a certain page fails
        /// </summary>
        /// <param name="sender">The Frame which failed navigation</param>
        /// <param name="e">Details about the navigation failure</param>
        void OnNavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            throw new Exception("Failed to load Page " + e.SourcePageType.FullName);
        }

        /// <summary>
        /// Invoked when application execution is being suspended.  Application state is saved
        /// without knowing whether the application will be terminated or resumed with the contents
        /// of memory still intact.
        /// </summary>
        /// <param name="sender">The source of the suspend request.</param>
        /// <param name="e">Details about the suspend request.</param>
        private void OnSuspending(object sender, SuspendingEventArgs e)
        {
            var deferral = e.SuspendingOperation.GetDeferral();
            //TODO: Save application state and stop any background activity
            deferral.Complete();
        }

        private const string ResourcesKey = "Resources";
        public static async Task<string> GetStringFromResources(string key, bool fromUiTread = true)
        {
            if (string.IsNullOrEmpty(key)) return null;
            if(fromUiTread)
            {
                try
                {
                    var resource = ResourceLoader.GetForCurrentView();
                    return resource.GetString(key);
                }
                catch (Exception)
                {
                    //TODO: Log error
                    return null;
                }
            }
            else
            {
                try
                {
                    string resource = null;
                    await Windows.System.Threading.ThreadPool.RunAsync((s) =>
                    {
                        var resourceMap = ResourceManager.Current.MainResourceMap.GetSubtree(ResourcesKey);

                        var context = ResourceContext.GetForViewIndependentUse();

                        resource = resourceMap.GetValue(key, context).ValueAsString;
                    });

                    return resource;
                }
                catch (Exception)
                {
                    //TODO: log error
                    return null;
                }
            }
        }
    }
}
