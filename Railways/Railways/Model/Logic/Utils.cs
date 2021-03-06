﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;

namespace Railways
{  
    public static class ModelUtils
    {

        /// <summary>
        /// Вычисление MD5-хэша строки
        /// </summary>
        /// <param name="sourceString"></param>
        /// <returns></returns>
        public static String EncryptString(String sourceString)
        {
            byte[] hash = Encoding.ASCII.GetBytes(sourceString);
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] hashenc = md5.ComputeHash(hash);
            string result = "";      
            foreach (var b in hashenc)
            {
                result += b.ToString("x2");
            }
            return result;
        }
        /// <summary>
        /// Проверка на соответствие MD5-хэша хэшу заданной строки
        /// </summary>
        /// <param name="hash"></param>
        /// <param name="sourceString"></param>
        /// <returns></returns>
        public static bool CorrectHash(String hash, String sourceString)
        {
            if (EncryptString(sourceString).Equals(hash))
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Приобразование результата сложения даты и сдвига даты в минутах в вид "ЧЧ:ММ"
        /// </summary>
        /// <param name="departureDateTime"></param>
        /// <param name="TimeOffset"></param>
        /// <returns></returns>
        public static String FormatDateOffset(DateTime departureDateTime, int? timeOffset)
        {
            return String.Format("t: {0:t}", departureDateTime.AddMinutes((double)timeOffset));
        }

        
    }
}
