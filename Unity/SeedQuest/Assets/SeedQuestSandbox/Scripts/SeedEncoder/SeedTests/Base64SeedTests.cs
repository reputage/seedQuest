using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using System.Text;
using SeedQuest.SeedEncoder;

public class Base64SeedTests : MonoBehaviour
{
    private string testSeed1 = "C5E3D45D341A";
    private string testSeed2 = "AAAA";
    public List<int> actionList = new List<int>();
    private SeedToByte seedToByte = new SeedToByte();

    private void Start()
    {
        runAllTests();
    }

    // Run all tests at once
    public string runAllTests()
    {
        int[] passed = new int[2];

        sumTest(ref passed, testHexToBase64());
        sumTest(ref passed, testBase64Validator());

        string passedString = "Successfully passed " + passed[0] + " of " + passed[1] + " base64 tests.";
        Debug.Log(passedString);
        return passedString;
    }

    // This function helps make the test running code a bit cleaner
    public void sumTest(ref int[] passed, int[] testPassed)
    {
        if (passed.Length < 2 || testPassed.Length < 2)
            Debug.Log("Error summing test results: int[] shorter than two elements");

        passed[0] += testPassed[0];
        passed[1] += testPassed[1];
    }

    // test for converting a hex seed into base64
    public int[] testHexToBase64()
    {
        int[] passed = new int[2];
        passed[1] = 6;

        string seed1 = "504142434445464748494A4B4C4D4E41";
        string seed2 = "EFDFCFBFAFFFF0FFFEFFFF1FFF2FFFFF";
        string seed3 = "00000000000000000000000000000000";

        byte[] bytes1 = SeedToByte.HexStringToByteArray(seed1);
        byte[] bytes2 = SeedToByte.HexStringToByteArray(seed2);
        byte[] bytes3 = SeedToByte.HexStringToByteArray(seed3);

        string base1 = Convert.ToBase64String(bytes1);
        string base2 = Convert.ToBase64String(bytes2);
        string base3 = Convert.ToBase64String(bytes3);
        base3 = Base64Converter.removeBufferChars(base3);

        byte[] bytes4 = Base64Converter.base64ToByte(base1);
        string hex = Base64Converter.base64ToHex(base1);
        string base4 = Base64Converter.hexToBase64(hex);
        string base5 = Base64Converter.byteToBase64(bytes4);

        if (base1 == "UEFCQ0RFRkdISUpLTE1OQQ==")
            passed[0] += 1;
        else
            Debug.Log("Test for Base64 conversion failed. Base64 string #1: " + base1 + " Should be: UEFCQ0RFRkdISUpLTE1OQQ==");

        if (base2 == "79/Pv6//8P/+//8f/y///w==")
            passed[0] += 1;
        else
            Debug.Log("Test for Base64 conversion failed. Base64 string #2: " + base2 + " Should be: 79/Pv6//8P/+//8f/y///w==");
        if (base3 == "AAAAAAAAAAAAAAAAAAAAAA")
            passed[0] += 1;
        else
            Debug.Log("Test for buffer char removal failed. Base64 string #3: " + base3 + " Should be: AAAAAAAAAAAAAAAAAAAAAA");
        if (hex == "504142434445464748494A4B4C4D4E41")
            passed[0] += 1;
        else
            Debug.Log("Test for base64 to hex conversion failed. Hex string: " + hex + " Should be: 504142434445464748494A4B4C4D4E41");
        if (base4 == "UEFCQ0RFRkdISUpLTE1OQQ")
            passed[0] += 1;
        else
            Debug.Log("Test for hex to Base64 conversion failed.Base64 string #4: " + base4 + " Should be: UEFCQ0RFRkdISUpLTE1OQQ");
        if (base5 == "UEFCQ0RFRkdISUpLTE1OQQ")
            passed[0] += 1;
        else
            Debug.Log("Test for Base64 conversion failed. Base64 string #5: " + base5 + " Should be: UEFCQ0RFRkdISUpLTE1OQQ");


        return passed;
    }

    public int[] testBase64ToHex()
    {
        int[] passed = new int[2];
        string j = "";
        return passed;
    }

    public int[] testBase64Validator()
    {
        int[] passed = new int[2];
        passed[1] = 2;
        string valid = "qwertyuiopasdfghjklzxcvbnmZXCVBNMASDFGHJKLQWERTYUIOP1234567890/+=";
        string invalid = "abcdefg123456[";

        if (SeedUtility.detectBase64(valid))
            passed[0] += 1;
        if (!SeedUtility.detectBase64(invalid))
            passed[0] += 1;
        
        return passed;
    }

}