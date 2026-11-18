using System;
using Microsoft.Data.Sqlite;

namespace RegistroVideojuegosADO
{
    class Program
    {
        static void Main(string[] args)
        {
            string connectionString = "Data Source=videojuegos.db";

            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                Console.WriteLine("Conexión establecida con SQLite correctamente.\n");

                string createTableQuery = @"
                    CREATE TABLE IF NOT EXISTS Videojuegos (
                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                        Titulo TEXT NOT NULL,
                        Genero TEXT NOT NULL,
                        AnioLanzamiento INTEGER NOT NULL
                    );";

                using (var createCmd = new SqliteCommand(createTableQuery, connection))
                {
                    createCmd.ExecuteNonQuery();
                }

                using (var deleteCmd = new SqliteCommand("DELETE FROM Videojuegos;", connection))
                {
                    deleteCmd.ExecuteNonQuery();
                }

                InsertarVideojuego(connection, "The Legend of Zelda: Breath of the Wild", "Aventura", 2017);
                InsertarVideojuego(connection, "Red Dead Redemption 2", "Acción / Mundo Abierto", 2018);
                InsertarVideojuego(connection, "Elden Ring", "RPG de Acción", 2022);

                Console.WriteLine("3 registros insertados exitosamente.\n");

                string selectQuery = "SELECT Id, Titulo, Genero, AnioLanzamiento FROM Videojuegos;";

                using (var selectCmd = new SqliteCommand(selectQuery, connection))
                using (var reader = selectCmd.ExecuteReader())
                {
                    Console.WriteLine("=== CATÁLOGO DE VIDEOJUEGOS REGISTRADOS ===");
                    Console.WriteLine("{0,-5} | {1,-42} | {2,-23} | {3,-6}", "ID", "TÍTULO", "GÉNERO", "AÑO");
                    Console.WriteLine(new string('-', 85));

                    while (reader.Read())
                    {
                        int id = reader.GetInt32(0);
                        string titulo = reader.GetString(1);
                        string genero = reader.GetString(2);
                        int anio = reader.GetInt32(3);

                        Console.WriteLine("{0,-5} | {1,-42} | {2,-23} | {3,-6}", id, titulo, genero, anio);
                    }
                }
            }

            Console.WriteLine("\nConexión cerrada correctamente. Presiona cualquier tecla para salir...");
            Console.ReadKey();
        }

        static void InsertarVideojuego(SqliteConnection connection, string titulo, string genero, int anio)
        {
            string insertQuery = @"
                INSERT INTO Videojuegos (Titulo, Genero, AnioLanzamiento)
                VALUES (@Titulo, @Genero, @AnioLanzamiento);";

            using (var cmd = new SqliteCommand(insertQuery, connection))
            {
                cmd.Parameters.AddWithValue("@Titulo", titulo);
                cmd.Parameters.AddWithValue("@Genero", genero);
                cmd.Parameters.AddWithValue("@AnioLanzamiento", anio);
                cmd.ExecuteNonQuery();
            }
        }
    }
}