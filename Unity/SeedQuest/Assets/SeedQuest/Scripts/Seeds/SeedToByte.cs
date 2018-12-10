using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using System;
using System.Collections.Specialized;


/*
 * The functions in this script can be used like this:
 * 
 * To get the int[] of actions to be performed from a seed:
 *  getActions(string inputStringName);
 * 
 * To get the seed from an int[] of actions:
 *  getSeed(int[] actionArray);
*/

public class SeedToByte : MonoBehaviour
{
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

    public static string inputSeed;
    public static string returnSeed;

    public static string seedBase58;
    public static string seedBase64;
    public static string seedBinary;

    public static byte[] inputBytes;
    public static byte[] returnBytes;
    public static int[] actionToDo;
    public int[] varSizeToDo;
    public static BitArray inputBits;

    // For reversing bits later
    public static byte[] BitReverseTable =
    {
            0x00, 0x80, 0x40, 0xc0, 0x20, 0xa0, 0x60, 0xe0,
            0x10, 0x90, 0x50, 0xd0, 0x30, 0xb0, 0x70, 0xf0,
            0x08, 0x88, 0x48, 0xc8, 0x28, 0xa8, 0x68, 0xe8,
            0x18, 0x98, 0x58, 0xd8, 0x38, 0xb8, 0x78, 0xf8,
            0x04, 0x84, 0x44, 0xc4, 0x24, 0xa4, 0x64, 0xe4,
            0x14, 0x94, 0x54, 0xd4, 0x34, 0xb4, 0x74, 0xf4,
            0x0c, 0x8c, 0x4c, 0xcc, 0x2c, 0xac, 0x6c, 0xec,
            0x1c, 0x9c, 0x5c, 0xdc, 0x3c, 0xbc, 0x7c, 0xfc,
            0x02, 0x82, 0x42, 0xc2, 0x22, 0xa2, 0x62, 0xe2,
            0x12, 0x92, 0x52, 0xd2, 0x32, 0xb2, 0x72, 0xf2,
            0x0a, 0x8a, 0x4a, 0xca, 0x2a, 0xaa, 0x6a, 0xea,
            0x1a, 0x9a, 0x5a, 0xda, 0x3a, 0xba, 0x7a, 0xfa,
            0x06, 0x86, 0x46, 0xc6, 0x26, 0xa6, 0x66, 0xe6,
            0x16, 0x96, 0x56, 0xd6, 0x36, 0xb6, 0x76, 0xf6,
            0x0e, 0x8e, 0x4e, 0xce, 0x2e, 0xae, 0x6e, 0xee,
            0x1e, 0x9e, 0x5e, 0xde, 0x3e, 0xbe, 0x7e, 0xfe,
            0x01, 0x81, 0x41, 0xc1, 0x21, 0xa1, 0x61, 0xe1,
            0x11, 0x91, 0x51, 0xd1, 0x31, 0xb1, 0x71, 0xf1,
            0x09, 0x89, 0x49, 0xc9, 0x29, 0xa9, 0x69, 0xe9,
            0x19, 0x99, 0x59, 0xd9, 0x39, 0xb9, 0x79, 0xf9,
            0x05, 0x85, 0x45, 0xc5, 0x25, 0xa5, 0x65, 0xe5,
            0x15, 0x95, 0x55, 0xd5, 0x35, 0xb5, 0x75, 0xf5,
            0x0d, 0x8d, 0x4d, 0xcd, 0x2d, 0xad, 0x6d, 0xed,
            0x1d, 0x9d, 0x5d, 0xdd, 0x3d, 0xbd, 0x7d, 0xfd,
            0x03, 0x83, 0x43, 0xc3, 0x23, 0xa3, 0x63, 0xe3,
            0x13, 0x93, 0x53, 0xd3, 0x33, 0xb3, 0x73, 0xf3,
            0x0b, 0x8b, 0x4b, 0xcb, 0x2b, 0xab, 0x6b, 0xeb,
            0x1b, 0x9b, 0x5b, 0xdb, 0x3b, 0xbb, 0x7b, 0xfb,
            0x07, 0x87, 0x47, 0xc7, 0x27, 0xa7, 0x67, 0xe7,
            0x17, 0x97, 0x57, 0xd7, 0x37, 0xb7, 0x77, 0xf7,
            0x0f, 0x8f, 0x4f, 0xcf, 0x2f, 0xaf, 0x6f, 0xef,
            0x1f, 0x9f, 0x5f, 0xdf, 0x3f, 0xbf, 0x7f, 0xff
    };

    public static string[] bitStrings =
    {
        "00000000", "00000001", "00000010", "00000011", "00000100",
        "00000101", "00000110", "00000111", "00001000", "00001001",
        "00001010", "00001011", "00001100", "00001101", "00001110", 
        "00001111", "00010000", "00010001"
    };

    void Start()
    {
        //testRun5();
        //testRun();
        //testRun2();
        //testRun3();
        testRun4();
    }

    // Test to make sure everything works
    void testRun()
    {
        // Just a test
        testByteArr = seedToByte(testSeed3);
        testReturnStr = byteToSeed(testByteArr);
        testBitArr = byteToBits(testByteArr);
        testReturnBytes = bitToByte(testBitArr);
        testReturnStr2 = byteToSeed(testReturnBytes);

        testActionToDo = bitToActions(testBitArr, actionList);

        actionToBits = actionConverter(testActionToDo, actionList);
        testReturnStr3 = byteToSeed(actionToBits);

        Debug.Log("Initial seed: " + testSeed3);
        Debug.Log("Final  seed: " + testReturnStr3);
    }

    public void testRun2()
    {
        byte[] testRunSeed = new byte[14];
        testRunSeed = OTPworker.randomSeedGenerator(testRunSeed);

        if (testRunSeed[13] > 15)
            testRunSeed[13] = (byte)( (int)testRunSeed[13] % 7);

        List<int> tempList = customList(3, 4, 2, 4, 4);

        BitArray seedBits = byteToBits(testRunSeed);
        int[] actions = bitToActions(seedBits, tempList);

        byte[] finalSeed = seed108Converter(actions, tempList);

        Debug.Log("Initial seed: " + byteToSeed(testRunSeed));
        Debug.Log("Final  seed: " + byteToSeed(finalSeed));
    }

    public void testRun3()
    {
        
        string testHex = "FFFFAAAAFFFFAAAAFFFFDDDDFFFF";
        byte[] testRunSeed = HexStringToByteArray(testHex);
        //byte[] testRunSeed = new byte[14];
        //testRunSeed = OTPworker.randomSeedGenerator(testRunSeed);

        List<int> tempList = customList(4, 4, 2, 4, 4);

        BitArray seedBits = byteToBits(testRunSeed);
        int[] actions = bitToActions(seedBits, tempList);

        byte[] finalSeed = seedConverterMod64(actions, tempList);


        Debug.Log("Initial seed: " + byteToSeed(testRunSeed));
        Debug.Log("Final  seed: " + byteToSeed(finalSeed));

    }

    public void testRun4()
    {
        //string testHex = "FFFFAAAAFFFFAAAAFFFFDDDDFFFF";
        //string testHex = "FFFFAAAAFFFFAAAAFFFFDDDDFFFF";
        string testHex = "FFFFFFFFFFFFFFFFFFFFFFFFFFFF";
        //string testHex = "EEEEEEEEEEEEEEEEDDDDEEEEEEEE";

        byte[] testHexSeed = HexStringToByteArray(testHex);
        byte[] testRunSeed = new byte[14];
        testRunSeed = OTPworker.randomSeedGenerator(testRunSeed);

        List<int> tempList1 = customList(4, 4, 2, 4, 4);
        //List<int> tempList2 = customList(3, 4, 2, 4, 4);

        BitArray seedBits1 = byteToBits(testRunSeed);
        BitArray seedBits2 = byteToBits(testHexSeed);

        //byte[] hexByte = bitToByte(seedBits2);
        //Debug.Log("Bits returned to string: " + byteToSeed(hexByte));

        int[] actions1 = bitToActions(seedBits1, tempList1);
        int[] actions2 = bitToActions(seedBits2, tempList1);

        byte[] finalSeed1 = seedConverterUniversal(actions1, tempList1);
        byte[] finalSeed2 = seedConverterUniversal(actions2, tempList1);

        Debug.Log("Initial seed: " + byteToSeed(testRunSeed));
        Debug.Log("Final  seed: " + byteToSeed(finalSeed1));

        Debug.Log("Initial seed: " + byteToSeed(testHexSeed));
        Debug.Log("Final  seed: " + byteToSeed(finalSeed2));

        //seedConverterUniversal
    }

    public void testRun5()
    {
        Debug.Log("Test finding leading bit value: " + findLeadingBitValue(2, 3, 15)[0]);
        Debug.Log("Test finding leading bit value: " + findLeadingBitValue(2, 3, 14)[0]);
        Debug.Log("Test finding leading bit value: " + findLeadingBitValue(2, 3, 9)[0]);
        Debug.Log("Test finding leading bit value: " + findLeadingBitValue(2, 3, 1)[0]);

        Debug.Log("Test finding leading bit value: " + findLeadingBitValue(3, 4, 15)[0]);
        Debug.Log("Test finding leading bit value: " + findLeadingBitValue(3, 4, 14)[0]);
        Debug.Log("Test finding leading bit value: " + findLeadingBitValue(3, 4, 9)[0]);
        Debug.Log("Test finding leading bit value: " + findLeadingBitValue(3, 4, 1)[0]);

        Debug.Log("Test finding leading bit value: " + findLeadingBitValue(4, 4, 15)[0]);
        Debug.Log("Test finding leading bit value: " + findLeadingBitValue(4, 4, 14)[0]);
        Debug.Log("Test finding leading bit value: " + findLeadingBitValue(4, 4, 9)[0]);
        Debug.Log("Test finding leading bit value: " + findLeadingBitValue(4, 4, 1)[0]);

    }

    // Take string for input, get the to-do list of actions
    public int[] getActions(string inputStr)
    {
        inputSeed = inputStr;
        inputBytes = seedToByte(inputSeed);
        inputBits = byteToBits(inputBytes);
        actionToDo = bitToActions(inputBits, actionList);
        int[] returnActions = actionToDo;
        return returnActions;
    }

    // Same as getActions, but for the 108 bit seed, does not use global variables
    public int[] getActions108(string inputStr)
    {
        byte[] bytes108 = seedToByte(inputStr);
        BitArray bits108 = byteToBits(bytes108);
        List<int> tempList = customList(3, 4, 2, 4, 4);
        Debug.Log("Byte count: " + bytes108.Length + " Bits count: " + bits108.Length);
        if (bytes108.Length > 14)
            Debug.Log("Warning! Seed is longer than 108 bits!");
        int[] returnActions = bitToActions(bits108, tempList);
        return returnActions;
    }

    // Take string for input, get the to-do list of actions
    public int[] getActionsFromBytes(byte[] inputBytes)
    {
        actionList = listBuilder();
        BitArray inputBits2 = byteToBits(inputBytes);
        int[] actionToDo2 = bitToActions(inputBits2, actionList);
        return actionToDo2;
    }

    // Get the return seed from a list of actions
    public string getSeed(int[] actionsPerformed)
    {
        returnBytes = actionConverter(actionsPerformed, actionList);
        string convertedSeed = byteToSeed(returnBytes);
        return convertedSeed;
    }

    // Get the return seed using 108 bits, does not use global variables
    public string getSeed108(int[] actionsPerformed)
    {
        List<int> tempList = customList(3, 4, 2, 4, 4);
        byte[] bytes108 = seed108Converter(actionsPerformed, tempList);
        string convertedSeed = byteToSeed(bytes108);
        return convertedSeed;
    }

    //  Convert string to byte array
    public byte[] seedToByte(string seedString)
    {
        byte[] seedByte = HexStringToByteArray(seedString);
        return seedByte;
    }

    // Convert byte array back to string
    public string byteToSeed(byte[] bytes)
    {
        string returnStr = ByteArrayToHex(bytes);
        return returnStr;
    }

    // Convert byte array to bit array
    public BitArray byteToBits(byte[] bytes)
    {
        var returnBits = new BitArray(bytes);
        return returnBits;
    }

    // Convert bit array to byte array
    public byte[] bitToByte(BitArray bits)
    {
        byte[] returnArr;
        returnArr = BitArrayToByteArray(bits);
        return returnArr;
    }

    // Convert bit array to byte array
    public byte[] BitArrayToByteArray(BitArray bits)
    {
        byte[] ret = new byte[(bits.Length - 1) / 8 + 1];
        bits.CopyTo(ret, 0);
        return ret;
    }

    // Convert hex string to byte array
    public static byte[] HexStringToByteArray(string hex)
    {
        if (hex.Length % 2 == 1)
            throw new Exception("The binary key cannot have an odd number of digits");

        byte[] bytes = new byte[hex.Length >> 1];

        for (int i = 0; i < hex.Length >> 1; ++i)
        {
            bytes[i] = (byte)((GetHexVal(hex[i << 1]) << 4) + (GetHexVal(hex[(i << 1) + 1])));
        }

        return bytes;
    }

    // Get hex value from a char
    public static int GetHexVal(char hex)
    {
        int val = (int)hex;
        return val - (val < 58 ? 48 : (val < 97 ? 55 : 87));
    }

    // Convert the byte array to hexidecimal
    public static string ByteArrayToHex(byte[] bytes)
    {
        return BitConverter.ToString(bytes).Replace("-", "");
    }

    // Convert the byte array to a binary string
    public static string ByteArrayToBinary(byte[] bytes)
    {
        string returnString = "";

        for (int i = 0; i < bytes.Length; i++)
        {
            returnString += Convert.ToString(bytes[i], 2).PadLeft(8, '0');
        }

        return returnString;
    }

    // Convert a byte array into a base58 string
    public static string ByteArrayToBase58(byte[] bytes)
    {
        string base58Digits = "123456789ABCDEFGHJKLMNPQRSTUVWXYZabcdefghijkmnopqrstuvwxyz";
        string returnString = "";

        ulong bytesInt = 0;
        for (int i = 0; i < bytes.Length; i++)
        {
            bytesInt = bytesInt * 256 + bytes[i];
        }

        while (bytesInt > 0)
        {
            int remainder = (int)(bytesInt % 58);
            bytesInt /= 58;
            returnString = base58Digits[remainder] + returnString;
        }

        for (int i = 0; i < bytes.Length && bytes[i] == 0; i++)
        {
            returnString = '1' + returnString;
        }

        return returnString;
    }

    // Convert a byte array to base64
    public static string ByteArrayToBase64(byte[] bytes)
    {
        string returnString = "";
        returnString = Convert.ToBase64String(bytes);
        return returnString;
    }

    // Construct the list of how many bits represent which parts of the Path to take
    public static List<int> listBuilder()
    {
        int numLocationBits = SeedManager.SiteBits;
        int numSpotBits = SeedManager.SpotBits;
        int numActionBits = SeedManager.ActionBits;
        int numActions = SeedManager.ActionCount;
        int numTotalLocations = SeedManager.SiteCount;

        List<int> newList = new List<int>();

        for (int j = 0; j < numTotalLocations; j++)
        {
            newList.Add(numLocationBits);

            for (int i = 0; i < numActions; i++)
            {
                newList.Add(numSpotBits);
                newList.Add(numActionBits);
            }
        }
        //Debug.Log("Total: " + newList.Count + " Loc: " + newList[0] + " Spot: " + newList[1] + " Act: " + newList[2]);

        return newList;
    }

    // Makes a list without using the variables in SeedManager
    public static List<int> customList(int numLocBit, int numSpotBit, int numActBit, int numAct, int numLoc, int trailingZeros = 0)
    {
        List<int> newList = new List<int>();

        for (int j = 0; j < numLoc; j++)
        {
            newList.Add(numLocBit);

            for (int i = 0; i < numAct; i++)
            {
                newList.Add(numSpotBit);
                newList.Add(numActBit);
            }
        }

        if (trailingZeros > 0)
        {
            for (int i = 0; i <= trailingZeros; i++)
                newList.Add(0);
        }
        //Debug.Log("Total: " + newList.Count + " Loc: " + newList[0] + " Spot: " + newList[1] + " Act: " + newList[2]);

        return newList;
    }

    // Convert bit array to int array representing the actions the player should take
    public int[] bitToActions(BitArray bits, List<int> varList)
    {
        if (varList.Count == 0)
            varList = listBuilder();

        int[] actionValues = new int[varList.Count];
        int value = 0;
        int valueIndex = 0;
        int locator = 0;
        int writeIndex = 0;

        if (bits.Length > 128)
        {
            Debug.Log("Error: Provided seed is greater than 128 bits.");
            return actionValues;
        }

        // NOTE TO SELF: Need to account for if the writeIndex is > varlist.length

        for (int i = 0; i < bits.Length; i++)
        {
            if (writeIndex >= (varList.Count))
                Debug.Log("Warning: more bits in bitarray than in the list");
            else if (bits[i])
            {
                // Yes, I know this is reading the bits in reverse, this is intentional, manager wanted it done this way
                    int bitValue = varList[writeIndex] - (valueIndex + 1);
                    value += Convert.ToInt32(Math.Pow(2, bitValue));
            }
            if (writeIndex > (varList.Count - 1))
                Debug.Log("Warning: more bits in bitarray than in the list");
            else if (locator == (varList[writeIndex] - 1))
            {
                // Store the location/spot/action
                actionValues[writeIndex] = value;
                writeIndex += 1;
                value = 0;
                valueIndex = 0;
                locator = 0;
            }
            else
            {
                valueIndex += 1;
                locator += 1;
            }
        }
        return actionValues;
    }

    // Takes the list of actions, converts it back into bytes
    public byte[] actionConverter(int[] actions, List<int> varList)
    {
        if (varList.Count == 0)
            varList = listBuilder();

        var actionBits = new BitArray(128);
        ulong path1 = 0;
        ulong path2 = 0;
        int[] tempArray = new int[36];

        int totalBits = 0;
        for (int i = 0; i < varList.Count; i++)
        {
            totalBits += varList[i];
        }

        if (totalBits < 64)
        {
            // Convert action ints into two ulong ints
            for (int i = 0; i < actions.Length; i++)
            {
                path1 += (ulong)actions[i];
                if (i < actions.Length - 1)
                    path1 = path1 << varList[i + 1];
            }
            path1 = path1 << (64 - totalBits);
        }
        else
        {
            for (int i = 0; i < 18; i++)
            {
                path1 += (ulong)actions[i];
                path2 += (ulong)actions[i + 18];
                if (i < 17)
                {
                    path1 = path1 << varList[i + 1];
                    path2 = path2 << varList[i + 19];
                    //Debug.Log("Shift 1: " + (i + 1) + " Shift 2: " + (i + 19));
                }
            }
        }

        // Convert ulong ints with actions into byte arrays
        byte[] bytes1 = BitConverter.GetBytes(path1);
        byte[] bytes2 = BitConverter.GetBytes(path2);

        // Reverse the endian of the bytes (yes, this is necessary to get the seed out properly)
        //  Yes, I know we are reversing the bits to get the actions,
        //  then reversing them again when extracting the bits.
        //  The higer-ups wanted me to do it this way.
        for (int i = 0; i < bytes1.Length / 2; i++)
        {
            byte tmp = bytes1[i];
            bytes1[i] = bytes1[bytes1.Length - i - 1];
            bytes1[bytes1.Length - i - 1] = tmp;
        }
        for (int i = 0; i < bytes2.Length / 2; i++)
        {
            byte tmp = bytes2[i];
            bytes2[i] = bytes2[bytes2.Length - i - 1];
            bytes2[bytes2.Length - i - 1] = tmp;
        }

        // Concatenate the bytearrays together into one
        byte[] bytes3 = new byte[bytes1.Length + bytes2.Length];
        System.Buffer.BlockCopy(bytes1, 0, bytes3, 0, bytes1.Length);
        System.Buffer.BlockCopy(bytes2, 0, bytes3, bytes1.Length, bytes2.Length);

        // Reverse the order of the bits within each byte (yes, this is also necessary)
        for (int i = 0; i < bytes3.Length; i++)
        {
            bytes3[i] = ReverseWithLookupTable(bytes3[i]);
        }

        return bytes3;
    }

    // Reverse the order of bits
    // This method is used since it's faster than other bit-reversal methods
    public static byte ReverseWithLookupTable(byte toReverse)
    {
        return BitReverseTable[toReverse];
    }

    // Takes the list of actions, converts it back into bytes, only works for 108 bit seeds
    public static byte[] seed108Converter(int[] actions, List<int> varList)
    {
        // The action list passed to this function must be the right size
        if (varList.Count == 0)
            varList = listBuilder();

        //var actionBits = new BitArray(size); // size = 128 in other function
        int[] tempArray = new int[36];

        int totalBits = 0;
        for (int i = 0; i < varList.Count; i++)
        {
            totalBits += varList[i];
        }

        byte[] bytesFin = new byte[0];
        int problem = 0;

        //Debug.Log("Total bit count: " + totalBits);

        if (totalBits % 8 != 0)
            Debug.Log("Warning! Bits not divisible by 8 - does not divide evenly into bytes!");

        if (actions.Length != varList.Count)
            Debug.Log("Warning! Actions and list are mismatched! They are not the same size!");

        if (totalBits < 64)
        {
            ulong path = 0;
            //Debug.Log("Actions.Length: " + actions.Length + " List length: " + varList.Count);
            for (int i = 0; i < varList.Count; i++)
            {
                if (i < actions.Length)
                    path += (ulong)actions[i];
                if (i < (varList.Count - 1))
                    path = path << varList[i + 1];
            }

            path = path << (64 - totalBits);
            byte[] bytesPath = BitConverter.GetBytes(path);
            Debug.Log("path int: " + path);

            bytesFin = new byte[bytesPath.Length];
            System.Buffer.BlockCopy(bytesPath, 0, bytesFin, 0, bytesPath.Length);
        }
        else
        {
            //Debug.Log("Actions.Length: " + actions.Length + " List.Count" + varList.Count);

            int modBits = totalBits % 64;
            int numLongs = totalBits / 64;
            int numShifts = 0;
            int remainder = 0;
            int numTraverse = 0;
            ulong path = 0;

            if (modBits > 0)
                numLongs += 1;

            for (int i = 0; i < numLongs; i++)
            {
                path = 0;
                numShifts = varList[numTraverse] + remainder;
                //Debug.Log("Value of traverse int: " + numTraverse);

                while (numShifts < 64)
                {
                    //Debug.Log("Numtraverse: " + numTraverse + " varlist length: " + varList.Count);
                    if (varList.Count <= numTraverse + 1)
                    {
                        //Debug.Log("Ran out of bits in the list..." + (totalBits % 8));
                        path += (ulong)actions[numTraverse];
                        path = path << (64 - (numShifts - (remainder * 2)));
                        if (actions.Length < numTraverse - 1)
                        {
                            path += (ulong)actions[numTraverse + 1];
                        }
                        numShifts = 65;
                    }
                    else if (actions.Length < numTraverse)
                    {
                        //Debug.Log("Ran out of actions...");
                        path = path << (64 - (numShifts - (remainder * 2)));
                        numShifts = 65;
                    }
                    else if (numShifts + varList[numTraverse + 1] + remainder > 64)
                    {
                        //Debug.Log("Numshifts: " + numShifts + " Final val of array: " + actions[numTraverse]);
                        //Debug.Log("Second to last int: " + actions[numTraverse-1] + " Next int: " + actions[numTraverse + 1]);
                        remainder = numShifts + varList[numTraverse + 1] - 64;

                        path += (ulong)actions[numTraverse];
                        path = path << (64 - numShifts);

                        if (actions[numTraverse + 1] > 8)
                        {
                            problem += 1;
                        }
                        numShifts = 65;
                    }
                    else
                    {
                        path += (ulong)actions[numTraverse];
                        path = path << varList[numTraverse + 1];
                        numShifts += varList[numTraverse + 1];
                        numTraverse++;
                    }
                    //Debug.Log("path int: " + path + " numShifts: " + numShifts);
                }

                byte[] bytesPath = BitConverter.GetBytes(path);
                byte[] bytesTemp = new byte[bytesPath.Length + bytesFin.Length];

                // Reverse the endian of the bytes (yes, this is necessary to get the seed out properly)
                for (int j = 0; j < ( bytesPath.Length / 2 ); j++)
                {
                    byte tmp = bytesPath[j];
                    bytesPath[j] = bytesPath[bytesPath.Length - j - 1];
                    bytesPath[bytesPath.Length - j - 1] = tmp;
                }

                System.Buffer.BlockCopy(bytesFin, 0, bytesTemp, 0, bytesFin.Length);
                System.Buffer.BlockCopy(bytesPath, 0, bytesTemp, bytesFin.Length, bytesPath.Length);

                bytesFin = new byte[bytesTemp.Length];
                System.Buffer.BlockCopy(bytesTemp, 0, bytesFin, 0, bytesTemp.Length);
            }
        }

        // Reverse the order of the bits within each byte (yes, this is also necessary)
        for (int i = 0; i < bytesFin.Length; i++)
        {
            bytesFin[i] = ReverseWithLookupTable(bytesFin[i]);
        }

        if (totalBits < 128)
        {
            byte[] bytesTemp = new byte[bytesFin.Length - ( (128 - totalBits) / 8 ) ];
            System.Buffer.BlockCopy(bytesFin, 0, bytesTemp, 0, bytesTemp.Length);
            bytesFin = bytesTemp;
        }
        if (problem > 0)
        {
            //Debug.Log("Solving problem... ");
            bytesFin[7] += 128;
        }

        return bytesFin;
    }

    // Takes the list of actions, converts it back into bytes, only works for seeds 
    //  where the bits divide evenly into chunks of 64 bits
    public static byte[] seedConverterMod64(int[] actions, List<int> varList)
    {
        // The action list passed to this function must be the right size
        if (varList.Count == 0)
            varList = listBuilder();

        int[] tempArray = new int[36];
        byte[] bytesFin = new byte[0];
        int totalBits = 0;

        for (int i = 0; i < varList.Count; i++)
        {
            totalBits += varList[i];
        }

        if (totalBits % 8 != 0)
            Debug.Log("Warning! Bits not divisible by 8 - does not divide evenly into bytes!");

        if (actions.Length != varList.Count)
            Debug.Log("Warning! Actions and list are mismatched! They are not the same size!");

        if (totalBits < 64)
        {
            ulong path = 0;
            for (int i = 0; i < varList.Count; i++)
            {
                if (i < actions.Length)
                    path += (ulong)actions[i];
                if (i < (varList.Count - 1))
                    path = path << varList[i + 1];
            }

            path = path << (64 - totalBits);
            byte[] bytesPath = BitConverter.GetBytes(path);
            Debug.Log("path int: " + path);

            bytesFin = new byte[bytesPath.Length];
            System.Buffer.BlockCopy(bytesPath, 0, bytesFin, 0, bytesPath.Length);
        }
        else
        {
            int modBits = totalBits % 64;
            int numLongs = totalBits / 64;
            int break64 = 0;
            int falseBreak = 0;
            int counter = 0;
            int numTraverse = 0;
            int numShifts = 0;
            ulong path = 0;

            if (modBits > 0)
                numLongs += 1;

            for (int i = 0; i < varList.Count; i++)
            {
                counter += varList[i];
                if (counter == 64)
                    break64 = i;
                else if (counter > 64)
                    falseBreak = i;
            }

            if (break64 == 0)
                Debug.Log("Warning! There does not appear to be a clean break in number of bytes for this action list!");

            //Debug.Log("Total actions: " + actions.Length + " Total bit list: " + varList.Count);
            for (int i = 0; i < numLongs; i++)
            {
                path = 0;
                numShifts = 0;//varList[numTraverse];
                //Debug.Log("New uint64 " + i);
                for (int j = 0; j < break64; j++)
                {
                    if (numTraverse < actions.Length)
                    {
                        //Debug.Log("Break64: " + break64 + " current iteration: " + (numTraverse) + " Value: " + actions[numTraverse]);
                        path += (ulong)actions[numTraverse];
                    }
                    if ((numTraverse + 1) < varList.Count )//&& numTraverse != break64-1)
                    {
                        //Debug.Log("Shifting: " + (numTraverse + 1) + " By: " + varList[numTraverse + 1]);
                        path = path << varList[numTraverse + 1];
                        numShifts += varList[numTraverse + 1];
                    }
                    else if(numTraverse == varList.Count-1)
                    {
                        //Debug.Log("Extra final shift " + (64-numShifts) );
                        path = path << (64 - numShifts);
                    }
                    else
                        //Debug.Log("Did not shift: " + (numTraverse));
                    if (j+1 == break64 && numTraverse+1 < actions.Length)
                    {
                        path += (ulong)actions[numTraverse+1];
                    }

                    numTraverse++;
                }

                //Debug.Log("Path int: " + path + " Num shifts: " + numShifts);

                byte[] bytesPath = BitConverter.GetBytes(path);
                byte[] bytesTemp = new byte[bytesPath.Length + bytesFin.Length];

                //Debug.Log("Path to hex: " + ByteArrayToHex(bytesPath));

                // Reverse the endian of the bytes (yes, this is necessary to get the seed out properly)
                for (int j = 0; j < (bytesPath.Length / 2); j++)
                {
                    byte tmp = bytesPath[j];
                    bytesPath[j] = bytesPath[bytesPath.Length - j - 1];
                    bytesPath[bytesPath.Length - j - 1] = tmp;
                }
                //Debug.Log("Path to hex: " + ByteArrayToHex(bytesPath));

                System.Buffer.BlockCopy(bytesFin, 0, bytesTemp, 0, bytesFin.Length);
                System.Buffer.BlockCopy(bytesPath, 0, bytesTemp, bytesFin.Length, bytesPath.Length);

                bytesFin = new byte[bytesTemp.Length];
                System.Buffer.BlockCopy(bytesTemp, 0, bytesFin, 0, bytesTemp.Length);
            }
        }

        // Reverse the order of the bits within each byte (yes, this is also necessary)
        for (int i = 0; i < bytesFin.Length; i++)
        {
            bytesFin[i] = ReverseWithLookupTable(bytesFin[i]);
        }

        if (totalBits < 128)
        {
            byte[] bytesTemp = new byte[bytesFin.Length - ((128 - totalBits) / 8)];
            System.Buffer.BlockCopy(bytesFin, 0, bytesTemp, 0, bytesTemp.Length);
            bytesFin = bytesTemp;
        }

        return bytesFin;
    }

    // Takes the list of actions, converts it back into bytes, should
    //  work for seeds of any bit size and configuration
    public static byte[] seedConverterUniversal(int[] actions, List<int> varList)
    {
        // The action list passed to this function must be the right size
        if (varList.Count == 0)
            varList = listBuilder();

        int[] tempArray = new int[36];
        byte[] bytesFin = new byte[0];
        int totalBits = 0;

        for (int i = 0; i < varList.Count; i++)
        {
            totalBits += varList[i];
        }

        if (actions.Length != varList.Count)
            Debug.Log("Warning! Actions and list are mismatched! They are not the same size!");

        if (totalBits < 64)
        {
            ulong path = 0;
            for (int i = 0; i < varList.Count; i++)
            {
                if (i < actions.Length)
                    path += (ulong)actions[i];
                if (i < (varList.Count - 1))
                    path = path << varList[i + 1];
            }

            path = path << (64 - totalBits);
            byte[] bytesPath = BitConverter.GetBytes(path);
            Debug.Log("path int: " + path);

            bytesFin = new byte[bytesPath.Length];
            System.Buffer.BlockCopy(bytesPath, 0, bytesFin, 0, bytesPath.Length);
        }
        else
        {
            int modBits = totalBits % 64;
            int numLongs = totalBits / 64;
            int numTraverse = 0;
            int numShifts = 0;
            int count = 0;
            int remainder = 0;
            int remainderBits = 0;
            ulong path = 0;

            if (modBits > 0)
                numLongs += 1;

            int[] breakPoints = new int[numLongs];

            for (int i = 0; i < breakPoints.Length; i++)
            {
                //find the breakpoints
                count += varList[i];
                if (count > 64)
                {
                    breakPoints[0] = i;
                    Debug.Log("Breakpoint: " + inputSeed + " bits: " + count);
                }
            }

            count = 0;

            //Debug.Log("Total actions: " + actions.Length + " Total bit list: " + varList.Count);
            for (int i = 0; i < numLongs; i++)
            {
                path = 0;
                numShifts = 0;

                if (i > 0)
                {
                    if (remainder > 0)
                    {
                        path += (ulong)remainder;
                        path = path << varList[numTraverse];
                        numShifts += varList[numTraverse+1];
                        numTraverse += 1;
                        Debug.Log("Adding remainder to new path int: " + remainder + " Shift: " + varList[numTraverse]);
                    }
                }

                //Debug.Log("New uint64 " + i);
                //for (int j = 0; j < 64; j++)
                while (numShifts < 64)
                {
                    if (numTraverse < actions.Length)
                    {
                        Debug.Log("Adding to 'path' " + numTraverse);
                        path += (ulong)actions[numTraverse];
                    }
                    if (numTraverse + 1 >= varList.Count)
                    {
                        Debug.Log("End of the list");
                        path = path << (64 - numShifts);
                        numShifts += 64;
                    }
                    else if (numShifts + varList[numTraverse + 1] > 64)
                    {
                        Debug.Log("Finding partial bit value: " + numTraverse + " bits: " + varList[numTraverse + 1]);
                        Debug.Log("Num shifts - 64 : " + (64 - numShifts));
                        int[] partialAction = findLeadingBitValue((64 - numShifts), varList[numTraverse + 1], actions[numTraverse+1]);
                        remainder = partialAction[1];
                        remainderBits = varList[numTraverse + 1] - (64 - numShifts);
                        path = path << (64 - numShifts);
                        path += (ulong)partialAction[0];
                        numShifts += 64;
                    }
                    else if ((numTraverse + 1) < varList.Count)
                    {
                        Debug.Log("Shifting: " + (numTraverse) + " By: " + varList[numTraverse + 1]);
                        //Debug.Log("Shifting by: " + varList[numTraverse + 1]);

                        path = path << varList[numTraverse + 1];
                        numShifts += varList[numTraverse + 1];
                    }
                    else if (numTraverse == varList.Count - 1)
                    {
                        Debug.Log("Extra final shift " + (64-numShifts) );
                        path = path << (64 - numShifts);
                        numShifts += 64;
                    }

                    count++;
                    numTraverse++;
                    if (count > 1000)
                    {
                        Debug.Log("While loop problem - count > 1000");
                        break;
                    }
                }

                //Debug.Log("Path int: " + path + " Num shifts: " + (numShifts - 64));

                byte[] bytesPath = BitConverter.GetBytes(path);
                byte[] bytesTemp = new byte[bytesPath.Length + bytesFin.Length];

                Debug.Log("Path to hex: " + ByteArrayToHex(bytesPath));

                // Reverse the endian of the bytes (yes, this is necessary to get the seed out properly)
                for (int j = 0; j < (bytesPath.Length / 2); j++)
                {
                    byte tmp = bytesPath[j];
                    bytesPath[j] = bytesPath[bytesPath.Length - j - 1];
                    bytesPath[bytesPath.Length - j - 1] = tmp;
                }
                //Debug.Log("Path to hex: " + ByteArrayToHex(bytesPath));

                System.Buffer.BlockCopy(bytesFin, 0, bytesTemp, 0, bytesFin.Length);
                System.Buffer.BlockCopy(bytesPath, 0, bytesTemp, bytesFin.Length, bytesPath.Length);

                bytesFin = new byte[bytesTemp.Length];
                System.Buffer.BlockCopy(bytesTemp, 0, bytesFin, 0, bytesTemp.Length);
            }
        }

        // Reverse the order of the bits within each byte (yes, this is also necessary)
        for (int i = 0; i < bytesFin.Length; i++)
        {
            bytesFin[i] = ReverseWithLookupTable(bytesFin[i]);
        }

        if (totalBits < 128)
        {
            byte[] bytesTemp = new byte[bytesFin.Length - ((128 - totalBits) / 8)];
            System.Buffer.BlockCopy(bytesFin, 0, bytesTemp, 0, bytesTemp.Length);
            bytesFin = bytesTemp;
        }

        return bytesFin;
    }

    public int detectExtraBits(List<int> varList)
    {


        return 0;
    }


    public static int[] findLeadingBitValue(int leadBits, int totalBits, int value)
    {
        int[] badReturn = new int[2];
        //Debug.Log("leadBits: " + leadBits + " totalBits: " + totalBits + " value: " + value + " 8-total: " + (8-totalBits));
        if (value == 0)
            return badReturn;
        string bits = bitStrings[value];
        string bits2;
        if ((8-totalBits) + leadBits <= bits.Length)
        {
            if ((8 - totalBits + leadBits) < 8)
                bits2 = bits.Substring(8 - totalBits + leadBits);
            else
                bits2 = "0";
            bits = bits.Substring(8 - totalBits, leadBits);
        }
        else
        {
            Debug.Log("Error with 'findLeadingBitValue(), incorrect parameters passed.");
            return badReturn;
        }

        int[] returnArr = new int[2];
        returnArr[0] = Convert.ToInt32(bits, 2);
        returnArr[1] = Convert.ToInt32(bits2, 2);
        return returnArr;
    }

}
