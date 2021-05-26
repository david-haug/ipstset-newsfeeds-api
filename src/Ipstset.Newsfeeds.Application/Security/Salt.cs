using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace Ipstset.Newsfeeds.Application.Security
{
    public class Salt
    {
        public Salt()
        {
            Value = _createSalt();
        }

        public string Value { get; set; }
        private const int saltLength = 12;

        private static string _createSalt()
        {
            byte[] r = _createRandomBytes(saltLength);
            return Convert.ToBase64String(r);
        }

        private static byte[] _createRandomBytes(int len)
        {
            byte[] r = new byte[len];
            new RNGCryptoServiceProvider().GetBytes(r);
            return r;
        }
    }
}
