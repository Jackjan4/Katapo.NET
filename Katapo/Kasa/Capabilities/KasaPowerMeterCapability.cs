using System;
using System.Collections.Generic;
using System.Text;
using Roslan.Katapo.Interfaces;
using Roslan.Katapo.Kasa.Devices;

namespace Roslan.Katapo.Kasa.Capabilities;

public class KasaPowerMeterCapability(KasaDevice device) {



    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public async Task<double> GetCurrentPowerAsync() {

        var response = await device.SendCommand("emeter", "get_realtime");

        var json = System.Text.Json.JsonDocument.Parse(response);
        var currentPower = json.RootElement.GetProperty("emeter").GetProperty("get_realtime").GetProperty("power_mw").GetDouble();

        return currentPower / 1000.0; // The value is in milliwatts, convert to watts
    }



    /// <summary>
    /// 
    /// </summary>
    /// <param name="month"></param>
    /// <param name="year"></param>
    /// <returns></returns>
    public async Task<string> GetDailyPowerAsync(int month, int year) {
        var response = await device.SendCommand("emeter", "get_daystat", ("month", month), ("year", year));

        var json = System.Text.Json.JsonDocument.Parse(response);
        var dailyPower = json.RootElement.GetProperty("emeter").GetProperty("get_daystat").GetProperty("energy_wh").GetDouble();

        return dailyPower.ToString();
    }



    /// <summary>
    /// 
    /// </summary>
    /// <param name="year"></param>
    /// <returns></returns>
    public async Task<string> GetMonthlyPowerAsync(int year) {
        var response = await device.SendCommand("emeter", "get_monthstat", ("year", year));

        var json = System.Text.Json.JsonDocument.Parse(response);
        var monthlyPower = json.RootElement.GetProperty("emeter").GetProperty("get_monthstat").GetProperty("energy_wh").GetDouble();

        return monthlyPower.ToString();
    }

}