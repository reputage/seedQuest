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


//{"otp_data": {"changed": "2018-10-22T13:09:44.393768-06:00", 
// "blob": "RmUSZkwwfAfHiN5G", 
// "id": "did:dad:2Xruj0zviOYBGUVK1vgQAedXH_Tx2XYC73nr0URqLBo="}, 
// "signatures": 
// {"signer": "4nzjj_9HH7HCXasmrkXoL9YZHQmMj7dRzYc5nKHXPfScB4ZYAwqNRergfl9FtvlvQp9ikAIO7KCaNF4YQ-aYAw=="}}