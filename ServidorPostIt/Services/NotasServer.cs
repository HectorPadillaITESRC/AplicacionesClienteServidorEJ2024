using ServidorPostIt.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Windows;

namespace ServidorPostIt.Services
{
    public class NotasServer
    {
        HttpListener server = new();

        public NotasServer()
        {
            server.Prefixes.Add("http://*:12345/notas/");
        }

        public void Iniciar()
        {
            if (!server.IsListening)
            {
                server.Start();

                new Thread(Escuchar)
                {
                    IsBackground = true
                }.Start();

            }
        }


        //http://127.0.0.1:12345/notas
        //http://127.0.0.1:12345/notas/crear

        public event EventHandler<Nota>? NotaRecibida;

        void Escuchar()
        {
            while (true)
            {
                var context = server.GetContext(); //pausa hasta que reciba el request

                var pagina = File.ReadAllText("assets/index.html");
                var bufferPagina = Encoding.UTF8.GetBytes(pagina);

                if (context.Request.Url != null)
                {
                    if (context.Request.Url.LocalPath == "/notas/")
                    {
                        context.Response.ContentLength64 = bufferPagina.Length;
                        context.Response.OutputStream.Write(bufferPagina, 0, bufferPagina.Length);
                        context.Response.StatusCode = 200; //ok
                        context.Response.Close();
                    }
                    else if (context.Request.HttpMethod == "POST" &&
                        context.Request.Url.LocalPath == "/notas/crear") //me mandan los datos del formulario
                    {
                        byte[] bufferDatos = new byte[context.Request.ContentLength64];
                        context.Request.InputStream.Read(bufferDatos, 0, bufferDatos.Length);
                        string datos = Encoding.UTF8.GetString(bufferDatos);
                
                        var diccionario = HttpUtility.ParseQueryString(datos);

                        Nota nota = new()
                        {
                            Titulo = diccionario["titulo"] ?? "",
                            Contenido = diccionario["contenido"] ?? "",
                            X = double.Parse(diccionario["x"] ?? "0"),
                            Y = double.Parse(diccionario["y"] ?? "0"),
                            Remitente = context.Request.RemoteEndPoint.Address.ToString()
                        };

                        Application.Current.Dispatcher.Invoke(() =>
                        {
                            NotaRecibida?.Invoke(this, nota);
                        });

                        context.Response.StatusCode = 200;
                        context.Response.Close();


                    }
                    else
                    {
                        context.Response.StatusCode = 404;
                        context.Response.Close();
                    }

                }

            }
        }

        public void Detener()
        {
            server.Stop();
        }

    }
}
