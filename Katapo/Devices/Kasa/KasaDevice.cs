using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using Roslan.Katapo.Crypto;

namespace Roslan.Katapo.Devices.Kasa;

public abstract class KasaDevice {

    private const int KasaPort = 9999;

    public string IpAddress { get; }



    public KasaDevice(string ipAddress) {
        IpAddress = ipAddress;
    }


    protected internal async Task SendCommand(string system, string command, params (string Key, object Value)[] parameters) {

        var message = CreateMessage(system, command, parameters);
        var encryptedMsg = KasaEncryption.Encrypt(message);

        var tcpClient = new TcpClient();
        await tcpClient.ConnectAsync(IpAddress, KasaPort).ConfigureAwait(false);

        await using var stream = tcpClient.GetStream();
        await stream.WriteAsync(encryptedMsg);

    }


    /// <summary>
    /// Creates the raw JSON message to be send to the Kasa device.
    /// </summary>
    /// <param name="system"></param>
    /// <param name="command"></param>
    /// <param name="parameters"></param>
    /// <returns></returns>
    private static string CreateMessage(string system, string command, params (string Key, object Value)[] parameters) {

        var parametersStr = parameters
            .Select(x => $"\"{x.Key}\":{x.Value}");

        var par = string.Join(",", parametersStr);

        var result =
            $"{{\"{system}\":{{" +
            $"\"{command}\":{{" +
            par +
            $"}}}}}}";

        return result;
    }

}