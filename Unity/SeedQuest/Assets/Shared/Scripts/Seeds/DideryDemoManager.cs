using System.Collections;
using System.Collections.Generic;
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
    //public string url = "http://178.128.0.203:8080/blob/";

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

    /*
    public void postRequest(string url, string postBody, string signature)
    {
        StartCoroutine(DideryInterface.PostRequest(url, postBody, signature));
    }

    public void getRequest(string uri)
    {
        StartCoroutine(DideryInterface.GetRequest(uri));
    }
    */

}
