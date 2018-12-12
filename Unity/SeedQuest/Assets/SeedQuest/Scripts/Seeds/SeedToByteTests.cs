using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeedToByteTests : MonoBehaviour {


    public string testSeed1 = "C5E3D45D341A";
    public string testSeed2 = "||||||||||||||||";
    public string testSeed3 = "825A";

    public string a1234 = "a1234";

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
    public List<int> actionList = new List<int>();

    private SeedToByte seedToByte = new SeedToByte();


	// Use this for initialization
	void Start () 
    {
        testRun();
        testRun2();
        testRun3();
        testRun4();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    // Test to make sure everything works
    void testRun()
    {
        // Just a test
        testByteArr = seedToByte.seedToByte(testSeed3);
        testReturnStr = seedToByte.byteToSeed(testByteArr);
        testBitArr = seedToByte.byteToBits(testByteArr);
        testReturnBytes = seedToByte.bitToByte(testBitArr);
        testReturnStr2 = seedToByte.byteToSeed(testReturnBytes);

        testActionToDo = seedToByte.bitToActions(testBitArr, actionList);

        actionToBits = seedToByte.actionConverter(testActionToDo, actionList);
        testReturnStr3 = seedToByte.byteToSeed(actionToBits);

        Debug.Log("Initial seed: " + testSeed3);
        Debug.Log("Final  seed: " + testReturnStr3);
    }

    public void testRun2()
    {
        byte[] testRunSeed = new byte[14];
        testRunSeed = OTPworker.randomSeedGenerator(testRunSeed);

        if (testRunSeed[13] > 15)
            testRunSeed[13] = (byte)((int)testRunSeed[13] % 7);

        List<int> tempList = SeedToByte.customList(3, 4, 2, 4, 4);

        BitArray seedBits = seedToByte.byteToBits(testRunSeed);
        int[] actions = seedToByte.bitToActions(seedBits, tempList);

        byte[] finalSeed = SeedToByte.seed108Converter(actions, tempList);

        Debug.Log("Initial seed: " + seedToByte.byteToSeed(testRunSeed));
        Debug.Log("Final  seed: " + seedToByte.byteToSeed(finalSeed));
    }

    public void testRun3()
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


        testRunSeed = OTPworker.randomSeedGenerator(testRunSeed);

        if (testRunSeed[13] > 15)
            testRunSeed[13] = (byte)((int)testRunSeed[13] % 7);

        BitArray seedBits = seedToByte.byteToBits(testRunSeed);
        int[] actions3 = seedToByte.bitToActions(seedBits, tempList2);

        byte[] finalSeed3 = SeedToByte.seed108Converter(actions3, tempList2);

        Debug.Log("Initial seed: " + seedToByte.byteToSeed(testRunSeed));
        Debug.Log("Final  seed: " + seedToByte.byteToSeed(finalSeed3));

    }

    public void testRun4()
    {
        Debug.Log("Test finding leading bit value: " + SeedToByte.findLeadingBitValue(2, 3, 15)[0]);
        Debug.Log("Test finding leading bit value: " + SeedToByte.findLeadingBitValue(2, 3, 14)[0]);
        Debug.Log("Test finding leading bit value: " + SeedToByte.findLeadingBitValue(2, 3, 9)[0]);
        Debug.Log("Test finding leading bit value: " + SeedToByte.findLeadingBitValue(2, 3, 1)[0]);

        Debug.Log("Test finding leading bit value: " + SeedToByte.findLeadingBitValue(3, 4, 15)[0]);
        Debug.Log("Test finding leading bit value: " + SeedToByte.findLeadingBitValue(3, 4, 14)[0]);
        Debug.Log("Test finding leading bit value: " + SeedToByte.findLeadingBitValue(3, 4, 9)[0]);
        Debug.Log("Test finding leading bit value: " + SeedToByte.findLeadingBitValue(3, 4, 1)[0]);

        Debug.Log("Test finding leading bit value: " + SeedToByte.findLeadingBitValue(4, 4, 15)[0]);
        Debug.Log("Test finding leading bit value: " + SeedToByte.findLeadingBitValue(4, 4, 14)[0]);
        Debug.Log("Test finding leading bit value: " + SeedToByte.findLeadingBitValue(4, 4, 9)[0]);
        Debug.Log("Test finding leading bit value: " + SeedToByte.findLeadingBitValue(4, 4, 1)[0]);
    }
}
