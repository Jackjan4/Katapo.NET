using System;
using System.Collections.Generic;
using System.Text;
using Roslan.Katapo.Kasa.Models;

namespace Roslan.Katapo.Kasa.Devices;

public interface IKasaDevice {


    public Task<KasaSysInfo> GetSysInfo();

}