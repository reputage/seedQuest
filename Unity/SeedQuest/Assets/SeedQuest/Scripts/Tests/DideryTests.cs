using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DideryTests : MonoBehaviour 
{

    public void runAllTests()
    {
        //testMakePost();
        //testMakeDid();
        testDecrypt();
        //testDecryptFromBlob();

    }

    // Test making a didery blob post for a post request
    public void testMakePost()
    {
        byte[] key = new byte[16];
        byte[] seed = new byte[16];

        for (int i = 0; i < key.Length; i++)
        {
            key[i] = (byte)i;
        }
        for (int i = 0; i < seed.Length; i++)
        {
            seed[i] = (byte)i;
        }

        string[] post = DideryInterface.makePost(key, seed);
        Debug.Log("Test post: " + post[0] + " " + post[1] + " " + post[2]);

    }

    // Test making a did for use with post requests to didery
    public void testMakeDid()
    {
        byte[] key = new byte[16];
        for (int i = 0; i < key.Length; i++)
        {
            key[i] = (byte)i;
        }

        string did = DideryInterface.makeDid(key);
        Debug.Log("Test did: " + did);
        //:AAECAwQFBgcICQoLDA0ODw==
    }

    // Test decrypting a key from a didery blob
    public void testDecrypt()
    {
        byte[] key = new byte[16];
        byte[] seed = new byte[16];
        byte[] encryptedKey = new byte[16];
        byte[] decryptedKey = new byte[16];
        byte[] otp = new byte[32];

        for (int i = 0; i < key.Length; i++)
        {
            key[i] = (byte)(i+1);
        }
        for (int i = 0; i < seed.Length; i++)
        {
            seed[i] = (byte)(i+1);
        }

        OTPworker.OTPGenerator(otp, 32, seed);
        encryptedKey = OTPworker.OTPxor(key, otp);
        decryptedKey = OTPworker.OTPxor(encryptedKey, otp);

        string keyString = OTPworker.ByteArrayToHex(key);
        string decryptedString = OTPworker.ByteArrayToHex(decryptedKey);

        Debug.Log("Key: " + OTPworker.ByteArrayToHex(key));
        Debug.Log("Decrypted key: " + OTPworker.ByteArrayToHex(decryptedKey));

        if (keyString == decryptedString)
        {
            Debug.Log("Decryption test successful");
        }

    }

    // Test decrypting a key from a didery blob - needs the bug fix for OTPgenerator to work
    public void testDecryptFromBlob()
    {
        byte[] key = new byte[16];
        byte[] seed = new byte[16];
        byte[] encryptedKey = new byte[16];
        byte[] decryptedKey = new byte[16];
        byte[] decryptedBlob = new byte[16];
        byte[] otp = new byte[32];

        for (int i = 0; i < key.Length; i++)
        {
            key[i] = (byte)(i + 1);
        }
        for (int i = 0; i < seed.Length; i++)
        {
            seed[i] = (byte)(i + 1);
        }

        OTPworker.OTPGenerator(otp, 32, seed);
        encryptedKey = OTPworker.OTPxor(key, otp);
        decryptedKey = OTPworker.OTPxor(encryptedKey, otp);

        string seedString = OTPworker.ByteArrayToHex(seed);
        string keyString = OTPworker.ByteArrayToHex(key);
        string decryptedString = OTPworker.ByteArrayToHex(decryptedKey);

        Debug.Log("Key: " + OTPworker.ByteArrayToHex(key));
        Debug.Log("Decrypted key: " + OTPworker.ByteArrayToHex(decryptedKey));

        //decryptedBlob = OTPworker.decryptFromBlob(seedString, Convert.ToBase64String(encryptedKey));

        Debug.Log("Decrypted key: " + OTPworker.ByteArrayToHex(decryptedBlob));

    }

    // Test that an invalid key fails the key validation function
    public void testBadDecrypt()
    {
        string seed = "A021E0A80264A33C08B6C2884AC0685C";
        string badBlob = "aaaabbbbaaaabbbbaaaabbbbaaaabbbb";
        byte[] keyByte = OTPworker.decryptFromBlob(seed, badBlob);
        string finalKey = Encoding.ASCII.GetString(keyByte);
        Debug.Log("Bad decrypted key: " + finalKey);
    }

    // Test the function to generate public address from private key
    public void testRegenerateAddress()
    {
        string privateKey = "0xb5b1870957d373ef0eeffecc6e4812c0fd08f554b37b233526acc331bf1544f7";
        string address = VerifyKeys.regeneratePublicAddress(privateKey);
        Debug.Log("Your public address is: " + address);
    }

    // Test that a valid key passes the key validation function
    public void testValidKey()
    {
        string key = "0xb5b1870957d373ef0eeffecc6e4812c0fd08f554b37b233526acc331bf1544f7";
        VerifyKeys.verifyKey(key);
    }

    // Test that a valid key passes the key validation function
    public void testGoodDecrypt()
    {
        string seed = "A021E0A80264A33C08B6C2884AC0685C";
        string key = "0xb5b1870957d373ef0eeffecc6e4812c0fd08f554b37b233526acc331bf1544f7";
        key = VerifyKeys.removeHexPrefix(key);
        byte[] otp = new byte[32];
        byte[] seedByte = new byte[14];
        byte[] encryptedKey = new byte[34];
        byte[] keyByte = Encoding.ASCII.GetBytes(key);
        seedByte = Encoding.ASCII.GetBytes(seed);
        byte[] goodKey = OTPworker.OTPxor(seedByte, keyByte);
        byte[] decryptedKey = OTPworker.decryptFromBlob(seed, Convert.ToBase64String(goodKey));
        string finalKey = Encoding.ASCII.GetString(keyByte);
        Debug.Log("Decrypted key: " + finalKey);
    }

}
