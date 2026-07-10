using System;
using System.Security.Cryptography;
using System.Text;

namespace ArkPilot.Core
{
    public static class CryptoService
    {
        public static string Encrypt(string plainText)
        {
            if (string.IsNullOrEmpty(plainText))
                return "";

            byte[] data = Encoding.UTF8.GetBytes(plainText);

            byte[] encrypted =
                ProtectedData.Protect(
                    data,
                    null,
                    DataProtectionScope.CurrentUser);

            return Convert.ToBase64String(encrypted);
        }

        public static string Decrypt(string encryptedText)
        {
            if (string.IsNullOrEmpty(encryptedText))
                return "";

            try
            {
                byte[] data =
                    Convert.FromBase64String(encryptedText);

                byte[] decrypted =
                    ProtectedData.Unprotect(
                        data,
                        null,
                        DataProtectionScope.CurrentUser);

                return Encoding.UTF8.GetString(decrypted);
            }
            catch
            {
                // Ancienne configuration non chiffrée
                return encryptedText;
            }
        }
    }
}