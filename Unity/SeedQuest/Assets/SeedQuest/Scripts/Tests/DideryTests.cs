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

        sumTest(ref passed, testMakePost());
        sumTest(ref passed, testMakeDid());
        sumTest(ref passed, testDecrypt());
        sumTest(ref passed, testValidKey());
        sumTest(ref passed, testRegenerateAddress());
        sumTest(ref passed, testHexToByte());
        sumTest(ref passed, testDecryptFromBlob());

        Debug.Log("Successfully passed " + passed[0] + " out of " + passed[1] + " didery tests.");
    }

    public void sumTest(ref int[] passed, int[] testPassed)
    {
        if (passed.Length < 2 || testPassed.Length < 2)
            Debug.Log("Error summing test results: int[] shorter than two elements");
        
        passed[0] += testPassed[0];
        passed[1] += testPassed[1];
    }

    // Test making a didery blob post for a post request
    public int[] testMakePost()
    {
        int[] passed = new int[2];
        passed[1] = 1;
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

        // If this has worked without getting an error then it's passed the test
        passed[0] = 1;
        return passed;
    }

    // Test making a did for use with post requests to didery
    public int[] testMakeDid()
    {
        int[] passed = new int[2];
        passed[1] = 1;

        byte[] key = new byte[16];
        for (int i = 0; i < key.Length; i++)
        {
            key[i] = (byte)i;
        }

        string did = DideryInterface.makeDid(key);

        if (did == "did:dad:AAECAwQFBgcICQoLDA0ODw==")
            passed[0] = 1;
        else
            Debug.Log("testMakeDid() failed.");

        return passed;
    }

    // Test decrypting a key from a didery blob
    public int[] testDecrypt()
    {
        int[] passed = new int[2];
        passed[1] = 1;

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

        if (keyString == decryptedString)
        {
            passed[0] = 1;
        }
        else
            Debug.Log("testDecrypt() failed.");

        return passed;
    }

    // Test decrypting a key from a didery blob - needs the bug fix for OTPgenerator to work
    public int[] testDecryptFromBlob()
    {
        int[] passed = new int[2];
        passed[1] = 1;

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

        if (keyString == decryptedString)
        {
            passed[0] += 1;
        }
        else
        {
            Debug.Log("Decrypt key test failed. Key: " + OTPworker.ByteArrayToHex(key));
            Debug.Log("Decrypted key: " + OTPworker.ByteArrayToHex(decryptedKey));

        }

        decryptedBlob = OTPworker.decryptFromBlob(seedString, Convert.ToBase64String(encryptedKey));
        passed[1] += 1;

        if (keyString == OTPworker.ByteArrayToHex(decryptedBlob))
        {
            passed[0] += 1;
        }
        else
        {
            Debug.Log("Decrypt blob test failed. Key string: " + keyString + " decrypted blob: " + OTPworker.ByteArrayToHex(decryptedBlob));
        }

        //Debug.Log("Decrypted blob: " + OTPworker.ByteArrayToHex(decryptedBlob));

        return passed;
    }

    // Test that an invalid key fails the key validation function
    public int[] testBadDecrypt()
    {
        int[] passed = new int[2];
        passed[1] = 1;

        string seed = "A021E0A80264A33C08B6C2884AC0685C";
        string badBlob = "aaaabbbbaaaabbbbaaaabbbbaaaabbbb";
        byte[] keyByte = OTPworker.decryptFromBlob(seed, badBlob);
        string finalKey = Encoding.ASCII.GetString(keyByte);
        //Debug.Log("Bad decrypted key: " + finalKey);

        // if the function has gotten to this point, and hasn't crashed, it's passed
        passed[0] = 1;
        return passed;
    }

    // Test the function to generate public address from private key
    public int[] testRegenerateAddress()
    {
        int[] passed = new int[2];
        passed[1] = 1;

        string privateKey = "0xb5b1870957d373ef0eeffecc6e4812c0fd08f554b37b233526acc331bf1544f7";
        string addressCheck = "0x12890D2cce102216644c59daE5baed380d84830c";
        string address = VerifyKeys.regeneratePublicAddress(privateKey);
        //Debug.Log("Your public address is: " + address);

        if (address == addressCheck)
        {
            passed[0] = 1;
        }
        else
            Debug.Log("testRegenerateAddress() failed.");

        return passed;
    }

    // Test that a valid key passes the key validation function
    public int[] testValidKey()
    {
        int[] passed = new int[2];
        passed[1] = 1;

        string key = "0xb5b1870957d373ef0eeffecc6e4812c0fd08f554b37b233526acc331bf1544f7";
        passed[0] = VerifyKeys.verifyKey(key);

        if (passed[0] == 1)
        {
            Debug.Log("testValidKey() failed");
            passed[0] = 0;
        }
        else
            passed[0] = 1;
        
        return passed;
    }

    // Test that a valid key passes the key validation function - needs changes to 
    //  OTPgenerator to work properly
    public int[] testGoodDecrypt()
    {
        int[] passed = new int[2];
        passed[1] = 1;

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

        if(finalKey == key)
        {
            passed[0] = 1;
        }

        return passed;
    }

    public int[] testHexToByte()
    {
        int[] passed = new int[2];
        passed[1] = 1;

        string even = "abdc";
        string odd = "abc";
        byte[] evenArray = OTPworker.HexStringToByteArray(even);
        byte[] oddArray = OTPworker.HexStringToByteArray(odd);

        // If it's gotten this far, it works
        passed[0] = 1;
        return passed;
    }

}
