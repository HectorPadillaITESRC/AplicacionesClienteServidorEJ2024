using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace MensajesClienteHTTP.Models
{
    public class ServerModel
    {
        public string NombreServer { get; set; } = "";
        public IPEndPoint? IPEndpoint { get; set; }
        public DateTime KeepAlive { get; set; }


     
    } 


}
