using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SeedQuest.SeedEncoder;

public class SeedToByteTests : MonoBehaviour
{
    private string testSeed1 = "C5E3D45D341A";
    private string testSeed2 = "AAAA";

    public List<int> actionList = new List<int>();

    private SeedToByte seedToByte = new SeedToByte();

	private void Start()
	{
        //runAllTests();
        runQuickTests();
	}

	// Run all tests at once
	public string runAllTests()
    {
        int[] passed = new int[2];

        sumTest(ref passed, testByteBitConversion());
        sumTest(ref passed, testMultipleSizeSeeds());
        sumTest(ref passed, testFindLeadingBits());
        sumTest(ref passed, testBreakPoints());
        sumTest(ref passed, testSmallSeeds());
        sumTest(ref passed, testAllSizeSeeds());
        sumTest(ref passed, testSeeds4BitActions());
        sumTest(ref passed, testSeeds5BitActions());
        sumTest(ref passed, testSeedsVariableBitActions());

        string passedString = "Successfully passed " + passed[0] + " of " + passed[1] + " seed tests.";
        Debug.Log(passedString);
        return passedString;
    }

    // run most tests, but exclude the more time consuming ones.
    public string runQuickTests()
    {
        int[] passed = new int[2];

        sumTest(ref passed, testByteBitConversion());
        sumTest(ref passed, testMultipleSizeSeeds());
        sumTest(ref passed, testFindLeadingBits());
        sumTest(ref passed, testBreakPoints());
        sumTest(ref passed, testSmallSeeds());
        sumTest(ref passed, testSeeds5BitActions());
        //sumTest(ref passed, testSpecialException());

        string passedString = "Successfully passed " + passed[0] + " of " + passed[1] + " seed tests.";
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

    // Test to make sure smaller conversion functions work as intended
    public int[] testByteBitConversion()
    {
        int[] passed = new int[2];

        byte[] testByteArr = seedToByte.seedToByte(testSeed2);
        string testReturnStr = seedToByte.byteToSeed(testByteArr);
        BitArray testBitArr = seedToByte.byteToBits(testByteArr);
        byte[] testReturnBytes = seedToByte.bitToByte(testBitArr);
        string testReturnStr2 = seedToByte.byteToSeed(testReturnBytes);
        int[] testActionToDo = seedToByte.bitToActions(testBitArr, actionList);

        passed[1] += 1;
        if (testSeed2 == testReturnStr)
        {
            passed[0] += 1;
        }
        else
            Debug.Log("Test for conversion from byte array to string failed");

        passed[1] += 1;
        if (seedToByte.byteToSeed(testByteArr) == seedToByte.byteToSeed(testReturnBytes))
        {
            passed[0] += 1;
        }
        else
            Debug.Log("Test for conversion from bit array to byte array failed");

        return passed;
    }

    // Test seeds with sizes of 112 bits and 108 bits
    public int[] testMultipleSizeSeeds()
    {
        int[] passed = new int[2];

        string testHex = "FFFFAAAAFFFFAAAAFFFFDDDDFFFF";

        byte[] testHexSeed = SeedToByte.HexStringToByteArray(testHex);
        byte[] testRunSeed = new byte[14];

        List<int> tempList1 = SeedToByte.customList(4, 4, 2, 4, 4);
        List<int> tempList2 = SeedToByte.customList(3, 4, 2, 4, 4);

        byte[] finalSeed1 = conversionHelper(testRunSeed, tempList1);
        byte[] finalSeed2 = conversionHelper(testHexSeed, tempList1);

        passed[1] += 1;
        if (seedToByte.byteToSeed(testRunSeed) == seedToByte.byteToSeed(finalSeed1))
        {
            passed[0] += 1;
        }
        else
            Debug.Log("Test 1 for converting 112 bit seed to action list and back failed");

        passed[1] += 1;
        if (seedToByte.byteToSeed(testHexSeed) == seedToByte.byteToSeed(finalSeed2))
        {
            passed[0] += 1;
        }
        else
            Debug.Log("Test 2 for converting 112 bit seed to action list and back failed");

        testHexSeed = new byte[14];
        byte[] finalSeed3 = conversionHelper(testHexSeed, tempList2);

        passed[1] += 1;
        if (seedToByte.byteToSeed(testHexSeed) == seedToByte.byteToSeed(finalSeed3))
        {
            passed[0] += 1;
        }
        else
            Debug.Log("Test for converting 108 bit seed to action list and back failed");

        return passed;
    }

    // Test smaller sized seeds of 16 bits and 24 bits
    public int[] testSmallSeeds()
    {
        int[] passed = new int[2];

        string testHex = "FFFF";
        byte[] testHexSeed = SeedToByte.HexStringToByteArray(testHex);
        List<int> tempList = SeedToByte.customList(6, 6, 4, 1, 1);
        byte[] finalSeed = conversionHelper(testHexSeed, tempList);

        passed[1] += 1;
        if (seedToByte.byteToSeed(testHexSeed) == seedToByte.byteToSeed(finalSeed))
        {
            passed[0] += 1;
        }
        else
            Debug.Log("Test for converting 16 bit seed to action list and back failed");

        testHex = "FFFFFF";
        testHexSeed = SeedToByte.HexStringToByteArray(testHex);
        tempList = SeedToByte.customList(2, 3, 2, 2, 2);
        finalSeed = finalSeed = conversionHelper(testHexSeed, tempList);

        passed[1] += 1;
        if (seedToByte.byteToSeed(testHexSeed) == seedToByte.byteToSeed(finalSeed))
        {
            passed[0] += 1;
        }
        else
            Debug.Log("Test for converting 24 bit seed to action list and back failed");

        return passed;
    }

    // Test seeds of size 64 bits and 128 bits. Important because these are the break points
    //  of the converting functions using more uInt 32 variables
    public int[] testBreakPoints()
    {
        int[] passed = new int[2];

        string testHex = "FFFFAAAAFFFFAAAA";
        byte[] testHexSeed = SeedToByte.HexStringToByteArray(testHex);
        List<int> tempList = SeedToByte.customList(4, 8, 6, 2, 2);
        byte[] finalSeed = conversionHelper(testHexSeed, tempList);

        passed[1] += 1;
        if (seedToByte.byteToSeed(testHexSeed) == seedToByte.byteToSeed(finalSeed))
        {
            passed[0] += 1;
        }
        else
            Debug.Log("Test for converting 64 bit seed to action list and back failed");

        testHex = "FFFFAAAAFFFFAAAAFFFFAAAAFFFFAAAA";
        testHexSeed = SeedToByte.HexStringToByteArray(testHex);
        tempList = SeedToByte.customList(4, 8, 6, 2, 4);
        finalSeed = conversionHelper(testHexSeed, tempList);

        passed[1] += 1;
        if (seedToByte.byteToSeed(testHexSeed) == seedToByte.byteToSeed(finalSeed))
        {
            passed[0] += 1;
        }
        else
            Debug.Log("Test for converting 128 bit seed to action list and back failed");

        return passed;
    }

    // Tests the 'findLeadingBitValue' function. Important for handling action
    //  lists with actions that aren't cumulatively evenly divisible by 64 bits
    public int[] testFindLeadingBits()
    {
        int[] passed = new int[2];
        int[] leadingBits = new int[3];

        runLeadingBits(ref passed, 2, 3, 15, 3, 1, 1, 1);
        runLeadingBits(ref passed, 2, 3, 14, 3, 0, 1, 2);
        runLeadingBits(ref passed, 2, 3, 9, 0, 1, 1, 3);
        runLeadingBits(ref passed, 2, 3, 1, 0, 1, 1, 4);
        runLeadingBits(ref passed, 3, 4, 15, 7, 1, 1, 5);
        runLeadingBits(ref passed, 3, 4, 14, 7, 0, 1, 6);
        runLeadingBits(ref passed, 3, 4, 9, 4, 1, 1, 7);
        runLeadingBits(ref passed, 3, 4, 1, 0, 1, 1, 8);
        runLeadingBits(ref passed, 4, 4, 15, 15, 0, 0, 9);
        runLeadingBits(ref passed, 4, 4, 14, 14, 0, 0, 10);
        runLeadingBits(ref passed, 4, 4, 9, 9, 0, 0, 11);
        runLeadingBits(ref passed, 4, 4, 1, 1, 0, 0, 12);
        runLeadingBits(ref passed, 2, 3, 0, 0, 0, 1, 13);
        runLeadingBits(ref passed, 0, 4, 9, 0, 0, 0, 14);
        runLeadingBits(ref passed, 1, 8, 255, 1, 127, 7, 15);
        runLeadingBits(ref passed, 2, 8, 255, 3, 63, 6, 16);
        runLeadingBits(ref passed, 3, 8, 255, 7, 31, 5, 17);
        runLeadingBits(ref passed, 4, 8, 255, 15, 15, 4, 18);
        runLeadingBits(ref passed, 5, 8, 255, 31, 7, 3, 19);
        runLeadingBits(ref passed, 6, 8, 255, 63, 3, 2, 20);
        runLeadingBits(ref passed, 7, 8, 255, 127, 1, 1, 21);
        runLeadingBits(ref passed, 8, 8, 255, 255, 0, 0, 22);

        return passed;
    }

    // Helper function for the findLeadingBitValue tests
    public void runLeadingBits(ref int[] passed, int leadingBit, int totalBit, int value,
                              int expected1, int expected2, int expected3, int testNumber)
    {
        passed[1] += 1;
        int[] leadingBits = new int[3];

        leadingBits = SeedToByte.findLeadingBitValue(leadingBit, totalBit, value);

        if (leadingBits[0] == expected1 && leadingBits[1] == expected2 && leadingBits[2] == expected3)
            passed[0] += 1;
        else
            Debug.Log("Leading bits test " + testNumber + " failed");
    }

    // Test seed converter for all possible seed sizes
    //  Creates a byte array filled with maximum possible values for N bits, and 
    //  converts to an action list and back with seedConverterUniversal()
    public int[] testAllSizeSeeds()
    {
        int[] passed = new int[2];

        for (int i = 10; i < 512; i++)
        {
            List<int> hexList = new List<int>();
            for (int j = 0; j < i; j++)
                hexList.Add(1);

            byte[] byteHex = new byte[(i / 8) + 1];

            if (i % 8 == 0)
                byteHex = new byte[i / 8];

            for (int j = 0; j < byteHex.Length; j++)
                byteHex[j] = 255;

            byteHex[byteHex.Length - 1] = (byte)(Math.Pow(2, i % 8) - 1);
            byte[] finalSeed = conversionHelper(byteHex, hexList);

            passed[1] += 1;

            if (seedToByte.byteToSeed(byteHex) == seedToByte.byteToSeed(finalSeed))
                passed[0] += 1;
            else
                Debug.Log("Test for seed size of: " + i + " failed.");
        }

        return passed;
    }

    // Similar to the above tests, but uses 'actions' of 4 bits instead of 1 bit
    public int[] testSeeds4BitActions()
    {
        int[] passed = new int[2];

        for (int i = 16; i < 264; i += 4)
        {
            List<int> hexList = new List<int>();
            for (int j = 0; j < i; j += 4)
            {
                hexList.Add(4);
            }

            byte[] byteHex = new byte[(i / 8) + 1];

            if (i % 8 == 0)
                byteHex = new byte[i / 8];

            for (int j = 0; j < byteHex.Length; j++)
            {
                byteHex[j] = 255;
            }

            byteHex[byteHex.Length - 1] = (byte)(Math.Pow(2, i % 8) - 1);
            byte[] finalSeed = conversionHelper(byteHex, hexList);

            passed[1] += 1;

            if (seedToByte.byteToSeed(byteHex) == seedToByte.byteToSeed(finalSeed))
                passed[0] += 1;
            
            else
                Debug.Log("Test for seed size of: " + i + " failed.");
        }

        return passed;
    }

    // Similar to the above tests, but uses 'actions' of 5 bits
    public int[] testSeeds5BitActions()
    {
        int[] passed = new int[2];

        for (int i = 10; i < 280; i += 5)
        {
            List<int> hexList = new List<int>();
            for (int j = 0; j < i; j += 5)
            {
                hexList.Add(5);
            }

            byte[] byteHex = new byte[(i / 8) + 1];

            if (i % 8 == 0)
                byteHex = new byte[i / 8];

            for (int j = 0; j < byteHex.Length; j++)
            {
                byteHex[j] = 255;
            }

            byteHex[byteHex.Length - 1] = (byte)(Math.Pow(2, i % 8) - 1);
            byte[] finalSeed = conversionHelper(byteHex, hexList);
            //Debug.Log("I: " + i + " seed: " + seedToByte.byteToSeed(byteHex) + " final seed: " + seedToByte.byteToSeed(finalSeed));

            passed[1] += 1;

            if (seedToByte.byteToSeed(byteHex) == seedToByte.byteToSeed(finalSeed))
                passed[0] += 1;

            else
                Debug.Log("Test for seed size of: " + i + " failed.");
        }

        return passed;
    }

    // Similar to the above tests, but tests many possible bit sizes
    public int[] testSeedsVariableBitActions()
    {
        int[] passed = new int[2];

        for (int kBit = 1; kBit < 9; kBit++)
        {
            for (int i = (kBit * 5); i < 280; i += kBit)
            {
                List<int> hexList = new List<int>();
                for (int j = 0; j < i; j += kBit)
                {
                    hexList.Add(kBit);
                }

                byte[] byteHex = new byte[(i / 8) + 1];

                if (i % 8 == 0)
                    byteHex = new byte[i / 8];

                for (int j = 0; j < byteHex.Length; j++)
                {
                    byteHex[j] = 255;
                }

                byteHex[byteHex.Length - 1] = (byte)(Math.Pow(2, i % 8) - 1);
                byte[] finalSeed = conversionHelper(byteHex, hexList);

                passed[1] += 1;

                if (seedToByte.byteToSeed(byteHex) == seedToByte.byteToSeed(finalSeed))
                    passed[0] += 1;

                else
                {
                    Debug.Log("Test for seed size of: " + i + " and bit size of: " + kBit + " failed.");
                }
            }
        }
        return passed;
    }

    // Helper function that converts a byte array into actions and back into a byte array
    public byte[] conversionHelper(byte[] byteHex, List<int> hexList)
    {
        BitArray hexBits = seedToByte.byteToBits(byteHex);
        int[] hexActions = seedToByte.bitToActions(hexBits, hexList);
        byte[] finalSeed = SeedToByte.seedConverterUniversal(hexActions, hexList);

        return finalSeed;
    }

    // Similar to the above tests, but tests many possible bit sizes
    public int[] testSpecialException()
    {
        int[] passed = new int[2];
        passed[1] = 4;

        string seed4 = "FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF";
        string seed1 = "52BE1E391AF51C8794E37CD3FEC3054D1";
        string seed2 = "6C87DDE23A0F865E087A4DDF2A7A6077A";
        string seed3 = "D159F42FE7DEE8161FDEE3BEF50C63F93";

        int[] actions1 = seedToByte.getActions(seed1);
        int[] actions2 = seedToByte.getActions(seed2);
        int[] actions3 = seedToByte.getActions(seed3);
        int[] actions4 = seedToByte.getActions(seed4);

        string final1 = seedToByte.getSeed(actions1);
        string final2 = seedToByte.getSeed(actions2);
        string final3 = seedToByte.getSeed(actions3);
        string final4 = seedToByte.getSeed(actions4);

        if (seed1 == final1)
            passed[0] += 1;
        else
        {
            Debug.Log("Test for seed: " + seed1 + " failed. final: " + final1);
        }
        if (seed2 == final2)
            passed[0] += 1;
        else
        {
            Debug.Log("Test for seed: " + seed2 + " failed. final: " + final2);
        }
        if (seed3 == final3)
            passed[0] += 1;
        else
        {
            Debug.Log("Test for seed: " + seed3 + " failed. final: " + final3);
        }
        if (seed4 == final4)
            passed[0] += 1;
        else
        {
            Debug.Log("Test for seed: " + seed4 + " failed. final: " + final4);
        }

        return passed;
    }
}
