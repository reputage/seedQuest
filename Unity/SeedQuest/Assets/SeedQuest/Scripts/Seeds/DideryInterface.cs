using System.Collections;
using System.Collections.Generic;
using System;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

public static class DideryInterface{

    // Create a did using the verification key from makeKeyPair()
    //  Uses the 'dad' method
    public static string makeDid(byte[] vk, string method = "dad")
    {
        string vk64u = Convert.ToBase64String(vk).Replace('+', '-').Replace('/', '_');
        string did = "did:" + method + ":" + vk64u;
        return did;
    }

    // Create signature to use in the header of POST and PUT requests to didery
    public static string signResource(byte[] sm, byte[] m, ulong mlen, byte[] sk, byte[] vk)
    {
        LibSalt.nacl_crypto_sign(sm, m, mlen, sk);
        byte[] sig = new byte[LibSalt.nacl_crypto_sign_BYTES()];

        for (int i = 0; i < sig.Length; i++)
        {
            sig[i] = sm[i];
        }

        byte[] usm = new byte[m.Length];
        int success = LibSalt.nacl_crypto_sign_open(usm, sm, (ulong)sm.Length, vk);

        if (success == 0)
        {
            //Debug.Log("Signing successful");
        }
        else
        {
            Debug.Log("Signing unsuccessful: " + success); 
        }

        string signature = Convert.ToBase64String(sig).Replace('+', '-').Replace('/', '_');
        return signature;
    }

    // Puts together the body of the post request for a OTP encrypted key, 
    //  returns a string[] with the did, the signature, and the body of the
    //  post request.
    public static string[] makePost(byte[] encryptedKey, byte[] seed)
    {
        byte[] vk = new byte[32];
        byte[] sk = new byte[64];
        string dateTime;
        string body;
        string did;
        string signature;
        string keyString = Convert.ToBase64String(encryptedKey);

        int signed_bytes = LibSalt.nacl_crypto_sign_BYTES();

        // This function eventually needs to be changed to use a deterministic keypair generator
        //  which should use the user's seed as the RNG seed
        LibSalt.nacl_crypto_sign_seed_keypair(vk, sk, seed);

        did = makeDid(vk);

        dateTime = DateTime.Now.ToString("yyyy-MM-ddTHH\\:mm\\:ss.ffffffzzz");
        body = "{\"id\":\"" + did + "\",\"blob\":\"" + keyString + "\",\"changed\":\"" + dateTime + "\"}";

        byte[] bodyByte = new byte[body.Length];
        bodyByte = Encoding.UTF8.GetBytes(body);

        byte[] sm = new byte[signed_bytes + bodyByte.Length];

        signature = signResource(sm, bodyByte, (ulong)bodyByte.Length, sk, vk);
        signature = "signer=\"" + signature + "\"";

        string[] data = new string[3];
        data[0] = did;
        data[1] = signature;
        data[2] = body;

        return data;
    }

    // Send a GET request to the uri, saves the requested blob to demoBlob in DideryDemoManager
    public static IEnumerator GetRequest(string uri)
    {
        string getResult;

        UnityWebRequest uwr = UnityWebRequest.Get(uri);
        yield return uwr.SendWebRequest();

        getResult = uwr.downloadHandler.text;

        if (uwr.isNetworkError)
        {
            Debug.Log("Error While Sending: " + uwr.error);
        }
        else
        {
            Debug.Log("Received: " + uwr.downloadHandler.text);
        }

        string[] getData = getResult.Split(':');

        dideryBlob getBlob = JsonUtility.FromJson<dideryBlob>(getResult);
        //Debug.Log("Blob: " + getBlob.otp_data.blob);

        DideryDemoManager.DemoBlob = getBlob.otp_data.blob;
        Debug.Log("GET request coroutine finished");
    }

    // Sends a POST request to didery at the url specified. 
    // Requires json body and signature from makePost()
    public static IEnumerator PostRequest(string url, string json, string signature)
    {
        var uwr = new UnityWebRequest(url, "POST");
        byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(json);
        uwr.uploadHandler = (UploadHandler)new UploadHandlerRaw(jsonToSend);
        uwr.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        uwr.SetRequestHeader("Content-Type", "application/json");
        uwr.SetRequestHeader("Signature", signature);

        yield return uwr.SendWebRequest();

        if (uwr.isNetworkError)
        {
            Debug.Log("Error While Sending: " + uwr.error);
        }
        else
        {
            Debug.Log("Received: " + uwr.downloadHandler.text);
        }
    }

    // Has not been configured for use with didery yet

    /*
    public static IEnumerator PutRequest(string url, string body)
    {
        byte[] dataToPut = System.Text.Encoding.UTF8.GetBytes(body);
        UnityWebRequest uwr = UnityWebRequest.Put(url, dataToPut);
        yield return uwr.SendWebRequest();

        if (uwr.isNetworkError)
        {
            Debug.Log("Error While Sending: " + uwr.error);
        }
        else
        {
            Debug.Log("Received: " + uwr.downloadHandler.text);
        }
    }
    */

}
