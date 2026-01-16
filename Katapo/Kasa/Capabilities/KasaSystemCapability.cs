using Roslan.Katapo.Kasa.Devices;
using System;
using System.Collections.Generic;
using System.Text;
using Roslan.Katapo.Kasa.Models;

namespace Roslan.Katapo.Kasa.Capabilities;

public class KasaSystemCapability(KasaDevice device) {


    public async Task<KasaSysInfo> GetSysInfo() {

        var response = await device.SendCommand("system", "get_sysinfo");

        var result = new KasaSysInfo();

        return result;
    }

}