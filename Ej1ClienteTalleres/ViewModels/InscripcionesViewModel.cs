using CommunityToolkit.Mvvm.Input;
using Ej1ClienteTalleres.Models;
using Ej1ClienteTalleres.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Ej1ClienteTalleres.ViewModels
{
    public class InscripcionesViewModel : INotifyPropertyChanged
    {

        public InscripcionDTO Datos { get; set; } = new();
        IncripcionesCliente ClienteUdp = new();
        public ICommand InscribirCommand { get; set; }
        public string IP { get; set; } = "0.0.0.0";

        public InscripcionesViewModel()
        {
            InscribirCommand = new RelayCommand(Inscribir);
        }

        void Inscribir()
        {
            ClienteUdp.Servidor = IP;
            ClienteUdp.EnviarInscripcion(Datos);
        }


        public event PropertyChangedEventHandler? PropertyChanged;
    }
}
