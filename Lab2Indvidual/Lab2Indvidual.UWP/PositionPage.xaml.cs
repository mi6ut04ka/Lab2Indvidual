using Lab2Indvidual.Services;
using System;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;


namespace Lab2Indvidual.UWP
{

    public sealed partial class PositionPage : Page
    {
        private PositionService positionService = new PositionService();
        private AuthService authService = AuthService.Instance;

        public PositionPage()
        {
            this.InitializeComponent();
            positionService = new PositionService();
            this.DataContext = positionService;

        }
        
        private void PositionPage_Loaded(object sender, RoutedEventArgs e)
        {
            if (!authService.IsAuthenticated || authService.GetCurrentUserRole() != AuthService.UserRole.Boss)
            {
                Console.WriteLine(authService.IsAuthenticated);
                Frame.Navigate(typeof(LoginPage));
                return;
            }

        }

        private void OnBackButtonClick(object sender, RoutedEventArgs e)
        {
            if (Frame.CanGoBack)
            {
                Frame.GoBack();
            }
        }

        private async void OnAddPositionClick(object sender, RoutedEventArgs e)
        {
            string newPosition = positionTextBox.Text.Trim();

            if (string.IsNullOrEmpty(newPosition))
            {
                var dialog = new MessageDialog("Введите название должности.");
                await dialog.ShowAsync();
                return;
            }
            positionService.AddPosition(newPosition);

            positionTextBox.Text = string.Empty;

            var successDialog = new MessageDialog("Должность успешно добавлена.");
            await successDialog.ShowAsync();
        }
        private async void OnDeletePositionClick(object sender, RoutedEventArgs e)
        {
            var button = (Button)sender;
            string positionToDelete = button.Tag.ToString();

            var dialog = new MessageDialog($"Вы уверены, что хотите удалить должность: {positionToDelete}?");
            dialog.Commands.Add(new UICommand("Удалить") { Id = 0 });
            dialog.Commands.Add(new UICommand("Отмена") { Id = 1 });
            var result = await dialog.ShowAsync();

            if ((int)result.Id == 0)
            {
                positionService.Positions.Remove(positionToDelete);
                positionService.SavePositions();

                var successDialog = new MessageDialog("Должность успешно удалена.");
                await successDialog.ShowAsync();
            }
        }
        private async void OnEditPositionClick(object sender, RoutedEventArgs e)
        {
            var button = (Button)sender;
            string positionToEdit = button.Tag.ToString();

            var editDialog = new ContentDialog
            {
                Title = "Редактировать должность",
                Content = new TextBox
                {
                    Text = positionToEdit,
                    Name = "editTextBox"
                },
                PrimaryButtonText = "Сохранить",
                CloseButtonText = "Отмена"
            };

            var result = await editDialog.ShowAsync();

            if (result == ContentDialogResult.Primary)
            {
                var editTextBox = (TextBox)editDialog.Content;
                string newPosition = editTextBox.Text.Trim();

                if (!string.IsNullOrEmpty(newPosition))
                {
                    int index = positionService.Positions.IndexOf(positionToEdit);
                    positionService.Positions[index] = newPosition;
                    positionService.SavePositions(); 
                    var successDialog = new MessageDialog("Должность успешно изменена.");
                    await successDialog.ShowAsync();
                }
                else
                {
                    var errorDialog = new MessageDialog("Название должности не может быть пустым.");
                    await errorDialog.ShowAsync();
                }
            }
        }
    }

}
