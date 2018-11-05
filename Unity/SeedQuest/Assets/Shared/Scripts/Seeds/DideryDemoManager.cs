using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class DideryDemoManager : MonoBehaviour {

    // Note: coroutines can only be called from a MonoBehavior class, and 
    //  can't be in a static function.

    // string url = "http://178.128.0.203:8080/blob/"; 
    // Didery server URL: http://178.128.0.203:8080/blob/
    // Local hosted server: http://localhost:8080/blob/

    static private DideryDemoManager instance = null;

    static public DideryDemoManager Instance
    {
        get
        {
            if (instance == null)
                instance = GameObject.FindObjectOfType<DideryDemoManager>();
            return instance;
        }
    }

    public string demoDid;
    public string demoBlob;
    public bool isDemo;
    private string url = "http://178.128.0.203:8080/blob/";

    public SeedToByte seedToByte;

    static public string DemoDid
    {
        get { return Instance.demoDid;  }
        set { Instance.demoDid = value; }
    }

    static public string DemoBlob
    {
        get { return Instance.demoBlob;  }
        set { Instance.demoBlob = value; }
    }

    static public bool IsDemo
    {
        get { return Instance.isDemo;  }
        set { Instance.isDemo = value; }
    }

    // Send POST request to didery
    public void postRequest(string url, string postBody, string signature)
    {
        StartCoroutine(DideryInterface.PostRequest(url, postBody, signature));
    }

    // Send GET request to didery
    public void getRequest(string uri)
    {
        StartCoroutine(DideryInterface.GetRequest(uri));
    }

    // Takes a key as input, encrypts the key, sends it to didery in a POST request
    public void demoEncryptKey(string inputKey)
    {
        int size = 32;
        string[] dideryData;

        byte[] otp = new byte[32];
        byte[] seed = new byte[16];
        byte[] encryptedKey = new byte[34];
        byte[] key = Encoding.ASCII.GetBytes(inputKey);

        seed = OTPworker.randomSeedGenerator(seed);
        seed = checkSeed(seed);
        //seed = HexStringToByteArray("4040C1A90886218984850151AC123249");

        OTPworker.OTPGenerator(otp, size, seed);

        encryptedKey = OTPworker.OTPxor(key, otp);

        dideryData = DideryInterface.makePost(encryptedKey);

        string did = dideryData[0];
        string signature = dideryData[1];
        string postBody = dideryData[2];

        //Debug.Log("Did: " + did + " signature: " + " postBody: " + postBody);
        SeedManager.InputSeed = OTPworker.ByteArrayToHex(seed);
        DideryDemoManager.DemoDid = did;
        Debug.Log("Did: " + DideryDemoManager.DemoDid);

        postRequest(url, postBody, signature);
        //StartCoroutine(DideryInterface.PostRequest(url, postBody, signature));
    }

    // Takes the last used did from DideryDemoManager, retrieves the key
    // from a didery server, and returns the encrypted key to DideryDemoManager.demoBlob
    public void demoGetEncryptedKey()
    {
        string uri = url + DideryDemoManager.DemoDid;
        //Debug.Log(uri);

        getRequest(uri);
        //StartCoroutine(DideryInterface.GetRequest(uri));
    }

    public byte[] encryptKey(string inputKey)
    {
        byte[] otp = new byte[32];
        byte[] seed = new byte[16];
        byte[] key = new byte[32];
        byte[] encryptedKey = new byte[34];

        int size = 32;

        seed = OTPworker.randomSeedGenerator(seed);
        seed = checkSeed(seed);
        //seed = HexStringToByteArray("4040C1A90886218984850151AC123249");

        OTPworker.OTPGenerator(otp, size, seed);
        key = Encoding.ASCII.GetBytes(inputKey);

        encryptedKey = OTPworker.OTPxor(key, otp);

        return encryptedKey;
    }

    // Sends the encrypted key to the didery server, returns a string with the did
    public string postEncryptedKey(byte[] encryptedKey)
    {
        string[] dideryData;
        dideryData = DideryInterface.makePost(encryptedKey);

        string did = dideryData[0];
        string signature = dideryData[1];
        string postBody = dideryData[2];

        Debug.Log("Did: " + DideryDemoManager.DemoDid);

        postRequest(url, postBody, signature);

        return did;
    }


    // Retrieves the string parameter 'did' from the didery server
    public void getEncryptedKey(string did)
    {
        string uri = url + did;
        getRequest(uri);
    }

    public byte[] checkSeed(byte[] seed)
    {
        int checkVal = OTPworker.checkValidSeed(seedToByte.getActionsFromBytes(seed));
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
            checkVal = OTPworker.checkValidSeed(seedToByte.getActionsFromBytes(seed));
        }

        return seed;
    }

}



/*
int checkVal = OTPworker.checkValidSeed(seedToByte.getActionsFromBytes(seed));
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
    checkVal = OTPworker.checkValidSeed(seedToByte.getActionsFromBytes(seed));
}
*/