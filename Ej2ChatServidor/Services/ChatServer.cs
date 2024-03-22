using Ej2ChatServidor.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace Ej2ChatServidor.Services
{
    public class ChatServer
    {
        TcpListener server = null!;
        List<TcpClient> clients = new List<TcpClient>();


        public void Iniciar()
        {
            server = new(new IPEndPoint(IPAddress.Any, 9000));
            server.Start();

            new Thread(Escuchar)
            {
                IsBackground = true
            }.Start();
        }

        public void Detener()
        {
            if (server != null)
            {
                server.Stop();
                foreach (var c in clients)
                {
                    c.Close();
                }
                clients.Clear();
            }
        }

        void Escuchar()
        {
            while (server.Server.IsBound)
            {
                var tcpClient = server.AcceptTcpClient();
                clients.Add(tcpClient);

                Thread t = new(() =>
                {
                    RecibirMensaje(tcpClient);
                });
                t.IsBackground = true;
                t.Start();
            }
        }

        public event EventHandler<MensajeDTO>? MensajeRecibido;

        void RecibirMensaje(TcpClient cliente)
        {

            while (cliente.Connected)
            {
                var ns = cliente.GetStream();

                while (cliente.Available == 0)
                {
                    Thread.Sleep(500);
                }

                byte[] buffer = new byte[cliente.Available];
                ns.Read(buffer, 0, buffer.Length);

                string json = Encoding.UTF8.GetString(buffer);

                var mensaje = JsonSerializer.Deserialize<MensajeDTO>(json);

                if (mensaje != null)
                {

                    //Reenviar el mensaje a los otros clientes y a la
                    //interfaz grafica con un evento
                    ReenviarMensaje(cliente, buffer); //relay message

                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        MensajeRecibido?.Invoke(this, mensaje);
                    });
                }

            }

            clients.Remove(cliente);

        }

        void ReenviarMensaje(TcpClient cliente, byte[] mensaje)
        {
            try
            {
                foreach (TcpClient c in clients)
                {
                    if (c != cliente && c.Connected) //Enviar a todos menos al origen
                    {
                        var ns = c.GetStream();
                        ns.Write(mensaje, 0, mensaje.Length);
                        ns.Flush();
                    }
                }
            }
            catch
            { //Expulsar al cliente si se desconecto}
            }

        }
    }
}
