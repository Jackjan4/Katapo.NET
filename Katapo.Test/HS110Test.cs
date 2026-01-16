using Roslan.Katapo.Kasa.Devices;

namespace Roslan.Katapo.Test;

public class HS110Test {



    [Fact]
    public async Task TestSetPower() {


        var device = new HS110("192.168.178.51");

        await device.SetPowerStateAsync(true);
    }

    [Fact]
    public async Task TestToggle()
    {
        var device = new HS110("192.168.178.51");

        await device.ToggleAsync();
    }
}