using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.OleDb;
using DatabaseController;
using ModbusController;
using Addresses;
using System.IO.Ports;
using System.Diagnostics.Eventing.Reader;
using System.Runtime.CompilerServices;


namespace accdb_isi
{
    public partial class Form1 : Form
    {
        DatabaseControl databaseControl;
        ModbusControl modbusControl;

        string databasePath = string.Empty;
        string modbusPort = string.Empty;
        int tempreture1ID = -1;
        int tempreture2ID = -1;
        int timerID = -1;
        string[] portNames = SerialPort.GetPortNames();
        string[] oldPortNames;

        string operatorName = string.Empty;
        string makineName = string.Empty;
        string TimerTime = string.Empty;
        int SetSicaklik1 = 0;
        int SetSicaklik2 = 0;
        int GetSicaklik1 = 0;
        int GetSicaklik2 = 0;
        int SetSure = 0;

        int setTableID = -1;
        
        bool dbConnected = false;
        bool modbusConnected = false;
        
        bool started = false;
        bool btnClicked = false;

        string pressStartTime = string.Empty;

        public Form1()
        {
            InitializeComponent();
            databaseControl = new DatabaseControl(databasePath);
        }

        private void btnConnectDB_Click(object sender, EventArgs e)
        {
            if (btnConnectDB.Text == "Veritabanına Bağlan")
            {
                ConnectDB(true);
            }
            else
            {
                ConnectDB(false);
            }
        }

        private void btnConnectModbus_Click(object sender, EventArgs e)
        {
            if (btnConnectModbus.Text == "Modbus Cihazına Bağlan")
            {
                ConnectModbus(true);
            }
            else
            {
                ConnectModbus(false);
            }
        }

        private void ConnectModbus(bool connect)
        {
            if (connect)
            {
                if (!modbusConnected)
                {
                    if (modbusPort == string.Empty)
                    {
                        MessageBox.Show("Ayarlardan modbus cihazı yolunu seçin");
                        return;
                    }
                    if (tempreture1ID < 0 || timerID < 0 || tempreture2ID < 0)
                    {
                        MessageBox.Show("Ayarlardan modbus cihazlarının slave idlerini girin");
                        return;
                    }

                    try
                    {
                        modbusControl = new ModbusControl(modbusPort);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Modbus bağlantı hatası: " + ex.Message);
                        return;
                    }

                    if (modbusControl.ConnectPlc() == null)
                        modbusConnected = true;
                }
            }
            else
            {
                if (modbusConnected)
                    modbusConnected = !modbusControl.DisconnectPlc();

                modbusControl = null;
            }
            ConnectionController();
        }

        private void ConnectDB(bool connect)
        {
            if (connect)
            {
                if (databasePath == string.Empty)
                {
                    MessageBox.Show("Ayarlardan veritabanı seçin");
                    return;
                }

                if (string.IsNullOrEmpty(textBoxSetTableID.Text) || !decimal.TryParse(textBoxSetTableID.Text, out decimal res))
                {
                    MessageBox.Show("Ayarlardan set tablosunun id'sini seçin");
                    return;
                }

                else if (!string.IsNullOrEmpty(textBoxSetTableID.Text) && decimal.TryParse(textBoxSetTableID.Text, out decimal result))
                    setTableID = Convert.ToInt32(textBoxSetTableID.Text);

                if (!dbConnected)
                {
                    databaseControl = new DatabaseControl(databasePath);
                    dbConnected = databaseControl.ConnectDatabase();
                }

                ConnectionController();

                int satirSayisi = Convert.ToInt32(databaseControl.DataCount("tblprestoserver"));

                if (satirSayisi == 0)
                {
                    btnClearLogs.BackColor = Color.Green;
                    btnClearLogs.Enabled = false;
                    btnClearLogs.Text = "Loglar Temiz";
                }
                else
                {
                    btnClearLogs.BackColor = Color.Red;
                    btnClearLogs.Enabled = true;
                    btnClearLogs.Text = "Logları Temizle";
                }
            }
            else
            {
                if (dbConnected)
                    dbConnected = !databaseControl.DisconnectDatabase();
                ConnectionController();
            }
        }

        private void LabelModbusConnected()
        {
            if (modbusConnected)
            {
                labelModbusConnected.Text = "Modbus Bağlı";
                labelModbusConnected.ForeColor = Color.Green;
                btnConnectModbus.Text = "Modbus Cihazı Bağlantısını Kes";
            }
            else
            {
                labelModbusConnected.Text = "Bağlı Değil";
                labelModbusConnected.ForeColor = Color.Red;
                btnConnectModbus.Text = "Modbus Cihazına Bağlan";
            }
        }

        private void LabelDBConnected()
        {
            if (dbConnected)
            {
                labelDBConnected.Text = "Veritabanı Bağlı";
                labelDBConnected.ForeColor = Color.Green;
                btnConnectDB.Text = "Veritabanı Bağlantısını Kes";
            }
            else
            {
                labelDBConnected.Text = "Bağlı Değil";
                labelDBConnected.ForeColor = Color.Red;
                btnConnectDB.Text = "Veritabanına Bağlan";
            }
        }

        // bu ikisini de kontrol edicek, ikisi de bağlı ise olucak
        private void ConnectionController()
        {
            if (dbConnected)
            {
                string timerData = string.Empty;
                int timerInterval = 100;

                try
                {
                    timerData = databaseControl.GetData("tblservertopres", "SetOrnekAraligi", setTableID);

                    operatorName = databaseControl.GetData("tblservertopres", "operator", setTableID).Split(':')[1].Trim();
                    makineName = databaseControl.GetData("tblservertopres", "makine", setTableID).Split(':')[1].Trim();
                    SetSicaklik1 = Convert.ToInt32(databaseControl.GetData("tblservertopres", "SetSicaklik1", setTableID).Split(':')[1].Trim());
                    SetSicaklik2 = Convert.ToInt32(databaseControl.GetData("tblservertopres", "SetSicaklik2", setTableID).Split(':')[1].Trim());
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Veritabanından veri çekme hatası: " + ex.Message);
                }

                operatorLabel.Text = "Operatör: " + operatorName;
                makineLabel.Text = "Makine: " + makineName;
                hedefSicaklik1.Text = "Hedef Sıcaklık: " + SetSicaklik1;
                hedefSicaklik2.Text = "Hedef Sıcaklık: " + SetSicaklik2;

                if (timerData != string.Empty)
                    timerInterval = Convert.ToInt32(timerData.Split(':')[1].Trim()) * 1000;
                else
                    MessageBox.Show("Veritabanında süre okuma bilgisi yok");
            }
            else
            {
                operatorName = string.Empty;
                makineName = string.Empty;
                SetSicaklik1 = 0;
                SetSicaklik2 = 0;

                operatorLabel.Text = "Operatör:";
                makineLabel.Text = "Makine:";
                hedefSicaklik1.Text = "Hedef Sıcaklık:";
                hedefSicaklik2.Text = "Hedef Sıcaklık:";

                writeDBTimer.Interval = 100;
                writeDBTimer.Enabled = false;
            }

            LabelDBConnected();
            LabelModbusConnected();
        }

        private void writeDBTimer_Tick(object sender, EventArgs e)
        {
            if (started)
            {
                try
                {
                    //! rbf
                    //string getSicaklik1 = modbusControl.ReadInputRegsData(tempreture1ID, (int)InputRegAddresses.olculenSicaklik);
                    //string getSicaklik2 = modbusControl.ReadInputRegsData(tempreture2ID, (int)InputRegAddresses.olculenSicaklik);

                    //string timerFormat = modbusControl.ReadHoldRegsData(timerID, (int)HoldRegAddresses.timerFormat);
                    //string timerTime = modbusControl.ReadInputRegsData(timerID, (int)InputRegAddresses.timerValue);

                    //if (string.IsNullOrEmpty(getSicaklik1) || string.IsNullOrEmpty(getSicaklik2) || string.IsNullOrEmpty(timerFormat) || string.IsNullOrEmpty(timerTime))
                    //{
                    //    ProcessController(false);
                    //    MessageBox.Show("Modbus cihazlarından veri alma sorunu");
                    //    return;
                    //}

                    //TimerTime = ConvertTimer(timerTime, timerFormat);

                    //! rbf
                    TimerTime = ConvertTimer("10", "2");

                    //! rbf
                    //GetSicaklik1 = Convert.ToInt32(getSicaklik1);
                    //GetSicaklik2 = Convert.ToInt32(getSicaklik2);

                    //! rbf
                    GetSicaklik1 = Convert.ToInt32("400");
                    GetSicaklik2 = Convert.ToInt32("300");

                    string blpnokafileData = databaseControl.GetData("tblservertopres", "blpnokafile", setTableID).Split(':')[1].Trim();
                    int Sicaklik1 = GetSicaklik1;// press1 sıcaklık
                    int Sicaklik2 = GetSicaklik2;// press2 sıcaklık
                    string GetSure = DateTime.Parse(pressStartTime).Add(TimeSpan.Parse(TimerTime)).ToString("yyyy-MM-dd HH:mm:ss");// timer zamani (baslangic + plc zamanı)
                    string GetStartTime = pressStartTime;// press başlama zamanı (başlata basınca gelen zaman)
                    string GetFinishTime = DateTime.Parse(pressStartTime).Add(TimeSpan.Parse($"{Convert.ToInt32(SetSure/3600)}:{Convert.ToInt32(SetSure/60)}:{Convert.ToInt32(SetSure%60)}")).ToString("yyyy-MM-dd HH:mm:ss");// press bitiş zaman (başlangıç + SetSure değeri)

                    string errMessage = databaseControl.WriteData(blpnokafileData, Sicaklik1, Sicaklik2, GetSure, GetStartTime, GetFinishTime, operatorName, makineName);
                    if (!string.IsNullOrEmpty(errMessage))
                    {
                        ProcessController(false);
                        MessageBox.Show("Veritabanına değerleri yazmada hata çıktı: " + errMessage);
                        return;
                    }
                }
                catch (Exception ex)
                {
                    ProcessController(false);
                    MessageBox.Show("Veritabanına değerleri yazmada hata çıktı: " + ex.Message);
                    return;
                }
                
            }
        }

        private string ConvertTimer(string TimerTime, string timerFormat)
        {
            double hour = 0;
            double minute = 0;
            double second = 0;
            string timerTime = "";

            // plc zamanını 4 haneli yapma
            if (timerTime.Length != 4)
            {
                for (int i = 0; i < timerTime.Length - 4; i++)
                    timerTime += "0";
                timerTime += TimerTime;
            }

            // plc zaman formatını normal saat:dakika:saniye formatına dondurme
            if (timerFormat == "0")
                second = Convert.ToInt32(timerTime) / 100.0;
            else if (timerFormat == "1")
                second = Convert.ToInt32(timerTime) / 10.0;
            else if (timerFormat == "2")
                second = Convert.ToDouble(timerTime);
            else if (timerFormat == "3")
            {
                minute = Convert.ToInt32(Convert.ToInt32(timerTime) / 100.0);
                second = Convert.ToInt32(timerTime) % 100;
            }
            else if (timerFormat == "4")
                minute = Convert.ToInt32(timerTime) / 10.0;
            else if (timerFormat == "5")
                minute = Convert.ToDouble(timerTime);
            else if (timerFormat == "6")
            {
                hour = Convert.ToInt32(Convert.ToInt32(timerTime) / 100.0);
                minute = Convert.ToInt32(timerTime) % 100;
            }
            else if (timerFormat == "7")
                hour = Convert.ToInt32(timerTime) / 10.0;
            else if (timerFormat == "8")
                hour = Convert.ToDouble(timerTime);

            // 60 dakika formatında dondurme
            if (second >= 60)
            {
                minute += Convert.ToInt32(second / 60);
                second %= 60;
            }
            if (minute >= 60)
            {
                hour += Convert.ToInt32(minute / 60);
                minute %= 60;
            }

            hour = Convert.ToInt32(hour);
            minute = Convert.ToInt32(minute);
            second = Convert.ToInt32(second);

            return $"{hour}:{minute}:{second}";
        }

        private void btnClearLogs_Click(object sender, EventArgs e)
        {
            try
            {
                databaseControl.DeleteData("tblprestoserver");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Veritabanını temizlerken sorun çıktı: " + ex.Message);
            }

            int satirSayisi = Convert.ToInt32(databaseControl.DataCount("tblprestoserver"));

            if (satirSayisi == 0)
            {
                btnClearLogs.BackColor = Color.Green;
                btnClearLogs.Enabled = false;
                btnClearLogs.Text = "Loglar Temiz";
            }
            else
            {
                btnClearLogs.BackColor = Color.Red;
                btnClearLogs.Enabled = true;
                btnClearLogs.Text = "Logları Temizle";
            }
        }

        private void generalTimer_Tick(object sender, EventArgs e)
        {
            if (tabControl1.SelectedTab.Name == "tabAyarlar")
            {
                portNames = SerialPort.GetPortNames();

                if (portNames != oldPortNames)
                {
                    oldPortNames = portNames;
                    comboBoxModbusConn.Items.Clear();
                    comboBoxModbusConn.Items.AddRange(portNames);
                }

                if (textBoxDBPath.Text != databasePath)
                    textBoxDBPath.Text = databasePath;

                if (!string.IsNullOrEmpty(textBoxTemp1ID.Text))
                {
                    if (Convert.ToInt32(textBoxTemp1ID.Text) != tempreture1ID)
                        tempreture1ID = Convert.ToInt32(textBoxTemp1ID.Text);
                }

                if (!string.IsNullOrEmpty(textBoxTemp2ID.Text))
                {
                    if (Convert.ToInt32(textBoxTemp2ID.Text) != tempreture2ID)
                        tempreture2ID = Convert.ToInt32(textBoxTemp2ID.Text);
                }

                if (!string.IsNullOrEmpty(textBoxTimerID.Text))
                {
                    if (Convert.ToInt32(textBoxTimerID.Text) != timerID)
                        timerID = Convert.ToInt32(textBoxTimerID.Text);
                }
            }

            if (modbusConnected)
            {
                try
                {
                    string getSicaklik1 = modbusControl.ReadInputRegsData(tempreture1ID, (int)InputRegAddresses.olculenSicaklik);
                    string getSicaklik2 = modbusControl.ReadInputRegsData(tempreture2ID, (int)InputRegAddresses.olculenSicaklik);

                    if (string.IsNullOrEmpty(getSicaklik1) && string.IsNullOrEmpty(getSicaklik2))
                    {
                        ProcessController(false);
                        return;
                    }

                    GetSicaklik1 = Convert.ToInt32(getSicaklik1);
                    GetSicaklik2 = Convert.ToInt32(getSicaklik2);
                }
                catch (Exception ex)
                {
                    ProcessController(false);
                    MessageBox.Show("Modbus cihazlarından veri alma sorunu: " + ex.Message);
                }

                UpdateLabels(true);
            }
        }

        private void FileRead()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Access Database Files (*.accdb)|*.accdb";
            openFileDialog.Multiselect = false;
            openFileDialog.Title = "Access Veritabanı Dosyaları Seç";

            if (openFileDialog.ShowDialog() == DialogResult.OK)
                databasePath = openFileDialog.FileName;
        }

        public void UpdateLabels(bool active)
        {
            if (active)
            {
                aktifSicaklik1.Text = "Aktif Sıcaklık: " + GetSicaklik1.ToString();
                aktifSicaklik2.Text = "Aktif Sıcaklık: " + GetSicaklik2.ToString();
            }
            else
            {
                aktifSicaklik1.Text = "Aktif Sıcaklık: ";
                aktifSicaklik2.Text = "Aktif Sıcaklık: ";
            }
        }

        public int ConvertToPLC(int setSure, string format)
        {
            if (format == "0")
            {
                if (setSure >= 100)
                {
                    MessageBox.Show("PLC sure formatı ile veritabanı formatı uyuşmazlığı.\nVeritabanındaki SetSure bilgisinin PLC'deki formata yazılabildiğinden emin olun!");
                    return -1;
                }
                else if (setSure < 100)
                    return setSure * 100;
            }
            if (format == "1")
            {
                if (setSure >= 1000)
                {
                    MessageBox.Show("PLC sure formatı ile veritabanı formatı uyuşmazlığı.\nVeritabanındaki SetSure bilgisinin PLC'deki formata yazılabildiğinden emin olun!");
                    return -1;
                }
                else if (setSure < 1000)
                    return setSure * 10;
            }
            if (format == "2")
            {
                if (setSure >= 10000)
                {
                    MessageBox.Show("PLC sure formatı ile veritabanı formatı uyuşmazlığı.\nVeritabanındaki SetSure bilgisinin PLC'deki formata yazılabildiğinden emin olun!");
                    return -1;
                }
                else if (setSure < 10000)
                    return setSure;
            }
            if (format == "3")
            {
                if (setSure >= 6000)
                {
                    MessageBox.Show("PLC sure formatı ile veritabanı formatı uyuşmazlığı.\nVeritabanındaki SetSure bilgisinin PLC'deki formata yazılabildiğinden emin olun!");
                    return -1;
                }
                else if (setSure < 6000)
                    return Convert.ToInt32(setSure / 60) * 100 + setSure % 60;
            }
            if (format == "4")
            {
                if (setSure >= 60000)
                {
                    MessageBox.Show("PLC sure formatı ile veritabanı formatı uyuşmazlığı.\nVeritabanındaki SetSure bilgisinin PLC'deki formata yazılabildiğinden emin olun!");
                    return -1;
                }
                else if (SetSure < 60000)
                    return Convert.ToInt32(setSure / 60) * 10 + Convert.ToInt32((setSure / 60 - Convert.ToInt32(setSure / 60)) * 10);
            }
            if (format == "5")
            {
                if (setSure >= 600000)
                {
                    MessageBox.Show("PLC sure formatı ile veritabanı formatı uyuşmazlığı.\nVeritabanındaki SetSure bilgisinin PLC'deki formata yazılabildiğinden emin olun!");
                    return -1;
                }
                else if (setSure < 600000)
                    return Convert.ToInt32(setSure / 60);
            }
            return -1;
        }

        public void FirstStart()
        {
            string timerData = string.Empty;
            int timerInterval = 10000;

            //! rbf
            //if (dbConnected && modbusConnected)
            if (dbConnected)
            {
                SetSicaklik1 = Convert.ToInt32(databaseControl.GetData("tblservertopres", "SetSicaklik1", setTableID).Split(':')[1].Trim());
                SetSicaklik2 = Convert.ToInt32(databaseControl.GetData("tblservertopres", "SetSicaklik2", setTableID).Split(':')[1].Trim());
                SetSure = Convert.ToInt32(databaseControl.GetData("tblservertopres", "SetSure", setTableID).Split(':')[1].Trim());

                btnConnectDB.Enabled = false;
                btnConnectModbus.Enabled = false;

                //! rbf
                //try
                //{
                //    string temp1Err = modbusControl.WriteHoldRegData(tempreture1ID, (int)HoldRegAddresses.setSicaklik1, SetSicaklik1);
                //    string temp2Err = modbusControl.WriteHoldRegData(tempreture2ID, (int)HoldRegAddresses.setSicaklik1, SetSicaklik2);

                //    //! setsurey'yi modbus formatında yaz
                //    string timerFormat = modbusControl.ReadHoldRegsData(timerID, (int)HoldRegAddresses.timerFormat).Split(':')[0];
                //    if (timerFormat == null)
                //        return;

                //    int timerTime = ConvertToPLC(SetSure, timerFormat);
                //    if (timerTime == -1)
                //        return;

                //    string timerErr = modbusControl.WriteHoldRegData(timerID, (int)HoldRegAddresses.timerSet, SetSure);

                //    if (timerErr != null || temp1Err != null || temp2Err != null)
                //    {
                //        MessageBox.Show("Modbus cihazına veri yazma hatası: " + timerErr + temp1Err + temp2Err);
                //        return;
                //    }
                //}
                //catch (Exception ex)
                //{
                //    MessageBox.Show("Modbus cihazına veri yazma hatası: " + ex.Message);
                //    return;
                //}

                try
                {
                    timerData = databaseControl.GetData("tblservertopres", "SetOrnekAraligi", setTableID);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Zamanla yazma hatası: " + ex.Message + $"\nSistem {timerInterval/1000} saniyede bir veri yazıcak");
                    timerData = string.Empty;
                }

                if (timerData != string.Empty)
                    timerInterval = Convert.ToInt32(timerData.Split(':')[1].Trim()) * 1000;

                writeDBTimer.Interval = timerInterval;
                writeDBTimer.Enabled = true;

                writerController.Interval = SetSure * 1000;
                writerController.Enabled = true;

                pressStartTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

                UpdateLabels(true);
            }
        }

        private void StopModbus()
        {
            //! modbusları durdur
        }

        private void EndProcess()
        {
            StopModbus();
            writerController.Stop();
            writerController.Enabled = false;
            writerController.Interval = 100;

            writeDBTimer.Interval = 100;
            writeDBTimer.Enabled = false;

            btnConnectDB.Enabled = true;
            btnConnectModbus.Enabled = true;
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            ProcessController(btnStart.Text == "Başlat");
        }

        private void ProcessController(bool connect)
        {
            if (connect)
            {
                ConnectDB(true);
                //! rbf
                //ConnectModbus(true);
                //if (!(dbConnected && modbusConnected))
                //{
                //    MessageBox.Show("Veritabanına veya PLC bağlantısı sağlanılamadı");
                //    return;
                //}

                btnStart.Text = "Durdur";
                btnStart.BackColor = Color.Red;
                btnStart.ForeColor = Color.White;

                started = true;
                FirstStart();
            }
            else
            {
                btnStart.Text = "Başlat";
                btnStart.BackColor = Color.Lime;
                btnStart.ForeColor = Color.Black;

                started = false;
                EndProcess();
            }
        }

        private void btnDBSelect_Click(object sender, EventArgs e)
        {
            FileRead();
        }

        private void comboBoxModbusConn_SelectionChangeCommited(object sender, EventArgs e)
        {
            modbusPort = comboBoxModbusConn.SelectedItem.ToString();
        }

        private void writerController_Tick(object sender, EventArgs e)
        {
            ProcessController(false);
            MessageBox.Show("İşlem tamamlandı");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ProcessController(true);
        }
    }
}
