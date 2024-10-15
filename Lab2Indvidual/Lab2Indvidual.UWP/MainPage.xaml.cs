using Lab2Indvidual.Services;
using System;
using System.Collections.Generic;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Xamarin.Forms.Platform.UWP;

namespace Lab2Indvidual.UWP
{
    public sealed partial class MainPage : WindowsPage
    {
        private AuthService authService = new AuthService();
        public List<string> employees = new List<string>();

        public MainPage()
        {
            this.InitializeComponent();
            Loaded += Page_Loaded;
        }

        private void OnEmployeesButtonClick(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(EmployeePage));
        }

        private void OnPositionsButtonClick(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(PositionPage));
        }



        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            if (!AuthService.Instance.IsAuthenticated)
            {
                Frame.Navigate(typeof(LoginPage));
                return;
            }
            var userRole = AuthService.Instance.GetCurrentUserRole();
            if (userRole == AuthService.UserRole.Boss)
            {
                positionsButton.Visibility = Visibility.Collapsed;
                employeesButton.Visibility = Visibility.Visible;
               
            }
            else if (userRole == AuthService.UserRole.HR)
            {
                positionsButton.Visibility = Visibility.Visible;
                employeesButton.Visibility = Visibility.Collapsed;

            }
        }
        private void LogoutButton_Click(object sender, RoutedEventArgs e)
        {
            authService.Logout();
            Frame.Navigate(typeof(LoginPage));
        }
    }
}
