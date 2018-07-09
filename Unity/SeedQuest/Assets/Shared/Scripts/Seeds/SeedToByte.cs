using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using System;
using System.Collections.Specialized;


/* For each action:
 * 4 bits for location
 *  0 
 *  32
 *  64
 *  96
 * 
 * 4 bits for each "spot" at the location
 *  4, 11, 18, 25
 *  36, 43, 50, 57
 *  68, 75, 82, 89
 *  100, 107, 114, 121
 * 
 * 3 bits for each action at the location
 *  8, 15, 22, 29
 *  40, 47, 54, 61
 *  72, 79, 86, 93
 *  104, 111, 118, 125
 * 
 * 4 actions at each location = 4 * 7 = 28
 * 28 + 4 (for the location) = 32
 * 4 locations to be visited
 * 
 * 4 bits = location
 * 4 bits = spot
 * 3 bits = action
 * 4 spot
 * 3 action
 * 4 spot
 * 3 action
 * 4 spot
 * 3 action
 */


public class SeedToByte : MonoBehaviour
{

    // Yes, I know this isn't the best way to do this operation, but it works
    // Please don't break this.

    public string testSeed3 = "C5E3D45D341A7";
    public string testSeed2 = "||||||||||||||||";

    public string testReturnStr;
    public string testReturnStr2;
    public string testReturnStr3;

    public string testBitStr;
    public byte[] testByteArr;
    public byte[] testReturnBytes;
    public int[] actionToDo;
    public BitArray testBitArr;
    public byte[] actionToBits;
    public List<int> actionList;

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

    void Start()
    {
        actionList = listBuilder();
        // Just a test
        testByteArr = seedToByte(testSeed3);
        testReturnStr = byteToSeed(testByteArr);
        testBitArr = byteToBits(testByteArr);
        testReturnBytes = bitToByte(testBitArr);
        testReturnStr2 = byteToSeed(testReturnBytes);

        actionToDo = bitConverter(testBitArr, actionList);

        actionToBits = actionConverter(actionToDo, actionList);
        testReturnStr3 = byteToSeed(actionToBits);

    }

    void Update()
    {

    }

    //  Convert string to byte array
    public byte[] seedToByte(string seedString)
    {
        // example string = C5E3D45D341C5
        byte[] seedByte = Encoding.UTF8.GetBytes(seedString);
        return seedByte;
    }

    // Convert byte array back to string
    public string byteToSeed(byte[] bytes)
    {
        string returnStr = Encoding.UTF8.GetString(bytes);
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
        byte[] returnBytes;
        returnBytes = BitArrayToByteArray(bits);
        return returnBytes;
    }

    // Convert bit array to byte array
    public byte[] BitArrayToByteArray(BitArray bits)
    {
        byte[] ret = new byte[(bits.Length - 1) / 8 + 1];
        bits.CopyTo(ret, 0);
        return ret;
    }

    // Construct the list of how many bits represent which parts of the path to take
    public List<int> listBuilder()
    {
        int numLocationBits = 4;        // Number of bits used to determine location
        int numSpotBits = 4;            // Number of bits used to determine spots for each action
        int numActionBits = 3;          // Number of bits used to determine action choice
        int numActions = 4;             // Total actions the player needs to take at each location
        int numTotalLocations = 4;      // Total number of locatiosn the player needs to visit

        List<int> actionList = new List<int>();

        for (int j = 0; j < numTotalLocations; j++)
        {
            actionList.Add(numLocationBits);

            for (int i = 0; i < numActions; i++)
            {
                actionList.Add(numSpotBits);
                actionList.Add(numActionBits);
            }
        }

        // Print total list items, and the values for location, spot, and action
        //Debug.Log("Total: " + actionList.Count + " Loc: " + actionList[0] + " Spot: " + actionList[1] + " Act: " + actionList[2]);

        return actionList;
    }

    // Convert bit array to int array representing the actions the player should take
    public int[] bitConverter(BitArray bits, List<int> actionList)
    {
        int[] actionValues = new int[36];
        int value = 0;
        int valueIndex = 0;
        int locator = 0;
        int writeIndex = 0;

        if (bits.Length > 128)
        {
            Debug.Log("Error: Provided seed is greater than 128 bits.");
            return actionValues;
        }

        for (int i = 0; i < bits.Length; i++)
        {
            //Debug.Log(value);
            if (bits[i])
            {
                //Debug.Log(valueIndex);
                // Yes, I know this is reading the bits in reverse, this is intentional
                int bitValue = actionList[writeIndex] - (valueIndex + 1);
                value += Convert.ToInt32(Math.Pow(2, bitValue));
                //value += Convert.ToInt32(Math.Pow(2, valueIndex));
            }

            if (locator == (actionList[writeIndex] - 1))
            {
                // Store the location/spot/action
                actionValues[writeIndex] = value;
                //Debug.Log(actionValues[writeIndex] + " i: " + i + " Loc: " + locator);

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


    public byte[] actionConverter(int[] actions, List<int> actionList)
    {
        var actionBits = new BitArray(128);
        ulong path1 = 0;
        ulong path2 = 0;

        // Convert action ints into two ulong ints
        for (int i = 0; i < 18; i++)
        {
            path1 += (ulong)actions[i];
            path2 += (ulong)actions[i + 18];
            if (i < 17)
            {
                path1 = path1 << actionList[i + 1];
                path2 = path2 << actionList[i + 19];
            }

        }

        // Convert ulong ints with actions into byte arrays
        byte[] bytes1 = BitConverter.GetBytes(path1);
        byte[] bytes2 = BitConverter.GetBytes(path2);

        // Reverse the endian of the bytes (yes, this is necessary to get the seed out properly)
        //  Yes, I know we are reversing the bits to get the actions,
        //  then reversing them again when extracting the bits.
        //  The manager-man wanted me to do it this way.
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
        System.Buffer.BlockCopy(bytes1, 0, bytes3, 0, bytes2.Length);
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



}
