using System.Collections;
using System.Collections.Generic;
using System.Text;
using System;
using UnityEngine;

public class DideryDemoManager : MonoBehaviour
{

    // Note: Coroutines can only be called from a MonoBehavior class, and 
    //  can't be in a static function. GET, POST, and PUT requests must
    //  be done in coroutines if using Unity's web request handler.

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
    public bool isDemo = false;

    public string saveFileVersion;
    public static Dictionary<string, string> userDids = new Dictionary<string, string>();
    public static Dictionary<string, string> userSeeds = new Dictionary<string, string>();

    private string urlAddress = "http://178.128.0.203:8080/blob/";
    private SeedToByte seedToByte = new SeedToByte();

	static public string DemoDid
    {
        get { return Instance.demoDid; }
        set { Instance.demoDid = value; }
    }

    static public string DemoBlob
    {
        get { return Instance.demoBlob; }
        set { Instance.demoBlob = value; }
    }

    static public bool IsDemo
    {
        get { return Instance.isDemo; }
        set { Instance.isDemo = value; }
    }

    static public string SaveFileVersion
    {
        get { return Instance.saveFileVersion; }
        set { Instance.saveFileVersion = value; }
    }

    static public Dictionary<string, string> UserDids
    {
        get { return userDids; }
        set { userDids = value; }
    }

    static public Dictionary<string, string> UserSeeds
    {
        get { return userSeeds; }
        set { userSeeds = value; }
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
    public void demoEncryptKey(string inputKey, string url=null)
    {
        inputKey = VerifyKeys.removeHexPrefix(inputKey);
        if (url == null)
            url = urlAddress;
        int size = 32;
        string[] dideryData;

        byte[] otp = new byte[32];
        //byte[] seed = new byte[16]; // used for 128 bit seed
        byte[] seed = new byte[14]; // used for 108 bit seed
        byte[] encryptedKey = new byte[34];
        byte[] key = Encoding.ASCII.GetBytes(inputKey);

        seed = OTPworker.randomSeedGenerator(seed);

        // Used for demo puroses - required if trying to use 108 bit seed
        if (seed[13] > 7)
            seed[13] = (byte)((int)seed[13] % 7);

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
    }

    // Takes the last used did from DideryDemoManager, retrieves the key
    // from a didery server, and returns the encrypted key to DideryDemoManager.demoBlob
    public void demoGetEncryptedKey(string url=null)
    {
        if (url == null)
            url = urlAddress;
        string uri = url + DideryDemoManager.DemoDid;
        getRequest(uri);
    }

    // Takes a string for a key as input, encrypts it, and returns a byte[] of the encrypted key
    public byte[] encryptKey(string inputKey)
    {
        inputKey = VerifyKeys.removeHexPrefix(inputKey);
        byte[] otp = new byte[32];
        byte[] seed = new byte[16];
        byte[] encryptedKey = new byte[34];
        byte[] key = Encoding.ASCII.GetBytes(inputKey);

        int size = 32;

        seed = OTPworker.randomSeedGenerator(seed);
        OTPworker.OTPGenerator(otp, size, seed);
        encryptedKey = OTPworker.OTPxor(key, otp);

        return encryptedKey;
    }

    // Sends the encrypted key to the didery server, returns a string with the did
    public string postEncryptedKey(byte[] encryptedKey, string url=null)
    {
        if (url == null)
            url = urlAddress;
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
    public void getEncryptedKey(string did, string url=null)
    {
        if (url == null)
            url = urlAddress;
        string uri = url + did;
        getRequest(uri);
    }

    // Checks to see if the seed is within demo parameters. For demo use only
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

    // Sets the did to be recovered at the end of the game
    public void setDid(string name)
    {
        if (userDids.ContainsKey(name))
        {
            if (userDids[name] == "")
                Debug.Log("Error: Name is found in user dids, but the did appears to be an empty string!");
            else
                demoDid = userDids[name];
        }
        else
        {
            Debug.Log("Error: Did with specified name does not appear to exist in user dids");
            return;
        }
    }

    // Adds a did with a label name to the dictionary of user dids, then saves the data in a file
    public void saveDidData(string name, string did)
    {
        SaveDids.saveDidData(userDids, name, did);
    }

    // Save seed data - necessary to rehearse the seed's route later
    //  Note to self: should probably encrypt this in some way, just so it isn't stored in plain text
    public void saveSeedData(string name, string seed)
    {
        SaveDids.saveSeedData(userSeeds, name, seed);
    }

    // Remove the seed data from the saved data file - this will render rehearse mode 
    //  impossible for the chosen seed once this is performed
    public void removeSeedData(string name)
    {
        SaveDids.removeSeedData(userSeeds, name);
    }


    public static string[] encryptAndSaveKey(string name, string inputKey, string url = null)
    {
        string urlAddress = "http://178.128.0.203:8080/blob/";

        inputKey = VerifyKeys.removeHexPrefix(inputKey);
        if (url == null)
            url = urlAddress;
        int size = 32;
        string[] dideryData;

        byte[] otp = new byte[32];
        //byte[] seed = new byte[16]; // used for 128 bit seed
        byte[] seed = new byte[14]; // used for 108 bit seed
        byte[] encryptedKey = new byte[34];
        byte[] key = Encoding.ASCII.GetBytes(inputKey);

        seed = OTPworker.randomSeedGenerator(seed);

        // Used for demo puroses - required if trying to use 108 bit seed
        if (seed[13] > 7)
            seed[13] = (byte)((int)seed[13] % 7);

        OTPworker.OTPGenerator(otp, size, seed);

        encryptedKey = OTPworker.OTPxor(key, otp);

        dideryData = DideryInterface.makePost(encryptedKey);

        string did = dideryData[0];
        string signature = dideryData[1];
        string postBody = dideryData[2];

        //Debug.Log("Did: " + did + " signature: " + " postBody: " + postBody);
        SeedManager.InputSeed = OTPworker.ByteArrayToHex(seed);
        DideryDemoManager.DemoDid = did;
        //Debug.Log("Did: " + DideryDemoManager.DemoDid);


        // Temporarily disabling this for testing purposes
        //postRequest(url, postBody, signature);

        string seedString = BitConverter.ToString(seed).Replace("-", "");
        string[] keyData = new string[2];
        keyData[0] = seedString;
        keyData[1] = SeedToByte.ByteArrayToHex(encryptedKey); // temporariyl only saving the encrypted key - later on should store the entire did

        return keyData;
    }
}
