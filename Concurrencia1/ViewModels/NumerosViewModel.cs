using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Concurrencia1.ViewModels
{
    public class NumerosViewModel : INotifyPropertyChanged
    {
        public string Tiempo { get; set; } = "";
        public long Suma { get; private set; }
        public ICommand SumarCommand { get; set; }
        public NumerosViewModel()
        {
            SumarCommand = new RelayCommand(SumarParallel);
        }


        private void SumarSincrono()
        {
            long suma = 0;
            for (long i = 1; i <= 10000000000; i++)
            {
                suma += i;
            }

            Suma = suma;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Suma)));
        }

        private async void SumarAsync()
        {

            await Task.Run(() =>
               {
                   long suma = 0;
                   for (long i = 1; i <= 10000000000; i++)
                   {
                       suma += i;
                   }

                   Suma = suma;
                   PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Suma)));


               });

        }

        private void SumarThread()
        {

            //Cross Thread Calls

            Thread hilo = new(() =>
            {
                Stopwatch s = new();
                s.Start();
                long suma = 0;
                for (long i = 1; i <= 10000000000; i++)
                {
                    suma += i;
                }

                Suma = suma;
                s.Stop();

                Tiempo = s.Elapsed.ToString();

                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(null));

            });
            hilo.IsBackground = true;
            hilo.Start();
        }

        private async void SumarParallel()
        {

            await Task.Run(() =>
            {


                long suma = 0;
                Parallel.For(1, 10, (x) =>
                {
                    long rango = 10000000000 / 10;
                    long inicial = rango * (x - 1) + 1;
                    for (long i = inicial; i < inicial + rango; i++)
                    {
                        suma += i;
                    }
                });


                Suma = suma;

                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(null));
            });
        }

        public event PropertyChangedEventHandler? PropertyChanged;
    }
}
