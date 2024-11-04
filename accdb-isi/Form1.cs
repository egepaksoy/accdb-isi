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


namespace accdb_isi
{
    public partial class Form1 : Form
    {
        DatabaseControl databaseControl;
        ModbusControl tempretureDevice1;
        ModbusControl tempretureDevice2;
        ModbusControl timeDevice;

        string databasePath = string.Empty;
        string modbusPort = string.Empty;
        int tempreture1ID = -1;
        int tempreture2ID = -1;
        int timerID = -1;
        string[] portNames = SerialPort.GetPortNames();
        string[] oldPortNames;

        string operatorName = string.Empty;
        string makineName = string.Empty;
        int SetSicaklik1 = 0;
        int SetSicaklik2 = 0;
        int GetSicaklik1 = 0;
        int GetSicaklik2 = 0;
        int SetSure = 0;
        
        bool dbConnected = false;
        bool tempreture1Connected = false;
        bool tempreture2Connected = false;
        bool timeConnected = false;
        bool started = false;
        bool btnClicked = false;

        public Form1()
        {
            InitializeComponent();
            // bu değişkenleri ayarlardan çek
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
                    tempretureDevice1 = new ModbusControl(modbusPort, tempreture1ID);
                    tempretureDevice2 = new ModbusControl(modbusPort, tempreture2ID);
                    timeDevice = new ModbusControl(modbusPort, timerID);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Modbus bağlantı hatası: " + ex.Message);
                    return;
                }

                if (!timeConnected)
                    if (timeDevice.ConnectPlc() == null)
                        timeConnected = true;

                if (!tempreture1Connected)
                    if (tempretureDevice1.ConnectPlc() == null)
                        tempreture1Connected = true;

                if (!tempreture2Connected)
                    if (tempretureDevice2.ConnectPlc() == null)
                        tempreture2Connected = true;

                if (tempreture1Connected != timeConnected || tempreture2Connected != timeConnected || tempreture2Connected != tempreture1Connected)
                {
                    tempreture1Connected = !tempretureDevice1.DisconnectPlc();
                    tempreture2Connected = !tempretureDevice2.DisconnectPlc();
                    timeConnected = !timeDevice.DisconnectPlc();
                }

                if (tempreture1Connected == timeConnected == tempreture2Connected)
                    ConnectionController();
            }
            else
            {
                if (tempreture1Connected)
                    tempretureDevice1.DisconnectPlc();
                if (tempreture2Connected)
                    tempretureDevice2.DisconnectPlc();
                if (timeConnected)
                    timeDevice.DisconnectPlc();

                tempretureDevice1 = null;
                tempretureDevice2 = null;
                timeDevice = null;

                if (timeConnected)
                    timeConnected = !timeDevice.DisconnectPlc();

                if (tempreture1Connected)
                    tempreture1Connected = !tempretureDevice1.DisconnectPlc();

                if (tempreture2Connected)
                    tempreture2Connected = !tempretureDevice2.DisconnectPlc();

                if (tempreture1Connected != timeConnected || tempreture2Connected != timeConnected || tempreture2Connected != tempreture1Connected)
                {
                    tempreture1Connected = !tempretureDevice1.DisconnectPlc();
                    tempreture2Connected = !tempretureDevice2.DisconnectPlc();
                    timeConnected = !timeDevice.DisconnectPlc();
                }

                if (tempreture1Connected == timeConnected == tempreture2Connected)
                    ConnectionController();
            }
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
            if (tempreture1Connected && timeConnected && tempreture2Connected)
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
                    timerData = databaseControl.GetData("tblservertopres", "SetOrnekAraligi");

                    operatorName = databaseControl.GetData("tblservertopres", "operator").Split(':')[1].Trim();
                    makineName = databaseControl.GetData("tblservertopres", "makine").Split(':')[1].Trim();
                    SetSicaklik1 = Convert.ToInt32(databaseControl.GetData("tblservertopres", "SetSicaklik1").Split(':')[1].Trim());
                    SetSicaklik2 = Convert.ToInt32(databaseControl.GetData("tblservertopres", "SetSicaklik2").Split(':')[1].Trim());
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

                UpdateLabels(true);
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
                UpdateLabels(false);
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
                    // PLC'den veri al
                    string getSicaklik1 = tempretureDevice1.ReadHoldRegsData((int)HoldRegAddresses.Sicaklik1);
                    string getSicaklik2 = tempretureDevice2.ReadHoldRegsData((int)HoldRegAddresses.Sicaklik2);

                    if (string.IsNullOrEmpty(getSicaklik1) && string.IsNullOrEmpty(getSicaklik2))
                    {
                        ProcessController(false);
                        MessageBox.Show("Modbus cihazlarından veri alma sorunu");
                        return;
                    }

                    GetSicaklik1 = Convert.ToInt32(getSicaklik1);
                    GetSicaklik2 = Convert.ToInt32(getSicaklik2);

                    string blpnokafileData = databaseControl.GetData("tblservertopres", "blpnokafile").Split(':')[1].Trim();
                    int Sicaklik1 = GetSicaklik1;// press sıcaklık 1
                    int Sicaklik2 = GetSicaklik2;// press sıcaklık 2
                    string GetSure = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");// pressten okunan zaman (modbus cihazındaki formatı datetime formatına convert edilmeli)
                    string GetStartTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");// press başlama zamanı
                    string GetFinishTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");// press bitiş zaman

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

            else if (timeConnected && tempreture1Connected && tempreture2Connected)
            {
                try
                {
                    string getSicaklik1 = tempretureDevice1.ReadHoldRegsData((int)HoldRegAddresses.Sicaklik1);
                    string getSicaklik2 = tempretureDevice1.ReadHoldRegsData((int)HoldRegAddresses.Sicaklik2);

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

        public void FirstStart()
        {
            string timerData = string.Empty;
            int timerInterval = 100;

            if (dbConnected && timeConnected && tempreture1Connected && tempreture2Connected)
            {
                SetSicaklik1 = Convert.ToInt32(databaseControl.GetData("tblservertopres", "SetSicaklik1").Split(':')[1].Trim());
                SetSicaklik2 = Convert.ToInt32(databaseControl.GetData("tblservertopres", "SetSicaklik2").Split(':')[1].Trim());
                SetSure = Convert.ToInt32(databaseControl.GetData("tblservertopres", "SetSure").Split(':')[1].Trim());

                btnConnectDB.Enabled = false;
                btnConnectModbus.Enabled = false;

                try
                {
                    //! modbus cihazı olmadığında burası veri yazmada sıkıntı cıkarıyor
                    string temp1Err = tempretureDevice1.WriteHoldRegData((int)HoldRegAddresses.Sicaklik1, SetSicaklik1);
                    string temp2Err = tempretureDevice2.WriteHoldRegData((int)HoldRegAddresses.Sicaklik2, SetSicaklik2);
                    string timerErr = timeDevice.WriteHoldRegData((int)HoldRegAddresses.Timer, SetSure);

                    if (timerErr != null || temp1Err != null || temp2Err != null)
                    {
                        MessageBox.Show("Modbus cihazlarına veri yazma hatası: " + timerErr + temp1Err + temp2Err);
                        return;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Modbus cihazlarına veri yazma hatası: " + ex.Message);
                    return;
                }

                try
                {
                    timerData = databaseControl.GetData("tblservertopres", "SetOrnekAraligi");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Veri çekme hatası: " + ex.Message);
                    return;
                }

                if (timerData != string.Empty)
                    timerInterval = Convert.ToInt32(timerData.Split(':')[1].Trim()) * 1000;
                else
                {
                    MessageBox.Show("Veritabanında süre okuma bilgisi yok");
                    return;
                }

                writeDBTimer.Interval = timerInterval;
                writeDBTimer.Enabled = true;

                UpdateLabels(true);
            }
        }

        private void EndProcess()
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

            btnConnectDB.Enabled = true;
            btnConnectModbus.Enabled = true;

            UpdateLabels(false);
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
                if (!(dbConnected && timeConnected && tempreture1Connected && tempreture2Connected))
                    return;

                btnStart.Text = "Durdur";
                btnStart.BackColor = Color.Red;
                btnStart.ForeColor = Color.White;

                started = true;
                FirstStart();
            }
            else
            {
                ConnectDB(false);
                ConnectModbus(false);

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

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                // PLC'den veri al
                string getSicaklik1 = "1";
                string getSicaklik2 = "2";

                if (string.IsNullOrEmpty(getSicaklik1) && string.IsNullOrEmpty(getSicaklik2))
                {
                    ProcessController(false);
                    MessageBox.Show("Modbus cihazlarından veri alma sorunu");
                    return;
                }

                GetSicaklik1 = Convert.ToInt32(getSicaklik1);
                GetSicaklik2 = Convert.ToInt32(getSicaklik2);

                string blpnokafileData = databaseControl.GetData("tblservertopres", "blpnokafile").Split(':')[1].Trim();
                int Sicaklik1 = GetSicaklik1;// press sıcaklık 1
                int Sicaklik2 = GetSicaklik2;// press sıcaklık 2

                // veritabanına sadece zaman bilgisi kaydedilmiyor onun yerine text olarak kaydetmek daha uygun olabilir
                string GetSure = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");// pressten okunan zaman (modbus cihazındaki formatı datetime formatına convert edilmeli)
                string GetStartTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");// press başlama zamanı
                string GetFinishTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");// press bitiş zaman

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

        private void button1_Click_1(object sender, EventArgs e)
        {
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

            ModbusControl tester1 = new ModbusControl("com3", tempreture1ID);
            //ModbusControl tester2 = new ModbusControl("com3", tempreture2ID);
            //ModbusControl tester3 = new ModbusControl("com3", timerID);

            tester1.ConnectPlc();
            //tester2.ConnectPlc();
            //tester3.ConnectPlc();
            //! tek bağlanıt uzerinden farklı slave adresleri ile yap
        }
    }
}
