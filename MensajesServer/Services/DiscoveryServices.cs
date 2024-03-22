using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace MensajesServer.Services
{
    public class DiscoveryService
    {
        UdpClient server = new()
        {
            EnableBroadcast = true
        };

        IPEndPoint destino = new IPEndPoint(IPAddress.Broadcast, 7000);

        byte[] buffer;

        public DiscoveryService()
        {
            var usuario = Environment.UserName;
            buffer = Encoding.UTF8.GetBytes(usuario);

            Saludar();  //Cuando lo creamos saludamos en la red
            new Thread(RecibirSaludo) { IsBackground = true }.Start(); //Para esperar que nos saluden
            new Thread(StillAlive) { IsBackground = true }.Start(); //Para informar que seguimos vivos

        }

        public void Saludar()
        {
            server.Send(buffer, buffer.Length, destino);
        }

        private void StillAlive()
        {
            while (true)
            {
                Thread.Sleep(TimeSpan.FromSeconds(30));
                Saludar();
            }
        }

        private void RecibirSaludo() //Voy a responder el saludo cuando me saludan
        {
            UdpClient udp2 = new UdpClient(7001);
            while (true)
            {
                IPEndPoint remoto = new(IPAddress.Any, 0);
                byte[] buffer = udp2.Receive(ref remoto);

                server.Send(buffer, buffer.Length, remoto);
            }
        }

    }
}
