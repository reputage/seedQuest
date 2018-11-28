using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text;
using System;

public class startMenuDemo : MonoBehaviour {

    public InputField keyInputField;
    public Text keyString = null;

    public GameObject encryptButton;
    public GameObject demoKeyButton;
    private bool allowEnter;
    private bool entered;

	void Start () 
    {
        entered = false;
        allowEnter = false;
    }
	
	void Update () 
    {
        if (allowEnter && keyInputField.text != "" && Input.GetKey(KeyCode.Return))
        {
            encryptKey();
        }
        else
            allowEnter = keyInputField.isFocused;
    }

    public void encryptKey()
    {
        if (!entered)
        {
            DideryDemoManager.Instance.demoEncryptKey(keyInputField.text);
            deactivateEncryptButtons();
            changeKeyToCensored();
            entered = true;
        }
        else
        {
            deactivateEncryptButtons();
        }
    }

    public void deactivateEncryptButtons()
    {
        encryptButton.SetActive(false);
        demoKeyButton.SetActive(false);
    }

    public void changeKeyToCensored()
    {
        keyInputField.text = censoredKey(keyInputField.text);
    }

    public void useDemoSeed()
    {
        DideryDemoManager.IsDemo = true;
        DideryDemoManager.DemoBlob = keyInputField.text;
        SeedManager.InputSeed =  "148436BD13EEB72557080989DF01"; //"A021E0A80264A33C08B6C2884AC0685C";
        deactivateEncryptButtons();
    }

    public void useDemoKeyAndStart() {
        DideryDemoManager.IsDemo = true;
        DideryDemoManager.DemoBlob = keyInputField.text;
        SeedManager.InputSeed = "A021E0A80264A33C08B6C2884AC0685C"; //"4040C1A90886218984850151AC123249";
        GameManager.State = GameState.Rehearsal;
    }

    public void testGetKey()
    {
        DideryDemoManager.Instance.demoGetEncryptedKey();
    }

    public void testDecrypt()
    {
        string seed = SeedManager.InputSeed;
        Debug.Log("Seed: " + seed);
        byte[] keyByte = OTPworker.decryptFromBlob(seed, DideryDemoManager.DemoBlob);
        string finalKey = Encoding.ASCII.GetString(keyByte);
        keyString.text = finalKey;
        Debug.Log("Decrypted key: " + finalKey);
    }

    // Test that an invalid key fails the key validation function
    public void testBadDecrypt()
    {
        string seed = "A021E0A80264A33C08B6C2884AC0685C";
        string badBlob = "aaaabbbbaaaabbbbaaaabbbbaaaabbbb";
        byte[] keyByte = OTPworker.decryptFromBlob(seed, badBlob);
        string finalKey = Encoding.ASCII.GetString(keyByte);
        Debug.Log("Bad decrypted key: " + finalKey);
    }

    // Test that a valid key passes the key validation function
    public void testValidKey()
    {
        string key = "0xb5b1870957d373ef0eeffecc6e4812c0fd08f554b37b233526acc331bf1544f7";
        VerifyKeys.verifyKey(key);
    }

    // Test that a valid key passes the key validation function
    public void testGoodDecrypt()
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
        Debug.Log("Bad decrypted key: " + finalKey);
    }

    // Censor all but the last 4 digits of the input key
    public string censoredKey(string key)
    {
        char[] oldKey = key.ToCharArray();
        char[] newKey = new char[key.Length];
        for (int i = 0; i < oldKey.Length; i++)
        {
            if (i > oldKey.Length - 5)
            {
                newKey[i] = oldKey[i];
            }
            else
                newKey[i] = '*';
        }
        string returnStr = new string(newKey);
        return returnStr;
    }

}
