using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ej1ServidorTalleres.Models
{
    public class Taller
    {
        public string Nombre { get; set; } = null!;
        public List<Alumno> Alumnos { get; set; } = new();
    }
}
