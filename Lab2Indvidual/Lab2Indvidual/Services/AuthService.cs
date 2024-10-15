using Lab2Indvidual.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lab2Indvidual.Services
{

        public class AuthService
        {
            private static AuthService _instance;

            public static AuthService Instance
            {
                get
                {
                    if (_instance == null)
                    {
                        _instance = new AuthService();
                    }
                    return _instance;
                }
            }

            public enum UserRole
            {
                None,
                Boss,
                HR
            }

            private UserRole currentUserRole = UserRole.None;

            public bool IsAuthenticated => currentUserRole != UserRole.None;

            public void Login(string username, string password)
            {
                if (username == "boss" && password == "boss")
                {
                    currentUserRole = UserRole.Boss;
                }
                else if (username == "hr" && password == "hr")
                {
                    currentUserRole = UserRole.HR;
                }
                else
                {
                    throw new UnauthorizedAccessException("Неверные учетные данные");
                }
            }

            public void Logout()
            {
                currentUserRole = UserRole.None;
            }

            public UserRole GetCurrentUserRole() => currentUserRole;
        }

    
 }
