using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DideryTests : MonoBehaviour 
{

    public void runAllTests()
    {
        int[] passed = new int[2];

        passed[0] += testMakePost();
        passed[1]++;
        passed[0] += testMakeDid();
        passed[1]++;
        passed[0] += testDecrypt();
        passed[1]++;
        passed[0] += testValidKey();
        passed[1]++;
        passed[0] += testRegenerateAddress();
        passed[1]++;
        passed[0] += testHexToByte();
        passed[1]++;
        //passed[0] += testBadDecrypt();
        //passed[1]++;

        Debug.Log("Successfully passed " + passed[0] + " out of " + passed[1] + " didery tests.");

    }

    // Test making a didery blob post for a post request
    public int testMakePost()
    {
        int pass = 1;
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
        //Debug.Log("Test post: " + post[0] + " " + post[1] + " " + post[2]);

        // If this has worked without getting an error then it's passed the test
        return pass;
    }

    // Test making a did for use with post requests to didery
    public int testMakeDid()
    {
        int pass = 0;

        byte[] key = new byte[16];
        for (int i = 0; i < key.Length; i++)
        {
            key[i] = (byte)i;
        }

        string did = DideryInterface.makeDid(key);
        //Debug.Log("Test did: " + did);

        if (did == "did:dad:AAECAwQFBgcICQoLDA0ODw==")
            pass = 1;
        else
            Debug.Log("testMakeDid() failed.");

        return pass;
    }

    // Test decrypting a key from a didery blob
    public int testDecrypt()
    {
        int pass = 0; 

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

        //Debug.Log("Key: " + OTPworker.ByteArrayToHex(key));
        //Debug.Log("Decrypted key: " + OTPworker.ByteArrayToHex(decryptedKey));

        if (keyString == decryptedString)
        {
            pass = 1;
            //Debug.Log("Decryption test successful");
        }
        else
            Debug.Log("testDecrypt() failed.");

        return pass;
    }

    // Test decrypting a key from a didery blob - needs the bug fix for OTPgenerator to work
    public int testDecryptFromBlob()
    {
        int pass = 0; 

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

        //Debug.Log("Key: " + OTPworker.ByteArrayToHex(key));
        //Debug.Log("Decrypted key: " + OTPworker.ByteArrayToHex(decryptedKey));

        //decryptedBlob = OTPworker.decryptFromBlob(seedString, Convert.ToBase64String(encryptedKey));

        //Debug.Log("Decrypted key: " + OTPworker.ByteArrayToHex(decryptedBlob));

        return pass;
    }

    // Test that an invalid key fails the key validation function
    public int testBadDecrypt()
    {
        int pass = 0; 

        string seed = "A021E0A80264A33C08B6C2884AC0685C";
        string badBlob = "aaaabbbbaaaabbbbaaaabbbbaaaabbbb";
        byte[] keyByte = OTPworker.decryptFromBlob(seed, badBlob);
        string finalKey = Encoding.ASCII.GetString(keyByte);
        //Debug.Log("Bad decrypted key: " + finalKey);

        // if the function has gotten to this point, and hasn't crashed, it's passed
        pass = 1;
        return pass;
    }

    // Test the function to generate public address from private key
    public int testRegenerateAddress()
    {
        int pass = 0;

        string privateKey = "0xb5b1870957d373ef0eeffecc6e4812c0fd08f554b37b233526acc331bf1544f7";
        string addressCheck = "0x12890D2cce102216644c59daE5baed380d84830c";
        string address = VerifyKeys.regeneratePublicAddress(privateKey);
        //Debug.Log("Your public address is: " + address);

        if (address == addressCheck)
        {
            pass = 1;
        }
        else
            Debug.Log("testRegenerateAddress() failed.");

        return pass;
    }

    // Test that a valid key passes the key validation function
    public int testValidKey()
    {
        string key = "0xb5b1870957d373ef0eeffecc6e4812c0fd08f554b37b233526acc331bf1544f7";
        int pass = VerifyKeys.verifyKey(key);

        if (pass == 1)
        {
            Debug.Log("testValidKey() failed");
            pass = 0;
        }
        else
            pass = 1;
        
        return pass;
    }

    // Test that a valid key passes the key validation function - needs changes to 
    //  OTPgenerator to work properly
    public int testGoodDecrypt()
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
        //Debug.Log("Decrypted key: " + finalKey);

        return 0;
    }

    public int testHexToByte()
    {
        string even = "abdc";
        string odd = "abc";
        byte[] evenArray = OTPworker.HexStringToByteArray(even);
        byte[] oddArray = OTPworker.HexStringToByteArray(odd);

        // If it's gotten this far, it works
        return 1;
    }

}
