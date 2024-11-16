using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Windows.Forms;
using NModbus;
using NModbus.IO;
using NModbus.Serial;

namespace ModbusController
{
    public class ModbusControl
    {
        public SerialPort serialPort;
        private IModbusSerialMaster modbusMaster;

        public bool connectedRTU = false;

        private int readTimeout;
        private int writeTimeout;

        public ModbusControl(int ReadTimeout = 300, int WriteTimeout = 300)
        {
            readTimeout = ReadTimeout;
            writeTimeout = WriteTimeout;

            SetTimeout(writeTimeout, readTimeout);
        }

        public string RTUConnect(string portAddress)
        {
            try
            {
                serialPort = new SerialPort(portAddress, 9600, Parity.None, 8, StopBits.One);
                modbusMaster = new ModbusFactory().CreateRtuMaster(serialPort);

                connectedRTU = true;
                return null;
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }

        public string ConnectPlc()
        {
            if (!SetTimeout(writeTimeout, readTimeout))
                MessageBox.Show("Timeout Ayarlanamadı");

            try
            {
                serialPort.Close();
                serialPort.Open();
            }
            catch (TimeoutException e)
            {
                MessageBox.Show("Timout Error: " + e.Message);
                return e.Message;
            }
            catch (Exception e)
            {
                MessageBox.Show("Error: " + e.Message);
                return e.Message;
            }
            return null;
        }

        public bool DisconnectPlc()
        {
            try
            {
                modbusMaster.Dispose();
                serialPort.Close();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public string TryConnect()
        {
            string serialPortError = "";

            if (!serialPort.IsOpen)
            {
                serialPortError = ConnectPlc();
                if (!string.IsNullOrEmpty(serialPortError))
                    return serialPortError;
            }
            return null;
        }

        public bool SetTimeout(int writeTimeout, int readTimeout)
        {
            try
            {
                serialPort.ReadTimeout = readTimeout;
                serialPort.WriteTimeout = writeTimeout;
            }
            catch
            {
                return false;
            }
            return true;
        }

        public string ReadCoilsData(int SlaveId, int CoilAddress, ushort numCoils = 1)
        {
            byte slaveId = Convert.ToByte(SlaveId);
            ushort coilAddress = Convert.ToUInt16(CoilAddress);
            
            bool veriCekildi = false;
            List<string> gelenVeri = new List<string>();
            bool[] coilis;
            
            string connectionError = TryConnect();

            if (!string.IsNullOrEmpty(connectionError))
            {
                return null;
            }

            try
            {
                gelenVeri.Clear();

                coilis = modbusMaster.ReadCoils(slaveId, coilAddress, numCoils);
                foreach (bool coil in coilis)
                    gelenVeri.Add(coil.ToString());

                veriCekildi = true;
            }
            catch
            {
                return null;
            }
            if (veriCekildi)
                return string.Join(":", gelenVeri);
            return null;
        }

        public string ReadInputRegsData(int SlaveId, int InputAddress, ushort numRegisters = 1)
        {
            byte slaveId = Convert.ToByte(SlaveId);
            ushort inputAddress = Convert.ToUInt16(InputAddress);
            
            List<string> gelenVeri = new List<string>();
            ushort[] inputRegisters;
            
            string connectionError = TryConnect();
            string returnData = null;

            if (!string.IsNullOrEmpty(connectionError))
            {
                return returnData;
            }

            try
            {
                gelenVeri.Clear();

                inputRegisters = modbusMaster.ReadInputRegisters(slaveId, inputAddress, numRegisters);
                foreach (ushort inputRegister in inputRegisters)
                    gelenVeri.Add(inputRegister.ToString());
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Plc'den (inputreg)veri okuma hatası. Plc ID: {SlaveId}\n{ex.Message}");
                return returnData;
            }
            
            return string.Join(":", gelenVeri);
        }

        public string ReadHoldRegsData(int SlaveId, int RegAddress, ushort numRegisters = 1)
        {
            byte slaveId = Convert.ToByte(SlaveId);
            ushort regAddress = Convert.ToUInt16(RegAddress);
            
            List<string> gelenVeri = new List<string>();
            ushort[] holdingRegisters;
            
            string connectionError = TryConnect();
            string returnData = null;

            if (!string.IsNullOrEmpty(connectionError))
            {
                return returnData;
            }

            try
            {
                gelenVeri.Clear();

                holdingRegisters = modbusMaster.ReadHoldingRegisters(slaveId, regAddress, numRegisters);
                foreach (ushort holdingRegister in holdingRegisters)
                    gelenVeri.Add(holdingRegister.ToString());
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Plc'den (holdreg)veri okuma hatası. Plc ID: {SlaveId}\n{ex.Message}");
                return returnData;
            }

            return string.Join(":", gelenVeri);
        }

        public string WriteHoldRegData(int SlaveId, int RegAddress, int WriteValue)
        {
            byte slaveId = Convert.ToByte(SlaveId);
            ushort regAddress = Convert.ToUInt16(RegAddress);
            ushort writeValue = Convert.ToUInt16(WriteValue);

            string connectionError = TryConnect();

            if (!string.IsNullOrEmpty(connectionError))
                return connectionError;

            try
            {
                modbusMaster.WriteSingleRegister(slaveId, regAddress, writeValue);
            }
            catch (TimeoutException e)
            {
                return e.Message;
            }
            catch (Exception e)
            {
                return e.Message;
            }
            return null;
        }

        public string WriteCoilData(int SlaveId, int CoilAddress, bool writeValue)
        {
            byte slaveId = Convert.ToByte(SlaveId);
            ushort coilAddress = Convert.ToUInt16(CoilAddress);
            
            string connectionError = TryConnect();

            if (!string.IsNullOrEmpty(connectionError))
                return connectionError;

            try
            {
                modbusMaster.WriteSingleCoil(slaveId, coilAddress, writeValue);
            }
            catch (TimeoutException e)
            {
                return e.Message;
            }
            catch (Exception e)
            {
                return e.Message;
            }
            return null;
        }
    }
}
