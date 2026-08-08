using System.Security.Cryptography;

namespace Store.Helpers
{
    public  class GenerateOTP
    {
        public static string GenerateSecureOtp(int length = 6)
        {
            var bytes = new byte[length];
            RandomNumberGenerator.Fill(bytes);

            var otp = "";

            for (int i = 0; i < length; i++)
            {
                otp += (bytes[i] % 10).ToString();
            }

            return otp;
        }
    }
}
