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

    //public string testSeed = "C5E3D45D341C5";
    public string testSeed2 = "||||||||||||||||";

    public string testReturnStr;
    public string testReturnStr2;
    public string testBitStr;
    public byte[] testByteArr;
    public byte[] testReturnBytes;
    public int[] actionToDo;
    public BitArray testBitArr;
    public byte[] actionToBits;
    public List<int> actionList;


    void Start () 
    {
        actionList = listBuilder();
        // Just a test
        testByteArr = seedToByte(testSeed2);
        testReturnStr = byteToSeed(testByteArr);
        testBitArr = byteToBits(testByteArr);
        testReturnBytes = bitToByte(testBitArr);
        testReturnStr2 = byteToSeed(testReturnBytes);

        actionToDo = bitConverter(testBitArr, actionList);

        actionToBits = actionConverter(actionToDo, actionList);

        /*
        for (int i = 0; i < 128; i++)
        {
            Debug.Log("Test bit: " + testBitArr[i] + " action bit: " + actionToBits[i]);
        }
        */

        //Debug.Log(testBitArr.Length + " " + actionToBits.Length);

        /*
        Debug.Log(testBitArr[0] + " " + testBitArr[1] + " " + testBitArr[2] + " " + testBitArr[3]);
        Debug.Log("Locations: " + actionToDo[0] + " " + actionToDo[9] + " " + actionToDo[18] + " " + actionToDo[27]);
        Debug.Log(testBitArr[4] + " " + testBitArr[5] + " " + testBitArr[6] + " " + testBitArr[7]);
        Debug.Log("First spots: " + actionToDo[1] + " " + actionToDo[10] + " " + actionToDo[19] + " " + actionToDo[28]);
        Debug.Log(testBitArr[8] + " " + testBitArr[9] + " " + testBitArr[10]);
        Debug.Log("First actions: " + actionToDo[2] + " " + actionToDo[11] + " " + actionToDo[20] + " " + actionToDo[29]);
        */

        /*
        for (int i = 0; i < actionToDo.Length; i++)
        {
            Debug.Log( "Index: " + i + " Value: " + actionToDo[i]);
        }
        */

    }
	
	void Update () {
		
	}

    //  Byte array conversion
    public byte[] seedToByte(string seedString)
    {
        // example string = C5E3D45D341C5
        byte[] seedByte = Encoding.UTF8.GetBytes(seedString);
        return seedByte;
    }


    public string byteToSeed(byte[] bytes)
    {
        string returnStr = Encoding.UTF8.GetString(bytes);
        return returnStr;
    }


    public BitArray byteToBits(byte[] bytes)
    {
        var returnBits = new BitArray(bytes);
        return returnBits;
    }


    public byte[] bitToByte(BitArray bits)
    {
        byte[] returnBytes;
        returnBytes = BitArrayToByteArray(bits);
        return returnBytes;
    }


    public byte[] BitArrayToByteArray(BitArray bits)
    {
        byte[] ret = new byte[(bits.Length - 1) / 8 + 1];
        bits.CopyTo(ret, 0);
        return ret;
    }


    private int getIntFromBitArray(BitArray bits)
    {
        if (bits.Length > 32)
        {
            //throw new ArgumentException("Argument length shall be at most 32 bits.");
        }

        int[] array = new int[1];
        bits.CopyTo(array, 0);
        return array[0];

    }

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
                int bitValue = actionList[writeIndex] - (valueIndex + 1);
                //Debug.Log("BitValue: " + bitValue);
                value += Convert.ToInt32(Math.Pow(2, bitValue));
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

        for (int i = 0; i < 18; i++)
        {
            path1 += (ulong)actions[i];
            path2 += (ulong)actions[i + 18];
            if (i < 17)
            {
                path1 = path1 << actionList[i];
                path2 = path2 << actionList[i + 18];
            }
            Debug.Log( i + " " + path1 + " " + path2);
        }

        /*
        for (int i = 0; i < actionBits.Length; i ++)
        {
            Debug.Log(actionBits.Length + " " + actionBits[i]);
        }
*/
        byte[] bytes1 = BitConverter.GetBytes(path1);
        byte[] bytes2 = BitConverter.GetBytes(path1);
        byte[] bytes3 = new byte[bytes1.Length + bytes2.Length];
        System.Buffer.BlockCopy(bytes1, 0, bytes3, 0, bytes2.Length);
        System.Buffer.BlockCopy(bytes2, 0, bytes3, bytes1.Length, bytes2.Length);



        return bytes3;
    }

}
