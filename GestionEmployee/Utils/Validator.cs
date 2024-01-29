using GestionEmployee.Entities;
using Microsoft.Extensions.FileSystemGlobbing.Internal;
using System.Text.RegularExpressions;

namespace GestionEmployee.Utils
{
    public static class Validator
    {
   
        public static void isStringValid(string message,string property ,int minimum, int maximum) { 
        
            if( message == null)
            {
                throw new Exception($"{property} ne peux pas être vide ");
            }

            message = message.Trim();

            if (message.Length < minimum) {
                throw new Exception($"{property} doit être supèrieur à {minimum} charactères ");
            }
            if (message.Length > maximum)
            {
                throw new Exception($"{property} doit être inférieur à {minimum} charactères");

            }        
        }
        public static void noSpecialCharacterAllowed(string str, string property)
        {
            string pattern = @"^[A-Za-z0-9 ]+$";
            Regex regex = new Regex(pattern);
            if (!regex.IsMatch(str))
            {
                throw new Exception($"{property} ne doit pas contenir doit être contenir de charactère spéciaux");
            }
        }
        
        public static void isMailValid(string email) {
            string mailPattern = @"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$";
                        Regex regex = new Regex(mailPattern);
            if (!regex.IsMatch(email))
            {
                throw new Exception("Le mail n'est pas valide");
            }

        }
        public static void IsValidPhoneNumber(string phoneNumber)
        {
            string phonePattern = @"^(?:(?:\+|00)33[\s.-]{0,3}(?:\(0\)[\s.-]{0,3})?|0)[1-9](?:(?:[\s.-]?\d{2}){4}|\d{2}(?:[\s.-]?\d{3}){2})$";
            Regex regex = new Regex(phonePattern);
            if (!regex.IsMatch(phoneNumber))
            {
                throw new Exception("Le téléphone n'est pas valide");
            }
        }

        public static void hasMinimumWorkingAge(DateTime BirthDate)
        {
            var today = DateTime.Today;
            var a = (today.Year * 100 + today.Month) * 100 + today.Day;
            var b = (BirthDate.Year * 100 + BirthDate.Month) * 100 + BirthDate.Day;
            var age = (a - b) / 10000;
            if (age < 16)
            {
                throw new Exception("L'employée n'a pas l'âge minimum requis pour travailler");
            }

        }

        public static void isEndDateGtStartDate (DateTime? endDate, DateTime startDate) {

            if ( startDate >= endDate)
            {
                throw new Exception($"Echec de création de la présence la date de début est supérieur à la date de fin");
            }
        }

        public static void isInsideWorkHours (DateTime startDate, DateTime endDate)
        {

            isEndDateGtStartDate(endDate, startDate);
            var minimumHour = new DateTime(2013, 9, 14, 7, 30, 0).TimeOfDay;
             var maximumHour = new DateTime(2013,9,14,19,30,0).TimeOfDay;
            if (startDate.TimeOfDay < minimumHour)
            {
                throw new Exception("C'est en dehors des heures de travail");
            }
            
            if (endDate.TimeOfDay > maximumHour) {
                throw new Exception("C'est en dehors des heures de travail");
            }
        }
    }
}
