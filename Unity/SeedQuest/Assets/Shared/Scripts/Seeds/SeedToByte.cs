using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using System;


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
    public string testSeed = "||||||||||||||||";

    public string testReturnStr;
    public string testReturnStr2;
    public string testBitStr;
    public byte[] testByteArr;
    public byte[] testReturnBytes;
    public int[] actionToDo;
    public BitArray testBitArr;


    void Start () 
    {
        // Just a test
        testByteArr = seedToByte(testSeed);
        testReturnStr = byteToSeed(testByteArr);
        testBitArr = byteToBits(testByteArr);
        testReturnBytes = bitToByte(testBitArr);
        testReturnStr2 = byteToSeed(testReturnBytes);

        actionToDo = bitConverter(testBitArr);

        //Debug.Log(testBitArr.Length);
        //Debug.Log(actionToDo);


        for (int i = 0; i < actionToDo.Length; i ++)
        {
            Debug.Log( "Index: " + i + " Value: " + actionToDo[i]);
        }


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


    public int[] bitConverter(BitArray bits)
    {

        /*
        bool[] ibits = new bool[128];
        for (int i = 0; i < 128; i++) {
            ibits[i] = bits[i];
        }
        */

        int numLocationBits = 4;
        int numSpotBits = 4;
        int numActionBits = 3;
        int numActions = 4;
        int numTotalLocations = 4;

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

        // TO DO:
        //  use actionList declared above in the function
        //  read bits forwards, not backwards
        //  use the list as the stop points when writing to the array

        int[] actionValues = new int[40];
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
                value += Convert.ToInt32(Math.Pow(2, valueIndex));
            }

            valueIndex += 1;


            //4, 11, 18, 25 locations end
            //8, 15, 22, 29 spots end
            //11, 18, 25, 32 action end

            if (locator == 3)
            {
                // get location
                actionValues[writeIndex] = value;
                Debug.Log(actionValues[writeIndex] + " i: " + i + " Loc: " + locator);

                writeIndex += 1;
                value = 0;
                valueIndex = 0;
            }
            if (locator == 7 || locator == 14 || locator == 21 || locator == 28)
            {
                // get spot
                actionValues[writeIndex] = value;
                Debug.Log(actionValues[writeIndex] + " i: " + i + " Loc: " + locator);

                writeIndex += 1;
                value = 0;
                valueIndex = 0;
            }
            if (locator == 10 || locator == 17 || locator == 24 || locator == 31)
            {
                // get action 
                actionValues[writeIndex] = value;
                Debug.Log(actionValues[writeIndex] + " i: " + i + " Loc: " + locator);

                writeIndex += 1;
                value = 0;
                valueIndex = 0;
            }

            locator += 1;

            if (locator > 1 && (i + 1) % 32 == 0)
            {
                //Debug.Log(locator + " and: " + (i % 32));
                locator = 0;
            }

        }

        return actionValues;
    }

    // repeat 4 times:
    // iterate 4 times, save to location
    // repeat 4 times:
    // iterate 4 times, save to spot
    // iterate 3 times, save to action

}
