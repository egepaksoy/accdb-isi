using System;
using System.Windows.Forms;

namespace Utils
{
    public static class Utils
    {
        public static string PlcToTime(string TimerTime, string timerFormat)
        {
            double hour = 0;
            double minute = 0;
            double second = 0;

            int timerTime = Convert.ToInt32(TimerTime);

            if (string.IsNullOrEmpty(TimerTime))
                return null;

            switch (timerFormat)
            {
                case "0":
                    second = timerTime / 100;
                    break;
                case "1":
                    second = timerTime / 10;
                    break;
                case "2":
                    second = timerTime;
                    break;
                case "3":
                    second = timerTime % 100;
                    minute = Convert.ToInt32(timerTime / 100);
                    break;
                case "4":
                    minute = timerTime / 10;
                    break;
                case "5":
                    minute = timerTime;
                    break;
                case "6":
                    minute = timerTime % 100;
                    hour = Convert.ToInt32(timerTime / 100);
                    break;
                case "7":
                    hour = timerTime / 10;
                    break;
                case "8":
                    hour = timerTime;
                    break;
                default:
                    MessageBox.Show($"Plc formatı {timerFormat} bulunamadı");
                    return null;
            }

            if (minute >= 60 || second >= 60 || hour >= 24)
            {
                MessageBox.Show($"Timer zamanında sıkıntı çıktı: {hour}:{minute}:{second}");
                return null;
            }

            return $"{hour}:{minute}:{second}";
        }

        public static int TimeToPlc(int setSure, string format)
        {
            switch (format)
            {
                case "0":
                    if (setSure < 100)
                        return setSure / 100;
                    break;
                case "1":
                    if (setSure < 1000)
                        return setSure / 10;
                    break;
                case "2":
                    if (setSure < 10000)
                        return setSure;
                    break;
                case "3":
                    if (setSure < 600)
                        return Convert.ToInt32(setSure / 60) * 100 + setSure % 60;
                    break;
                case "4":
                    if (setSure < 60000)
                        return (setSure / 60) / 10;
                    break;
                case "5":
                    if (setSure < 600000)
                        return Convert.ToInt32(setSure / 60);
                    break;
                case "6":
                    if (setSure < 360000)
                        return Convert.ToInt32(setSure / 3600) * 100 + setSure % 3600;
                    break;
                case "7":
                    if (setSure < 3600000)
                        return Convert.ToInt32(setSure / 3600) / 10;
                    break;
                case "8":
                    if (setSure < 36000000)
                        return Convert.ToInt32(setSure / 3600);
                    break;
            }
            MessageBox.Show($"Veritabanındaki süre formatı plc'deki ile uyumsuz\nPlc Formatı: {format} Veritabanı süre: {setSure}\nPlc formatını değiştirin.");
            return -1;
        }

        public static string FileRead()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Access Database Files (*.accdb)|*.accdb";
            openFileDialog.Multiselect = false;
            openFileDialog.Title = "Access Veritabanı Dosyaları Seç";

            if (openFileDialog.ShowDialog() == DialogResult.OK)
                return openFileDialog.FileName;

            return null;
        }
    }
}
