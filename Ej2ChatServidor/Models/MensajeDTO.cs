﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ej2ChatServidor.Models
{
    public class MensajeDTO
    {
        public string Mensaje { get; set; } = "";
        public string Origen { get; set; } = "";
        public DateTime Fecha { get; set; }
    }
}
