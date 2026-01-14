using System;
using System.Collections.Generic;
using System.Text;
using Roslan.Katapo.Interfaces;

namespace Roslan.Katapo.Devices.Kasa;

public abstract class KasaPowerMeterPlug : KasaDevice, IPowerMeterPlug {



    public KasaPowerMeterPlug(string ipAddress) : base(ipAddress) {
    }



    /// <summary>
    /// 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public async Task SetPower(bool value) {
        var val = value ? 1 : 0;
        await SendCommand("system", "set_relay_state", ("state", val));
    }


    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public async Task Toggle() {
    }
}