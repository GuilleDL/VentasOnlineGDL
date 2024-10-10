using VentasOnlineGDL;
using System.Collections.Generic;
using LogicaDeNegocio;

namespace VentasOnlineGDL
{
    public class Program
    {
        static void Main(string[] args)
        {
            Sistema sistema = Sistema.Instancia;
            string seleccion = "";
            while (seleccion != "0")
            {
                Console.Clear();
                MostrarMenu();
                seleccion = Console.ReadLine();
                try
                {
                    switch (seleccion)
                    {
                        case "0":
                            HastaLuego();
                            break;
                        case "1":
                            Console.Clear();
                            AgregarArticulo(sistema);
                            break;
                        case "2":
                            Console.Clear();
                            ListarClientes();
                            break;
                        case "3":
                            Console.Clear();
                            ListarArticulosPorCategoria();
                            break;
                        case "4":
                            Console.Clear();
                            ListarPublicacionesPorFechas();
                            break;
                        default:
                            Console.WriteLine("\n  #                 Opción Invalida                #\n");
                            EnterParaContinuar();
                            break;
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                
            }
        }
        static void MostrarMenu()
        {
            Console.WriteLine("\n  #########  V E N T A S    O N L I N E  #########\n" +
                              "  #                                              #\n" +
                              "  #      1 - Agregar Articulo                    #\n" +
                              "  #      2 - Listar Clientes                     #\n" +
                              "  #      3 - Listar Artículos por Categoría      #\n" +
                              "  #      4 - Listar Publicaciones por Fecha      #\n" +
                              "  #                                              #\n" +
                              "  #      0 - Salir                               #\n" +
                              "  #                                              #\n" +
                              "  ################ Ventas Online #################\n");
        }

        // 1 - Agregar Articulo
        static void AgregarArticulo(Sistema sistema)
        {
            Console.Write("Nombre del artículo: ");
            string ArtNombre = Console.ReadLine();

            CategoriaArticulo categoriaArticulo = SeleccionarCategoria();
            //CategoriaArticulo Categoria = 
            //Console.ReadLine();

            Console.Write("Precio de venta: ");
            decimal PrecioVenta = decimal.Parse(Console.ReadLine());
            //Valido que ingrese números
            while (PrecioVenta <= 0)
            {
                Console.WriteLine("Por favor, ingrese un valor numérico válido para el precio.");
                try
                {
                    PrecioVenta = decimal.Parse(Console.ReadLine());
                }
                catch
                {
                    PrecioVenta = 0;
                }
            }/*****************************************************CORREGIR EL INGRESO DE ARTÍCULOS*********************************************************/

            Articulo articulo = new Articulo(ArtNombre, categoriaArticulo, PrecioVenta);//verificar si queda autonumérico
            sistema.AgregarArticulo(articulo);

            Console.WriteLine("-------------------------------------------------------");
            Console.WriteLine($"Artículo {ArtNombre} agregado al catálogo.");
            Console.WriteLine("-------------------------------------------------------");

            EnterParaContinuar();
        }//cierre Agregar Articulo


        //2 - Listar Clientes
        static void ListarClientes()
        {
            Sistema sis = Sistema.Instancia;
            Console.WriteLine("Lista de Clientes:");

                foreach (Clientes unCliente in sis.ListadoClientes())
                {
                    Console.WriteLine($"Nombre: {unCliente.Nombre}" +
                                      $"\nApellido: {unCliente.Apellido} " +
                                      $"\nEmail: {unCliente.Mail}" +
                                      $"\nSaldo: {unCliente.Saldo}");
                    Console.WriteLine("------------------------------------");
                }

            EnterParaContinuar();
        }//Cierre  - Listar Clientes1




        //3 - Listar Artículos por Categoría
        static void ListarArticulosPorCategoria()
        {
            //Selecciona una Categoria
            CategoriaArticulo categoriaSeleccionada = SeleccionarCategoria();
            Console.Clear();

            // Filtrar y listar los artículos por la categoría seleccionada
            List<Articulo> articulosFiltrados = Sistema.Instancia.FiltrarArticuloPorCategoria(categoriaSeleccionada);

            // Verificar si hay artículos en la categoría seleccionada
            if (articulosFiltrados.Count > 0)
            {
                Console.WriteLine($"\nArtículos en la categoría {categoriaSeleccionada}: \n");

                // Listar los artículos de la categoría
                foreach (Articulo articulo in articulosFiltrados)
                {
                    Console.WriteLine($"Nombre: {articulo.ArtNombre}, Categoría: {articulo.Categoria}, Precio: {articulo.PrecioVenta:C}");
                }
            }
            else
            {
                Console.WriteLine($"No se encontraron artículos en la categoría {categoriaSeleccionada}.");
                Console.ReadLine();
            }
            EnterParaContinuar();
        }//Cierre Listar Artículos por Categoría



        //Listar Publicaciones por Fecha:
        static void ListarPublicacionesPorFechas()
        {
            DateTime fechaInicio, fechaFin;

            // Solicitar la primera fecha (fecha de inicio)
            Console.WriteLine("Ingrese la fecha de inicio (formato: YYYY-MM-DD): ");
            while (!DateTime.TryParse(Console.ReadLine(), out fechaInicio))
            {
                Console.WriteLine("La fecha ingresada no es válida. Intente nuevamente (formato: YYYY-MM-DD): ");
            }

            // Solicitar la segunda fecha (fecha de fin)
            Console.WriteLine("Ingrese la fecha de fin (formato: YYYY-MM-DD): ");
            while (!DateTime.TryParse(Console.ReadLine(), out fechaFin) || fechaFin < fechaInicio)
            {
                if (fechaFin < fechaInicio)
                {
                    Console.WriteLine("La fecha de fin no puede ser anterior a la fecha de inicio.");
                }
                else
                {
                    Console.WriteLine("La fecha ingresada no es válida. Intente nuevamente (formato: YYYY-MM-DD): ");
                }
            }

            // Filtrar las publicaciones que están dentro del rango de fechas
            List<Publicacion> publicacionesFiltradas = new List<Publicacion>()
                .Where(p => p.FechaPublicacion >= fechaInicio && p.FechaPublicacion <= fechaFin)
                .ToList();

            // Verificar si hay publicaciones que cumplan con el filtro
            if (publicacionesFiltradas.Count > 0)
            {
                Console.WriteLine("\nPublicaciones entre las fechas especificadas:");
                Console.WriteLine("-------------------------------------------------------------");

                // Listar las publicaciones
                foreach (Publicacion pub in publicacionesFiltradas)
                {
                    Console.WriteLine($"ID: {pub.PubId}, Nombre: {pub.Nombre}, Estado: {pub.EstadoPublicacion}, Fecha de Publicación: {pub.FechaPublicacion.ToString("yyyy-MM-dd")}");
                }

                Console.WriteLine("-------------------------------------------------------------");
            }
            else
            {
                Console.WriteLine("\nNo se encontraron publicaciones entre las fechas especificadas.");
            }

            EnterParaContinuar();
            
        }//Listar Publicaciones por Fecha:









        /*******************REFACTORIZACIONES ********************/

        //Listar Categorías - Refactorizar.
            static void ListarCategorias()
            {
                int i = 0;
                foreach (var unaCategoria in Enum.GetValues(typeof(CategoriaArticulo)))
                {
                    i++;
                    Console.WriteLine($"  {(int)unaCategoria} - {unaCategoria}");
                }
                Console.WriteLine(" ");
            }//Cierre Listar Categorías


            // Seleccionar y validar una categoría
            static CategoriaArticulo SeleccionarCategoria()
            {
                // Mostrar las categorías
                Console.WriteLine("Seleccione una categoría de la siguiente lista:\n");
                ListarCategorias();

                // Validar la selección de categoría
                bool esValida = false;
                CategoriaArticulo categoriaSeleccionada = CategoriaArticulo.OTROS; // Inicialización por defecto

                while (!esValida)
                {
                    Console.Write("Ingrese una categoría válida: ");
                    string inputCategoria = Console.ReadLine();

                    // Verificar si la categoría ingresada es válida
                    if (Enum.TryParse(inputCategoria, true, out categoriaSeleccionada) && Enum.IsDefined(typeof(CategoriaArticulo), categoriaSeleccionada))
                    {
                        esValida = true; // Categoría válida
                    }
                    else
                    {
                        Console.WriteLine("\nLa categoría ingresada no es válida. Intente nuevamente.");
                    }
                }

                return categoriaSeleccionada; // Devolver la categoría validada
            }// Cierre Seleccionar y validar una categoría

        static void EnterParaContinuar()
        {
            Console.WriteLine("\n  #                                                #\n" +
                                "  #  Presione Enter para volver al MENÚ PRINCIPAL  #\n" +
                                "  #                                                #\n");
            Console.ReadLine();
            }



            static void HastaLuego()
            {
                Console.Clear();
                Console.WriteLine("\n                         ////\n" +
                                  "                        (0 -)\n" +
                                  "               ----oOO-- (-) ----OOo---\n" +
                                  "          ╔═════════════════════════════════╗\n" +
                                  "          ║ Hasta luego, nos vemos pronto!  ║\n" +
                                  "          ╚═════════════════════════════════╝\n" +
                                  "                 -------------------\n" +
                                  "                       |__|__|\n" +
                                  "                        || ||\n" +
                                  "                       ooO Ooo\n" +
                                  "                       #279482\n" +
                                  "    Presione cualquie tecla para Salir de la consola\n");
            }

    }//Cierre public class Program
}//Cierre namespace VentasOnlineGDL
