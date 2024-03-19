using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace HospitalApi
{
    class DataBaseWorker
    {
        public static string ConnectString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\Hospital.mdf;Integrated Security=True;Connect Timeout=30";
        static SqlConnection Connection;

        /// <summary>
        /// Открывает соединение с БД.
        /// </summary>
        private static void OpenConnection()
        {
            Connection = new SqlConnection(ConnectString);
            Connection.Open();
        }

        /// <summary>
        /// Закрывает соединение с БД.
        /// </summary>
        private static void CloseConnection()
        {
            Connection.Close();
        }

        /// <summary>
        /// Метод для получения более одной строки из БД.
        /// Для его работы, ему необходимо передать сам запрос в формате строки и кол-во столбцов которые метод должен вернуть.
        /// В случае, если данные в БД по указанному запросу есть, метод возвращает лист массивов строк, иначе, метод возвращает null.
        /// <example>
        /// <code>
        /// Пример правильного использования метода:
        /// List≺string[]≻ Response = DataBaseWorker.ExecuteQuery("SELECT Name, Role FROM Users", 2);
        /// foreach (string[] ResponseItem in Response)
        /// {
        ///     Console.WriteLine(ResponseItem[0] + " " + ResponseItem[1]);
        /// }
        /// </code>
        /// </example>
        /// </summary>
        public static List<String[]> ExecuteQuery(string query, int col) 
        {
            OpenConnection();

            SqlCommand command = new SqlCommand(query, Connection);
            SqlDataReader reader = command.ExecuteReader();

            List<string[]> Response = new List<string[]>();

            while (reader.Read())
            {
                Response.Add(new string[col]);

                for (int i = 0; i < col; i++)
                {
                    Response[Response.Count - 1][i] = reader[i].ToString();
                }
            }

            reader.Close();
            CloseConnection();

            if (Response.Count != 0)
                return Response;
            else
                return null;
        }



        /// <summary>
        /// Метод для получения более одной строки из БД.
        /// Для его работы, ему необходимо передать сам запрос в формате строки и кол-во столбцов, которые метод должен вернуть.
        /// В случае, если данные в БД по указанному запросу есть, метод возвращает список массивов объектов, иначе, метод возвращает null.
        /// <example>
        /// <code>
        /// Пример правильного использования метода:
        /// List≺object[]≻ responsePatients = DataBaseWorker.ExecuteQueryObject("SELECT * FROM Patients", 11);
        /// foreach (object[] patient in responsePatients)
        /// {
        ///     patients.Add(new Patient(patient));
        /// }
        /// </code>
        /// </example>
        /// </summary>
        public static List<object[]> ExecuteQueryObject(string query, int col)
        {
            OpenConnection();

            SqlCommand command = new SqlCommand(query, Connection);
            SqlDataReader reader = command.ExecuteReader();

            List<object[]> Response = new List<object[]>();

            while (reader.Read())
            {
                object[] rowData = new object[col];

                for (int i = 0; i < col; i++)
                    rowData[i] = reader[i];

                Response.Add(rowData);
            }

            reader.Close();
            CloseConnection();

            if (Response.Count != 0)
                return Response;
            else
                return null;
        }

        /// <summary>
        /// Метод для получения 1-го значения из БД.
        /// Для его работы, ему необходимо передать сам запрос в формате строки.
        /// В случае, если данные в БД по указанному запросу есть, метод возвращает строку, иначе, метод возвращает null.
        /// Важно: если указанный запрос возвращает не 1 значение, то метод вернет последнее из полученных значений
        /// <example>
        /// <code>
        /// Пример правильного использования метода:
        /// string Password = DataBaseWorker.ExecuteQueryOnlyOne("SELECT Password FROM Abonents WHERE PersonalAccount=" + TextBoxNumber.Text)
        /// </code>
        /// </example>
        /// </summary>
        public static string ExecuteQueryOnlyOne(string query)
        {
            OpenConnection();

            SqlCommand command = new SqlCommand(query, Connection);
            SqlDataReader reader = command.ExecuteReader();

            String Response = null;

            while (reader.Read())
                Response = reader[0].ToString();

            reader.Close();
            CloseConnection();

            return Response;
        }

        /// <summary>
        /// Метод для получения 1-го значения с типом данных int из БД.
        /// Для его работы, ему необходимо передать сам запрос в формате строки.
        /// В случае, если данные в БД по указанному запросу есть, метод возвращает значение, иначе, метод возвращает -1
        /// Важно: если указанный запрос возвращает не 1 значение, то метод вернет последнее из полученных значений
        /// <example>
        /// <code>
        /// Пример правильного использования метода:
        /// int Price = DataBaseWorker.ExecuteQueryOnlyOneInt("SELECT Price FROM EquipmentForArenda WHERE ID_EquipmentForArenda=5");
        /// </code>
        /// </example>
        /// </summary>
        public static int ExecuteQueryOnlyOneInt(string query)
        {
            OpenConnection();

            SqlCommand command = new SqlCommand(query, Connection);
            SqlDataReader reader = command.ExecuteReader();

            int Response = -1;

            while (reader.Read())
                Response = Convert.ToInt32(reader[0]);

            reader.Close();
            CloseConnection();

            return Response;
        }

        /// <summary>
        /// Метод для получения 1-го значения с типом данных double (real) из БД.
        /// Для его работы, ему необходимо передать сам запрос в формате строки.
        /// В случае, если данные в БД по указанному запросу есть, метод возвращает значение, иначе, метод возвращает -1
        /// Важно: если указанный запрос возвращает не 1 значение, то метод вернет последнее из полученных значений
        /// <example>
        /// <code>
        /// Пример правильного использования метода:
        /// double Price = DataBaseWorker.ExecuteQueryOnlyOneDouble("SELECT Price FROM EquipmentForArenda WHERE ID_EquipmentForArenda=5");
        /// </code>
        /// </example>
        /// </summary>
        public static double ExecuteQueryOnlyOneDouble(string query)
        {
            OpenConnection();

            SqlCommand command = new SqlCommand(query, Connection);
            SqlDataReader reader = command.ExecuteReader();

            double Response = -1;

            while (reader.Read())
                Response = Convert.ToInt32(reader[0]);

            reader.Close();
            CloseConnection();

            return Response;
        }

        /// <summary>
        /// Метод для выполнения запроса без ответа от БД (Например выполнение операций INSERT или UPDATE).
        /// Для его работы, ему необходимо передать сам запрос в формате строки.
        /// <example>
        /// <code>
        /// Пример правильного использования метода:
        /// DataBaseWorker.ExecuteQueryWithoutResponse("UPDATE Abonents set Password = '"NewPassword"' where PersonalAccount = 10");
        /// </code>
        /// </example>
        /// </summary>
        public static void ExecuteQueryWithoutResponse(string query)
        {
            OpenConnection();

            SqlCommand cmd = new SqlCommand(query, Connection);
            cmd.ExecuteNonQuery();

            CloseConnection();
        }

        /// <summary>
        /// Метод для выполнения хранимой процедуры в БД.
        /// Для его работы, ему необходимо передать название хранимой процедуры в виде строки и ArrayList с названиями параметров и их значений.
        /// <example>
        /// <code>
        /// Пример правильного использования метода:
        /// ArrayList Parameters = new ArrayList
        /// {
        /// "@PersonalAccount", 10,
        /// "@Amount", TextBoxSummaPay.Text,
        /// "@DatePayment", DateTime.Now
        /// };
        /// DataBaseWorker.ExecuteStoredProcedure("AbonentsPaymentsAdd", Parameters);
        /// </code>
        /// </example>
        /// </summary>
        public static void ExecuteStoredProcedure(string ProcedureName, ArrayList Parameters)
        {
            OpenConnection();

            SqlCommand cmd = new SqlCommand(ProcedureName, Connection)
            {
                CommandType = CommandType.StoredProcedure
            };

            for (int i = 0; i < Parameters.Count; i += 2)
                cmd.Parameters.AddWithValue((string)Parameters[i], Parameters[i + 1]);

            cmd.ExecuteNonQuery();

            CloseConnection();
        }
    }
}
