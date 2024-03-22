using CommunityToolkit.Mvvm.ComponentModel;
using MensajesServer.Models;
using MensajesServer.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MensajesServer.ViewModels
{
    public partial class MensajesViewModel : ObservableObject
    {

        [ObservableProperty]
        private Mensaje mensaje = new();

        MensajesService server = new();
        DiscoveryService discoveryService = new();

        public MensajesViewModel()
        {
            server.MensajeRecibido += Server_MensajeRecibido;
        }

        private void Server_MensajeRecibido(object? sender, Mensaje e)
        {
            Mensaje = e;

        }
    }
}

