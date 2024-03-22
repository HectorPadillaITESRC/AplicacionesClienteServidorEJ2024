using System.Net;
using System.Net.Sockets;
using System.Text;

TcpClient cliente = new();

Console.WriteLine("Escribe la IP del servidor:");
var ip=Console.ReadLine()??"127.0.0.1";

var ipe = new IPEndPoint(IPAddress.Parse(ip), 8000);
//Solicitar una conexion
cliente.Connect(ipe);

string cadena;
while((cadena = Console.ReadLine()) != null){
    byte[] buffer = Encoding.UTF8.GetBytes(cadena);
    var ns = cliente.GetStream();

    ns.Write(buffer, 0, buffer.Length);

}


