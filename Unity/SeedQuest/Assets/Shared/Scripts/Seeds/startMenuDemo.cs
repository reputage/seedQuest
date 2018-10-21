using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text;

public class startMenuDemo : MonoBehaviour {

    public InputField keyInputField;
    public OTPworker otpWorker;

	void Start () 
    {

    }
	
	void Update () {
		
	}

    public void encryptKey()
    {
        otpWorker.encryptKey(keyInputField.text);
    }


}
