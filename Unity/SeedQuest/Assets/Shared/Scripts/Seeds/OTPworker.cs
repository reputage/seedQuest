using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System;
using System.Text;

public class OTPworker : MonoBehaviour
{

    public DideryDemoManager dideryDemoManager;

    public SeedToByte seedToByte;
    public byte[] otp = new byte[32];
    public byte[] seed = new byte[16];
    public byte[] key = new byte[32];
    public byte[] formatKey = new byte[34];
    int size = 32;
    string url = "http://178.128.0.203:8080/blob/"; // change this to the url of the actual didery server
                                                    // Didery server URL: http://178.128.0.203:8080/blob/
                                                    // Local hosted server: http://localhost:8080/blob/

	private void Start()
	{
        seedToByte = new SeedToByte();
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
        //seed = HexStringToByteArray("4040C1A90886218984850151AC123249");

        // Check seed to see if it is within the demo parameters
        // Should remove this eventually
        // changes the seed's bytes instead of generating a new one, since it's faster this way
        //seed = checkValidSeed(seedToByte.getActionsFromBytes(seed));

        int checkVal = checkValidSeed(seedToByte.getActionsFromBytes(seed));
        while (checkVal > 1)
        {
            Debug.Log("Generating new seed");
            for (int i = 0; i < seed.Length; i++)
            {
                if (seed[i] > 0)
                {
                    seed[i] -= 1;
                }
            }
            checkVal = checkValidSeed(seedToByte.getActionsFromBytes(seed));
        }

        Debug.Log(ByteArrayToHex(seed));

        OTPGenerator(otp, size, seed);
        key = Encoding.ASCII.GetBytes(inputKey);

        formatKey = OTPxor(key, otp);
        dideryData = DideryInterface.makePost(formatKey);

        did = dideryData[0];
        signature = dideryData[1];
        postBody = dideryData[2];

        //Debug.Log("Did: " + did + " signature: " + " postBody: " + postBody);

        SeedManager.InputSeed = ByteArrayToHex(seed);
        DideryDemoManager.DemoDid = did;
        Debug.Log("Did: " + DideryDemoManager.DemoDid);

        dideryDemoManager.postRequest(url, postBody, signature);
        //StartCoroutine(DideryInterface.PostRequest(url, postBody, signature));
    }

    // Takes the last used did from DideryDemoManager, retrieves the key
    // from a didery server, and returns the encrypted key to DideryDemoManager.demoBlob
    public void getEncryptedKey()
    {
        string uri = url + DideryDemoManager.DemoDid;
        Debug.Log(uri);

        dideryDemoManager.getRequest(uri);
        //StartCoroutine(DideryInterface.GetRequest(uri));
    }

    // Decrypts the blob saved at DideryDemoManager.demoBlob
    public byte[] decryptFromBlob(string seed)
    {
        //Debug.Log("Seed: " + seed);
        byte[] seedByte = HexStringToByteArray(seed);
        byte[] demoBlob = Convert.FromBase64String(DideryDemoManager.DemoBlob);
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
            seed[i] = (byte)LibSodiumManager.nacl_randombytes_random();
        
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
    public int checkValidSeed(int[] actions)
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

