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
using System.Threading;
using Utils;


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

        int SetSicaklik1 = int.MinValue;
        int SetSicaklik2 = int.MinValue;
        
        int SetSure = 0;

        int GetSicaklik1 = int.MinValue;
        int GetSicaklik2 = int.MinValue;

        string TimerTime = string.Empty;
        string TimerFormat = string.Empty;

        int setTableID = -1;
        
        bool dbConnected = false;
        bool modbusConnected = false;
        bool threadsCreated = false;
        
        bool started = false;

        bool running = true;

        string pressStartTime = string.Empty;

        Thread updaterThread;

        public Form1()
        {
            InitializeComponent();

            databaseControl = new DatabaseControl(databasePath);
            modbusControl = new ModbusControl();
        }

        private void btnConnectDB_Click(object sender, EventArgs e)
        {
            ConnectDB(btnConnectDB.Text == "Veritabanına Bağlan");
        }

        private void btnConnectModbus_Click(object sender, EventArgs e)
        {
            ConnectModbus(btnConnectModbus.Text == "Modbus Cihazına Bağlan");

            if (threadsCreated == false)
            {
                updaterThread = new Thread(UpdateValues);

                threadsCreated = true;
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
                        if (!modbusControl.connectedRTU)
                            modbusControl.RTUConnect(modbusPort);
                        modbusControl.ConnectPlc();
                    }
                    catch (Exception ex)
                    {
                        modbusControl.DisconnectPlc();
                        
                        if (!string.IsNullOrEmpty(modbusControl.ConnectPlc()))
                        {
                            MessageBox.Show("Modbus bağlantı hatası: " + ex.Message);
                            return;
                        }
                        else
                            modbusConnected = true;
                    }
                }
                modbusControl.ConnectPlc();
                modbusConnected = true;

                TimerFormat = modbusControl.ReadHoldRegsData(timerID, (int)HoldRegAddresses.timerFormat);
            }
            else
            {
                if (modbusConnected)
                {
                    modbusControl.DisconnectPlc();
                    modbusConnected = false;
                }
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
                    btnClearLogs.BackColor = Color.LimeGreen;
                    btnClearLogs.ForeColor = Color.White;
                    btnClearLogs.Enabled = false;
                    btnClearLogs.Text = "Loglar Temiz";
                }
                else
                {
                    btnClearLogs.BackColor = Color.Red;
                    btnClearLogs.ForeColor = Color.White;
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
                btnConnectModbus.BackColor = Color.Red;
                btnConnectModbus.ForeColor = Color.White;
                btnConnectModbus.Text = "Modbus Cihazı Bağlantısını Kes";
            }
            else
            {
                btnConnectModbus.BackColor = Color.LimeGreen;
                btnConnectModbus.ForeColor = Color.Black;
                btnConnectModbus.Text = "Modbus Cihazına Bağlan";
            }
        }

        private void LabelDBConnected()
        {
            if (dbConnected)
            {
                btnConnectDB.BackColor = Color.Red;
                btnConnectDB.ForeColor = Color.White;
                btnConnectDB.Text = "Veritabanı Bağlantısını Kes";
            }
            else
            {
                btnConnectDB.BackColor = Color.LimeGreen;
                btnConnectDB.ForeColor = Color.Black;
                btnConnectDB.Text = "Veritabanına Bağlan";
            }
        }

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

                operatorLabel.Text = "Operatör: -";
                makineLabel.Text = "Makine: -";
                hedefSicaklik1.Text = "Hedef Sıcaklık: -";
                hedefSicaklik2.Text = "Hedef Sıcaklık: -";

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
                    string blpnokafileData = databaseControl.GetData("tblservertopres", "blpnokafile", setTableID).Split(':')[1].Trim();
                    int Sicaklik1 = GetSicaklik1;// press1 sıcaklık
                    int Sicaklik2 = GetSicaklik2;// press2 sıcaklık

                    string timerTime = TimerTime;

                    string GetSure = DateTime.Parse(pressStartTime).Add(TimeSpan.Parse(timerTime)).ToString("yyyy-MM-dd HH:mm:ss");// timer zamani (baslangic + plc zamanı)
                    string GetStartTime = pressStartTime;// press başlama zamanı (başlata basınca gelen zaman)
                    string GetFinishTime = DateTime.Parse(pressStartTime).Add(TimeSpan.Parse($"{Convert.ToInt32(SetSure/3600)}:{Convert.ToInt32(SetSure/60)}:{Convert.ToInt32(SetSure%60)}")).ToString("yyyy-MM-dd HH:mm:ss");// press bitiş zaman (başlangıç + SetSure değeri)

                    string errMessage = databaseControl.WriteData(blpnokafileData, Sicaklik1, Sicaklik2, GetSure, GetStartTime, GetFinishTime, operatorName, makineName);
                    if (!string.IsNullOrEmpty(errMessage))
                    {
                        ProcessController(false);
                        MessageBox.Show("Veritabanına yazmada hata çıktı: " + errMessage);
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
                btnClearLogs.ForeColor = Color.White;
                btnClearLogs.Enabled = false;
                btnClearLogs.Text = "Loglar Temiz";
            }
            else
            {
                btnClearLogs.BackColor = Color.Red;
                btnClearLogs.ForeColor = Color.White;
                btnClearLogs.Enabled = true;
                btnClearLogs.Text = "Logları Temizle";
            }
        }

        private void generalTimer_Tick(object sender, EventArgs e)
        {
            UpdateLabels(modbusConnected & tabControl1.SelectedTab.Name == "tabIslemler");
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

        public void UpdateValues()
        {
            while (running)
            {
                if (modbusConnected)
                {
                    try
                    {
                        GetSicaklik1 = Convert.ToInt32(modbusControl.ReadInputRegsData(tempreture1ID, (int)InputRegAddresses.olculenSicaklik));
                        GetSicaklik2 = Convert.ToInt32(modbusControl.ReadInputRegsData(tempreture2ID, (int)InputRegAddresses.olculenSicaklik));

                        TimerTime = Utils.Utils.PlcToTime(modbusControl.ReadInputRegsData(timerID, (int)InputRegAddresses.timerValue), TimerFormat);
                    }
                    catch
                    {
                        GetSicaklik1 = int.MinValue;
                        GetSicaklik2 = int.MinValue;

                        TimerTime = null;
                    }
                }
            }
        }

        public void UpdateLabels(bool active)
        {
            string tempVal1 = "-";
            string tempVal2 = "-";

            string timeVal = "-:-:-";

            if (active)
            {
                if (!updaterThread.IsAlive)
                    updaterThread.Start();


                if (GetSicaklik1 != int.MinValue)
                    tempVal1 = GetSicaklik1.ToString();
                if (GetSicaklik2 != int.MinValue)
                    tempVal2 = GetSicaklik2.ToString();
                
                if (!string.IsNullOrEmpty(TimerTime))
                    timeVal = TimerTime;
            }
            aktifSicaklik1.Text = $"Aktif Sıcaklık: {tempVal1}";
            aktifSicaklik2.Text = $"Aktif Sıcaklık: {tempVal1}";

            labelTimerValue.Text = timeVal;
        }

        public void FirstStart()
        {
            string timerData = string.Empty;
            int timerInterval = 10000;

            if (dbConnected && modbusConnected)
            {
                SetSicaklik1 = Convert.ToInt32(databaseControl.GetData("tblservertopres", "SetSicaklik1", setTableID).Split(':')[1].Trim());
                SetSicaklik2 = Convert.ToInt32(databaseControl.GetData("tblservertopres", "SetSicaklik2", setTableID).Split(':')[1].Trim());
                SetSure = Convert.ToInt32(databaseControl.GetData("tblservertopres", "SetSure", setTableID).Split(':')[1].Trim());

                try
                {
                    string temp1Err = modbusControl.WriteHoldRegData(tempreture1ID, (int)HoldRegAddresses.setSicaklik1, SetSicaklik1);
                    string temp2Err = modbusControl.WriteHoldRegData(tempreture2ID, (int)HoldRegAddresses.setSicaklik1, SetSicaklik2);

                    if (TimerFormat == null)
                        return;

                    int timerTime = Utils.Utils.TimeToPlc(SetSure, TimerFormat);
                    if (timerTime == -1)
                        return;

                    string timerErr = modbusControl.WriteHoldRegData(timerID, (int)HoldRegAddresses.timerSet, timerTime);

                    if (timerErr != null || temp1Err != null || temp2Err != null)
                    {
                        MessageBox.Show("Modbus cihazına veri yazma hatası: " + timerErr + temp1Err + temp2Err);
                        return;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Modbus cihazına veri yazma hatası: " + ex.Message);
                    return;
                }

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

                btnConnectDB.Enabled = false;
                btnConnectModbus.Enabled = false;

                writeDBTimer.Interval = timerInterval;
                writeDBTimer.Enabled = true;

                started = true;

                writerController.Interval = SetSure * 1000;
                writerController.Enabled = true;

                pressStartTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            }
        }

        private void StopModbus()
        {
            ConnectModbus(false);
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
                ConnectModbus(true);
                if (!(dbConnected && modbusConnected))
                {
                    MessageBox.Show("Veritabanına veya PLC bağlantısı sağlanılamadı");
                    return;
                }

                btnStart.Text = "Durdur";
                btnStart.BackColor = Color.Red;
                btnStart.ForeColor = Color.White;

                started = true;
                FirstStart();
            }
            else
            {
                btnStart.Text = "Başlat";
                btnStart.BackColor = Color.LimeGreen;
                btnStart.ForeColor = Color.Black;

                started = false;
                EndProcess();
            }
        }

        private void btnDBSelect_Click(object sender, EventArgs e)
        {
            FileRead();
            textBoxDBPath.Text = databasePath;
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

        private void settingsTextsChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBoxDBPath.Text) && databasePath != textBoxDBPath.Text)
                databasePath = textBoxDBPath.Text;

            if (!string.IsNullOrEmpty(textBoxSetTableID.Text) && Convert.ToInt32(textBoxSetTableID.Text) != setTableID)
                setTableID = Convert.ToInt32(textBoxSetTableID.Text);

            if (!string.IsNullOrEmpty(textBoxTimerID.Text) && Convert.ToInt32(textBoxTimerID.Text) != timerID)
                timerID = Convert.ToInt32(textBoxTimerID.Text);
            if (!string.IsNullOrEmpty(textBoxTemp1ID.Text) && Convert.ToInt32(textBoxTemp1ID.Text) != timerID)
                tempreture1ID = Convert.ToInt32(textBoxTemp1ID.Text);
            if (!string.IsNullOrEmpty(textBoxTemp2ID.Text) && Convert.ToInt32(textBoxTemp2ID.Text) != timerID)
                tempreture2ID = Convert.ToInt32(textBoxTemp2ID.Text);
        }

        private void comboBoxModbusConn_Click(object sender, EventArgs e)
        {
            portNames = SerialPort.GetPortNames();

            if (portNames != oldPortNames)
            {
                oldPortNames = portNames;
                comboBoxModbusConn.Items.Clear();
                comboBoxModbusConn.Items.AddRange(portNames);
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            running = false;
            Application.ExitThread();
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (updaterThread.IsBackground || updaterThread.IsAlive)
                updaterThread.Abort();

            Application.Exit();
        }
    }
}
