using System;
using System.Data.OleDb;
using System.Windows.Forms;
using ADOX;
using System.IO;

namespace DatabaseController
{
    public class DatabaseControl
    {
        public OleDbConnection connection;
        public bool connected;
        public string DatabasePath;

        /// <summary>
        /// Veritabanına bağlanır ve diğer işlemleri yapar.
        /// </summary>
        /// <param name="DatabasePath">Veritabanının yolu.</param>
        public DatabaseControl(string DatabasePath)
        {
            this.DatabasePath = DatabasePath;
        }

        public string CreateDatabase(string DatabasePath)
        {
            string connectionString = $@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source={DatabasePath};";

            if (File.Exists($"{DatabasePath}"))
            {
                MessageBox.Show($"'{new FileInfo(DatabasePath).FullName}' veritabanı mevcut");
                return "db-exist";
            }

            try
            {
                Catalog catalog = new Catalog();
                catalog.Create(connectionString);
                MessageBox.Show($"Veritabanı '{new FileInfo(DatabasePath).FullName}' başarıyla oluşturuldu.");

                return DatabasePath;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Veritabanı '{new FileInfo(DatabasePath).FullName}' oluşturulurken bir hata oluştu: " + ex.Message);

                return null;
            }
        }

        public bool ConnectDatabase()
        {
            try
            {
                this.connection = new OleDbConnection($"Provider=Microsoft.ACE.OLEDB.12.0;Data Source={this.DatabasePath};Persist Security Info=True");
                this.connection.Open();
            }
            catch (Exception ex)
            {
                this.connection = null;
                MessageBox.Show("Veritabanı ile bağlantı sağlanamadı: " + ex.Message);
            }

            if (this.connection == null)
            {
                connected = false;
                return false;
            }
            connected = true;
            return true;
        }

        public bool DisconnectDatabase()
        {
            try
            {
                this.connection.Close();

                return true;
            }
            catch
            {
                return false;
            }
        }

        public string UpdateTable(string TableName, string row, string value, int rowId = 1)
        {
            string[] decimalRows = {"SetSicaklik1", "SetSicaklik2", "SetSure", "SetOrnekAraligi", "GetSicaklik1", "GetSicaklik2", "GetSure"};
            string[] datetimeRows = {"IsEmirZamani", "GetTimeStamp", "GetStartTime", "GetFinishTime"};

            try
            {
                string query = $"UPDATE SET_TABLES SET Operator = @Operator WHERE PressDataID= 1";

                using (OleDbCommand command = new OleDbCommand(query, connection))
                {
                    if (Array.Exists(decimalRows, element => element == value))
                        command.Parameters.AddWithValue($"@Operator", 12);
                    // "2024-10-24 15:30:00"
                    else if (Array.Exists(decimalRows, element => element == value))
                        command.Parameters.AddWithValue($"@Operator", DateTime.Parse(value));
                    else
                        command.Parameters.AddWithValue($"@Operator", "Ege1");

                    command.ExecuteNonQuery();
                }

                MessageBox.Show($"{TableName} tablosuna veriler kaydedildi");
                return TableName;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{TableName} tablosuna {value} verisi kaydedilemedi: " + ex.Message);
                return null;
            }
        }

        public string DataCount(string tableName, int startRowID = 1, int endRowID = -1)
        {
            try
            {
                string query;

                if (endRowID == -1)
                    query = $"SELECT * FROM {tableName}";
                else
                    query = $"SELECT * FROM {tableName} WHERE PressResDataID BETWEEN {startRowID} AND {endRowID}";

                using (OleDbCommand command = new OleDbCommand(query, connection))
                using (OleDbDataReader reader = command.ExecuteReader())
                {
                    int rowCount = 0;

                    while (reader.Read())
                    {
                        rowCount++;
                    }

                    return rowCount.ToString();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return null;
            }

        }

        public string DeleteData(string tableName, int startRowID = 0, int endRowID = -1)
        {
            try
            {
                string query;

                if (endRowID == -1)
                    query = $"DELETE FROM {tableName} WHERE PressResDataID > {startRowID}";
                else
                    query = $"DELETE FROM {tableName} WHERE PressResDataID BETWEEN {startRowID} AND {endRowID}";

                int rowsAffected = 0;

                using (OleDbCommand command = new OleDbCommand(query, connection))
                {
                    rowsAffected = command.ExecuteNonQuery();
                }

                MessageBox.Show($"{rowsAffected + 1} satır silindi");
                return rowsAffected.ToString();
            }
            catch ( Exception ex )
            {
                MessageBox.Show(ex.Message);
                return null;
            }
            
        }

        public string WriteData(string blpnokafile, int GetSicaklik1, int GetSicaklik2, string GetSure, string GetStartTime, string GetFinishTime, string operatorName, string makine)
        {
            try
            {
                string query;
                if (DataCount("tblprestoserver") == null || DataCount("tblprestoserver") == "0")
                    query = $"INSERT INTO tblprestoserver (PressResDataID, blpnokafile, GetSicaklik1, GetSicaklik2, GetSure, GetTimeStamp, GetStartTime, GetFinishTime, operator, makine) VALUES (1, @BlpNoKafile, @GetSicaklik1, @GetSicaklik2, @GetSure, @GetTimeStamp, @GetStartTime, @GetFinishTime, @operator, @makine)";
                else
                    query = $"INSERT INTO tblprestoserver (blpnokafile, GetSicaklik1, GetSicaklik2, GetSure, GetTimeStamp, GetStartTime, GetFinishTime, operator, makine) VALUES (@BlpNoKafile, @GetSicaklik1, @GetSicaklik2, @GetSure, @GetTimeStamp, @GetStartTime, @GetFinishTime, @operator, @makine)";

                using (OleDbCommand command = new OleDbCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@blpnokafile", blpnokafile);
                    command.Parameters.AddWithValue("@GetSicaklik1", GetSicaklik1);
                    command.Parameters.AddWithValue("@GetSicaklik2", GetSicaklik2);
                    command.Parameters.AddWithValue("@GetSure", GetSure);
                    command.Parameters.AddWithValue("@GetTimeStamp", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                    command.Parameters.AddWithValue("@GetStartTime", GetStartTime);
                    command.Parameters.AddWithValue("@GetFinishTime", GetFinishTime);
                    command.Parameters.AddWithValue("@operator", operatorName);
                    command.Parameters.AddWithValue("@makine", makine);

                    command.ExecuteNonQuery();
                }

                return null;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public string CreateTable(string TableName, string rows)
        {
            string query = $"CREATE TABLE {TableName} ({rows})";
            try
            {
                using (OleDbCommand command = new OleDbCommand(query, connection))
                {
                    // Komutu çalıştır
                    command.ExecuteNonQuery();
                    MessageBox.Show("Tablo başarıyla oluşturuldu.");
                }

                return TableName;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Tablo oluşturulurken bir hata oluştu: " + ex.Message);

                return null;
            }
        }

        /// <summary>
        /// Bağlı veritabanının TableName tablosundaki rows değerlerini döndürür.
        /// </summary>
        /// <param name="TableName">Tablo adı.</param>
        /// <param name="rows">Tablodaki columnlar (* verilince hepsi).</param>
        /// <returns>string "ColumnAdı:ColumnDeğeri\n"</returns>
        public string GetData(string TableName, string rows, int tableId = 2)
        {
            string returnData = "";
            string query = $"SELECT {rows} FROM {TableName} WHERE PressDataID = {tableId}";

            try
            {
                OleDbCommand returnQuery = new OleDbCommand(query, this.connection);

                using (OleDbDataReader reader = returnQuery.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        returnData = "";
                        while (reader.Read())
                        {
                            for (int i = 0; i < reader.FieldCount; i++)
                            {
                                returnData += $"{reader.GetName(i)}:{reader.GetValue(i)}\n";
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Veri okuma hatası: " + ex.Message);
            }

            return returnData;
        }
    }
}
