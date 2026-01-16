using System;
using System.Collections.Generic;
using System.Text;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Roslan.Katapo.Kasa;

internal static class KasaEncryption {

    private const byte InitializationVector = 171;



    /// <summary>
    /// 
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
    internal static byte[] Encrypt(string data) {
        var encryptedMsg = Encrypt(Encoding.ASCII.GetBytes(data));

        return encryptedMsg;
    }



    /// <summary>
    /// 
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
    internal static byte[] Encrypt(byte[] data) {

        var result = new byte[data.Length];
        var key = InitializationVector; // TP-Link Constant
        for (var i = 0; i < data.Length; i++) {
            result[i] = (byte)(key ^ data[i]);
            key = result[i];
        }

        return result;
    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
    internal static string DecryptStr(byte[] data) {
        var dec = Decrypt(data);
        return Encoding.ASCII.GetString(dec);
    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
    internal static byte[] Decrypt(byte[] data) {
        var buf = (byte[])data.Clone();
        var key = InitializationVector; // TP-Link Constant
        for (var i = 0; i < data.Length; i++) {
            var nextKey = buf[i];
            buf[i] = (byte)(key ^ buf[i]);
            key = nextKey;
        }
        return buf;
    }
}