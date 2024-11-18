namespace accdb_isi
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.hedefSicaklik1 = new System.Windows.Forms.Label();
            this.aktifSicaklik1 = new System.Windows.Forms.Label();
            this.hedefSicaklik2 = new System.Windows.Forms.Label();
            this.aktifSicaklik2 = new System.Windows.Forms.Label();
            this.operatorLabel = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.writeDBTimer = new System.Windows.Forms.Timer(this.components);
            this.btnConnectDB = new System.Windows.Forms.Button();
            this.btnConnectModbus = new System.Windows.Forms.Button();
            this.makineLabel = new System.Windows.Forms.Label();
            this.btnClearLogs = new System.Windows.Forms.Button();
            this.generalTimer = new System.Windows.Forms.Timer(this.components);
            this.btnStart = new System.Windows.Forms.Button();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabIslemler = new System.Windows.Forms.TabPage();
            this.groupBox = new System.Windows.Forms.GroupBox();
            this.labelTimerValue = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.tabAyarlar = new System.Windows.Forms.TabPage();
            this.textBoxSetTableID = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.textBoxTemp2ID = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.textBoxTemp1ID = new System.Windows.Forms.TextBox();
            this.textBoxTimerID = new System.Windows.Forms.TextBox();
            this.labelTempertureID = new System.Windows.Forms.Label();
            this.labelTimerID = new System.Windows.Forms.Label();
            this.comboBoxModbusConn = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.textBoxDBPath = new System.Windows.Forms.TextBox();
            this.btnDBSelect = new System.Windows.Forms.Button();
            this.labelDatabase = new System.Windows.Forms.Label();
            this.writerController = new System.Windows.Forms.Timer(this.components);
            this.tabControl1.SuspendLayout();
            this.tabIslemler.SuspendLayout();
            this.groupBox.SuspendLayout();
            this.tabAyarlar.SuspendLayout();
            this.SuspendLayout();
            // 
            // hedefSicaklik1
            // 
            this.hedefSicaklik1.AutoSize = true;
            this.hedefSicaklik1.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.hedefSicaklik1.Location = new System.Drawing.Point(64, 138);
            this.hedefSicaklik1.Name = "hedefSicaklik1";
            this.hedefSicaklik1.Size = new System.Drawing.Size(115, 18);
            this.hedefSicaklik1.TabIndex = 0;
            this.hedefSicaklik1.Text = "Hedef Sıcaklık: -";
            // 
            // aktifSicaklik1
            // 
            this.aktifSicaklik1.AutoSize = true;
            this.aktifSicaklik1.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.aktifSicaklik1.Location = new System.Drawing.Point(64, 162);
            this.aktifSicaklik1.Name = "aktifSicaklik1";
            this.aktifSicaklik1.Size = new System.Drawing.Size(104, 18);
            this.aktifSicaklik1.TabIndex = 1;
            this.aktifSicaklik1.Text = "Aktif Sıcaklık: -";
            // 
            // hedefSicaklik2
            // 
            this.hedefSicaklik2.AutoSize = true;
            this.hedefSicaklik2.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.hedefSicaklik2.Location = new System.Drawing.Point(568, 138);
            this.hedefSicaklik2.Name = "hedefSicaklik2";
            this.hedefSicaklik2.Size = new System.Drawing.Size(115, 18);
            this.hedefSicaklik2.TabIndex = 2;
            this.hedefSicaklik2.Text = "Hedef Sıcaklık: -";
            // 
            // aktifSicaklik2
            // 
            this.aktifSicaklik2.AutoSize = true;
            this.aktifSicaklik2.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.aktifSicaklik2.Location = new System.Drawing.Point(568, 162);
            this.aktifSicaklik2.Name = "aktifSicaklik2";
            this.aktifSicaklik2.Size = new System.Drawing.Size(104, 18);
            this.aktifSicaklik2.TabIndex = 3;
            this.aktifSicaklik2.Text = "Aktif Sıcaklık: -";
            // 
            // operatorLabel
            // 
            this.operatorLabel.AutoSize = true;
            this.operatorLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.operatorLabel.Location = new System.Drawing.Point(22, 17);
            this.operatorLabel.Name = "operatorLabel";
            this.operatorLabel.Size = new System.Drawing.Size(76, 20);
            this.operatorLabel.TabIndex = 4;
            this.operatorLabel.Text = "Operatör:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.label1.Location = new System.Drawing.Point(64, 95);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(146, 16);
            this.label1.TabIndex = 5;
            this.label1.Text = "Press 1 Sıcaklık Değeri";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.label2.Location = new System.Drawing.Point(568, 95);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(146, 16);
            this.label2.TabIndex = 6;
            this.label2.Text = "Press 2 Sıcaklık Değeri";
            // 
            // writeDBTimer
            // 
            this.writeDBTimer.Tick += new System.EventHandler(this.writeDBTimer_Tick);
            // 
            // btnConnectDB
            // 
            this.btnConnectDB.BackColor = System.Drawing.Color.LimeGreen;
            this.btnConnectDB.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnConnectDB.ForeColor = System.Drawing.Color.White;
            this.btnConnectDB.Location = new System.Drawing.Point(0, 0);
            this.btnConnectDB.Name = "btnConnectDB";
            this.btnConnectDB.Size = new System.Drawing.Size(94, 47);
            this.btnConnectDB.TabIndex = 7;
            this.btnConnectDB.Text = "Veritabanına Bağlan";
            this.btnConnectDB.UseVisualStyleBackColor = false;
            this.btnConnectDB.Click += new System.EventHandler(this.btnConnectDB_Click);
            // 
            // btnConnectModbus
            // 
            this.btnConnectModbus.BackColor = System.Drawing.Color.LimeGreen;
            this.btnConnectModbus.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnConnectModbus.ForeColor = System.Drawing.Color.White;
            this.btnConnectModbus.Location = new System.Drawing.Point(89, 0);
            this.btnConnectModbus.Name = "btnConnectModbus";
            this.btnConnectModbus.Size = new System.Drawing.Size(94, 47);
            this.btnConnectModbus.TabIndex = 8;
            this.btnConnectModbus.Text = "Modbus Cihazına Bağlan";
            this.btnConnectModbus.UseVisualStyleBackColor = false;
            this.btnConnectModbus.Click += new System.EventHandler(this.btnConnectModbus_Click);
            // 
            // makineLabel
            // 
            this.makineLabel.AutoSize = true;
            this.makineLabel.Font = new System.Drawing.Font("Microsoft New Tai Lue", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.makineLabel.Location = new System.Drawing.Point(23, 49);
            this.makineLabel.Name = "makineLabel";
            this.makineLabel.Size = new System.Drawing.Size(53, 17);
            this.makineLabel.TabIndex = 11;
            this.makineLabel.Text = "Makine:";
            // 
            // btnClearLogs
            // 
            this.btnClearLogs.BackColor = System.Drawing.Color.Red;
            this.btnClearLogs.Enabled = false;
            this.btnClearLogs.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClearLogs.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.btnClearLogs.ForeColor = System.Drawing.Color.White;
            this.btnClearLogs.Location = new System.Drawing.Point(654, 352);
            this.btnClearLogs.Name = "btnClearLogs";
            this.btnClearLogs.Size = new System.Drawing.Size(113, 47);
            this.btnClearLogs.TabIndex = 12;
            this.btnClearLogs.Text = "Logları Temizle";
            this.btnClearLogs.UseVisualStyleBackColor = false;
            this.btnClearLogs.Click += new System.EventHandler(this.btnClearLogs_Click);
            // 
            // generalTimer
            // 
            this.generalTimer.Enabled = true;
            this.generalTimer.Interval = 3000;
            this.generalTimer.Tick += new System.EventHandler(this.generalTimer_Tick);
            // 
            // btnStart
            // 
            this.btnStart.BackColor = System.Drawing.Color.LimeGreen;
            this.btnStart.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnStart.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.btnStart.ForeColor = System.Drawing.Color.White;
            this.btnStart.Location = new System.Drawing.Point(368, 352);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(86, 47);
            this.btnStart.TabIndex = 13;
            this.btnStart.Text = "Başlat";
            this.btnStart.UseVisualStyleBackColor = false;
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabIslemler);
            this.tabControl1.Controls.Add(this.tabAyarlar);
            this.tabControl1.Location = new System.Drawing.Point(3, 2);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(795, 446);
            this.tabControl1.TabIndex = 14;
            // 
            // tabIslemler
            // 
            this.tabIslemler.Controls.Add(this.groupBox);
            this.tabIslemler.Controls.Add(this.labelTimerValue);
            this.tabIslemler.Controls.Add(this.label7);
            this.tabIslemler.Controls.Add(this.btnClearLogs);
            this.tabIslemler.Controls.Add(this.makineLabel);
            this.tabIslemler.Controls.Add(this.btnStart);
            this.tabIslemler.Controls.Add(this.operatorLabel);
            this.tabIslemler.Controls.Add(this.label1);
            this.tabIslemler.Controls.Add(this.label2);
            this.tabIslemler.Controls.Add(this.aktifSicaklik1);
            this.tabIslemler.Controls.Add(this.hedefSicaklik1);
            this.tabIslemler.Controls.Add(this.aktifSicaklik2);
            this.tabIslemler.Controls.Add(this.hedefSicaklik2);
            this.tabIslemler.Location = new System.Drawing.Point(4, 22);
            this.tabIslemler.Name = "tabIslemler";
            this.tabIslemler.Padding = new System.Windows.Forms.Padding(3);
            this.tabIslemler.Size = new System.Drawing.Size(787, 420);
            this.tabIslemler.TabIndex = 0;
            this.tabIslemler.Text = "İşlemler";
            this.tabIslemler.UseVisualStyleBackColor = true;
            // 
            // groupBox
            // 
            this.groupBox.Controls.Add(this.btnConnectModbus);
            this.groupBox.Controls.Add(this.btnConnectDB);
            this.groupBox.Location = new System.Drawing.Point(27, 353);
            this.groupBox.Name = "groupBox";
            this.groupBox.Size = new System.Drawing.Size(183, 46);
            this.groupBox.TabIndex = 18;
            this.groupBox.TabStop = false;
            // 
            // labelTimerValue
            // 
            this.labelTimerValue.AutoSize = true;
            this.labelTimerValue.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.labelTimerValue.Location = new System.Drawing.Point(362, 138);
            this.labelTimerValue.Name = "labelTimerValue";
            this.labelTimerValue.Size = new System.Drawing.Size(57, 31);
            this.labelTimerValue.TabIndex = 17;
            this.labelTimerValue.Text = "-:-:-";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.label7.Location = new System.Drawing.Point(347, 95);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(90, 16);
            this.label7.TabIndex = 16;
            this.label7.Text = "Timer Zamanı";
            // 
            // tabAyarlar
            // 
            this.tabAyarlar.Controls.Add(this.textBoxSetTableID);
            this.tabAyarlar.Controls.Add(this.label5);
            this.tabAyarlar.Controls.Add(this.textBoxTemp2ID);
            this.tabAyarlar.Controls.Add(this.label4);
            this.tabAyarlar.Controls.Add(this.textBoxTemp1ID);
            this.tabAyarlar.Controls.Add(this.textBoxTimerID);
            this.tabAyarlar.Controls.Add(this.labelTempertureID);
            this.tabAyarlar.Controls.Add(this.labelTimerID);
            this.tabAyarlar.Controls.Add(this.comboBoxModbusConn);
            this.tabAyarlar.Controls.Add(this.label3);
            this.tabAyarlar.Controls.Add(this.textBoxDBPath);
            this.tabAyarlar.Controls.Add(this.btnDBSelect);
            this.tabAyarlar.Controls.Add(this.labelDatabase);
            this.tabAyarlar.Location = new System.Drawing.Point(4, 22);
            this.tabAyarlar.Name = "tabAyarlar";
            this.tabAyarlar.Padding = new System.Windows.Forms.Padding(3);
            this.tabAyarlar.Size = new System.Drawing.Size(787, 420);
            this.tabAyarlar.TabIndex = 1;
            this.tabAyarlar.Text = "Ayarlar";
            this.tabAyarlar.UseVisualStyleBackColor = true;
            // 
            // textBoxSetTableID
            // 
            this.textBoxSetTableID.Location = new System.Drawing.Point(122, 159);
            this.textBoxSetTableID.Name = "textBoxSetTableID";
            this.textBoxSetTableID.Size = new System.Drawing.Size(100, 20);
            this.textBoxSetTableID.TabIndex = 3;
            this.textBoxSetTableID.TextChanged += new System.EventHandler(this.settingsTextsChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(35, 162);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(77, 13);
            this.label5.TabIndex = 11;
            this.label5.Text = "Set tablosu ID:";
            // 
            // textBoxTemp2ID
            // 
            this.textBoxTemp2ID.Location = new System.Drawing.Point(137, 339);
            this.textBoxTemp2ID.Name = "textBoxTemp2ID";
            this.textBoxTemp2ID.Size = new System.Drawing.Size(100, 20);
            this.textBoxTemp2ID.TabIndex = 7;
            this.textBoxTemp2ID.TextChanged += new System.EventHandler(this.settingsTextsChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(35, 342);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(100, 13);
            this.label4.TabIndex = 9;
            this.label4.Text = "Sıcaklık cihazı 2 ID:";
            // 
            // textBoxTemp1ID
            // 
            this.textBoxTemp1ID.Location = new System.Drawing.Point(137, 305);
            this.textBoxTemp1ID.Name = "textBoxTemp1ID";
            this.textBoxTemp1ID.Size = new System.Drawing.Size(100, 20);
            this.textBoxTemp1ID.TabIndex = 6;
            this.textBoxTemp1ID.TextChanged += new System.EventHandler(this.settingsTextsChanged);
            // 
            // textBoxTimerID
            // 
            this.textBoxTimerID.Location = new System.Drawing.Point(137, 270);
            this.textBoxTimerID.Name = "textBoxTimerID";
            this.textBoxTimerID.Size = new System.Drawing.Size(100, 20);
            this.textBoxTimerID.TabIndex = 5;
            this.textBoxTimerID.TextChanged += new System.EventHandler(this.settingsTextsChanged);
            // 
            // labelTempertureID
            // 
            this.labelTempertureID.AutoSize = true;
            this.labelTempertureID.Location = new System.Drawing.Point(35, 308);
            this.labelTempertureID.Name = "labelTempertureID";
            this.labelTempertureID.Size = new System.Drawing.Size(100, 13);
            this.labelTempertureID.TabIndex = 6;
            this.labelTempertureID.Text = "Sıcaklık cihazı 1 ID:";
            // 
            // labelTimerID
            // 
            this.labelTimerID.AutoSize = true;
            this.labelTimerID.Location = new System.Drawing.Point(35, 273);
            this.labelTimerID.Name = "labelTimerID";
            this.labelTimerID.Size = new System.Drawing.Size(87, 13);
            this.labelTimerID.TabIndex = 5;
            this.labelTimerID.Text = "Zaman cihazı ID:";
            // 
            // comboBoxModbusConn
            // 
            this.comboBoxModbusConn.FormattingEnabled = true;
            this.comboBoxModbusConn.Location = new System.Drawing.Point(137, 229);
            this.comboBoxModbusConn.Name = "comboBoxModbusConn";
            this.comboBoxModbusConn.Size = new System.Drawing.Size(100, 21);
            this.comboBoxModbusConn.TabIndex = 4;
            this.comboBoxModbusConn.SelectionChangeCommitted += new System.EventHandler(this.comboBoxModbusConn_SelectionChangeCommited);
            this.comboBoxModbusConn.TextUpdate += new System.EventHandler(this.settingsTextsChanged);
            this.comboBoxModbusConn.Click += new System.EventHandler(this.comboBoxModbusConn_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(35, 232);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(96, 13);
            this.label3.TabIndex = 3;
            this.label3.Text = "Modbus Bağlantısı:";
            // 
            // textBoxDBPath
            // 
            this.textBoxDBPath.Location = new System.Drawing.Point(122, 133);
            this.textBoxDBPath.Name = "textBoxDBPath";
            this.textBoxDBPath.Size = new System.Drawing.Size(319, 20);
            this.textBoxDBPath.TabIndex = 2;
            this.textBoxDBPath.TextChanged += new System.EventHandler(this.settingsTextsChanged);
            // 
            // btnDBSelect
            // 
            this.btnDBSelect.Location = new System.Drawing.Point(440, 133);
            this.btnDBSelect.Name = "btnDBSelect";
            this.btnDBSelect.Size = new System.Drawing.Size(99, 20);
            this.btnDBSelect.TabIndex = 1;
            this.btnDBSelect.Text = "Veritabanı Seç";
            this.btnDBSelect.UseVisualStyleBackColor = true;
            this.btnDBSelect.Click += new System.EventHandler(this.btnDBSelect_Click);
            // 
            // labelDatabase
            // 
            this.labelDatabase.AutoSize = true;
            this.labelDatabase.Location = new System.Drawing.Point(35, 136);
            this.labelDatabase.Name = "labelDatabase";
            this.labelDatabase.Size = new System.Drawing.Size(81, 13);
            this.labelDatabase.TabIndex = 0;
            this.labelDatabase.Text = "Veritabanı Yolu:";
            // 
            // writerController
            // 
            this.writerController.Tick += new System.EventHandler(this.writerController_Tick);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.tabControl1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.Text = "Press Kontrol Programı";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Form1_FormClosed);
            this.tabControl1.ResumeLayout(false);
            this.tabIslemler.ResumeLayout(false);
            this.tabIslemler.PerformLayout();
            this.groupBox.ResumeLayout(false);
            this.tabAyarlar.ResumeLayout(false);
            this.tabAyarlar.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label hedefSicaklik1;
        private System.Windows.Forms.Label aktifSicaklik1;
        private System.Windows.Forms.Label hedefSicaklik2;
        private System.Windows.Forms.Label aktifSicaklik2;
        private System.Windows.Forms.Label operatorLabel;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Timer writeDBTimer;
        private System.Windows.Forms.Button btnConnectDB;
        private System.Windows.Forms.Button btnConnectModbus;
        private System.Windows.Forms.Label makineLabel;
        private System.Windows.Forms.Button btnClearLogs;
        private System.Windows.Forms.Timer generalTimer;
        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabIslemler;
        private System.Windows.Forms.TabPage tabAyarlar;
        private System.Windows.Forms.Button btnDBSelect;
        private System.Windows.Forms.Label labelDatabase;
        private System.Windows.Forms.ComboBox comboBoxModbusConn;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox textBoxDBPath;
        private System.Windows.Forms.Label labelTempertureID;
        private System.Windows.Forms.Label labelTimerID;
        private System.Windows.Forms.TextBox textBoxTemp1ID;
        private System.Windows.Forms.TextBox textBoxTimerID;
        private System.Windows.Forms.TextBox textBoxTemp2ID;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Timer writerController;
        private System.Windows.Forms.TextBox textBoxSetTableID;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label labelTimerValue;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.GroupBox groupBox;
    }
}

