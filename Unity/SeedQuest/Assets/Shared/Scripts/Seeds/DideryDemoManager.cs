using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DideryDemoManager : MonoBehaviour {

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

}
