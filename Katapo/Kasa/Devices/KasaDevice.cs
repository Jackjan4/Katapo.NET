using System.Net.Sockets;
using Roslan.Katapo.Kasa.Capabilities;
using Roslan.Katapo.Kasa.Models;

namespace Roslan.Katapo.Kasa.Devices;

public abstract class KasaDevice : IKasaDevice {

    // Constants
    private const int KasaPort = 9999;

    // Fields
    private readonly KasaSystemCapability _systemCapability;

    // Properties
    public string IpAddress { get; }



    /// <summary>
    /// 
    /// </summary>
    /// <param name="ipAddress"></param>
    protected KasaDevice(string ipAddress) {
        IpAddress = ipAddress;
        _systemCapability = new KasaSystemCapability(this);
    }



    // IKasaDevice
    public Task<KasaSysInfo> GetSysInfo() => _systemCapability.GetSysInfo();



    /// <summary>
    /// Sends a command to the Kasa device and reads the response.
    /// </summary>
    /// <param name="system"></param>
    /// <param name="command"></param>
    /// <param name="parameters"></param>
    /// <returns></returns>
    internal async Task<string> SendCommand(string system, string command, params (string Key, object Value)[] parameters) {

        // Create JSON message
        var message = CreateKasaMessage(system, command, parameters);

        // Encrypt the message into Kasa XOR encryption format
        var encryptedMsg = KasaEncryption.Encrypt(message);

        // Send the packet and read the response
        var responseBytes = await SendKasaTcp(encryptedMsg);

        // Decrypt the response message
        var decryptedResponse = KasaEncryption.DecryptStr(responseBytes);

        return decryptedResponse;
    }


    /// <summary>
    /// Sends the message bytes to the Kasa device via TCP and reads the response.
    /// Constructs a Kasa TCP packet (with 4-byte prefix) to send the 
    /// </summary>
    /// <param name="messageData">The encrypted Kasa message that should be send.</param>
    /// <returns></returns>
    internal async Task<byte[]> SendKasaTcp(byte[] messageData) {

        var lengthBytesSend = BitConverter.GetBytes((uint)messageData.Length);
        if (BitConverter.IsLittleEndian) Array.Reverse(lengthBytesSend, 0, lengthBytesSend.Length); // this value needs to be in big-endian
        var packet = lengthBytesSend.Concat(messageData).ToArray();

        var tcpClient = new TcpClient();
        await tcpClient.ConnectAsync(IpAddress, KasaPort).ConfigureAwait(false);

        await using var stream = tcpClient.GetStream();
        await stream.WriteAsync(packet);

        // === Read response ===
        var buffer = new byte[4096];

        // Read header to determine length
        var memory = buffer.AsMemory(0, 4);
        var lengthBytesRecv = await stream.ReadAsync(memory).ConfigureAwait(false);
        if (lengthBytesRecv != 4) throw new Exception("Failed to read Kasa TCP response length prefix.");
        if (BitConverter.IsLittleEndian) Array.Reverse(buffer, 0, 4); // Convert to little-endian if necessary
        var responseLength = BitConverter.ToUInt32(buffer, 0);

        var bytesReceived = 0;
        // Read buffer in 1024 bytes chunks, if available
        memory = buffer.AsMemory(4);

        while (bytesReceived < responseLength) {
            var bytesRead = await stream.ReadAsync(memory).ConfigureAwait(false);
            bytesReceived += bytesRead;
            memory = buffer.AsMemory(bytesReceived + 4); // Move memory window
        }

        // Return only the response data, excluding the 4-byte length prefix
        var result = buffer[4..];

        return result;
    }



    /// <summary>
    /// Creates the raw JSON message to be send to the Kasa device.
    /// </summary>
    /// <param name="system"></param>
    /// <param name="command"></param>
    /// <param name="parameters"></param>
    /// <returns></returns>
    private static string CreateKasaMessage(string system, string command, params (string Key, object Value)[] parameters) {

        var parametersStr = parameters.Select(x => $"\"{x.Key}\":{x.Value}");
        var par = string.Join(",", parametersStr);

        var result =
            $"{{\"{system}\":{{" +
            $"\"{command}\":{{" +
            par +
            $"}}}}}}";

        return result;
    }


}