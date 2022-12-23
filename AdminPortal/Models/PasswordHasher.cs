﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;

namespace AdminPortal.Models
{
    public class PasswordHasher
    {
        public class HashSalt
        {
            public string Hash { get; set; }
            public string Salt { get; set; }
        }
        public static HashSalt GenerateSaltedHash(string password)
        {
            var saltBytes = new byte[20];
            var provider = new RNGCryptoServiceProvider();

            provider.GetNonZeroBytes(saltBytes);
            var salt = Convert.ToBase64String(saltBytes);

            var rfc2898DeriveBytes = new Rfc2898DeriveBytes(password, saltBytes, 10000);
            var hashPassword = Convert.ToBase64String(rfc2898DeriveBytes.GetBytes(256));

            HashSalt hashSalt = new HashSalt { Hash = hashPassword, Salt = salt };
            return hashSalt;
        }

    }
}
