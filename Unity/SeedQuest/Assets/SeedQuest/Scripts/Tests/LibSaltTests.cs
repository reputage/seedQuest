using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LibSaltTests : MonoBehaviour {

	void Start () {
		
	}
	
    public void runAllTests()
    {
        testOtpGenerator();
    }

    private void testOtpGenerator()
    {
        int size = 16;
        bool failure = false;    

        string s1 = "9876abcdef";
        string s2 = "1234567890";
        string s3 = "0245789bce";

        byte[] seed1 = OTPworker.HexStringToByteArray(s1);
        byte[] seed2 = OTPworker.HexStringToByteArray(s2);
        byte[] seed3 = OTPworker.HexStringToByteArray(s3);

        byte[] output = new byte[size];
        List<string> result1 = new List<string>();
        List<string> result2 = new List<string>();
        List<string> result3 = new List<string>();

        for (int i = 0; i < 10; i++)
        {
            OTPworker.OTPGenerator(output, size, seed1);
            result1.Add(OTPworker.ByteArrayToHex(output));
        }

        for (int i = 0; i < 10; i++)
        {
            OTPworker.OTPGenerator(output, size, seed2);
            result2.Add(OTPworker.ByteArrayToHex(output));
        }

        for (int i = 0; i < 10; i++)
        {
            OTPworker.OTPGenerator(output, size, seed2);
            result3.Add(OTPworker.ByteArrayToHex(output));
        }

        // To do: iterate through all lists and compare strings to see if they are all the same
        for (int i = 0; i < result1.Count; i++)
        {
            if (result1[0] != result1[i])
            {
                Debug.Log("String mismatch in result1");
                failure = true;
            }
            if (result2[0] != result2[i])
            {
                Debug.Log("String mismatch in result1");
                failure = true;
            }
            if (result3[0] != result3[i])
            {
                Debug.Log("String mismatch in result1");
                failure = true;
            }
        }

        if (failure == false)
            Debug.Log("RandomBytesDeterministic test passed");
    }

    // To be finished at a later date
    public void testRandomSeedGenerator()
    {
        byte[] testSeed1 = new byte[10];
        byte[] testSeed2 = new byte[10];
        testSeed1 = OTPworker.randomSeedGenerator(testSeed1);
        testSeed2 = OTPworker.randomSeedGenerator(testSeed2);

        //Debug.Log("First value: " + testSeed1[0] + " " + testSeed2[0]);
        Debug.Log("RandomSeedGenerator() test passed");

    }

    // To do
    public void testDecryptKey()
    {
        
    }


}
