namespace EzCommon.Infra.Security
{
    public static class CryptographyService
    {
        public static string CreateHash(string value)
        {
            var stringBytes = System.Text.Encoding.ASCII.GetBytes(value);
            var hashBytes = System.Security.Cryptography.SHA256.Create().ComputeHash(stringBytes);
            return System.Text.Encoding.ASCII.GetString(hashBytes);
        }
    }
}
