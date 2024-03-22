//TcpListener, TcpClient

//Iniciamos un servidor TCP

using System.Net.Sockets;
using System.Text;

TcpListener server = new(System.Net.IPAddress.Any, 8000);
server.Start();

while (true)
{

    TcpClient client = server.AcceptTcpClient();

    Console.WriteLine("Cliente aceptado: " + client.Client.RemoteEndPoint?.ToString()); 
    
    Thread hilo = new(() =>
    {
        AtenderCliente(client);
    });
    hilo.IsBackground = true;
    hilo.Start();

}

//Thread hilo2 = new Thread(new ParameterizedThreadStart(AtenderCliente));
//hilo2.Start(client);

void AtenderCliente(TcpClient client)
{
    while (true)
    {
        if (client.Available > 0)
        {
            var ns = client.GetStream();

            byte[] buffer = new byte[client.Available];
            ns.Read(buffer, 0, buffer.Length);

            Console.WriteLine(client.Client.RemoteEndPoint?.ToString() + ": " +
                Encoding.UTF8.GetString(buffer));

            //Flushing

        }
    }
}