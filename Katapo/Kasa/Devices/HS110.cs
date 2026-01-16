using Roslan.Katapo.Interfaces;
using Roslan.Katapo.Kasa.Capabilities;

namespace Roslan.Katapo.Kasa.Devices;

public class HS110 : KasaDevice, IPowerMeterPlug {

    private readonly KasaPlugCapability _plug;
    private readonly KasaPowerMeterCapability _powerMeter;



    /// <summary>
    /// 
    /// </summary>
    /// <param name="ipAddress"></param>
    public HS110(string ipAddress) : base(ipAddress) {
        _plug = new KasaPlugCapability(this);
        _powerMeter = new KasaPowerMeterCapability(this);
    }



    // IPlug
    public async Task SetPowerStateAsync(bool turnOn) =>  await _plug.SetPowerStateAsync(turnOn);
    public async Task ToggleAsync() =>  await _plug.ToggleAsync();
}