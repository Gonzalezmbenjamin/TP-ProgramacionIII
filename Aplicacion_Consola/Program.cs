using System;
using MySql.Data.MySqlClient; 

namespace Progra3Card.Administrativo
{
    class Program
    {
        private static string connectionString = "Server=localhost;Database=mi_banco_db;Uid=root;Pwd=;";

        static void Main(string[] args)
        {
            bool salir = false;
            while (!salir)
            {
                Console.Clear();
                Console.WriteLine("========================================");
                Console.WriteLine("    SISTEMA ADMINISTRATIVO PROGRA3CARD   ");
                Console.WriteLine("========================================");
                Console.WriteLine("1. Emitir Nueva Tarjeta (Alta de Cliente)");
                Console.WriteLine("2. Listar Tarjetas");
                Console.WriteLine("3. Ver Detalle de una Tarjeta / Cliente");
                Console.WriteLine("4. Eliminar Tarjeta (Baja de Sistema)");
                Console.WriteLine("5. Emitir Nueva Liquidación Mensual");
                Console.WriteLine("6. Salir");
                Console.WriteLine("========================================");
                Console.Write("Seleccione una opción: ");

                switch (Console.ReadLine())
                {
                    case "1": MenuEmitirTarjeta(); break;
                    case "2": MenuListarTarjetas(); break;
                    case "3": MenuVerDetalleTarjeta(); break;
                    case "4": MenuEliminarTarjeta(); break;
                    case "5": MenuEmitirLiquidacion(); break;
                    case "6": salir = true; break;
                    default:
                        Console.WriteLine("Opción no válida. Presione una tecla para continuar...");
                        Console.ReadKey();
                        break;
                }
            }
        }
        static void MenuEmitirTarjeta()
        {
            Console.Clear();
            Console.WriteLine("--- EMITIR NUEVA TARJETA / ALTA DE CLIENTE ---");

            // Pedimos datos del cliente
            Console.Write("Documento: ");
            string documento = Console.ReadLine();

            Console.Write("Tipo documento (DNI/PASAPORTE): ");
            string tipoDoc = Console.ReadLine().ToUpper();

            if (tipoDoc != "DNI" && tipoDoc != "PASAPORTE")
            {
                Console.WriteLine("Tipo de documento inválido.");
                Console.ReadKey();
                return;
            }

            Console.Write("Nombre: ");
            string nombre = Console.ReadLine();

            Console.Write("Apellido: ");
            string apellido = Console.ReadLine();

            Console.Write("Fecha de nacimiento (YYYY-MM-DD): ");
            string fechaNacimiento = Console.ReadLine();

            Console.Write("Email: ");
            string email = Console.ReadLine();

            // Pedimos datos de la tarjeta
            Console.Write("Número de tarjeta (16 dígitos): ");
            string numeroTarjeta = Console.ReadLine();

            Console.WriteLine("\nSeleccione banco emisor:");
            Console.WriteLine("1. Banco Nación");
            Console.WriteLine("2. Banco Provincia");
            Console.WriteLine("3. Banco Galicia");
            Console.WriteLine("4. Banco Santander");
            Console.WriteLine("5. Banco BBVA");
            Console.WriteLine("6. Banco Macro");
            Console.Write("Opción: ");

            string opcionBanco = Console.ReadLine();
            string bancoEmisor = "";

            switch (opcionBanco)
            {
                case "1": bancoEmisor = "Banco Nación"; break;
                case "2": bancoEmisor = "Banco Provincia"; break;
                case "3": bancoEmisor = "Banco Galicia"; break;
                case "4": bancoEmisor = "Banco Santander"; break;
                case "5": bancoEmisor = "Banco BBVA"; break;
                case "6": bancoEmisor = "Banco Macro"; break;
                default:
                    Console.WriteLine("Banco inválido.");
                    Console.ReadKey();
                    return;
            }

            Console.Write("Saldo inicial: ");
            decimal saldo = Convert.ToDecimal(Console.ReadLine());

            // Insertamos en la base de datos
            using (MySqlConnection conexion = new MySqlConnection(connectionString))
            {
                conexion.Open();

                // Primero insertamos el cliente en usuarios.
                // usuario y password quedan en NULL porque después se activan desde la web.
                string queryUsuario = @"INSERT INTO usuarios
                                        (documento, tipo_doc, nombre, apellido, fecha_nacimiento, email, usuario, password)
                                        VALUES
                                        (@documento, @tipoDoc, @nombre, @apellido, @fechaNacimiento, @email, NULL, NULL)";

                MySqlCommand comandoUsuario = new MySqlCommand(queryUsuario, conexion);
                comandoUsuario.Parameters.AddWithValue("@documento", documento);
                comandoUsuario.Parameters.AddWithValue("@tipoDoc", tipoDoc);
                comandoUsuario.Parameters.AddWithValue("@nombre", nombre);
                comandoUsuario.Parameters.AddWithValue("@apellido", apellido);
                comandoUsuario.Parameters.AddWithValue("@fechaNacimiento", fechaNacimiento);
                comandoUsuario.Parameters.AddWithValue("@email", email);

                comandoUsuario.ExecuteNonQuery();

                // Después insertamos la tarjeta vinculada al documento del titular.
                string queryTarjeta = @"INSERT INTO tarjetas
                                        (numero_tarjeta, banco_emisor, estado, saldo, dni_titular)
                                        VALUES
                                        (@numeroTarjeta, @bancoEmisor, 'Activa', @saldo, @dniTitular)";

                MySqlCommand comandoTarjeta = new MySqlCommand(queryTarjeta, conexion);
                comandoTarjeta.Parameters.AddWithValue("@numeroTarjeta", numeroTarjeta);
                comandoTarjeta.Parameters.AddWithValue("@bancoEmisor", bancoEmisor);
                comandoTarjeta.Parameters.AddWithValue("@saldo", saldo);
                comandoTarjeta.Parameters.AddWithValue("@dniTitular", documento);

                comandoTarjeta.ExecuteNonQuery();

                Console.WriteLine("\nCliente y tarjeta creados correctamente.");
            }

            Console.WriteLine("\nPresione una tecla para volver al menú...");
            Console.ReadKey();
        }
        // Funciones a completar:

        static void MenuListarTarjetas()
        {
            Console.Clear();
            Console.WriteLine("--- LISTADO GENERAL DE TARJETAS ---");
            Console.WriteLine("{0,-12} {1,-18} {2,-20} {3,-15}", "Nro Cuenta", "Nro Tarjeta", "Banco Emisor", "DNI Titular");
            Console.WriteLine("----------------------------------------------------------------------");

            // === A realizar ===
            // Aquí deben implementar un SELECT sobre la tabla 'tarjetas'
            // para recorrer las filas e imprimirlas en la consola.
            
            ObtenerYMostrarTarjetas();

            Console.WriteLine("\nPresione una tecla para volver al menú...");
            Console.ReadKey();
        }

        static void MenuVerDetalleTarjeta()
        {
            Console.Clear();
            Console.WriteLine("--- DETALLE DE TARJETA Y CLIENTE ---");
            Console.Write("Ingrese el Número de Cuenta a consultar: ");
            int numCuenta = Convert.ToInt32(Console.ReadLine());

            // === A realizar ===
            // Aquí deben realizar un SELECT con un JOIN entre 'tarjetas' y 'usuarios' 
            // filtrando por el numCuenta para traer todos los campos (Nombre, Apellido, Email, Saldo, etc.)
            
            MostrarDetalleCompleto(numCuenta);

            Console.WriteLine("\nPresione una tecla para volver al menú...");
            Console.ReadKey();
        }

        static void MenuEliminarTarjeta()
        {
            Console.Clear();
            Console.WriteLine("--- ELIMINAR TARJETA DEL SISTEMA ---");
            Console.Write("Ingrese el Número de Cuenta de la tarjeta a dar de baja: ");
            int numCuenta = Convert.ToInt32(Console.ReadLine());

            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("\n⚠️ ADVERTENCIA: Se eliminará la tarjeta, sus liquidaciones y los datos de acceso web vinculados.");
            Console.ResetColor();
            Console.Write("¿Está seguro de continuar? (S/N): ");
            
            if (Console.ReadLine().ToUpper() == "S")
            {
                // === A realizar ===
                // Aquí deben ejecutar un DELETE sobre la tabla 'tarjetas' donde num_cuenta = numCuenta.
                // Como definimos ON DELETE CASCADE en la base de datos, las liquidaciones se borrarán solas.
                // Opcional: Evaluar si también eliminan al usuario de la tabla 'usuarios' o si lo mantienen.
                
                bool exito = DarDeBajaTarjeta(numCuenta);

                if (exito)
                    Console.WriteLine("\nTarjeta eliminada correctamente del sistema.");
                else
                    Console.WriteLine("\nError al intentar eliminar la tarjeta. Verifique el número de cuenta.");
            }
            else
            {
                Console.WriteLine("\nOperación cancelada.");
            }

            Console.WriteLine("\nPresione una tecla para volver al menú...");
            Console.ReadKey();
        }

        
        static void MenuEmitirLiquidacion()
{
    Console.Clear();
    Console.WriteLine("--- EMITIR NUEVA LIQUIDACIÓN MENSUAL ---");

    Console.Write("Número de cuenta: ");
    int numCuenta = Convert.ToInt32(Console.ReadLine());

    Console.Write("Período (YYYY-MM): ");
    string periodo = Console.ReadLine();

    Console.Write("Fecha de vencimiento (YYYY-MM-DD): ");
    string fechaVencimiento = Console.ReadLine();

    Console.Write("Total a pagar: ");
    decimal totalAPagar = Convert.ToDecimal(Console.ReadLine());

    Console.Write("Pago mínimo: ");
    decimal pagoMinimo = Convert.ToDecimal(Console.ReadLine());

    using (MySqlConnection conexion = new MySqlConnection(connectionString))
    {
        conexion.Open();

        string query = @"INSERT INTO liquidaciones
                        (num_cuenta, periodo, fecha_vencimiento, total_a_pagar, pago_minimo)
                        VALUES
                        (@numCuenta, @periodo, @fechaVencimiento, @totalAPagar, @pagoMinimo)";

        MySqlCommand comando = new MySqlCommand(query, conexion);

        comando.Parameters.AddWithValue("@numCuenta", numCuenta);
        comando.Parameters.AddWithValue("@periodo", periodo);
        comando.Parameters.AddWithValue("@fechaVencimiento", fechaVencimiento);
        comando.Parameters.AddWithValue("@totalAPagar", totalAPagar);
        comando.Parameters.AddWithValue("@pagoMinimo", pagoMinimo);

        comando.ExecuteNonQuery();

        Console.WriteLine("\nLiquidación emitida correctamente.");
    }

    Console.WriteLine("\nPresione una tecla para volver al menú...");
    Console.ReadKey();
}


        // =========================================================================
        // MÉTODOS BASE QUE DEBEN COMPLETAR CON LA LÓGICA 
        // =========================================================================

static void ObtenerYMostrarTarjetas()
{
    // Completar
    // Ejemplo de impresión dentro del bucle:
    // Console.WriteLine("{0,-12} {1,-18} {2,-20} {3,-15}", reader["num_cuenta"], reader["numero_tarjeta"], ...);

    // Crear conexión con la base de datos
    using (MySqlConnection conexion = new MySqlConnection(connectionString))
    {
        // Abrir conexión
        conexion.Open();

        // Consulta SQL para obtener las tarjetas
        string query = "SELECT num_cuenta, numero_tarjeta, banco_emisor, dni_titular FROM tarjetas";

        // Preparar comando
        MySqlCommand comando = new MySqlCommand(query, conexion);

        // Ejecutar SELECT
        MySqlDataReader reader = comando.ExecuteReader();

        // Recorrer resultados
        while (reader.Read())
        {
            Console.WriteLine(
                "{0,-12} {1,-18} {2,-20} {3,-15}",
                reader["num_cuenta"],
                reader["numero_tarjeta"],
                reader["banco_emisor"],
                reader["dni_titular"]
            );
        }

        // Cerrar el lector
        reader.Close();
    }
}
       static void MostrarDetalleCompleto(int cuenta)
{
    // Completar haciendo un SELECT con JOIN de usuarios y tarjetas WHERE num_cuenta = @cuenta

    // Crear conexión con la base de datos
    using (MySqlConnection conexion = new MySqlConnection(connectionString))
    {
        // Abrir conexión
        conexion.Open();

        // Consulta SQL con JOIN entre tarjetas y usuarios
        string query = @"SELECT 
                            t.num_cuenta,
                            t.numero_tarjeta,
                            t.banco_emisor,
                            t.estado,
                            t.saldo,
                            u.documento,
                            u.tipo_doc,
                            u.nombre,
                            u.apellido,
                            u.fecha_nacimiento,
                            u.email,
                            u.usuario
                         FROM tarjetas t
                         INNER JOIN usuarios u 
                         ON t.dni_titular = u.documento
                         WHERE t.num_cuenta = @cuenta";

        // Preparar comando
        MySqlCommand comando = new MySqlCommand(query, conexion);

        // Pasar el parámetro cuenta
        comando.Parameters.AddWithValue("@cuenta", cuenta);

        // Ejecutar SELECT
        MySqlDataReader reader = comando.ExecuteReader();

        // Leer resultado
        if (reader.Read())
        {
            Console.WriteLine("\n--- DATOS DEL CLIENTE ---");
            Console.WriteLine("Documento: " + reader["documento"]);
            Console.WriteLine("Tipo documento: " + reader["tipo_doc"]);
            Console.WriteLine("Nombre: " + reader["nombre"]);
            Console.WriteLine("Apellido: " + reader["apellido"]);
            Console.WriteLine("Fecha nacimiento: " + reader["fecha_nacimiento"]);
            Console.WriteLine("Email: " + reader["email"]);
            Console.WriteLine("Usuario web: " + reader["usuario"]);

            Console.WriteLine("\n--- DATOS DE LA TARJETA ---");
            Console.WriteLine("Número de cuenta: " + reader["num_cuenta"]);
            Console.WriteLine("Número de tarjeta: " + reader["numero_tarjeta"]);
            Console.WriteLine("Banco emisor: " + reader["banco_emisor"]);
            Console.WriteLine("Estado: " + reader["estado"]);
            Console.WriteLine("Saldo: $" + reader["saldo"]);
        }
        else
        {
            Console.WriteLine("No se encontró una tarjeta con ese número de cuenta.");
        }

        // Cerrar lector
        reader.Close();
    }
}

       static bool DarDeBajaTarjeta(int cuenta)
{
    // Completar usando un DELETE FROM tarjetas WHERE num_cuenta = @cuenta

    // Crear conexión con la base de datos
    using (MySqlConnection conexion = new MySqlConnection(connectionString))
    {
        // Abrir conexión
        conexion.Open();

        // Consulta SQL para eliminar la tarjeta
        string query = "DELETE FROM tarjetas WHERE num_cuenta = @cuenta";

        // Preparar comando
        MySqlCommand comando = new MySqlCommand(query, conexion);

        // Pasar el parámetro cuenta
        comando.Parameters.AddWithValue("@cuenta", cuenta);

        // Ejecutar DELETE
        int filasAfectadas = comando.ExecuteNonQuery();

        // Si eliminó una fila devuelve true, sino false
        return filasAfectadas > 0;
    }
}
}
}