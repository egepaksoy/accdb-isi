using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Addresses
{
    internal enum HoldRegAddresses
    {
        // sicaklik cihazi degerleri
        setSicaklik1 = 0,
        setSicaklik2 = 1,
        
        // zamanlayici cihazi degerleri
        timerSet = 0,
        timerFormat = 2,
    }
    internal enum InputRegAddresses
    {
        // sicaklik cihazi degerleri
        olculenSicaklik = 0,

        // zamanlayici cihazi degerleri
        timerValue = 0,
    }
}
