using System;
using System.Collections.Generic;
using UnityEngine;
using SeedQuest.SeedEncoder;

public static class Base64Converter 
{
    public static byte[] base64ToByte(string base64)
    {
        return Convert.FromBase64String(base64);
    }

    public static string byteToBase64(byte[] bytes)
    {
        return removeBufferChars(Convert.ToBase64String(bytes));
    }

    public static string hexToBase64(string hex)
    {
        byte[] bytes = SeedToByte.HexStringToByteArray(hex);
        return removeBufferChars(Convert.ToBase64String(bytes));
    }

    public static string base64ToHex(string base64)
    {
        if (base64.Length % 3 == 1)
            base64 += "==";
        else if (base64.Length % 3 == 1)
            base64 += "=";
        byte[] bytes = Convert.FromBase64String(base64);
        return SeedToByte.ByteArrayToHex(bytes);
    }

    public static string removeBufferChars(string base64)
    {
        base64 = base64.Trim('=');
        return base64;
    }
}
