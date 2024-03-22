using Ej2ChatClient.Models;
using Ej2ChatClient.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using System.Net;
using System.Printing;
using System.Windows.Threading;

namespace Ej2ChatClient.ViewModels
{
    public class ChatViewModel : INotifyPropertyChanged
    {

        public ObservableCollection<MensajeDTO> Mensajes { get; set; } = new();
        public string Mensaje { get; set; } = "";
        public ICommand EnviarCommand { get; set; }
        ChatClient cliente = new();
        public string IP { get; set; } = "";
        public ICommand ConectarCommand { get; set; }
        public bool Conectado { get; set; }
        public int NumMensaje { get; set; }


        public ChatViewModel()
        {
            cliente.MensajeRecibido += Cliente_MensajeRecibido;
            EnviarCommand = new RelayCommand(Enviar);
            ConectarCommand = new RelayCommand(Conectar);
        }

        private void Conectar()
        {
            IPAddress.TryParse(IP, out IPAddress? ipAddress);
            if (ipAddress != null)
            {
                cliente.Conectar(ipAddress);
                Conectado = true;
                PropertyChanged?.Invoke(this, new(nameof(Conectado)));

            }
        }

        private void Enviar()
        {
            if (!string.IsNullOrWhiteSpace(Mensaje))
            {
                cliente.EnviarMensaje(new MensajeDTO
                {
                    Fecha = DateTime.Now,
                    Origen = cliente.Equipo,
                    Mensaje = Mensaje
                });
            }
        }

        private void Cliente_MensajeRecibido(object? sender, MensajeDTO e)
        {
            Dispatcher.CurrentDispatcher.Invoke(() =>
            {

                if (e.Mensaje == "**HELLO")
                {
                    e.Mensaje = $"{e.Origen} se ha conectado";
                }
                if (e.Mensaje == "**BYE")
                {
                    e.Mensaje = $"{e.Origen} se ha desconectado";
                }

                Mensajes.Add(e);
                NumMensaje = Mensajes.Count - 1;
                PropertyChanged?.Invoke(this, new(nameof(NumMensaje)));
            });
        }

        public event PropertyChangedEventHandler? PropertyChanged;
    }
}
