using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MensajesClienteHTTP.Models
{
    public class MensajeDTO
    {
        public string Texto { get; set; } = "";
        public string ColorFondo { get; set; } = "#fff";
        public string ColorLetra { get; set; } = "#000";
    }
}
