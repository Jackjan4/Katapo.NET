using Roslan.Katapo.Kasa.Devices;

namespace Roslan.Katapo.Kasa.Capabilities;

public class KasaPlugCapability(KasaDevice device) {



    /// <summary>
    /// 
    /// </summary>
    /// <param name="turnOn"></param>
    /// <returns></returns>
    public async Task SetPowerStateAsync(bool turnOn) {
        var val = turnOn ? 1 : 0;
        await device.SendCommand("system", "set_relay_state", ("state", val));
    }



    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public async Task ToggleAsync() {

        // Check relay state with get_sysinfo
        var sysinfoResponse = await device.SendCommand("system", "get_sysinfo");
        var isOn = sysinfoResponse.Contains("\"relay_state\":1");

        await SetPowerStateAsync(!isOn);
    }
}