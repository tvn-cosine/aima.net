using System;
using System.Security.Cryptography;

namespace aima.net.text.hashing
{
    public class TextHasher : IEquatable<string>
    {
        private const int saltSize = 16;
        private const int hashSize = 64;
        private const int hashIterationCount = 10000;

        private readonly byte[] salt;
        private readonly byte[] hash;
         
        private TextHasher()
        {
            salt = new byte[saltSize];
            hash = new byte[hashSize];
        }

        public TextHasher(string input)
            : this()
        {
            new RNGCryptoServiceProvider().GetBytes(salt);
            hash = new Rfc2898DeriveBytes(input, salt, hashIterationCount)
                .GetBytes(hashSize);
        }

        public TextHasher(byte[] salt, byte[] hash)
            : this()
        {
            Array.Copy(salt, 0, this.salt, 0, saltSize);
            Array.Copy(hash, 0, this.hash, 0, hashSize);
        }

        public byte[] Salt { get { return (byte[])salt.Clone(); } }
        public byte[] Hash { get { return (byte[])hash.Clone(); } }

        public bool Equals(string input)
        {
            byte[] test = new Rfc2898DeriveBytes(input, salt, hashIterationCount)
                .GetBytes(hashSize);

            for (int i = 0; i < hashSize; i++)
            {
                if (test[i] != hash[i])
                {
                    return false;
                }
            }

            return true;
        }
    }
}
