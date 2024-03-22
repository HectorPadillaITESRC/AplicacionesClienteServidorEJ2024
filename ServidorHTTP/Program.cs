//HTTPListener

using System.Net;
using System.Text;

HttpListener server = new();

var ip = Dns.GetHostAddresses(Dns.GetHostName())
    .Where(x => x.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
    .Select(x => x.ToString()).FirstOrDefault();

server.Prefixes.Add("http://localhost:10000/");
//server.Prefixes.Add($"http://{ip}:10000/");

server.Start();



Console.WriteLine("Escuchando en la ip: " + ip);

while (true)
{
    HttpListenerContext context = server.GetContext();

    string respuesta = "";
    var nombre = context.Request.QueryString["nombre"];

    //context.Request.HttpMethod == HttpMethod.Get;

    if (nombre != null)//manda una variable llamada nombre
    {
        Console.WriteLine(nombre + " ha hecho una petición.");
        respuesta = $"<html><body><h1>Saludos {nombre}</h1></body></html>";
    }
    else
    {
        respuesta = "<html><body><h1>Respuesta desde el servidor</h1></body></html>";
    }

    byte[] buffer = Encoding.UTF8.GetBytes(respuesta);
    context.Response.ContentLength64 = buffer.Length;
    var ns = context.Response.OutputStream;
    ns.Write(buffer, 0, buffer.Length);
    context.Response.StatusCode = 200;//OK
    context.Response.Close();


}