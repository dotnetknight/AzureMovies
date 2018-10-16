using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace AzureMovies
{
    public class PasswordHashing
    {
        public static string Hash(string value)
        {
            var bytes = new UTF8Encoding().GetBytes(value);
            byte[] hashBytes;
            using (var algorithm = new System.Security.Cryptography.SHA512Managed())
            { hashBytes = algorithm.ComputeHash(bytes); }
            string Hash = Convert.ToBase64String(hashBytes);
            return Hash;
        }
    }
}