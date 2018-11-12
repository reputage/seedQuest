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

    void Start()
    {
        testRun(); 
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

        testActionToDo = bitConverter(testBitArr, actionList);

        actionToBits = actionConverter(testActionToDo, actionList);
        actionToBitsVariant = variableSizeConverter(testActionToDo, actionList, 128);
        testReturnStr3 = byteToSeed(actionToBits);
    }

    // Take string for input, get the to-do list of actions
    public int[] getActions(string inputStr)
    {
        inputSeed = inputStr;
        inputBytes = seedToByte(inputSeed);
        inputBits = byteToBits(inputBytes);
        actionToDo = bitConverter(inputBits, actionList);
        int[] returnActions = actionToDo;
        //Debug.Log(actionToDo);
        return returnActions;
    }

    // Take string for input, get the to-do list of actions
    public int[] getActionsFromBytes(byte[] inputBytes)
    {
        actionList = listBuilder();
        BitArray inputBits2 = byteToBits(inputBytes);
        int[] actionToDo2 = bitConverter(inputBits2, actionList);
        return actionToDo2;
    }

    // Get the return seed from a list of actions
    public string getSeed(int[] actionsPerformed)
    {
        // Don't change the actionList - it will break everything
        returnBytes = actionConverter(actionsPerformed, actionList);
        string convertedSeed = byteToSeed(returnBytes);
        // Just going to put these here for now... I'm not sure where else to put them
        seedBase58 = ByteArrayToBase58(returnBytes);
        seedBase64 = ByteArrayToBase64(returnBytes);
        seedBinary = ByteArrayToBinary(returnBytes);
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
    public List<int> listBuilder()
    {
        int numLocationBits = SeedManager.SiteBits ;        
        int numSpotBits = SeedManager.SpotBits;            
        int numActionBits = SeedManager.ActionBits;         
        int numActions = SeedManager.ActionCount;           
        int numTotalLocations = SeedManager.SiteCount;      

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
        if (actionList.Count == 0)
            actionList = listBuilder();
        
        int[] actionValues = new int[actionList.Count];
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
            if (bits[i])
            {
                // Yes, I know this is reading the bits in reverse, this is intentional, manager wanted it done this way
                int bitValue = actionList[writeIndex] - (valueIndex + 1);
                value += Convert.ToInt32(Math.Pow(2, bitValue));
            }
            if (locator == (actionList[writeIndex] - 1))
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
    public byte[] actionConverter(int[] actions, List<int> actionList)
    {
        if (actionList.Count == 0)
            actionList = listBuilder();

        var actionBits = new BitArray(128);
        ulong path1 = 0;
        ulong path2 = 0;
        int[] tempArray = new int[36];

        int totalBits = 0;
        for (int i = 0; i < actionList.Count; i++) {
            totalBits += actionList[i];
        }

        if (totalBits < 128)
        {
            // Convert action ints into two ulong ints
            for (int i = 0; i < actions.Length; i++)
            { 
                path1 += (ulong)actions[i];
                if(i < actions.Length - 1)
                    path1 = path1 << actionList[i + 1];
            }
            path1 = path1 << (128 - totalBits);
        }
        else
        {
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
            Debug.Log("path1 int: " + path1);
            Debug.Log("path2 int: " + path2);
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

    // Takes the list of actions, converts it back into bytes
    public byte[] variableSizeConverter(int[] actions, List<int> actionList, int size)
    {
        // actionList contains the list of how many bit each value is
        // actions contains the actions themselves to be converted back into the seed
        if (actionList.Count == 0)
            actionList = listBuilder();

        var actionBits = new BitArray(size); // size = 128 in other function
        int[] tempArray = new int[36];

        int totalBits = 0;
        for (int i = 0; i < actionList.Count; i++)
        {
            totalBits += actionList[i];
        }

        byte[] bytesFin = new byte[0];

        Debug.Log("Total bit count: " + totalBits);

        if (totalBits < 128)
        {
            ulong path = 0;
            Debug.Log("Actions.Length: " + actions.Length);
            for (int i = 0; i < actions.Length; i++)
            {
                path += (ulong)actions[i];
                if (i < actions.Length - 1)
                    path = path << actionList[i + 1];
            }

            path = path << (128 - totalBits);
            byte[] bytesPath = BitConverter.GetBytes(path);

            bytesFin = new byte[bytesPath.Length];
            System.Buffer.BlockCopy(bytesPath, 0, bytesFin, 0, bytesPath.Length);
        }
        else
        {
            Debug.Log("Actions.Length: " + actions.Length);

            int modBits  = totalBits % 64; // needs to be implemented
            int numLongs = totalBits / 64;
            ulong path = 0;

            if (modBits > 0)
                numLongs += 1;
            
            for (int i = 0; i < numLongs; i++)
            {
                int longOffset = i * 18;
                for (int j = 0; j < 18; j++)
                {
                    path += (ulong)actions[j + longOffset];
                    if (j < 17)
                        path = path << actionList[j + 1 + longOffset];
                }
                Debug.Log("path int: " + path);

                byte[] bytesPath = BitConverter.GetBytes(path);
                byte[] bytesTemp = new byte[bytesPath.Length + bytesFin.Length];

                // Reverse the endian of the bytes (yes, this is necessary to get the seed out properly)
                for (int h = 0; h < bytesPath.Length / 2; h++)
                {
                    byte tmp = bytesPath[h];
                    bytesPath[h] = bytesPath[bytesPath.Length - h - 1];
                    bytesPath[bytesPath.Length - h - 1] = tmp;
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

        return bytesFin;
    }

}
