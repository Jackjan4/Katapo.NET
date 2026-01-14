using Roslan.Katapo.Devices.Kasa;

namespace Roslan.Katapo.Test;

public class UnitTest1 {



    [Fact]
    public async Task Test1() {


        var device = new HS110("192.168.178.51");

        await device.SetPower(true);
    }
}