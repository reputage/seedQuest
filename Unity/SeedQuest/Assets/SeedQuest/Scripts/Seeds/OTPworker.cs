using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System;
using System.Text;
using Nethereum.Signer;

public static class OTPworker
{

    // Decrypts the blob saved at DideryDemoManager.demoBlob
    public static byte[] decryptFromBlob(string seed, string blobString)
    {
        //Debug.Log("Seed: " + seed);
        byte[] seedByte = HexStringToByteArray(seed);
        byte[] demoBlob = Convert.FromBase64String(blobString);
        byte[] decryptedKey = decryptKey(demoBlob, seedByte);
        int valid = VerifyKeys.verifyKey(Encoding.ASCII.GetString(decryptedKey));
        return decryptedKey;
    }

    // Takes an encrypted key, and a seed, returns a byte array of the the otp decrypted seed
    public static byte[] decryptKey(byte[] key, byte[] seed, int size = 32)
    {
        byte[] otp = new byte[32];

        OTPGenerator(otp, size, seed);
        key = OTPxor(key, otp);
        int valid = VerifyKeys.verifyKey(Encoding.ASCII.GetString(key));
        return key;
    }

    // Generates a random seed based on the size of the byte array argument passed in
    public static byte[] randomSeedGenerator(byte[] seed)
    {
        for (int i = 0; i < seed.Length; i++)
            seed[i] = (byte)LibSodiumManager.nacl_randombytes_random();
        
        return seed;
    }

    // Generates the one-time pad from a seed
    public static void OTPGenerator(byte[] otp, int size, byte[] seed)
    {
        LibSodiumManager.nacl_randombytes_buf_deterministic(otp, size, seed);
        //Debug.Log("Seed length: " + seed.Length + " Seed string: " + ByteArrayToHex(seed));
        //Debug.Log("OTP length: " + otp.Length + " OTP first bytes: " + otp[0] + " " + otp[1] + " " + otp[2] + " " + otp[3]);
    }

    // Used to encrypt and decrypt the key using the one-time pad, using the xor method
    public static byte[] OTPxor(byte[] key, byte[] otp)
    {
        byte[] result = new byte[key.Length];
        if (key.Length > otp.Length)
        {
            Debug.Log("Error: One time pad is not longer than key");
            return result;
        }
        for (int i = 0; i < key.Length; ++i)
            result[i] = (byte)(key[i] ^ otp[i]);

        return result;
    }

    // Convert the byte array to hexidecimal
    public static string ByteArrayToHex(byte[] bytes)
    {
        return BitConverter.ToString(bytes).Replace("-", "");
    }

    // Convert hex string to byte array
    public static byte[] HexStringToByteArray(string hex)
    {
        if (hex.Length % 2 == 1)
            throw new Exception("The binary key cannot have an odd number of digits");

        byte[] bytes = new byte[hex.Length >> 1];

        for (int i = 0; i < hex.Length >> 1; ++i)
            bytes[i] = (byte)((GetHexVal(hex[i << 1]) << 4) + (GetHexVal(hex[(i << 1) + 1])));

        return bytes;
    }

    // Get hex value from a char 
    public static int GetHexVal(char hex)
    {
        int val = (int)hex;
        return val - (val < 58 ? 48 : (val < 97 ? 55 : 87));
    }

    //check to see if the seed is valid within what is currently available
    public static int checkValidSeed(int[] actions)
    {

        int[] sites = { 1, 3, 5, 7, 10, 12, 14, 16, 19, 21, 23, 25, 28, 30, 32, 34 };

        for (int i = 0; i < actions.Length; i++)
        {
            if (i == 0 || i == 9 || i == 18 || i == 27)
            {
                if (actions[i] > 7)
                    return i;
            }
            else if (sites.Contains(i))
            {
                if (actions[i] > 15)
                    return i;
            }
        }
        return 0;
    }

}

