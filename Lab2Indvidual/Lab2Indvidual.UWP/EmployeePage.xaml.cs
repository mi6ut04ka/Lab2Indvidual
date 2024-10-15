using Lab2Indvidual.Models;
using Lab2Indvidual.Services;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;



namespace Lab2Indvidual.UWP
{

    public sealed partial class EmployeePage : Page
    {
        private EmployeeService employeeService;
        private PositionService positionService;
        private AuthService authService = AuthService.Instance;

        private void EmployeePage_Loaded(object sender, RoutedEventArgs e)
        {
            if (!authService.IsAuthenticated || authService.GetCurrentUserRole() != AuthService.UserRole.HR)
            {
                Frame.Navigate(typeof(LoginPage));
                return;
            }

        }


        public EmployeePage()
        {
            this.InitializeComponent();

            employeeService = new EmployeeService();
            positionService = new PositionService();
            this.DataContext = employeeService;
            positionComboBox.ItemsSource = positionService.Positions;
        }

        private async void OnAddEmployeeClick(object sender, RoutedEventArgs e)
        {
            string fullName = fullNameTextBox.Text.Trim();
            string position = positionComboBox.SelectedItem as string;
            int yearOfJoining;
            decimal salary;

            if (string.IsNullOrEmpty(fullName) || string.IsNullOrEmpty(position) ||
                !int.TryParse(yearOfJoiningTextBox.Text, out yearOfJoining) ||
                !decimal.TryParse(salaryTextBox.Text, out salary))
            {
                var errorDialog = new MessageDialog("Пожалуйста, заполните все поля корректно.");
                await errorDialog.ShowAsync();
                return;
            }
            var newEmployee = new Employee
            {
                FullName = fullName,
                Position = position,
                YearOfJoining = yearOfJoining,
                Salary = salary
            };

            employeeService.AddEmployee(newEmployee);
            fullNameTextBox.Text = string.Empty;
            yearOfJoiningTextBox.Text = string.Empty;
            salaryTextBox.Text = string.Empty;

            var successDialog = new MessageDialog("Сотрудник успешно добавлен.");
            await successDialog.ShowAsync();
        }
        private async void OnDeleteEmployeeClick(object sender, RoutedEventArgs e)
        {
            var button = (Button)sender;
            Employee employeeToDelete = button.Tag as Employee;

            var dialog = new MessageDialog($"Вы уверены, что хотите удалить сотрудника: {employeeToDelete.FullName}?");
            dialog.Commands.Add(new UICommand("Удалить") { Id = 0 });
            dialog.Commands.Add(new UICommand("Отмена") { Id = 1 });
            var result = await dialog.ShowAsync();

            if ((int)result.Id == 0)
            {
                employeeService.RemoveEmployee(employeeToDelete);

                var successDialog = new MessageDialog("Сотрудник успешно удалён.");
                await successDialog.ShowAsync();
            }
        }

        private async void OnEditEmployeeClick(object sender, RoutedEventArgs e)
        {
            var button = (Button)sender;
            Employee employeeToEdit = button.Tag as Employee;
            var fullNameTextBox = new TextBox { Text = employeeToEdit.FullName };
            var positionComboBox = new ComboBox { ItemsSource = positionService.Positions, SelectedItem = employeeToEdit.Position };
            var yearOfJoiningTextBox = new TextBox { Text = employeeToEdit.YearOfJoining.ToString() };
            var salaryTextBox = new TextBox { Text = employeeToEdit.Salary.ToString() };

            var editDialog = new ContentDialog
            {
                Title = "Редактировать сотрудника",
                Content = new StackPanel
                {
                    Children =
            {
                fullNameTextBox,
                positionComboBox,
                yearOfJoiningTextBox,
                salaryTextBox
            }
                },
                PrimaryButtonText = "Сохранить",
                CloseButtonText = "Отмена"
            };

            var result = await editDialog.ShowAsync();

            if (result == ContentDialogResult.Primary)
            {
                string fullName = fullNameTextBox.Text;
                string position = positionComboBox.SelectedItem as string;
                int yearOfJoining;
                decimal salary;

                if (!int.TryParse(yearOfJoiningTextBox.Text, out yearOfJoining) || !decimal.TryParse(salaryTextBox.Text, out salary))
                {
                    var errorDialog = new MessageDialog("Ошибка в данных.");
                    await errorDialog.ShowAsync();
                    return;
                }

                var updatedEmployee = new Employee
                {
                    FullName = fullName,
                    Position = position,
                    YearOfJoining = yearOfJoining,
                    Salary = salary
                };

                employeeService.UpdateEmployee(employeeToEdit, updatedEmployee);

                var successDialog = new MessageDialog("Сотрудник успешно обновлён.");
                await successDialog.ShowAsync();
            }
        }
        private void OnSortChanged(object sender, SelectionChangedEventArgs e)
        {
            var comboBox = sender as ComboBox;
            var selectedOption = comboBox.SelectedItem as ComboBoxItem;
            string sortBy = selectedOption.Tag.ToString(); 

            SortEmployees(sortBy);
        }

        private void SortEmployees(string sortBy)
        {
            switch (sortBy)
            {
                case "FullName":
                    employeeService.Employees = new ObservableCollection<Employee>(
                        employeeService.Employees.OrderBy(emp => emp.FullName));
                    break;

                case "Salary":
                    employeeService.Employees = new ObservableCollection<Employee>(
                        employeeService.Employees.OrderBy(emp => emp.Salary));
                    break;

                case "YearOfJoining":
                    employeeService.Employees = new ObservableCollection<Employee>(
                        employeeService.Employees.OrderBy(emp => emp.YearOfJoining));
                    break;
            }

            employeesListView.ItemsSource = employeeService.Employees;
        }
        private async void OnSearchEmployeeClick(object sender, RoutedEventArgs e)
        {
            string searchName = searchTextBox.Text.ToLower();

            var foundEmployee = employeeService.Employees.FirstOrDefault(emp => emp.FullName.ToLower().Contains(searchName));

            if (foundEmployee != null)
            {
                var employeeInfoDialog = new ContentDialog
                {
                    Title = "Информация о сотруднике",
                    Content = new StackPanel
                    {
                        Children =
                {
                    new TextBlock { Text = $"Фамилия и инициалы: {foundEmployee.FullName}" },
                    new TextBlock { Text = $"Должность: {foundEmployee.Position}" },
                    new TextBlock { Text = $"Год поступления: {foundEmployee.YearOfJoining}" },
                    new TextBlock { Text = $"Оклад: {foundEmployee.Salary}" }
                }
                    },
                    CloseButtonText = "Закрыть"
                };

                await employeeInfoDialog.ShowAsync();
            }
            else
            {
                var errorDialog = new MessageDialog("Сотрудник с такой фамилией не найден.");
                await errorDialog.ShowAsync();
            }
        }
        private async void OnExportToFileClick(object sender, RoutedEventArgs e)
        {
            var selectedCondition = exportConditionComboBox.SelectedItem as ComboBoxItem;
            string condition = selectedCondition.Tag.ToString();
            string conditionValue = exportConditionValueTextBox.Text;

            IEnumerable<Employee> employeesToExport = employeeService.Employees;

            switch (condition)
            {
                case "StartsWith":
                    if (!string.IsNullOrEmpty(conditionValue))
                    {
                        employeesToExport = employeesToExport.Where(emp => emp.FullName.StartsWith(conditionValue, StringComparison.OrdinalIgnoreCase));
                    }
                    break;
                case "SalaryGreaterThan":
                    if (decimal.TryParse(conditionValue, out decimal salary))
                    {
                        employeesToExport = employeesToExport.Where(emp => emp.Salary > salary);
                    }
                    break;
                case "All":
                default:
                    break;
            }

            string json = JsonConvert.SerializeObject(employeesToExport, Formatting.Indented);

            var savePicker = new Windows.Storage.Pickers.FileSavePicker
            {
                SuggestedStartLocation = Windows.Storage.Pickers.PickerLocationId.DocumentsLibrary
            };
            savePicker.FileTypeChoices.Add("JSON File", new List<string>() { ".json" });
            savePicker.SuggestedFileName = "employees";

            Windows.Storage.StorageFile file = await savePicker.PickSaveFileAsync();
            if (file != null)
            {
                await Windows.Storage.FileIO.WriteTextAsync(file, json);

                var successDialog = new ContentDialog
                {
                    Title = "Успешно",
                    Content = "Список сотрудников успешно выгружен в файл.",
                    CloseButtonText = "OK"
                };
                await successDialog.ShowAsync();
            }
            else
            {
                var cancelDialog = new ContentDialog
                {
                    Title = "Отмена",
                    Content = "Сохранение файла отменено.",
                    CloseButtonText = "OK"
                };
                await cancelDialog.ShowAsync();
            }
        }
        private void OnExportConditionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedCondition = exportConditionComboBox.SelectedItem as ComboBoxItem;
            string condition = selectedCondition.Tag.ToString();

            if (condition == "StartsWith" || condition == "SalaryGreaterThan")
            {
                exportConditionValueTextBox.Visibility = Visibility.Visible;
            }
            else
            {
                exportConditionValueTextBox.Visibility = Visibility.Collapsed;
            }
        }

        private void OnBackButtonClick(object sender, RoutedEventArgs e)
        {
            if (Frame.CanGoBack)
            {
                Frame.GoBack();
            }
        }
    }
}
