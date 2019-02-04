using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DideryTests : MonoBehaviour 
{

    public void runAllTests()
    {
        testMakePost();
        testMakeDid();
    }

    public void testMakePost()
    {
        byte[] key = new byte[16];
        byte[] seed = new byte[16];

        for (int i = 0; i < key.Length; i++)
        {
            key[i] = (byte)i;
        }
        for (int i = 0; i < seed.Length; i++)
        {
            seed[i] = (byte)i;
        }

        string[] post = DideryInterface.makePost(key, seed);

        Debug.Log("Test post: " + post[0] + " " + post[1] + " " + post[2]);

    }

    public void testMakeDid()
    {
        byte[] key = new byte[16];
        for (int i = 0; i < key.Length; i++)
        {
            key[i] = (byte)i;
        }

        string did = DideryInterface.makeDid(key);

        Debug.Log("Test did: " + did);
        //:AAECAwQFBgcICQoLDA0ODw==
    }

}
