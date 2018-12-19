using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeedToByteTests : MonoBehaviour {


    private string testSeed1 = "C5E3D45D341A";
    private string testSeed2 = "AAAA";

    public List<int> actionList = new List<int>();

    private SeedToByte seedToByte = new SeedToByte();

	// Use this for initialization
	void Start () 
    {
        runAllTests();
	}
	
	// Update is called once per frame
	void Update () 
    {
		
	}

    // Run all tests at once
    void runAllTests()
    {
        int[] passed = new int[2];
        int[] test1 = testByteBitConversion();
        int[] test2 = testMultipleSizeSeeds();
        int[] test3 = testFindLeadingBits();
        int[] test4 = testBreakPoints();
        int[] test5 = testSmallSeeds();

        passed[0] = test1[0] + test2[0] + test3[0] + test4[0] + test5[0];
        passed[1] = test1[1] + test2[1] + test3[1] + test4[1] + test5[1];

        Debug.Log("Successfully passed " + passed[0] + " of " + passed[1] + " tests.");
    }

    // Test to make sure everything works
    public int[] testByteBitConversion()
    {
        int[] passed = new int[2];

        byte[] testByteArr = seedToByte.seedToByte(testSeed2);
        string testReturnStr = seedToByte.byteToSeed(testByteArr);
        BitArray testBitArr = seedToByte.byteToBits(testByteArr);
        byte[] testReturnBytes = seedToByte.bitToByte(testBitArr);
        string testReturnStr2 = seedToByte.byteToSeed(testReturnBytes);
        int[] testActionToDo = seedToByte.bitToActions(testBitArr, actionList);

        //Debug.Log("Test seed: " + testSeed2 + " return str: " + testReturnStr);

        passed[1] += 1;
        if (testSeed2 == testReturnStr)
        {
            //Debug.Log("Test for conversion from byte array to string passed");
            passed[0] += 1;
        }
        else
            Debug.Log("Test for conversion from byte array to string failed");

        passed[1] += 1;
        if (seedToByte.byteToSeed(testByteArr) == seedToByte.byteToSeed(testReturnBytes))
        {
            //Debug.Log("Test for conversion from bit array to byte array passed");
            passed[0] += 1;
        }
        else
            Debug.Log("Test for conversion from bit array to byte array failed");

        return passed;
    }

    public int[] testMultipleSizeSeeds()
    {
        int[] passed = new int[2];

        string testHex = "FFFFAAAAFFFFAAAAFFFFDDDDFFFF";
        //string testHex = "FFFFFFFFFFFFFFFFFFFFFFFFFFFF";

        byte[] testHexSeed = SeedToByte.HexStringToByteArray(testHex);
        byte[] testRunSeed = new byte[14];
        testRunSeed = OTPworker.randomSeedGenerator(testRunSeed);

        List<int> tempList1 = SeedToByte.customList(4, 4, 2, 4, 4);
        List<int> tempList2 = SeedToByte.customList(3, 4, 2, 4, 4);

        BitArray seedBits1 = seedToByte.byteToBits(testRunSeed);
        BitArray seedBits2 = seedToByte.byteToBits(testHexSeed);

        int[] actions1 = seedToByte.bitToActions(seedBits1, tempList1);
        int[] actions2 = seedToByte.bitToActions(seedBits2, tempList1);

        byte[] finalSeed1 = SeedToByte.seedConverterUniversal(actions1, tempList1);
        byte[] finalSeed2 = SeedToByte.seedConverterUniversal(actions2, tempList1);

        //Debug.Log("Initial seed: " + seedToByte.byteToSeed(testRunSeed));
        //Debug.Log("Final  seed: " + seedToByte.byteToSeed(finalSeed1));

        //Debug.Log("Initial seed: " + seedToByte.byteToSeed(testHexSeed));
        //Debug.Log("Final  seed: " + seedToByte.byteToSeed(finalSeed2));

        passed[1] += 1;
        if (seedToByte.byteToSeed(testRunSeed) == seedToByte.byteToSeed(finalSeed1))
        {
            //Debug.Log("Test 1 for converting 112 bit seed to action list and back passed");
            passed[0] += 1;
        }
        else
            Debug.Log("Test 1 for converting 112 bit seed to action list and back failed");

        passed[1] += 1;
        if (seedToByte.byteToSeed(testHexSeed) == seedToByte.byteToSeed(finalSeed2))
        {
            //Debug.Log("Test 2 for converting 112 bit seed to action list and back passed");
            passed[0] += 1;
        }
        else
            Debug.Log("Test 2 for converting 112 bit seed to action list and back failed");

        testHexSeed = new byte[14];
        testRunSeed = OTPworker.randomSeedGenerator(testRunSeed);

        if (testRunSeed[13] > 7)
            testRunSeed[13] = (byte)((int)testRunSeed[13] % 7);

        BitArray seedBits = seedToByte.byteToBits(testHexSeed);
        int[] actions3 = seedToByte.bitToActions(seedBits, tempList2);

        byte[] finalSeed3 = SeedToByte.seedConverterUniversal(actions3, tempList2);

        //Debug.Log("Initial seed: " + seedToByte.byteToSeed(testRunSeed));
        //Debug.Log("Final  seed: " + seedToByte.byteToSeed(finalSeed3));

        passed[1] += 1;
        if (seedToByte.byteToSeed(testHexSeed) == seedToByte.byteToSeed(finalSeed3))
        {
            //Debug.Log("Test for converting 108 bit seed to action list and back passed");
            passed[0] += 1;
        }
        else
            Debug.Log("Test for converting 108 bit seed to action list and back failed");

        return passed;
    }

    public int[] testSmallSeeds()
    {
        int[] passed = new int[2];

        string testHex = "FFFF";
        byte[] testHexSeed = SeedToByte.HexStringToByteArray(testHex);
        List<int> tempList = SeedToByte.customList(6, 6, 4, 1, 1);
        BitArray seedBits = seedToByte.byteToBits(testHexSeed);
        int[] actions = seedToByte.bitToActions(seedBits, tempList);
        byte[] finalSeed = SeedToByte.seedConverterUniversal(actions, tempList);

        //Debug.Log("Initial seed: " + seedToByte.byteToSeed(testHexSeed));
        //Debug.Log("Final  seed: " + seedToByte.byteToSeed(finalSeed));

        passed[1] += 1;
        if (seedToByte.byteToSeed(testHexSeed) == seedToByte.byteToSeed(finalSeed))
        {
            //Debug.Log("Test for converting 16 bit seed to action list and back passed");
            passed[0] += 1;
        }
        else
            Debug.Log("Test for converting 16 bit seed to action list and back failed");
        
        testHex = "FFFFFF";
        testHexSeed = SeedToByte.HexStringToByteArray(testHex);
        tempList = SeedToByte.customList(2, 3, 2, 2, 2);
        seedBits = seedToByte.byteToBits(testHexSeed);
        actions = seedToByte.bitToActions(seedBits, tempList);
        finalSeed = SeedToByte.seedConverterUniversal(actions, tempList);

        passed[1] += 1;
        if (seedToByte.byteToSeed(testHexSeed) == seedToByte.byteToSeed(finalSeed))
        {
            //Debug.Log("Test for converting 24 bit seed to action list and back passed");
            passed[0] += 1;
        }
        else
            Debug.Log("Test for converting 24 bit seed to action list and back failed");

        return passed;
    }

    public int[] testBreakPoints()
    {
        int[] passed = new int[2];

        string testHex = "FFFFAAAAFFFFAAAA";
        byte[] testHexSeed = SeedToByte.HexStringToByteArray(testHex);
        List<int> tempList = SeedToByte.customList(4, 8, 6, 2, 2);
        BitArray seedBits = seedToByte.byteToBits(testHexSeed);
        int[] actions = seedToByte.bitToActions(seedBits, tempList);
        byte[] finalSeed = SeedToByte.seedConverterUniversal(actions, tempList);

        passed[1] += 1;
        if (seedToByte.byteToSeed(testHexSeed) == seedToByte.byteToSeed(finalSeed))
        {
            //Debug.Log("Test for converting 64 bit seed to action list and back passed");
            passed[0] += 1;
        }
        else
            Debug.Log("Test for converting 64 bit seed to action list and back failed");

        testHex = "FFFFAAAAFFFFAAAAFFFFAAAAFFFFAAAA";
        testHexSeed = SeedToByte.HexStringToByteArray(testHex);
        tempList = SeedToByte.customList(4, 8, 6, 2, 4);
        seedBits = seedToByte.byteToBits(testHexSeed);
        actions = seedToByte.bitToActions(seedBits, tempList);
        finalSeed = SeedToByte.seedConverterUniversal(actions, tempList);

        passed[1] += 1;
        if (seedToByte.byteToSeed(testHexSeed) == seedToByte.byteToSeed(finalSeed))
        {
            //Debug.Log("Test for converting 128 bit seed to action list and back passed");
            passed[0] += 1;
        }
        else
            Debug.Log("Test for converting 128 bit seed to action list and back failed");

        return passed;
    }

    public int[] testFindLeadingBits()
    {
        int[] passed = new int[2];
        int[] leadingBits = new int[3];

        leadingBits = SeedToByte.findLeadingBitValue(2, 3, 15);
        passed[1] += 1;
        if (leadingBits[0] == 3 && leadingBits[1] == 1 && leadingBits[2] == 1)
        {
            //Debug.Log("Leading bits test 1 passed");
            passed[0] += 1;
        }
        else
            Debug.Log("Leading bits test 1 failed");

        leadingBits = SeedToByte.findLeadingBitValue(2, 3, 14);
        passed[1] += 1;
        if (leadingBits[0] == 3 && leadingBits[1] == 0 && leadingBits[2] == 1)
        {
            //Debug.Log("Leading bits test 2 passed");
            passed[0] += 1;
        }
        else
            Debug.Log("Leading bits test 2 failed");
        
        leadingBits = SeedToByte.findLeadingBitValue(2, 3, 9);
        passed[1] += 1;
        if (leadingBits[0] == 0 && leadingBits[1] == 1 && leadingBits[2] == 1)
        {
            //Debug.Log("Leading bits test 3 passed");
            passed[0] += 1;
        }
        else
            Debug.Log("Leading bits test 3 failed");

        leadingBits = SeedToByte.findLeadingBitValue(2, 3, 1);
        passed[1] += 1;
        if (leadingBits[0] == 0 && leadingBits[1] == 1 && leadingBits[2] == 1)
        {
            //Debug.Log("Leading bits test 4 passed");
            passed[0] += 1;
        }
        else
            Debug.Log("Leading bits test 4 failed");

        leadingBits = SeedToByte.findLeadingBitValue(3, 4, 15);
        passed[1] += 1;
        if (leadingBits[0] == 7 && leadingBits[1] == 1 && leadingBits[2] == 1)
        {
            //Debug.Log("Leading bits test 5 passed");
            passed[0] += 1;
        }
        else
            Debug.Log("Leading bits test 5 failed");

        leadingBits = SeedToByte.findLeadingBitValue(3, 4, 14);
        passed[1] += 1;
        if (leadingBits[0] == 7 && leadingBits[1] == 0 && leadingBits[2] == 1)
        {
            //Debug.Log("Leading bits test 6 passed"); 
            passed[0] += 1;
        }

        else
            Debug.Log("Leading bits test 6 failed");

        leadingBits = SeedToByte.findLeadingBitValue(3, 4, 9);
        passed[1] += 1;
        if (leadingBits[0] == 4 && leadingBits[1] == 1 && leadingBits[2] == 1)
        {
            //Debug.Log("Leading bits test 7 passed");
            passed[0] += 1;
        }
        else
            Debug.Log("Leading bits test 7 failed");

        leadingBits = SeedToByte.findLeadingBitValue(3, 4, 1);
        passed[1] += 1;
        if (leadingBits[0] == 0 && leadingBits[1] == 1 && leadingBits[2] == 1)
        {
            //Debug.Log("Leading bits test 8 passed");
            passed[0] += 1;
        }
        else
            Debug.Log("Leading bits test 8 failed");

        leadingBits = SeedToByte.findLeadingBitValue(4, 4, 15);
        passed[1] += 1;
        if (leadingBits[0] == 15 && leadingBits[1] == 0 && leadingBits[2] == 0)
        {
            //Debug.Log("Leading bits test 9 passed");
            passed[0] += 1;
        }
        else
            Debug.Log("Leading bits test 9 failed");

        leadingBits = SeedToByte.findLeadingBitValue(4, 4, 14);
        passed[1] += 1;
        if (leadingBits[0] == 14 && leadingBits[1] == 0 && leadingBits[2] == 0)
        {
            //Debug.Log("Leading bits test 10 passed");
            passed[0] += 1;
        }
        else
            Debug.Log("Leading bits test 10 failed");

        leadingBits = SeedToByte.findLeadingBitValue(4, 4, 9);
        passed[1] += 1;
        if (leadingBits[0] == 9 && leadingBits[1] == 0 && leadingBits[2] == 0)
        {
            //Debug.Log("Leading bits test 11 passed");
            passed[0] += 1;
        }
        else
            Debug.Log("Leading bits test 11 failed");

        leadingBits = SeedToByte.findLeadingBitValue(4, 4, 1);
        passed[1] += 1;
        if (leadingBits[0] == 1 && leadingBits[1] == 0 && leadingBits[2] == 0)
        {
            //Debug.Log("Leading bits test 12 passed");
            passed[0] += 1;
        }
        else
            Debug.Log("Leading bits test 12 failed");

        leadingBits = SeedToByte.findLeadingBitValue(2, 3, 0);
        passed[1] += 1;
        if (leadingBits[0] == 0 && leadingBits[1] == 0 && leadingBits[2] == 0)
        {
            //Debug.Log("Leading bits test 13 passed");
            passed[0] += 1;
        }
        else
            Debug.Log("Leading bits test 13 failed");

        leadingBits = SeedToByte.findLeadingBitValue(0, 4, 9);
        passed[1] += 1;
        if (leadingBits[0] == 0 && leadingBits[1] == 9 && leadingBits[2] == 4)
        {
            //Debug.Log("Leading bits test 14 passed");
            passed[0] += 1;
        }
        else
            Debug.Log("Leading bits test 14 failed");

        return passed;
    }

}
