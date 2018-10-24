using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class dideryBlob {

    public dideryOtpData otp_data;
    public string signatures;

    public static dideryBlob CreateFromJSON(string jsonString)
    {
        return JsonUtility.FromJson<dideryBlob>(jsonString);
    }

}

[System.Serializable]
public class dideryOtpData{
    
    public string changed;
    public string blob;
    public string id;

}
