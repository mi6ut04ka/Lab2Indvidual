using Lab2Indvidual.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;


namespace Lab2Indvidual.UWP
{
    public sealed partial class LoginPage : Page
    {
        private AuthService authService = new AuthService();
        public LoginPage()
        {
            this.InitializeComponent();
        }

        private async void OnLoginButtonClick(object sender, RoutedEventArgs e)
        {
            string username = usernameTextBox.Text;
            string password = passwordTextBox.Password;

            try
            {
                AuthService.Instance.Login(username, password);
                Frame.Navigate(typeof(MainPage));
            }
            catch (UnauthorizedAccessException ex)
            {
                var dialog = new MessageDialog(ex.Message);
                await dialog.ShowAsync();
            }
        }
    }
}
