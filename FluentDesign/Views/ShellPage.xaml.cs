using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Core;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace FluentDesign.Views
{
    public sealed partial class ShellPage : Page
    {
        public string TitleBarTitle { get; set; }
        public double TitleBarHeight { get; set; }
        public Thickness TitleBarMargin { get; set; }
        public Thickness ContentMargin { get; set; }

        public ShellPage()
        {
            this.InitializeComponent();
            initializeTitleBar();

            ContentFrame.Navigated += (s, e) =>
            {
                SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility =
                    ContentFrame.CanGoBack ? AppViewBackButtonVisibility.Visible : AppViewBackButtonVisibility.Collapsed;
            };

            SystemNavigationManager.GetForCurrentView().BackRequested += (bs, be) =>
            {
                if (ContentFrame.CanGoBack)
                {
                    be.Handled = true;
                    ContentFrame.GoBack();
                }
            };
        }

        private void MenuButton_Click(object sender, RoutedEventArgs e)
        {
            MainSplitView.IsPaneOpen = !MainSplitView.IsPaneOpen;
        }

        private void NavRadioButton_Checked(object sender, RoutedEventArgs e)
        {
            var s = sender as RadioButton;

            switch (s.Content)
            {
                case "Home":
                    ContentFrame.Navigate(typeof(HomePage));
                    break;

                case "Other":
                    ContentFrame.Navigate(typeof(OtherPage));
                    break;
            }
        }

        private void initializeTitleBar()
        {
            CoreApplicationViewTitleBar coreTileBar = CoreApplication.GetCurrentView().TitleBar;
            coreTileBar.ExtendViewIntoTitleBar = true;
            TitleBarHeight = coreTileBar.Height;
            TitleBarMargin = new Thickness(coreTileBar.SystemOverlayLeftInset + 12, 0, coreTileBar.SystemOverlayRightInset - 12, 0);
            ContentMargin = new Thickness(0, coreTileBar.Height, 0, 0);
            TitleBarTitle = Package.Current.DisplayName;
            Bindings.Update();

            coreTileBar.LayoutMetricsChanged += (s, e) =>
            {
                TitleBarHeight = s.Height;
                TitleBarMargin = new Thickness(s.SystemOverlayLeftInset + 12, 0, s.SystemOverlayRightInset - 12, 0);
                ContentMargin = new Thickness(0, coreTileBar.Height, 0, 0);
                Bindings.Update();
            };
        }
    }
}
