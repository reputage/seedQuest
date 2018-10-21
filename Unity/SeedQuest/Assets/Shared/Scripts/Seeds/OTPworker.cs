using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;

public class OTPworker : MonoBehaviour {
    
    public byte[] otp = new byte[32];
    public byte[] seed = {1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16};
    public byte[] key = new byte[32];
    int size = 32;
    string url = "http://localhost:8080/blob/"; // change this to the url of the actual didery server



	private void Start()
	{
        // Used for testing purposes, not needed right now, can be deleted
        //OTPGenerator(otp, size, seed);
        //key = OTPxor(seed, otp);
        //key = OTPxor(key, otp);

    }

    // takes an inputKey string, generates a one time pad, encrypts the key,
    // sends the encrypted key to didery, saves the did to a manager 
    // to retrieve from didery later
    public void encryptKey(string inputkey)
    {
        string[] dideryData;

        string did = "";
        string signature = "";
        string postBody = "";

        seed = randomSeedGenerator(seed);
        OTPGenerator(otp, size, seed);
        key = Encoding.ASCII.GetBytes(inputkey);
        key = OTPxor(key, otp);
        dideryData = DideryInterface.makePost(key);

        did = dideryData[0];
        signature = dideryData[1];
        postBody = dideryData[2];

        DideryDemoManager.demoDid = did;

        StartCoroutine(DideryInterface.PostRequest(url, postBody, signature));
    }

    // Takes the last used did from DideryDemoManager, retrieves the key
    // from a didery server, and returns the encrypted key to DideryDemoManager.demoBlob
    public void getEncryptedKey()
    {
        string uri = url + DideryDemoManager.demoDid;
        StartCoroutine(DideryInterface.GetRequest(uri));
    }

    // Decrypts the blob saved at DideryDemoManager.demoBlob
    public byte[] decryptFromBlob(byte[] seed)
    {
        byte[] demoBlob = Encoding.ASCII.GetBytes(DideryDemoManager.demoBlob);
        byte[] decryptedKey = decryptKey(demoBlob, seed);
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

    // Used to encrypt and decrypt the key using the one-time pad
    public byte[] OTPxor(byte[] key, byte[] otp)
    {
        byte[] result = new byte[key.Length];

        if (key.Length < otp.Length)
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

}
