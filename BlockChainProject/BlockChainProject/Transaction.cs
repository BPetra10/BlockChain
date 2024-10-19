using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace BlockChainProject
{
    /// <summary>
    /// Represents a financial transaction between two parties, using digital signature.
    /// </summary>
    internal class Transaction
    {
        public string From { get; }
        public string To { get; }
        public decimal Amount { get; }
        public byte[] Signature { get; private set; }

        public Transaction(string from, string to, decimal amount)
        {
            From = from;
            To = to;
            Amount = amount;
        }

        /// <summary>
        /// With the parameter, we sign the concatenated data, and store it inside Signature property.
        /// </summary>
        /// <param name="privateKey">The key used for digital signature.</param>
        public void Sign(RSAParameters privateKey)
        {
            using (var rsa = RSA.Create())
            {
                rsa.ImportParameters(privateKey);
                var dataToSign = $"{From}{To}{Amount}";
                var dataBytes = Encoding.UTF8.GetBytes(dataToSign);
                /*Padding: ensure that plaintext data is of a suitable length for encryption algorithms, 
                 * especially block ciphers.*/
                Signature = rsa.SignData(dataBytes, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);
            }
        }

        /// <summary>
        ///  Verifying the authenticity and integrity of signed data using RSA. 
        /// </summary>
        /// <param name="publicKey">The key used for verification.</param>
        public bool Verify(RSAParameters publicKey)
        {
            using (var rsa = RSA.Create())
            {
                rsa.ImportParameters(publicKey);
                var dataToVerify = $"{From}{To}{Amount}";
                var dataBytes = Encoding.UTF8.GetBytes(dataToVerify);
                return rsa.VerifyData(dataBytes, Signature, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);
            }
        }

    }
}
