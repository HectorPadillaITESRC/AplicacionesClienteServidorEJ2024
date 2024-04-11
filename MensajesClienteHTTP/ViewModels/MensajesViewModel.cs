using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MensajesClienteHTTP.Models;
using MensajesClienteHTTP.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace MensajesClienteHTTP.ViewModels
{
    public partial class MensajesViewModel:ObservableObject
    {
        //Servicios para que mi app reciba servidores y envie mensajes
        MensajesService mensajesService = new();
        DiscoveryService discoveryService = new();

        public MensajeDTO Mensajes { get; set; } = new();
        public ServerModel Seleccionado { get; set; } = null!;

        [RelayCommand]
        private void Enviar()
        {
        }

        public List<SolidColorBrush> Colores { get; set; } = new();


        public ObservableCollection<ServerModel> Servidores { get; set; } = new();

        public MensajesViewModel()
        {

            foreach (var propiedad in typeof(Brushes).GetProperties())
            {
                if (propiedad != null)
                {
                    Colores.Add((SolidColorBrush)(propiedad.GetValue(null)??new SolidColorBrush()));
                }
            }

            //Colores = new(typeof(Brushes).GetProperties().Select(c => (SolidColorBrush)(c.GetValue(null) ?? new SolidColorBrush())));
            

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
