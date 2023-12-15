using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace PeerServerInterface
{
    public class Solution
    {
        public string EncodedSolution { get; }
        public string Hash { get; }

        public Solution(string EncodedSolution, string Hash)
        {
            this.EncodedSolution = EncodedSolution;
            this.Hash = Hash;
        }
    }
}
