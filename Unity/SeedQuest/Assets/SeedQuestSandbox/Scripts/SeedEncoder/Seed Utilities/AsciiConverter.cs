using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using UnityEngine;
using SeedQuest.SeedEncoder;

public static class AsciiConverter 
{
    public static char[] asciiArray = "0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ!\"#$%&'()*+,-./:;<=>?@[\\]^_`{|}~ ".ToCharArray();

    public static byte[] asciiToByte(string ascii)
    {
        byte[] bytes = new byte[ascii.Length];
        for (int i = 0; i < ascii.Length; i++)
        {
            char j = ascii[i];
            if (asciiArray.Contains(j))
                bytes[i] = Convert.ToByte(Array.IndexOf(asciiArray, j));
            else
                bytes[i] = 94; // 94 is the value used for whitespace in the custom table
        }

        return bytes;
    }

    public static string byteToAscii(byte[] bytes)
    {
        string ascii = "";

        for (int i = 0; i < bytes.Length; i++)
        {
            if (bytes[i] >= 94)
                ascii += " ";
            else
                ascii += asciiArray[bytes[i]].ToString();
        }

        return ascii;
    }

    public static string hexToAscii(string hex)
    {
        byte[] bytes = SeedToByte.HexStringToByteArray(hex);
        return byteToAscii(bytes);
    }

    public static string asciiToHex(string ascii)
    {
        byte[] bytes = asciiToByte(ascii);
        return SeedToByte.ByteArrayToHex(bytes);
    }
}
