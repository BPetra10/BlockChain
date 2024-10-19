using System.Security.Cryptography;

namespace BlockChainProject
{
    internal class Program
    {
        static void Main(string[] args)
        {

            int difficulty = 4; // Number of leading zeros required in the hash
            var blockchain = new Blockchain(difficulty); // Create a new blockchain with the specified difficulty

            // Generate RSA keys for Alice
            using var aliceKeyPair = RSA.Create();
            //ExportParameters(false): Requesting the public key parameters only, without the private key.
            var alicePublicKey = aliceKeyPair.ExportParameters(false); // Public key for Alice
            var alicePrivateKey = aliceKeyPair.ExportParameters(true); // Private key for Alice

            // Create and sign transactions
            var transaction1 = new Transaction("Alice", "Bob", 5);
            transaction1.Sign(alicePrivateKey);

            var transaction2 = new Transaction("Alice", "Charlie", 2);
            transaction2.Sign(alicePrivateKey);

            var transaction3 = new Transaction("Alice", "Harry", 8);
            transaction3.Sign(alicePrivateKey);

            // Create blocks with signed transactions
            blockchain.CreateBlock(new List<Transaction> { transaction1});
            blockchain.CreateBlock(new List<Transaction> { transaction2, transaction3 });

            Console.WriteLine("Mining done. \n");

            // Print the details of the entire blockchain and verify transactions
            Console.WriteLine("Printing the entire blockchain:");
            foreach (var block in blockchain.GetChain())
            {
                Console.WriteLine($"Index: {block.Index}");
                Console.WriteLine($"Nonce: {block.Nonce}");
                Console.WriteLine($"Timestamp: {block.Timestamp}");
                Console.WriteLine($"Hash: {block.BlockHash}");
                Console.WriteLine($"Previous Hash: {block.PreviousHash}");
                Console.WriteLine($"Merkle root: {block.MerkleRoot}");

                foreach (var transaction in block.Transactions)
                {
                    Console.WriteLine($"Transaction: {transaction.From} pays {transaction.To} {transaction.Amount}");
                    bool isValid = transaction.Verify(alicePublicKey); 
                    Console.WriteLine($"Signature valid: {isValid}");
                }
                Console.WriteLine();
            }

        }
    }
}
