using Lab2Indvidual.Models;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Xamarin.Essentials;

namespace Lab2Indvidual.Services
{
    public class EmployeeService
    {
        private List<Employee> employees = new List<Employee>();
        private const string EmployeesKey = "employees";
        private PositionService positionService;
        public ObservableCollection<Employee> Employees { get; set; }
        public ObservableCollection<string> Positions => positionService.Positions;
        public EmployeeService()
        {
            Employees = LoadEmployees();
            positionService = new PositionService();
        }

        private ObservableCollection<Employee> LoadEmployees()
        {
            string json = Preferences.Get(EmployeesKey, string.Empty);
            if (string.IsNullOrEmpty(json))
            {
                return new ObservableCollection<Employee>();
            }
            var employeeList = JsonConvert.DeserializeObject<List<Employee>>(json);
            return new ObservableCollection<Employee>(employeeList);
        }
        public void SaveEmployees()
        {
            string json = JsonConvert.SerializeObject(Employees);
            Preferences.Set(EmployeesKey, json);
        }
        public void AddEmployee(Employee employee)
        {
            Employees.Add(employee);
            SaveEmployees();
        }
        public void RemoveEmployee(Employee employee)
        {
            Employees.Remove(employee);
            SaveEmployees();
        }
        public void UpdateEmployee(Employee oldEmployee, Employee updatedEmployee)
        {
            int index = Employees.IndexOf(oldEmployee);
            if (index >= 0)
            {
                Employees[index] = updatedEmployee;
                SaveEmployees();
            }
        }

    }
}
