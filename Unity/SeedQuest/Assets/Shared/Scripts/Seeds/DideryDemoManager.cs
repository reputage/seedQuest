using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class DideryDemoManager : MonoBehaviour {

    // Note: coroutines can only be called from a MonoBehavior class, and 
    //  can't be in a static function.

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


    public void postRequest(string url, string postBody, string signature)
    {
        StartCoroutine(DideryInterface.PostRequest(url, postBody, signature));
    }

    public void getRequest(string uri)
    {
        StartCoroutine(DideryInterface.GetRequest(uri));
    }

    public void demoEncryptKey(string inputKey)
    {
        string[] dideryData;

        byte[] otp = new byte[32];
        byte[] seed = new byte[16];
        byte[] key = new byte[32];
        byte[] formatKey = new byte[34];

        int size = 32;

        string did = "";
        string signature = "";
        string postBody = "";

        seed = OTPworker.randomSeedGenerator(seed);
        //seed = HexStringToByteArray("4040C1A90886218984850151AC123249");

        // Check seed to see if it is within the demo parameters
        //seed = checkValidSeed(seedToByte.getActionsFromBytes(seed));

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

        //Debug.Log(ByteArrayToHex(seed));

        OTPworker.OTPGenerator(otp, size, seed);
        key = Encoding.ASCII.GetBytes(inputKey);

        formatKey = OTPworker.OTPxor(key, otp);
        dideryData = DideryInterface.makePost(formatKey);

        did = dideryData[0];
        signature = dideryData[1];
        postBody = dideryData[2];

        //Debug.Log("Did: " + did + " signature: " + " postBody: " + postBody);

        SeedManager.InputSeed = OTPworker.ByteArrayToHex(seed);
        DideryDemoManager.DemoDid = did;
        Debug.Log("Did: " + DideryDemoManager.DemoDid);

        //dideryDemoManager.postRequest(url, postBody, signature);
        StartCoroutine(DideryInterface.PostRequest(url, postBody, signature));
    }

    // Takes the last used did from DideryDemoManager, retrieves the key
    // from a didery server, and returns the encrypted key to DideryDemoManager.demoBlob
    public void demoGetEncryptedKey()
    {
        string uri = url + DideryDemoManager.DemoDid;
        Debug.Log(uri);

        //dideryDemoManager.getRequest(uri);
        StartCoroutine(DideryInterface.GetRequest(uri));
    }

}
