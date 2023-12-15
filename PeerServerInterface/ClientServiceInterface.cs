using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace PeerServerInterface
{
    [ServiceContract]
    public interface ClientServiceInterface
    {

        [OperationContract]
        bool CheckIsJobAvailabe();

        [OperationContract]
        void GetAvailabeJob(out string jobCodeEncoded, out string hash);

        [OperationContract]
        void UploadJobSolution(string solutionEncoded);



    }
}
