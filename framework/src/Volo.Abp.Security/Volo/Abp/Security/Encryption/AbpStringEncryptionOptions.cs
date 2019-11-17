using System.Text;

namespace Volo.Abp.Security.Encryption
{
    /// <summary>
    /// Options used by <see cref="IStringEncryptionService"/>.
    /// </summary>
    public class AbpStringEncryptionOptions
    {
        /// <summary>
        /// This constant is used to determine the keysize of the encryption algorithm.
        /// Default value: 256.
        /// </summary>
        public int Keysize { get; set; }

        /// <summary>
        /// Default password to encrypt/decrypt texts.
        /// It's recommended to set to another value for security.
        /// Default value: "gsKnGZ041HLL4IM8"
        /// </summary>
        public string DefaultPassPhrase { get; set; }

        /// <summary>
        /// This constant string is used as a "salt" value for the PasswordDeriveBytes function calls.
        /// This size of the IV (in bytes) must = (keysize / 8).  Default keysize is 256, so the IV must be
        /// 32 bytes long.  Using a 16 character string here gives us 32 bytes when converted to a byte array.
        /// Default value: Encoding.ASCII.GetBytes("jkE49230Tf093b42")
        /// </summary>
        public byte[] InitVectorBytes { get; set; }

        /// <summary>
        /// Default value: Encoding.ASCII.GetBytes("hgt!16kl")
        /// </summary>
        public byte[] DefaultSalt { get; set; }

        public AbpStringEncryptionOptions()
        {
            Keysize = 256;
            DefaultPassPhrase = "gsKnGZ041HLL4IM8";
            InitVectorBytes = Encoding.ASCII.GetBytes("jkE49230Tf093b42");
            DefaultSalt = Encoding.ASCII.GetBytes("hgt!16kl");
        }
    }
}