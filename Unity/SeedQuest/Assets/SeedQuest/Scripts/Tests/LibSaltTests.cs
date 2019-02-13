using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LibSaltTests : MonoBehaviour {

	void Start () {
		
	}
	
    public void runAllTests()
    {
        int[] passed = new int[2];
        sumTest(ref passed, testOtpGenerator());
        sumTest(ref passed, testRandomSeedGenerator());
        sumTest(ref passed, testDecryptKey());
        Debug.Log("Successfully passed " + passed[0] + " out of " + passed[1] + " LibSalt tests.");
    }

    // This function helps make the test running code a bit cleaner
    public void sumTest(ref int[] passed, int[] testPassed)
    {
        if (passed.Length < 2 || testPassed.Length < 2)
            Debug.Log("Error summing test results: int[] shorter than two elements");

        passed[0] += testPassed[0];
        passed[1] += testPassed[1];
    }

    public int[] testOtpGenerator()
    {
        int[] passed = new int[2];
        passed[1] = 1;
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
            OTPworker.OTPGenerator(output, size, seed3);
            result3.Add(OTPworker.ByteArrayToHex(output));
        }

        for (int i = 0; i < result1.Count; i++)
        {
            if (result1[0] != result1[i])
            {
                Debug.Log("String mismatch in result1");
                failure = true;
            }
            if (result2[0] != result2[i])
            {
                Debug.Log("String mismatch in result2");
                failure = true;
            }
            if (result3[0] != result3[i])
            {
                Debug.Log("String mismatch in result3");
                failure = true;
            }
        }

        if (failure == false)
        {
            passed[0] = 1;
        }
        else
            Debug.Log("RandomBytesDeterministic test failed");

        return passed;
    }

    // To be finished at a later date
    public int[] testRandomSeedGenerator()
    {
        int[] passed = new int[2];
        passed[1] = 1;

        byte[] testSeed1 = new byte[10];
        byte[] testSeed2 = new byte[10];
        testSeed1 = OTPworker.randomSeedGenerator(testSeed1);
        testSeed2 = OTPworker.randomSeedGenerator(testSeed2);

        Debug.Log("RandomSeedGenerator() test passed");

        // if no errors have happened by here, the test has passed
        passed[0] = 1;
        return passed;
    }

    // To do
    public int[] testDecryptKey()
    {
        int[] passed = new int[2];
        passed[1] = 1;
        int size = 32;
        byte[] key = new byte[size];
        byte[] eKey = new byte[size];
        byte[] dKey = new byte[size];
        byte[] seed = new byte[size];
        byte[] otp = new byte[size];
        bool same = true;

        for (int i = 0; i < key.Length; i++)
        {
            key[i] = (byte)i;
        }

        for (int i = 0; i < seed.Length; i++)
        {
            seed[i] = (byte)i;
        }

        OTPworker.OTPGenerator(otp, size, seed);
        eKey = OTPworker.OTPxor(key, otp);
        dKey = OTPworker.OTPxor(eKey, otp);

        for (int i = 0; i < key.Length; i ++)
        {
            if (key[i] != dKey[i])
            {
                same = false;
            }
        }

        if(same)
        {
            Debug.Log("Test for encryption and decryption passed");
            passed[0] = 1;
        }
        else
        {
            Debug.Log("Test for encryption and decryption failed");
            Debug.Log("Values: " + key[0] + "-" + dKey[0] + " " + key[1] + "-" + dKey[1]);
        }

        return passed;
    }

}
