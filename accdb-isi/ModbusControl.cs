using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Net;
using System.Net.Sockets;
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

        private byte slaveId;
        private ushort startAddress;

        private int readTimeout;
        private int writeTimeout;

        public ModbusControl(string address, byte SlaveId, int ReadTimeout = 300, int WriteTimeout = 300)
        {
            RTUConnect(address);

            slaveId = SlaveId;

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

                return null;
            }
            catch (Exception e)
            {
                MessageBox.Show("Modbus RTU Bağlantı hatası: " + e.Message);
                return e.Message;
            }
        }

        public string ConnectPlc()
        {
            SetTimeout(writeTimeout, readTimeout);

            try
            {
                serialPort.Open();
                if (serialPort.IsOpen)
                    return null;
            }
            catch (TimeoutException e)
            {
                return e.Message;
            }
            catch (Exception e)
            {
                MessageBox.Show($"Bağlantı hatası {e.Message}");
                return e.Message;
            }
            return null;
        }

        public bool DisconnectPlc()
        {
            try
            {
                if (!serialPort.IsOpen)
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
            catch (Exception e)
            {
                MessageBox.Show($"Timeout ayarlama hatası: {e.Message}");
                return false;
            }
            return true;
        }

        public string ReadCoilsData(int CoilAddress, ushort numCoils = 1)
        {
            ushort coilAddress = Convert.ToUInt16(CoilAddress);
            bool veriCekildi = false;
            List<string> gelenVeri = new List<string>();
            bool[] coilis;
            string connectionError = TryConnect();

            if (!string.IsNullOrEmpty(connectionError))
            {
                MessageBox.Show("PLC'ye bağlanılamadı: " + connectionError);
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
            catch (TimeoutException ex)
            {
                MessageBox.Show(ex.Message);
                return null;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return null;
            }
            if (veriCekildi)
                return string.Join(":", gelenVeri);
            return null;
        }

        public string ReadHoldRegsData(int RegAddress, ushort numRegisters = 1)
        {
            ushort regAddress = Convert.ToUInt16(RegAddress);
            bool veriCekildi = false;
            List<string> gelenVeri = new List<string>();
            ushort[] holdingRegisters;
            string connectionError = TryConnect();
            string returnData = null;

            if (!string.IsNullOrEmpty(connectionError))
            {
                MessageBox.Show("PLC'ye bağlanılamadı: " + connectionError);
                return returnData;
            }

            try
            {
                gelenVeri.Clear();

                holdingRegisters = modbusMaster.ReadHoldingRegisters(slaveId, regAddress, numRegisters);
                foreach (ushort holdingRegister in holdingRegisters)
                    gelenVeri.Add(holdingRegister.ToString());

                veriCekildi = true;
            }
            catch (TimeoutException ex)
            {
                //MessageBox.Show(ex.Message);
                return returnData;
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message);
                return returnData;
            }
            finally
            {
                if (veriCekildi)
                    returnData = string.Join(":", gelenVeri);
            }
            return returnData;
        }

        public string WriteHoldRegData(int RegAddress, int WriteValue)
        {
            ushort regAddress = Convert.ToUInt16(RegAddress);
            ushort writeValue = Convert.ToUInt16(WriteValue);
            string connectionError = TryConnect();

            if (!string.IsNullOrEmpty(connectionError))
                return connectionError;

            try
            {
                modbusMaster.WriteMultipleRegisters(slaveId, regAddress, new ushort[] { writeValue });
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

        public string WriteCoilData(int CoilAddress, bool writeValue)
        {
            ushort coilAddress = Convert.ToUInt16(CoilAddress);
            string connectionError = TryConnect();

            if (!string.IsNullOrEmpty(connectionError))
                return connectionError;

            try
            {
                modbusMaster.WriteMultipleCoils(slaveId, coilAddress, new bool[] { writeValue });
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
