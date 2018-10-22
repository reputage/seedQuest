using System.Collections;
using System.Collections.Generic;
using System;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

public static class DideryInterface{

    public static string getResult;

    // Create a did using the verification key from makeKeyPair()
    public static string makeDid(byte[] vk, string method = "dad")
    {
        string vk64u = Convert.ToBase64String(vk).Replace('+', '-').Replace('/', '_');
        string did = "did:" + method + ":" + vk64u;
        return did;
    }

    // nacl_crypto_sign(signed_message, message, (ulong)message.Length, sk);
    // nacl_crypto_sign_open(unsigned_message, signed_message, (ulong)signed_message.Length, pk);

    // Create signature to use in the header of POST and PUT requests to didery
    public static string signResource(byte[] sm, byte[] m, ulong mlen, byte[] sk, byte[] vk)
    {
        LibSodiumManager.nacl_crypto_sign(sm, m, mlen, sk);
        byte[] sig = new byte[LibSodiumManager.nacl_crypto_sign_BYTES()];
        for (int i = 0; i < sig.Length; i++)
        {
            sig[i] = sm[i];
        }
        //Debug.Log(LibSodiumManager.nacl_crypto_sign_BYTES());

        byte[] usm = new byte[m.Length];
        int success = LibSodiumManager.nacl_crypto_sign_open(usm, sm, (ulong)sm.Length, vk);
        if (success == 0)
        {
            Debug.Log("Signing successful");
        }
        else
        {
            Debug.Log("Signing unsuccessful: " + success); 
        }

        string signature = Convert.ToBase64String(sig).Replace('+', '-').Replace('/', '_');
        //Debug.Log("Sig: " + signature);

        return signature;
    }

    // Puts together the body of the post request for a OTP encrypted key, 
    //  returns a string[] with the did, the signature, and the body of the
    //  post request.
    public static string[] makePost(byte[] encryptedKey)
    {
        byte[] vk = new byte[32];
        byte[] sk = new byte[64];
        string dateTime;
        string body;
        string did;
        string signature;
        string keyString = Convert.ToBase64String(encryptedKey);

        //Debug.Log("Key string in makeDid(): " + keyString);

        int signed_bytes = LibSodiumManager.nacl_crypto_sign_BYTES();

        LibSodiumManager.nacl_crypto_sign_keypair(vk, sk);
        did = makeDid(vk);

        dateTime = DateTime.Now.ToString("yyyy-MM-ddTHH\\:mm\\:ss.ffffffzzz");
        body = "{\"id\":\"" + did + "\",\"blob\":\"" + keyString + "\",\"changed\":\"" + dateTime + "\"}";

        //byte[] sm = new byte[signed_bytes + body.Length];
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

        // Debug.Log(getData[6]);
        string blob = getData[5];
        blob = blob.Replace("changed", string.Empty);
        blob = blob.Replace(":", string.Empty);
        blob = blob.Replace(",", string.Empty);
        blob = blob.Replace(" ", string.Empty);
        blob = blob.Replace("\"", string.Empty);
        Debug.Log("Recieved blob: " + blob);

        dideryBlob getBlob = JsonUtility.FromJson<dideryBlob>(getResult);
        Debug.Log("Blob: " + getBlob.otp_data.blob);

        //Debug.Log(getResult);

        DideryDemoManager.demoBlob = getBlob.otp_data.blob;
    }

    // dideryBlob getBlob = JsonUtility.FromJson<dideryBlob>(getResult);
    // Debug.Log(getBlob.otp_data.blob);

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
    public static IEnumerator PutRequest(string url)
    {
        byte[] dataToPut = System.Text.Encoding.UTF8.GetBytes("Hello, This is a test");
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
