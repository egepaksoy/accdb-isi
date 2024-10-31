﻿using System;
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
        ModbusControl tempretureDevice;
        ModbusControl timeDevice;

        string databasePath = string.Empty;
        string modbusPort = string.Empty;
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
        bool tempretureConnected = false;
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

                try
                {
                    tempretureDevice = new ModbusControl(modbusPort, 1);
                    timeDevice = new ModbusControl(modbusPort, 3);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Modbus bağlantı hatası: " + ex.Message);
                    return;
                }

                //if (!timeConnected)
                //    if (timeDevice.ConnectPlc() == null)
                //        timeConnected = true;

                //if (!tempretureConnected)
                //    if (tempretureDevice.ConnectPlc() == null)
                //        tempretureConnected = true;

                //if (tempretureConnected != timeConnected)
                //{
                //    tempretureConnected = !tempretureDevice.DisconnectPlc();
                //    timeConnected = !timeDevice.DisconnectPlc();
                //}

                timeConnected = true;
                tempretureConnected = true;

                if (tempretureConnected == timeConnected)
                    ConnectionController();
            }
            else
            {
                //if (timeConnected)
                //    timeConnected = !timeDevice.DisconnectPlc();

                //if (tempretureConnected)
                //    tempretureConnected = !tempretureDevice.DisconnectPlc();

                //if (tempretureConnected != timeConnected)
                //{
                //    tempretureConnected = !tempretureDevice.DisconnectPlc();
                //    timeConnected = !timeDevice.DisconnectPlc();
                //}

                tempretureConnected = false;
                timeConnected = false;

                if (tempretureConnected == timeConnected)
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
            if (tempretureConnected && timeConnected)
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
            if (dbConnected && timeConnected && tempretureConnected)
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
                    string blpnokafileData = databaseControl.GetData("tblservertopres", "blpnokafile").Split(':')[1].Trim();
                    int Sicaklik1 = GetSicaklik1;// press sıcaklık 1
                    int Sicaklik2 = GetSicaklik2;// press sıcaklık 2
                    string GetSure = "13";// pressten okunan zaman (bunu kendim görmeliyim)
                    string GetStartTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");// press başlama zamanı
                    string GetFinishTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");// press bitiş zaman

                    databaseControl.WriteData(blpnokafileData, Sicaklik1, Sicaklik2, GetSure, GetStartTime, GetFinishTime, operatorName, makineName);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Veritabanına değerleri yazmada hata çıktı: " + ex.Message);
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
            }

            else if (timeConnected && tempretureConnected)
            {
                try
                {
                    string getSicaklik1 = tempretureDevice.ReadHoldRegsData((int)HoldRegAddresses.Sicaklik1);
                    string getSicaklik2 = tempretureDevice.ReadHoldRegsData((int)HoldRegAddresses.Sicaklik2);

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

            if (dbConnected && timeConnected && tempretureConnected)
            {
                SetSicaklik1 = Convert.ToInt32(databaseControl.GetData("tblservertopres", "SetSicaklik1").Split(':')[1].Trim());
                SetSicaklik2 = Convert.ToInt32(databaseControl.GetData("tblservertopres", "SetSicaklik2").Split(':')[1].Trim());
                SetSure = Convert.ToInt32(databaseControl.GetData("tblservertopres", "SetSure").Split(':')[1].Trim());

                btnConnectDB.Enabled = false;
                btnConnectModbus.Enabled = false;

                try
                {
                    //! modbus cihazı olmadığında burası veri yazmada sıkıntı cıkarıyor
                    string temp1Err = tempretureDevice.WriteHoldRegData((int)HoldRegAddresses.Sicaklik1, SetSicaklik1);
                    string temp2Err = tempretureDevice.WriteHoldRegData((int)HoldRegAddresses.Sicaklik2, SetSicaklik2);
                    string timerErr = timeDevice.WriteHoldRegData((int)HoldRegAddresses.Sure, SetSure);

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
                if (!(dbConnected && timeConnected && tempretureConnected))
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
    }
}
