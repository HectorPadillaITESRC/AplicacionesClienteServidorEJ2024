using ServidorPostIt.Models;
using ServidorPostIt.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ServidorPostIt.ViewModels
{
    public class NotasViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<Nota> Notas { get; set; } = new();
        public string IP
        {
            get
            {
                return string.Join(",", Dns.GetHostAddresses(Dns.GetHostName()).
                    Where(x => x.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                    .Select(x => x.ToString()));
            }
        }

        NotasServer server = new();

        public NotasViewModel()
        {
            server.NotaRecibida += Server_NotaRecibida;
            server.Iniciar();

        }
        Random r = new();
        private void Server_NotaRecibida(object? sender, Nota e)
        {
            e.X = r.Next(0, 1000);
            e.Y = r.Next(0, 1000);
            e.Angulo = r.Next(-5, 6);
            Notas.Add(e);
        }



        public event PropertyChangedEventHandler? PropertyChanged;
    }
}
