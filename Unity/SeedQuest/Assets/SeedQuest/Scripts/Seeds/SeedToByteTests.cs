using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeedToByteTests : MonoBehaviour {


    public string testSeed1 = "C5E3D45D341A";
    public string testSeed2 = "||||||||||||||||";
    public string testSeed3 = "825A";

    public string a1234 = "a1234";
    /*
    public string testReturnStr;
    public string testReturnStr2;
    public string testReturnStr3;

    public string testBitStr;
    public byte[] testByteArr;
    public byte[] testReturnBytes;
    public int[] testActionToDo;
    public BitArray testBitArr;
    public byte[] actionToBits;
    public byte[] actionToBitsVariant;
    */
    public List<int> actionList = new List<int>();

    private SeedToByte seedToByte = new SeedToByte();


	// Use this for initialization
	void Start () 
    {
        runAllTests();
	}
	
	// Update is called once per frame
	void Update () {
		
	}


    void runAllTests()
    {
        testByteBitConversion();
        //test108BitSeed();
        testMultipleSizeSeeds();
        //testFindLeadingBits();
        testSmallSeeds();
    }

    // Test to make sure everything works
    void testByteBitConversion()
    {
        // Just a test
        byte[] testByteArr = seedToByte.seedToByte(testSeed3);
        string testReturnStr = seedToByte.byteToSeed(testByteArr);
        BitArray testBitArr = seedToByte.byteToBits(testByteArr);
        byte[] testReturnBytes = seedToByte.bitToByte(testBitArr);
        string testReturnStr2 = seedToByte.byteToSeed(testReturnBytes);
        int[] testActionToDo = seedToByte.bitToActions(testBitArr, actionList);

        if (testSeed3 == testReturnStr)
            Debug.Log("Test for conversion from byte array to string passed");
        else
            Debug.Log("Test for conversion from byte array to string failed");

        if (seedToByte.byteToSeed(testByteArr) == seedToByte.byteToSeed(testReturnBytes))
            Debug.Log("Test for conversion from bit array to byte array passed");
        else
            Debug.Log("Test for conversion from bit array to byte array failed");
    }

    public void test108BitSeed()
    {
        byte[] testRunSeed = new byte[14];
        testRunSeed = OTPworker.randomSeedGenerator(testRunSeed);

        if (testRunSeed[13] > 15)
            testRunSeed[13] = (byte)((int)testRunSeed[13] % 7);

        List<int> tempList = SeedToByte.customList(3, 4, 2, 4, 4);

        BitArray seedBits = seedToByte.byteToBits(testRunSeed);
        int[] actions = seedToByte.bitToActions(seedBits, tempList);

        byte[] finalSeed = SeedToByte.seed108Converter(actions, tempList);

        //Debug.Log("Initial seed: " + seedToByte.byteToSeed(testRunSeed));
        //Debug.Log("Final  seed: " + seedToByte.byteToSeed(finalSeed));

        if (testRunSeed == finalSeed)
            Debug.Log("Test for converting 108 bit seed to action list and back passed");
        else
            Debug.Log("Test for converting 108 bit seed to action list and back failed");
    }

    public void testMultipleSizeSeeds()
    {
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

        Debug.Log("Initial seed: " + seedToByte.byteToSeed(testRunSeed));
        Debug.Log("Final  seed: " + seedToByte.byteToSeed(finalSeed1));

        Debug.Log("Initial seed: " + seedToByte.byteToSeed(testHexSeed));
        Debug.Log("Final  seed: " + seedToByte.byteToSeed(finalSeed2));

        if (seedToByte.byteToSeed(testRunSeed) == seedToByte.byteToSeed(finalSeed1))
            Debug.Log("Test 1 for converting 112 bit seed to action list and back passed");
        else
            Debug.Log("Test 1 for converting 112 bit seed to action list and back failed");

        if (seedToByte.byteToSeed(testHexSeed) == seedToByte.byteToSeed(finalSeed2))
            Debug.Log("Test 2 for converting 112 bit seed to action list and back passed");
        else
            Debug.Log("Test 2 for converting 112 bit seed to action list and back failed");


        testRunSeed = OTPworker.randomSeedGenerator(testRunSeed);

        if (testRunSeed[13] > 15)
            testRunSeed[13] = (byte)((int)testRunSeed[13] % 7);

        BitArray seedBits = seedToByte.byteToBits(testRunSeed);
        int[] actions3 = seedToByte.bitToActions(seedBits, tempList2);

        byte[] finalSeed3 = SeedToByte.seed108Converter(actions3, tempList2);

        Debug.Log("Initial seed: " + seedToByte.byteToSeed(testRunSeed));
        Debug.Log("Final  seed: " + seedToByte.byteToSeed(finalSeed3));

        if (seedToByte.byteToSeed(testRunSeed) == seedToByte.byteToSeed(finalSeed3))
            Debug.Log("Test for converting 108 bit seed to action list and back passed");
        else
            Debug.Log("Test for converting 108 bit seed to action list and back failed");

    }

    public void testSmallSeeds()
    {
        string testHex = "FFFF";
        byte[] testHexSeed = SeedToByte.HexStringToByteArray(testHex);
        List<int> tempList = SeedToByte.customList(6, 6, 4, 1, 1);
        BitArray seedBits = seedToByte.byteToBits(testHexSeed);
        int[] actions = seedToByte.bitToActions(seedBits, tempList);
        byte[] finalSeed = SeedToByte.seedConverterUniversal(actions, tempList);

        Debug.Log("Initial seed: " + seedToByte.byteToSeed(testHexSeed));
        Debug.Log("Final  seed: " + seedToByte.byteToSeed(finalSeed));

        if (seedToByte.byteToSeed(testHexSeed) == seedToByte.byteToSeed(finalSeed))
            Debug.Log("Test 2 for converting 16 bit seed to action list and back passed");
        else
            Debug.Log("Test 2 for converting 16 bit seed to action list and back failed");

    }

    public void testFindLeadingBits()
    {
        int[] leadingBits = new int[3];

        leadingBits = SeedToByte.findLeadingBitValue(2, 3, 15);
        if (leadingBits[0] == 3 && leadingBits[1] == 1 && leadingBits[2] == 1)
            Debug.Log("Leading bits test 1 passed");
        else
            Debug.Log("Leading bits test 1 failed");

        leadingBits = SeedToByte.findLeadingBitValue(2, 3, 14);
        if (leadingBits[0] == 3 && leadingBits[1] == 0 && leadingBits[2] == 1)
            Debug.Log("Leading bits test 2 passed");
        else
            Debug.Log("Leading bits test 2 failed");

        leadingBits = SeedToByte.findLeadingBitValue(2, 3, 9);
        if (leadingBits[0] == 0 && leadingBits[1] == 1 && leadingBits[2] == 1)
            Debug.Log("Leading bits test 3 passed");
        else
            Debug.Log("Leading bits test 3 failed");

        leadingBits = SeedToByte.findLeadingBitValue(2, 3, 1);
        if (leadingBits[0] == 0 && leadingBits[1] == 1 && leadingBits[2] == 1)
            Debug.Log("Leading bits test 4 passed");
        else
            Debug.Log("Leading bits test 4 failed");

        leadingBits = SeedToByte.findLeadingBitValue(3, 4, 15);
        if (leadingBits[0] == 7 && leadingBits[1] == 1 && leadingBits[2] == 1)
            Debug.Log("Leading bits test 5 passed");
        else
            Debug.Log("Leading bits test 5 failed");

        leadingBits = SeedToByte.findLeadingBitValue(3, 4, 14);
        if (leadingBits[0] == 7 && leadingBits[1] == 0 && leadingBits[2] == 1)
            Debug.Log("Leading bits test 6 passed");
        else
            Debug.Log("Leading bits test 6 failed");

        leadingBits = SeedToByte.findLeadingBitValue(3, 4, 9);
        if (leadingBits[0] == 4 && leadingBits[1] == 1 && leadingBits[2] == 1)
            Debug.Log("Leading bits test 7 passed");
        else
            Debug.Log("Leading bits test 7 failed");

        leadingBits = SeedToByte.findLeadingBitValue(3, 4, 1);
        if (leadingBits[0] == 0 && leadingBits[1] == 1 && leadingBits[2] == 1)
            Debug.Log("Leading bits test 8 passed");
        else
            Debug.Log("Leading bits test 8 failed");

        leadingBits = SeedToByte.findLeadingBitValue(4, 4, 15);
        if (leadingBits[0] == 15 && leadingBits[1] == 0 && leadingBits[2] == 0)
            Debug.Log("Leading bits test 9 passed");
        else
            Debug.Log("Leading bits test 9 failed");

        leadingBits = SeedToByte.findLeadingBitValue(4, 4, 14);
        if (leadingBits[0] == 14 && leadingBits[1] == 0 && leadingBits[2] == 0)
            Debug.Log("Leading bits test 10 passed");
        else
            Debug.Log("Leading bits test 10 failed");

        leadingBits = SeedToByte.findLeadingBitValue(4, 4, 9);
        if (leadingBits[0] == 9 && leadingBits[1] == 0 && leadingBits[2] == 0)
            Debug.Log("Leading bits test 11 passed");
        else
            Debug.Log("Leading bits test 11 failed");

        leadingBits = SeedToByte.findLeadingBitValue(4, 4, 1);
        if (leadingBits[0] == 1 && leadingBits[1] == 0 && leadingBits[2] == 0)
            Debug.Log("Leading bits test 12 passed");
        else
            Debug.Log("Leading bits test 12 failed");

        leadingBits = SeedToByte.findLeadingBitValue(2, 3, 0);
        if (leadingBits[0] == 0 && leadingBits[1] == 0 && leadingBits[2] == 0)
            Debug.Log("Leading bits test 13 passed");
        else
            Debug.Log("Leading bits test 13 failed");

        leadingBits = SeedToByte.findLeadingBitValue(0, 4, 9);
        if (leadingBits[0] == 0 && leadingBits[1] == 9 && leadingBits[2] == 4)
            Debug.Log("Leading bits test 14 passed");
        else
            Debug.Log("Leading bits test 14 failed");
    }

}
