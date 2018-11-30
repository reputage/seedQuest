using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Nethereum.Signer;
using Nethereum;

public static class VerifyKeys {


    // Checks to see if the key is valid by signing dummy data. Returns 0 if valid, 1 otherwise.
    public static int verifyKey(string key)
    {
        key = addHexPrefix(key);
        var dummyMessage = "test dummy message";
        var signer = new MessageSigner();
        var signature = "";
        var address = "";
        Debug.Log("Verify key: " + key);

        try
        {
            signature = signer.HashAndSign(dummyMessage, key);
        }
        catch (Exception sign)
        {
            Debug.Log("Key appears to be invalid! Cannot sign data. " + sign);
            return 1;
        }

        try
        {
            address = signer.HashAndEcRecover(dummyMessage, signature);
        }
        catch (Exception recover)
        {
            Debug.Log("Key appears to be invalid! Cannot recover data. " + recover);
            return 1;
        }

        Debug.Log("Varification successful: " + address + ". Private key appears to be valid.");
        return 0;
    }

    // Regenerate public address from private key, so the user can check to see if their
    // key was recovered correctly.
    public static string regeneratePublicAddress(string privateKey)
    {
        privateKey = addHexPrefix(privateKey);
        string account = Nethereum.Signer.EthECKey.GetPublicAddress(privateKey);
        return account;
    }

    // Adds a hex prefix to a string, if it is missing one
    public static string addHexPrefix(string hexString)
    {
        string prefixed = "";
        if (hexString != null && !hexString.StartsWith("0x"))
            prefixed = "0x" + hexString;
        else
            return hexString;
        return prefixed;
    }

    // Removes the hex prefix from a string, if it has one
    public static string removeHexPrefix(string hexString)
    {
        if (hexString != null && hexString.StartsWith("0x"))
            hexString = hexString.Substring(2);
        return hexString;
    }

}
