namespace WebServer.Models
{
    public class ClientDB
    {
        // In-memory list of clients
        static List<Client> clients = new List<Client>();
 
        public void RegisterClient(Client client)
        {
            clients.Add(client);
        }

        public List<Client> GetAllClients()
        {
            return clients;
        }

        public bool UpdateClient(Client updatedClient)
        {
            Client existingClient = clients.FirstOrDefault(c => c.IpAddress == updatedClient.IpAddress && c.Port == updatedClient.Port);

            if (existingClient != null)
            {
                // Update the client's properties with the new values
                existingClient.Port = updatedClient.Port;
                existingClient.IpAddress = updatedClient.IpAddress;
                existingClient.TotalJobsCompleted = updatedClient.TotalJobsCompleted;
                return true;
            }
            else
            {
                // The client was not found, so no update was performed
                return false;
            }
        }

        public bool DeleteClient(Client client)
        {
            Client existingClient = clients.FirstOrDefault(c => c.IpAddress == client.IpAddress && c.Port == client.Port);
            return clients.Remove(existingClient);
        }
    }
}
