using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace PeerServerInterface
{
    public class Job
    {
        public string JobEncoded { get; }
        public string hash { get; }

        public Job(string JobEncoded, string hash)
        {
            this.JobEncoded = JobEncoded;
            this.hash = hash;
        }
    }
}





