using System;
using System.Collections.Generic;
using System.Text;

namespace Lab2Indvidual.Models
{
    public class Employee
    {
        public string FullName { get; set; } // Фамилия и инициалы
        public string Position { get; set; } // Должность
        public int YearOfJoining { get; set; } // Год поступления на работу
        public decimal Salary { get; set; } // Оклад
    }

}
