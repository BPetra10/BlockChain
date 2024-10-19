using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace BlockChainProject
{
    /// <summary>
    /// Represents a block in the blockchain.
    /// </summary>
    internal class Block
    {
        public int Index { get; private set; } 
        public long Timestamp { get; private set; }
        public string BlockHash { get; private set; } 
        public string PreviousHash { get; private set; }
        public List<Transaction> Transactions { get; private set; }
        public string MerkleRoot { get; private set; } 
        public int Nonce { get; private set; } 
        public int Difficulty { get; private set; } 
        

        /// <summary>
        /// Initializes a new instance of the Block class.
        /// </summary>
        public Block(int index, string previousHash, List<Transaction> transactions, int difficulty)
        {
            Index = index; 
            PreviousHash = previousHash; 
            Transactions = transactions; 
            Timestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds(); // Get current Unix timestamp
            MerkleRoot = new MerkleTree(GenerateTransactionData(transactions)).Root; ; // Build the Merkle root from transaction signatures
            Difficulty = difficulty; 
            Nonce = 0; 
            BlockHash = ""; 
        }

        /// <summary>
        /// Mines the block by finding a valid hash with the required number of leading zeros (PoW condition).
        /// </summary>
        public void MineBlock()
        {
            string prefixStr = new string('0', Difficulty); 
            while (true)
            {
                BlockHash = CalculateHash(); 
                if (BlockHash.StartsWith(prefixStr)) 
                {
                    Console.WriteLine($"Block mined with nonce: {Nonce} and hash: {BlockHash}"); 
                    break; 
                }
                Nonce++; 
            }
        }

        /// <summary>
        /// Calculates the SHA-256 hash of the block's attributes.
        /// </summary>
        /// <returns>The calculated hash as a hexadecimal string.</returns>
        private string CalculateHash()
        {
            var blockString = $"{Index}{Timestamp}{string.Join(",", Transactions)}{MerkleRoot}{PreviousHash}{Nonce}";


            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(blockString));
                /*BitConverter.ToString(bytes): Converts the byte array into a string
                 where each byte is represented as a two-digit hexadecimal value,
                 separated by hyphens.
                    This is why we have to replace hypens to "". 
                 */
                return BitConverter.ToString(bytes).Replace("-", "").ToLower();
            }
        }

        /// <summary>
        ///  Takes a list of Transaction objects and returns a list of formatted strings 
        ///  that describe each transaction in a readable format. 
        ///  This makes the Merkle tree building easier.
        /// </summary>
        /// <returns>transactionData string list.</returns>
        private List<string> GenerateTransactionData(List<Transaction> transactions)
        {
            var transactionData = new List<string>();
            foreach (var transaction in transactions)
            {
                transactionData.Add($"{transaction.From} pays {transaction.To} {transaction.Amount}");
            }
            return transactionData;
        }

    }
}
