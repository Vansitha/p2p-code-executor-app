using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebServer.Models;

namespace WebServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ConnectionController : ControllerBase
    {
        // Client DB is initilized in Program.cs server start up script
        private readonly ClientDB _clientDb;
        public ConnectionController(ClientDB clientDb) {
            _clientDb = clientDb;
        }

        [HttpPost]
        [Route("/register")]
        public IActionResult RegisterClient([FromBody] Client newClient)
        {
            if (newClient.IpAddress == null) return StatusCode(400, new { message = "ip address is required" });

            try
            {
                newClient.TotalJobsCompleted = 0;
                _clientDb.RegisterClient(newClient);
                return Ok(new { message = "Client added" });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "An error occurred while registering the client", error = ex.Message });
            }
        }

        [HttpPost]
        [Route("/remove")]
        public IActionResult RemoveClient([FromBody] Client client)
        {
            try
            {
                bool isRemoved = _clientDb.DeleteClient(client);
                if (isRemoved)
                {
                    return Ok();   
                } else
                {
                    return StatusCode(400, "Invalid client details");
                }
            } catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "An error occurred while removing the client", error = ex.Message });
            }
        }

        [HttpGet]
        [Route("/peers")]
        public IActionResult GetPeers()
        {
            try
            {
                List<Client> peers = _clientDb.GetAllClients();
                return Ok(peers);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "An error occurred while fetching peers", error = ex.Message });
            }
        }

        [HttpPost]
        [Route("/jobComplete")]
        public IActionResult UpdateClientDetails([FromBody] Client client)
        {
            bool isUpdated = _clientDb.UpdateClient(client);
            if (isUpdated) return Ok();    
            return StatusCode(400, new { message = "invalid client details" });
        }
    }
}
