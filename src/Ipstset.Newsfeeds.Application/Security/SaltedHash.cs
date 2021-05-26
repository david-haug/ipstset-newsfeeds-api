using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace Ipstset.Newsfeeds.Application.Security
{
    public class SaltedHash
    {
        public string Salt { get; }
        public string Hash { get; }

        public static SaltedHash Create(string password, string salt, string key)
        {
            _hashKey = key;
            string hash = _calculateHash(salt, password);
            return new SaltedHash(salt, hash);
        }

        private SaltedHash(string s, string h)
        {
            Salt = s;
            Hash = h;
        }

        private static string _calculateHash(string salt, string password)
        {
            byte[] data = _toByteArray(salt + _hashKey + password);
            byte[] hash = _calculateHash(data);
            return Convert.ToBase64String(hash);
        }

        private static byte[] _calculateHash(byte[] data)
        {
            return new SHA256CryptoServiceProvider().ComputeHash(data);
        }

        private static byte[] _toByteArray(string s)
        {
            return System.Text.Encoding.UTF8.GetBytes(s);
        }

        private static string _hashKey;

    }
}
