using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SeedQuest.Interactables;

public static class SeedUtility 
{
    // Checks a string to see if it's a valid bip-39 seed phrase
    public static bool validBip(string seed)
    {
        BIP39Converter bpc = new BIP39Converter();
        string hex = "";

        if (InteractableConfig.SitesPerGame < 6)
        {
            try
            {
                hex = bpc.getHexFromShortSentence(seed, InteractableConfig.SitesPerGame * 2);
            }
            catch (Exception e)
            {
                Debug.Log("Exception: " + e);
                return false;
            }
            string[] words = seed.Split(null);
            if (words.Length != InteractableConfig.SitesPerGame * 2)
                return false;
        }
        else
        {
            try
            {
                hex = bpc.getHexFromSentence(seed);
            }
            catch (Exception e)
            {
                Debug.Log("Exception: " + e);
                return false;
            }
        }
        return true;
    }

    // Removes the hex prefix from a string, if it has one
    public static string removeHexPrefix(string hexString)
    {
        if (hexString != null && hexString.StartsWith("0x"))
            hexString = hexString.Substring(2);
        return hexString;
    }

    public static bool validBase64(string input)
    {
        int base64Length = ((5 + InteractableConfig.BitEncodingCount) / 6);

        if (detectBase64(input) && input.Length == base64Length)
            return true;

        return false;
    }

    public static bool detectBase64(string input)
    {
        if (System.Text.RegularExpressions.Regex.IsMatch(input, @"^[a-zA-Z0-9\+/]*={0,2}$"))
            return true;
            
        return false;
    }

    public static bool validAscii(string input)
    {
        int AsciiLength = ((InteractableConfig.BitEncodingCount) / 8);
        byte[] bytes = AsciiConverter.asciiToByte(input);
        if (input.Length == AsciiLength && detectAscii(input))
            return true;

        return false;
    }

    public static bool detectAscii(string input)
    {
        if (System.Text.RegularExpressions.Regex.IsMatch(input, @"^[ -~]"))
            return true;

        return false;
    }

    public static string asciiToHexLengthCheck(string hex)
    {
        if (hex.Length < InteractableConfig.BitEncodingCount / 4)
        {
            for (int i = 0; i < ((InteractableConfig.BitEncodingCount / 4) - hex.Length); i++)
            {
                hex += "0";
            }
        }

        if (hex.Length % 2 == 1)
            hex += "0";
        
        return hex;
    }

    public static string hexToAsciiLengthCheck(string hex)
    {
        if (hex.Length > InteractableConfig.BitEncodingCount / 4)
        {
            Debug.Log("Shortening hex for Ascii conversion");
            hex = hex.Substring(0, (hex.Length - 2));
        }

        return hex;
    }


    public static bool validHex(string input)
    {
        if (InteractableConfig.SeedHexLength % 2 == 1)
        {
            if (input.Length == InteractableConfig.SeedHexLength + 1 && input[input.Length - 2] != '0')
                return false;
        }

        bool valid = true;
        foreach (char c in input)
        {
            valid = (c == '0' || c == '1' || c == '2' || c == '3' || c == '4' ||
                        c == '5' || c == '6' || c == '7' || c == '8' || c == '9' ||
                        c == 'a' || c == 'A' || c == 'b' || c == 'B' || c == 'c' ||
                        c == 'C' || c == 'd' || c == 'D' || c == 'e' || c == 'E' ||
                        c == 'f' || c == 'F') && valid;
        }

        return valid;
    }

    public static bool detectHex(string seed)
    {
        if (seed.Length <= InteractableConfig.SeedHexLength + 1 &&
                 System.Text.RegularExpressions.Regex.IsMatch(seed, @"\A\b[0-9a-fA-F]+\b\Z"))
        {
            return true;
        }
        return false;
    }

}
