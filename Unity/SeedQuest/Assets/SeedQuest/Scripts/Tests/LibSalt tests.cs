using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LibSalttests : MonoBehaviour {

	void Start () {
		
	}
	
    public void runAllTests()
    {
        
    }

    private void testRandomBytesDeterministic()
    {
        int size = 16;
            
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

    }
}
