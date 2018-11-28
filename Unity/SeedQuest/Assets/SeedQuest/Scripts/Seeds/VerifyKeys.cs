using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Nethereum.Signer;

public static class VerifyKeys {

    public static void verifyKey(string key)
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
            return;
        }

        try
        {
            address = signer.HashAndEcRecover(dummyMessage, signature);
        }
        catch (Exception recover)
        {
            Debug.Log("Key appears to be invalid! Cannot recover data. " + recover);
            return;
        }

        Debug.Log("Varification successful: " + address + ". Private key appears to be valid.");
    }

    public static string addHexPrefix(string hexString)
    {
        string prefixed = "";
        if (hexString != null && !hexString.StartsWith("0x"))
            prefixed = "0x" + hexString;
        else
            return hexString;
        return prefixed;
    }

    public static string removeHexPrefix(string hexString)
    {
        if (hexString != null && hexString.StartsWith("0x"))
            hexString = hexString.Substring(2);
        return hexString;
    }

}
