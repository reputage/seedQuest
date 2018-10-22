using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text;
using System;

public class startMenuDemo : MonoBehaviour {

    public InputField keyInputField;
    public Text keyString = null;
    public OTPworker otpWorker;
    //public SeedManager seedManager;

	void Start () 
    {
        otpWorker = FindObjectOfType<OTPworker>();
    }
	
	void Update () {
		
	}

    public void encryptKey()
    {
        //Debug.Log(keyInputField.text);
        otpWorker.encryptKey(keyInputField.text);
    }

    public void testGetKey()
    {
        otpWorker.getEncryptedKey();
    }

    public void testDecrypt()
    {
        string seed = SeedManager.InputSeed;
        //Debug.Log("Seed: " + seed);
        byte[] keyByte = otpWorker.decryptFromBlob(seed);
        string finalKey = Encoding.ASCII.GetString(keyByte);
        keyString.text = finalKey;
        Debug.Log("Decrypted key: " + finalKey);
    }


}
