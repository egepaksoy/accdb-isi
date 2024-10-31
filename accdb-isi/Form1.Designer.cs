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
            this.labelDBConnected = new System.Windows.Forms.Label();
            this.labelModbusConnected = new System.Windows.Forms.Label();
            this.makineLabel = new System.Windows.Forms.Label();
            this.btnClearLogs = new System.Windows.Forms.Button();
            this.generalTimer = new System.Windows.Forms.Timer(this.components);
            this.btnStart = new System.Windows.Forms.Button();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabIslemler = new System.Windows.Forms.TabPage();
            this.tabAyarlar = new System.Windows.Forms.TabPage();
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
            this.tabControl1.SuspendLayout();
            this.tabIslemler.SuspendLayout();
            this.tabAyarlar.SuspendLayout();
            this.SuspendLayout();
            // 
            // hedefSicaklik1
            // 
            this.hedefSicaklik1.AutoSize = true;
            this.hedefSicaklik1.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.hedefSicaklik1.Location = new System.Drawing.Point(197, 138);
            this.hedefSicaklik1.Name = "hedefSicaklik1";
            this.hedefSicaklik1.Size = new System.Drawing.Size(106, 18);
            this.hedefSicaklik1.TabIndex = 0;
            this.hedefSicaklik1.Text = "Hedef Sıcaklık:";
            // 
            // aktifSicaklik1
            // 
            this.aktifSicaklik1.AutoSize = true;
            this.aktifSicaklik1.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.aktifSicaklik1.Location = new System.Drawing.Point(197, 162);
            this.aktifSicaklik1.Name = "aktifSicaklik1";
            this.aktifSicaklik1.Size = new System.Drawing.Size(91, 18);
            this.aktifSicaklik1.TabIndex = 1;
            this.aktifSicaklik1.Text = "Aktif Sıcaklık";
            // 
            // hedefSicaklik2
            // 
            this.hedefSicaklik2.AutoSize = true;
            this.hedefSicaklik2.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.hedefSicaklik2.Location = new System.Drawing.Point(470, 138);
            this.hedefSicaklik2.Name = "hedefSicaklik2";
            this.hedefSicaklik2.Size = new System.Drawing.Size(106, 18);
            this.hedefSicaklik2.TabIndex = 2;
            this.hedefSicaklik2.Text = "Hedef Sıcaklık:";
            // 
            // aktifSicaklik2
            // 
            this.aktifSicaklik2.AutoSize = true;
            this.aktifSicaklik2.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.aktifSicaklik2.Location = new System.Drawing.Point(470, 162);
            this.aktifSicaklik2.Name = "aktifSicaklik2";
            this.aktifSicaklik2.Size = new System.Drawing.Size(95, 18);
            this.aktifSicaklik2.TabIndex = 3;
            this.aktifSicaklik2.Text = "Aktif Sıcaklık:";
            // 
            // operatorLabel
            // 
            this.operatorLabel.AutoSize = true;
            this.operatorLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.operatorLabel.Location = new System.Drawing.Point(23, 18);
            this.operatorLabel.Name = "operatorLabel";
            this.operatorLabel.Size = new System.Drawing.Size(76, 20);
            this.operatorLabel.TabIndex = 4;
            this.operatorLabel.Text = "Operatör:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.label1.Location = new System.Drawing.Point(197, 95);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(146, 16);
            this.label1.TabIndex = 5;
            this.label1.Text = "Press Sıcaklık Değeri 1";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.label2.Location = new System.Drawing.Point(470, 95);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(146, 16);
            this.label2.TabIndex = 6;
            this.label2.Text = "Press Sıcaklık Değeri 2";
            // 
            // writeDBTimer
            // 
            this.writeDBTimer.Enabled = true;
            this.writeDBTimer.Tick += new System.EventHandler(this.writeDBTimer_Tick);
            // 
            // btnConnectDB
            // 
            this.btnConnectDB.Location = new System.Drawing.Point(26, 367);
            this.btnConnectDB.Name = "btnConnectDB";
            this.btnConnectDB.Size = new System.Drawing.Size(94, 47);
            this.btnConnectDB.TabIndex = 7;
            this.btnConnectDB.Text = "Veritabanına Bağlan";
            this.btnConnectDB.UseVisualStyleBackColor = true;
            this.btnConnectDB.Click += new System.EventHandler(this.btnConnectDB_Click);
            // 
            // btnConnectModbus
            // 
            this.btnConnectModbus.Location = new System.Drawing.Point(182, 367);
            this.btnConnectModbus.Name = "btnConnectModbus";
            this.btnConnectModbus.Size = new System.Drawing.Size(94, 47);
            this.btnConnectModbus.TabIndex = 8;
            this.btnConnectModbus.Text = "Modbus Cihazına Bağlan";
            this.btnConnectModbus.UseVisualStyleBackColor = true;
            this.btnConnectModbus.Click += new System.EventHandler(this.btnConnectModbus_Click);
            // 
            // labelDBConnected
            // 
            this.labelDBConnected.AutoSize = true;
            this.labelDBConnected.ForeColor = System.Drawing.Color.Red;
            this.labelDBConnected.Location = new System.Drawing.Point(42, 351);
            this.labelDBConnected.Name = "labelDBConnected";
            this.labelDBConnected.Size = new System.Drawing.Size(57, 13);
            this.labelDBConnected.TabIndex = 9;
            this.labelDBConnected.Text = "Bağlı Değil";
            // 
            // labelModbusConnected
            // 
            this.labelModbusConnected.AutoSize = true;
            this.labelModbusConnected.ForeColor = System.Drawing.Color.Red;
            this.labelModbusConnected.Location = new System.Drawing.Point(197, 351);
            this.labelModbusConnected.Name = "labelModbusConnected";
            this.labelModbusConnected.Size = new System.Drawing.Size(57, 13);
            this.labelModbusConnected.TabIndex = 10;
            this.labelModbusConnected.Text = "Bağlı Değil";
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
            this.btnClearLogs.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.btnClearLogs.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.btnClearLogs.Location = new System.Drawing.Point(668, 367);
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
            this.generalTimer.Interval = 800;
            this.generalTimer.Tick += new System.EventHandler(this.generalTimer_Tick);
            // 
            // btnStart
            // 
            this.btnStart.BackColor = System.Drawing.Color.Lime;
            this.btnStart.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.btnStart.ForeColor = System.Drawing.Color.Black;
            this.btnStart.Location = new System.Drawing.Point(490, 367);
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
            this.tabIslemler.Controls.Add(this.btnClearLogs);
            this.tabIslemler.Controls.Add(this.makineLabel);
            this.tabIslemler.Controls.Add(this.btnStart);
            this.tabIslemler.Controls.Add(this.operatorLabel);
            this.tabIslemler.Controls.Add(this.label1);
            this.tabIslemler.Controls.Add(this.label2);
            this.tabIslemler.Controls.Add(this.aktifSicaklik1);
            this.tabIslemler.Controls.Add(this.labelModbusConnected);
            this.tabIslemler.Controls.Add(this.hedefSicaklik1);
            this.tabIslemler.Controls.Add(this.btnConnectModbus);
            this.tabIslemler.Controls.Add(this.aktifSicaklik2);
            this.tabIslemler.Controls.Add(this.labelDBConnected);
            this.tabIslemler.Controls.Add(this.hedefSicaklik2);
            this.tabIslemler.Controls.Add(this.btnConnectDB);
            this.tabIslemler.Location = new System.Drawing.Point(4, 22);
            this.tabIslemler.Name = "tabIslemler";
            this.tabIslemler.Padding = new System.Windows.Forms.Padding(3);
            this.tabIslemler.Size = new System.Drawing.Size(787, 420);
            this.tabIslemler.TabIndex = 0;
            this.tabIslemler.Text = "İşlemler";
            this.tabIslemler.UseVisualStyleBackColor = true;
            // 
            // tabAyarlar
            // 
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
            // textBoxTemp2ID
            // 
            this.textBoxTemp2ID.Location = new System.Drawing.Point(137, 339);
            this.textBoxTemp2ID.Name = "textBoxTemp2ID";
            this.textBoxTemp2ID.Size = new System.Drawing.Size(100, 20);
            this.textBoxTemp2ID.TabIndex = 10;
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
            this.textBoxTemp1ID.TabIndex = 8;
            // 
            // textBoxTimerID
            // 
            this.textBoxTimerID.Location = new System.Drawing.Point(137, 270);
            this.textBoxTimerID.Name = "textBoxTimerID";
            this.textBoxTimerID.Size = new System.Drawing.Size(100, 20);
            this.textBoxTimerID.TabIndex = 7;
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
            this.textBoxDBPath.Size = new System.Drawing.Size(422, 20);
            this.textBoxDBPath.TabIndex = 2;
            // 
            // btnDBSelect
            // 
            this.btnDBSelect.Location = new System.Drawing.Point(38, 159);
            this.btnDBSelect.Name = "btnDBSelect";
            this.btnDBSelect.Size = new System.Drawing.Size(99, 23);
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
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.tabControl1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.tabControl1.ResumeLayout(false);
            this.tabIslemler.ResumeLayout(false);
            this.tabIslemler.PerformLayout();
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
        private System.Windows.Forms.Label labelDBConnected;
        private System.Windows.Forms.Label labelModbusConnected;
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
    }
}

