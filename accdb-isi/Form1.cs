using System;
using System.Drawing;
using System.Windows.Forms;
using DatabaseController;
using ModbusController;
using Addresses;
using System.IO.Ports;
using System.Threading;

namespace accdb_isi
{
    public partial class Form1 : Form
    {
        DatabaseControl DatabaseControl;
        ModbusControl ModbusControl;

        string DatabasePath = string.Empty;
        string ModbusPort = string.Empty;
        int Tempreture1ID = -1;
        int Tempreture2ID = -1;
        int TimerID = -1;
        string[] PortNames = SerialPort.GetPortNames();
        string[] OldPortNames;

        string OperatorName = string.Empty;
        string MakineName = string.Empty;
        string BlpNoKafileData = string.Empty;

        int SetSicaklik1 = int.MinValue;
        int SetSicaklik2 = int.MinValue;

        int SetSure = 0;

        int GetSicaklik1 = int.MinValue;
        int GetSicaklik2 = int.MinValue;

        string TimerTime = string.Empty;
        string TimerFormat = string.Empty;

        int SetTableID = -1;

        bool DBConnected = false;
        bool ModbusConnected = false;

        bool PlcWriting = false;

        bool Started = false;

        string PressStartTime = string.Empty;

        Thread UpdaterThread;

        public Form1()
        {
            InitializeComponent();

            DatabaseControl = new DatabaseControl(DatabasePath);
            
            UpdaterThread = new Thread(UpdateValues);
            
            ModbusControl = new ModbusControl();
        }

        private void btnConnectDB_Click(object sender, EventArgs e)
        {
            ConnectDB(btnConnectDB.Text == "Veritabanına Bağlan");
        }

        private void btnConnectModbus_Click(object sender, EventArgs e)
        {
            ConnectModbus(btnConnectModbus.Text == "Modbus Cihazına Bağlan");
        }

        private void ConnectModbus(bool connect)
        {
            if (connect)
            {
                if (!ModbusConnected)
                {
                    if (ModbusPort == string.Empty)
                    {
                        MessageBox.Show("Ayarlardan modbus cihazı yolunu seçin");
                        return;
                    }
                    if (Tempreture1ID < 0 || TimerID < 0 || Tempreture2ID < 0)
                    {
                        MessageBox.Show("Ayarlardan modbus cihazlarının slave idlerini girin");
                        return;
                    }

                    try
                    {
                        if (!ModbusControl.connectedRTU)
                            ModbusControl.RTUConnect(ModbusPort);
                        ModbusControl.ConnectPlc();
                    }
                    catch (Exception ex)
                    {
                        ModbusControl.DisconnectPlc();

                        if (!string.IsNullOrEmpty(ModbusControl.ConnectPlc()))
                        {
                            MessageBox.Show("Modbus bağlantı hatası: " + ex.Message);
                            return;
                        }
                        else
                            ModbusConnected = true;
                    }
                }
                ModbusControl.ConnectPlc();
                ModbusConnected = true;

                TimerFormat = ModbusControl.ReadHoldRegsData(TimerID, (int)HoldRegAddresses.timerFormat);
            }
            else
            {
                if (ModbusConnected)
                {
                    ModbusControl.DisconnectPlc();
                    ModbusConnected = false;
                }
            }
            ConnectionController();
        }

        private void ConnectDB(bool connect)
        {
            if (connect)
            {
                if (DatabasePath == string.Empty)
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
                    SetTableID = Convert.ToInt32(textBoxSetTableID.Text);

                if (!DBConnected)
                {
                    DatabaseControl = new DatabaseControl(DatabasePath);
                    DBConnected = DatabaseControl.ConnectDatabase();
                }

                ConnectionController();

                int satirSayisi = Convert.ToInt32(DatabaseControl.DataCount("tblprestoserver"));

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
                if (DBConnected)
                    DBConnected = !DatabaseControl.DisconnectDatabase();
                ConnectionController();
            }
        }

        private void LabelModbusConnected()
        {
            if (ModbusConnected)
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
            if (DBConnected)
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
            if (DBConnected)
            {
                string timerData = string.Empty;
                int timerInterval = 100;

                try
                {
                    timerData = DatabaseControl.GetData("tblservertopres", "SetOrnekAraligi", SetTableID);

                    OperatorName = DatabaseControl.GetData("tblservertopres", "operator", SetTableID).Split(':')[1].Trim();
                    MakineName = DatabaseControl.GetData("tblservertopres", "makine", SetTableID).Split(':')[1].Trim();
                    BlpNoKafileData = DatabaseControl.GetData("tblservertopres", "blpnokafile", SetTableID).Split(':')[1].Trim();

                    SetSicaklik1 = Convert.ToInt32(DatabaseControl.GetData("tblservertopres", "SetSicaklik1", SetTableID).Split(':')[1].Trim());
                    SetSicaklik2 = Convert.ToInt32(DatabaseControl.GetData("tblservertopres", "SetSicaklik2", SetTableID).Split(':')[1].Trim());
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Veritabanından veri çekme hatası: " + ex.Message);
                }

                operatorLabel.Text = "Operatör: " + OperatorName;
                makineLabel.Text = "Makine: " + MakineName;
                hedefSicaklik1.Text = "Hedef Sıcaklık: " + SetSicaklik1;
                hedefSicaklik2.Text = "Hedef Sıcaklık: " + SetSicaklik2;

                if (timerData != string.Empty)
                    timerInterval = Convert.ToInt32(timerData.Split(':')[1].Trim()) * 1000;
                else
                    MessageBox.Show("Veritabanında süre okuma bilgisi yok");
            }
            else
            {
                OperatorName = string.Empty;
                MakineName = string.Empty;
                BlpNoKafileData = string.Empty;

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
            if (Started)
            {
                try
                {
                    if (GetSicaklik1 == int.MinValue || GetSicaklik2 == int.MinValue || string.IsNullOrEmpty(TimerTime))
                    {
                        ProcessController(false);
                        MessageBox.Show("PLC verileri alınamadı");
                        return;
                    }

                    string GetSure = DateTime.Parse(PressStartTime).Add(TimeSpan.Parse(TimerTime)).ToString("yyyy-MM-dd HH:mm:ss");// timer zamani (baslangic + plc zamanı)
                    string GetStartTime = PressStartTime;// press başlama zamanı (başlata basınca gelen zaman)
                    string GetFinishTime = DateTime.Parse(PressStartTime).Add(TimeSpan.Parse($"{Convert.ToInt32(SetSure / 3600)}:{Convert.ToInt32(SetSure / 60)}:{Convert.ToInt32(SetSure % 60)}")).ToString("yyyy-MM-dd HH:mm:ss");// press bitiş zaman (başlangıç + SetSure değeri)

                    string errMessage = DatabaseControl.WriteData(BlpNoKafileData, GetSicaklik1, GetSicaklik2, GetSure, GetStartTime, GetFinishTime, OperatorName, MakineName);
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
                DatabaseControl.DeleteData("tblprestoserver");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Veritabanını temizlerken sorun çıktı: " + ex.Message);
            }

            int satirSayisi = Convert.ToInt32(DatabaseControl.DataCount("tblprestoserver"));

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
            UpdateLabels(ModbusConnected & tabControl1.SelectedTab.Name == "tabIslemler");
        }

        private void FileRead()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Access Database Files (*.accdb)|*.accdb";
            openFileDialog.Multiselect = false;
            openFileDialog.Title = "Access Veritabanı Dosyaları Seç";

            if (openFileDialog.ShowDialog() == DialogResult.OK)
                DatabasePath = openFileDialog.FileName;
        }

        public void UpdateValues()
        {
            int tempSicaklik1 = 0;
            int tempSicaklik2 = 0;

            while (true)
            {
                if (ModbusConnected)
                {
                    if (!PlcWriting)
                    {
                        lock (ModbusControl)
                        {
                            try
                            {
                                if (ModbusConnected)
                                {
                                    tempSicaklik1 = Convert.ToInt32(ModbusControl.ReadInputRegsData(Tempreture1ID, (int)InputRegAddresses.olculenSicaklik));
                                    if (tempSicaklik1 >= 1000)
                                        tempSicaklik1 /= 1000;
                                    GetSicaklik1 = tempSicaklik1;
                                }
                                if (ModbusConnected)
                                {
                                    tempSicaklik2 = Convert.ToInt32(ModbusControl.ReadInputRegsData(Tempreture2ID, (int)InputRegAddresses.olculenSicaklik));
                                    if (tempSicaklik2 >= 1000)
                                        tempSicaklik2 /= 1000;
                                    GetSicaklik2 = tempSicaklik2;
                                }

                                if (ModbusConnected)
                                    TimerTime = Utils.Utils.PlcToTime(ModbusControl.ReadInputRegsData(TimerID, (int)InputRegAddresses.timerValue), TimerFormat);
                            }
                            catch
                            {
                                GetSicaklik1 = int.MinValue;
                                GetSicaklik2 = int.MinValue;

                                TimerTime = null;
                            }
                        }
                        Thread.Sleep(generalTimer.Interval);
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
                if (!UpdaterThread.IsAlive)
                    UpdaterThread.Start();


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

            if (DBConnected && ModbusConnected)
            {
                SetSicaklik1 = Convert.ToInt32(DatabaseControl.GetData("tblservertopres", "SetSicaklik1", SetTableID).Split(':')[1].Trim());
                SetSicaklik2 = Convert.ToInt32(DatabaseControl.GetData("tblservertopres", "SetSicaklik2", SetTableID).Split(':')[1].Trim());
                SetSure = Convert.ToInt32(DatabaseControl.GetData("tblservertopres", "SetSure", SetTableID).Split(':')[1].Trim());

                lock (ModbusControl)
                {
                    PlcWriting = true;
                    try
                    {
                        string temp1Err = ModbusControl.WriteHoldRegData(Tempreture1ID, (int)HoldRegAddresses.setSicaklik1, SetSicaklik1);
                        string temp2Err = ModbusControl.WriteHoldRegData(Tempreture2ID, (int)HoldRegAddresses.setSicaklik1, SetSicaklik2);

                        if (TimerFormat == null)
                            return;

                        int timerTime = Utils.Utils.TimeToPlc(SetSure, TimerFormat);
                        if (timerTime == -1)
                            return;

                        string timerErr = ModbusControl.WriteHoldRegData(TimerID, (int)HoldRegAddresses.timerSet, timerTime);

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
                    finally
                    {
                        PlcWriting = false;
                    }
                }

                try
                {
                    timerData = DatabaseControl.GetData("tblservertopres", "SetOrnekAraligi", SetTableID);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Zamanla yazma hatası: " + ex.Message + $"\nSistem {timerInterval / 1000} saniyede bir veri yazıcak");
                    timerData = string.Empty;
                }

                if (timerData != string.Empty)
                    timerInterval = Convert.ToInt32(timerData.Split(':')[1].Trim()) * 1000;

                btnConnectDB.Enabled = false;
                btnConnectModbus.Enabled = false;

                writeDBTimer.Interval = timerInterval;
                writeDBTimer.Enabled = true;

                Started = true;

                writerController.Interval = SetSure * 1000;
                writerController.Enabled = true;

                PressStartTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            }
        }

        private void EndProcess()
        {
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
                if (!(DBConnected && ModbusConnected))
                {
                    MessageBox.Show("Veritabanına veya PLC bağlantısı sağlanılamadı");
                    return;
                }

                if (ModbusConnected && GetSicaklik1 == int.MinValue)
                {
                    MessageBox.Show("Modbus verilerinin cekilmesi bekleniyor");
                    return;
                }
                btnStart.Text = "Durdur";
                btnStart.BackColor = Color.Red;
                btnStart.ForeColor = Color.White;

                Started = true;
                FirstStart();
            }
            else
            {
                btnStart.Text = "Başlat";
                btnStart.BackColor = Color.LimeGreen;
                btnStart.ForeColor = Color.Black;

                Started = false;
                EndProcess();
            }
        }

        private void btnDBSelect_Click(object sender, EventArgs e)
        {
            FileRead();
            textBoxDBPath.Text = DatabasePath;
        }

        private void comboBoxModbusConn_SelectionChangeCommited(object sender, EventArgs e)
        {
            ModbusPort = comboBoxModbusConn.Text;
        }

        private void writerController_Tick(object sender, EventArgs e)
        {
            ProcessController(false);
            MessageBox.Show("İşlem tamamlandı");
        }

        private void settingsTextsChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBoxDBPath.Text) && DatabasePath != textBoxDBPath.Text)
                DatabasePath = textBoxDBPath.Text;

            if (!string.IsNullOrEmpty(textBoxSetTableID.Text) && Convert.ToInt32(textBoxSetTableID.Text) != SetTableID)
                SetTableID = Convert.ToInt32(textBoxSetTableID.Text);

            if (!string.IsNullOrEmpty(textBoxTimerID.Text) && Convert.ToInt32(textBoxTimerID.Text) != TimerID)
                TimerID = Convert.ToInt32(textBoxTimerID.Text);
            if (!string.IsNullOrEmpty(textBoxTemp1ID.Text) && Convert.ToInt32(textBoxTemp1ID.Text) != TimerID)
                Tempreture1ID = Convert.ToInt32(textBoxTemp1ID.Text);
            if (!string.IsNullOrEmpty(textBoxTemp2ID.Text) && Convert.ToInt32(textBoxTemp2ID.Text) != TimerID)
                Tempreture2ID = Convert.ToInt32(textBoxTemp2ID.Text);
        }

        private void comboBoxModbusConn_Click(object sender, EventArgs e)
        {
            PortNames = SerialPort.GetPortNames();

            if (PortNames != OldPortNames)
            {
                OldPortNames = PortNames;
                comboBoxModbusConn.Items.Clear();
                comboBoxModbusConn.Items.AddRange(PortNames);
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.ExitThread();
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (UpdaterThread.IsBackground || UpdaterThread.IsAlive)
                UpdaterThread.Abort();

            Application.Exit();
        }

        private void comboBoxModbusConn_TextUpdate(object sender, EventArgs e)
        {
            ModbusPort = comboBoxModbusConn.Text;
        }
    }
}
