using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;

namespace Application.Services
// gestão de utilizadores.
{
    internal class UserService
    {
        /* 
        * Valida um utilizador com base na sua idade e força da password.
        * A idade deve estar entre 18 e 70 anos.
        * A password deve conter letras maiúsculas, minúsculas, números e caracteres especiais.
        * 
        * Retorna true se o utilizador for válido, caso contrário false e uma mensagem de erro.
        */
        public bool ValidateUser(Users user, out string errorMessage)
        {
            errorMessage = string.Empty;
            int age = GetAge(user);

            if (age < 18 || age > 70)
            {
                errorMessage = "O utilizador deve ter entre 18 e 70 anos.";
                return false;
            }

            if (!IsStrongPassword(user.Password))
            {
                errorMessage = "A password deve conter letras maiúsculas, minúsculas, números e caracteres especiais.";
                return false;
            }

            return true;
        }

        /* 
        * Calcula a idade do utilizador com base na sua data de nascimento.
        */
        private int GetAge(Users user)
        {
            var today = DateOnly.FromDateTime(DateTime.Today);
            int age = today.Year - user.DateOfBirth.Year;
            if (today < new DateOnly(today.Year, user.DateOfBirth.Month, user.DateOfBirth.Day))
                age--;
            return age;
        }

        /* 
        * Verifica se a password é forte.
        * Uma password forte deve ter pelo menos 8 caracteres, incluindo letras maiúsculas, minúsculas, números e caracteres especiais.
        */
        private bool IsStrongPassword(string password)
        {
            if (string.IsNullOrEmpty(password))
                return false;

            return password.Length >= 8 &&
                   password.Any(char.IsUpper) &&
                   password.Any(char.IsLower) &&
                   password.Any(char.IsDigit) &&
                   password.Any(ch => "!@#$%^&*()_+-=[]{}|;:,.<>?".Contains(ch));
        }
    }
}
