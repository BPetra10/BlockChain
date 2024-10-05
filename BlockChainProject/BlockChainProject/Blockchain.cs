using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlockChainProject
{
    /// <summary>
    /// Represents a blockchain with its blocks. 
    /// </summary>
    internal class Blockchain
    {
        private List<Block> chain; 
        public int Difficulty { get; private set; } 

        public Blockchain(int difficulty)
        {
            chain = new List<Block>(); 
            Difficulty = difficulty;
            CreateBlock(new List<string>()); // Create the genesis block
        }

        /// <summary>
        /// Creates a new block, mining it, and adding to the chain.
        /// </summary>
        public Block CreateBlock(List<string> transactions)
        {
            int newBlockIndex = chain.Count + 1;

            var block = new Block(newBlockIndex, LastBlockHash(), transactions, Difficulty); 
            
            block.MineBlock(); 
            chain.Add(block); 
            return block; 
        }

        /// <summary>
        /// Gets the hash of the last block in the chain.
        /// </summary>
        public string LastBlockHash()
        {
            return chain.Count > 0 ? chain[^1].BlockHash :"0"; 
        }

        /// <summary>
        /// Gets the blockchain.
        /// </summary>
        /// /// <returns>the list of blocks from the blockchain.</returns>
        public List<Block> GetChain()
        {
            return chain;
        }
    }
}
