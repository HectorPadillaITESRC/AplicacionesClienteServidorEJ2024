using Ej1ServidorTalleres.Models.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Security.AccessControl;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Windows;

namespace Ej1ServidorTalleres.Services
{
    public class InscripcionesServer
    {

        public InscripcionesServer()
        {
            var hilo = new Thread(new ThreadStart(Iniciar))
            {
                IsBackground = true
            };
            hilo.Start();
        }

        public event EventHandler<InscripcionDTO>? InscripcionRealizada;

        void Iniciar()
        {
            UdpClient server = new(5001);
            while (true)
            {
                IPEndPoint remoto = new(IPAddress.Any, 5001);
                byte[] buffer = server.Receive(ref remoto);

                InscripcionDTO? dto = JsonSerializer.Deserialize<InscripcionDTO>(
                    Encoding.UTF8.GetString(buffer)
                    );

                if (dto != null)
                {
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        InscripcionRealizada?.Invoke(this, dto);
                    });
                }
            }
        }
    }
}
