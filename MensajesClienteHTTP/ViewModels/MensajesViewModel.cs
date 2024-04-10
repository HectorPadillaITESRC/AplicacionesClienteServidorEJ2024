using CommunityToolkit.Mvvm.ComponentModel;
using MensajesClienteHTTP.Models;
using MensajesClienteHTTP.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MensajesClienteHTTP.ViewModels
{
    public class MensajesViewModel:ObservableObject
    {
        //Servicios para que mi app reciba servidores y envie mensajes
        MensajesService mensajesService = new();
        DiscoveryService discoveryService = new();

        public ObservableCollection<ServerModel> Servidores { get; set; } = new();

        public MensajesViewModel()
        {
            discoveryService.ServidorRecibido += DiscoveryService_ServidorRecibido;
        }

        private void DiscoveryService_ServidorRecibido(object? sender, ServerModel e)
        {

            var server = Servidores.FirstOrDefault(x=>x.NombreServer==e.NombreServer);

            if (server == null)
            {
                //Agregar si no esta
                Servidores.Add(e);
            }
            else
            {
                //Editar el ka si esta
                server.KeepAlive = e.KeepAlive;
            }
            //Eliminar si excedio el ka

            foreach (var s in Servidores.ToList())
            {
                if((DateTime.Now- s.KeepAlive).TotalSeconds > 30)
                {
                    Servidores.Remove(s);
                }
            }



        }
    }
}
