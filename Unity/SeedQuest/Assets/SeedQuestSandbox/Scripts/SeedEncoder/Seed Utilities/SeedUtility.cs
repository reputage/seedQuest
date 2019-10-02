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

    public static bool validHex(string input)
    {
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
