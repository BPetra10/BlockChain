using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace BlockChainProject
{
    /// <summary>
    /// Represents a Merkle tree for organizing and validating transactions.
    /// </summary>
    internal class MerkleTree
    {
        private List<string> transactions;
        public string Root { get; private set; }

        /// <summary>
        /// Initializes a new instance of the MerkleTree with the given transactions.
        /// </summary>
        /// <param name="transactions">List of transactions to include in the Merkle tree.</param>
        public MerkleTree(List<string> transactions)
        {
            this.transactions = transactions;
            this.Root = BuildTree(transactions);
        }

        /// <summary>
        /// Builds the Merkle tree from the transactions.
        /// </summary>
        /// <param name="transactions">List of transactions to process.</param>
        /// <returns>The root hash of the Merkle tree.</returns>
        private string BuildTree(List<string> transactions)
        {
            // Base case: if there are no transactions, return an empty string
            if (transactions.Count == 0) 
                return string.Empty;

            // If there is only one transaction, hash and return it as the root
            if (transactions.Count == 1) 
                return Hash(transactions[0]);

            // Hash all transactions and store them in a list
            List<string> hashedTransactions = new List<string>();
            foreach (var tran in transactions)
            {
                hashedTransactions.Add(Hash(tran));
            }

            // Build the tree by continuously hashing pairs of transactions
            while (hashedTransactions.Count > 1)
            {
                List<string> newHashedTransactions = new List<string>(); //next level hashes
                for (int i = 0; i < hashedTransactions.Count - 1; i += 2)
                {
                    // Hash pairs of transactions
                    newHashedTransactions.Add(Hash(hashedTransactions[i] + hashedTransactions[i + 1]));
                }

                /* If there's an odd number of hashed transactions, then
                   one transaction is doubled (in here, that is the last transaction), 
                   and its hash is concatenated with itself.
                */
                if (hashedTransactions.Count % 2 == 1)
                {
                    newHashedTransactions.Add(hashedTransactions[^1]); // Use the last item if odd
                }

                /* Move to the next level, the list of hashed transactions
                 is updated for the next iteration.*/
                hashedTransactions = newHashedTransactions; 
            }
            /* Once the loop is done, the last remaining hash in hashedTransactions 
               is returned as the root hash of the Merkle tree.*/
            return hashedTransactions[0];  
        }

        /// <summary>
        /// Computes the SHA-256 hash of the given data.
        /// </summary>
        /// <param name="data">The data to hash.</param>
        /// <returns>The hexadecimal hash of the data.</returns>
        private string Hash(string data)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(data));
                return BitConverter.ToString(bytes).Replace("-", "").ToLower(); 
            }
        }

    }
}
