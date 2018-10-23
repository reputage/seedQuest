using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Text;

public class OTPworker : MonoBehaviour {
    
    public byte[] otp = new byte[32];
    public byte[] seed = {1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16};
    public byte[] key = new byte[32];
    public byte[] formatKey = new byte[34];
    int size = 32;
    string url = "http://178.128.0.203:8080/blob/"; // change this to the url of the actual didery server
    // Didery server URL: http://178.128.0.203:8080/blob/
    // Local hosted server: http://localhost:8080/blob/


	private void Start()
	{

    }

    // takes an inputKey string, generates a one time pad, encrypts the key,
    // sends the encrypted key to didery, saves the did to a manager 
    // to retrieve from didery later
    public void encryptKey(string inputKey)
    {
        string[] dideryData;

        string did = "";
        string signature = "";
        string postBody = "";

        seed = randomSeedGenerator(seed);
        OTPGenerator(otp, size, seed);
        key = Encoding.ASCII.GetBytes(inputKey);

        formatKey = OTPxor(key, otp);
        dideryData = DideryInterface.makePost(formatKey);

        did = dideryData[0];
        signature = dideryData[1];
        postBody = dideryData[2];

        Debug.Log( "Did: " + did + " signature: " + " postBody: " + postBody );

        SeedManager.InputSeed = ByteArrayToHex(seed);
        DideryDemoManager.demoDid = did;
        Debug.Log("did: " + DideryDemoManager.demoDid);

        StartCoroutine(DideryInterface.PostRequest(url, postBody, signature));
    }

    // Takes the last used did from DideryDemoManager, retrieves the key
    // from a didery server, and returns the encrypted key to DideryDemoManager.demoBlob
    public void getEncryptedKey()
    {
        string uri = url + DideryDemoManager.demoDid;
        Debug.Log(uri);
        StartCoroutine(DideryInterface.GetRequest(uri));
    }

    // Decrypts the blob saved at DideryDemoManager.demoBlob
    public byte[] decryptFromBlob(string seed)
    {
        //Debug.Log("Seed: " + seed);
        byte[] seedByte = HexStringToByteArray(seed);
        Debug.Log(DideryDemoManager.demoBlob);
        byte[] demoBlob = Convert.FromBase64String(DideryDemoManager.demoBlob);
        byte[] decryptedKey = decryptKey(demoBlob, seedByte);
        return decryptedKey;
    }

    // Takes an encrypted key, and a seed, returns a byte array of the the otp decrypted seed
    public byte[] decryptKey(byte[] getKey, byte[] getSeed)
    {
        OTPGenerator(otp, size, getSeed);
        getKey = OTPxor(getKey, otp);
        return getKey;
    }

    // Generates a random 16-byte (or 128-bit) seed
    public byte[] randomSeedGenerator(byte[] seed)
    {
        for (int i = 0; i < seed.Length; i++)
        {
            seed[i] = (byte)LibSodiumManager.nacl_randombytes_random();
        }

        return seed;
    }

    // Generates the one-time pad from a seed
	public void OTPGenerator(byte[] otp, int size, byte[] seed)
    {
        LibSodiumManager.nacl_randombytes_buf_deterministic(otp, size, seed);
    }

    // Used to encrypt and decrypt the key using the one-time pad, using the xor method
    public byte[] OTPxor(byte[] key, byte[] otp)
    {
        byte[] result = new byte[key.Length];
        if (key.Length > otp.Length)
        {
            Debug.Log("Error: One time pad is not longer than key");
            return result;
        }

        for (int i = 0; i < key.Length; ++i)
        {
            result[i] = (byte)(key[i] ^ otp[i]);
        }

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
        {
            bytes[i] = (byte)((GetHexVal(hex[i << 1]) << 4) + (GetHexVal(hex[(i << 1) + 1])));
        }

        return bytes;
    }

    // Get hex value from a char
    public static int GetHexVal(char hex)
    {
        int val = (int)hex;
        return val - (val < 58 ? 48 : (val < 97 ? 55 : 87));
    }

}


/*

******
4040C1A90886218984850151AC123249
******

enter should do the same thing as the "encryptKey" button

show key like this:
***************01234
all asterisks except the last four digits

show the decrypted key in the same box as the seed in the end game ui


--------------------------------

other things that should change eventually but are low priority for right now: 

probably shouldn't use global variables not stored in DideryDemoManager

should change OTPworker to be static public class, not monobehavior

DideryDemoManager should use the same manager code as the other manager scripts 
(the if __instance = NULL type stuff - can just copy and paste from the other 
manager scripts, effects manager is probably a good example[?])

*/