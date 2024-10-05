namespace BlockChainProject
{
    internal class Program
    {
        static void Main(string[] args)
        {

            int difficulty = 4; // Number of leading zeros required in the hash
            var blockchain = new Blockchain(difficulty); // Create a new blockchain with the specified difficulty

            // Add some transactions to the blockchain
            blockchain.CreateBlock(new List<string> { "Alice pays Bob 5 BTC", "Alice pays Charlie 2 BTC"});
            blockchain.CreateBlock(new List<string> { "Charlie pays Dave 1 BTC" });
            Console.WriteLine("Mining done. \n");

            Console.WriteLine("Printing the entire blockchain:");
            foreach (var block in blockchain.GetChain())
            {
                Console.WriteLine($"Index: {block.Index}");
                Console.WriteLine($"Nonce: {block.Nonce}");
                Console.WriteLine($"Timestamp: {block.Timestamp}");
                Console.WriteLine($"Hash: {block.BlockHash}");
                Console.WriteLine($"Previous Hash: {block.PreviousHash}");
                Console.WriteLine($"Merkle root: {block.MerkleRoot}");
                Console.WriteLine($"Transactions: {string.Join(", ", block.Transactions)}\n");
            }

        }
    }
}
