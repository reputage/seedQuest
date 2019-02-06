using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

using SeedQuest.Interactables;

namespace SeedQuest.SeedEncoder
{
    /*
     * The functions in this script can be used like this:
     * 
     * To get the int[] of actions to be performed from a seed:
     *  getActions(string inputStringName, List<int> actionList);
     * 
     * if no action list is provided, the default list will be used.
     * 
     * To get the seed from an int[] of actions:
     *  getSeed(int[] actionArray, List<int> actionList);
    */

    /// <summary>
    /// Encoder for String Seed to ActionList
    /// </summary>
    public class SeedToByte
    {
        public List<int> actionList = new List<int>();

        public static string inputSeed;
        public static string returnSeed;

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

        public static string[] bitStrings =
        {
        "00000000", "00000001", "00000010", "00000011", "00000100",
        "00000101", "00000110", "00000111", "00001000", "00001001",
        "00001010", "00001011", "00001100", "00001101", "00001110",
        "00001111", "00010000", "00010001", "00010010", "00010011",
        "00010100", "00010101", "00010110", "00010111", "00011000",
        "00011001", "00011010", "00011011", "00011100", "00011101",
        "00011110", "00011111", "00100000"
    };

        // Take string for input, get the to-do list of actions
        public int[] getActions(string inputStr, List<int> actionList = null)
        {
            if (actionList == null)
                actionList = listBuilder();
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
        public int[] getActionsFromBytes(byte[] inputBytes, List<int> actionList = null)
        {
            if (actionList == null)
                actionList = listBuilder();
            BitArray inputBits2 = byteToBits(inputBytes);
            int[] actionToDo2 = bitToActions(inputBits2, actionList);
            return actionToDo2;
        }

        // Get the return seed from a list of actions
        public string getSeed(int[] actionsPerformed, List<int> actionList = null)
        {
            if (actionList == null)
                actionList = listBuilder();
            returnBytes = seedConverterUniversal(actionsPerformed, actionList);
            string convertedSeed = byteToSeed(returnBytes);
            return convertedSeed;
        }

        // Get the return seed from a list of actions
        public string getSeedCustomList(int[] actionsPerformed, List<int> customList)
        {
            returnBytes = seedConverterUniversal(actionsPerformed, customList);
            string convertedSeed = byteToSeed(returnBytes);
            return convertedSeed;
        }

        // Get the return seed using 108 bits, does not use global variables
        public string getSeed108(int[] actionsPerformed)
        {
            List<int> tempList = customList(3, 4, 2, 4, 4); //this probably shouldn't be hard-coded, but was necessary for the demo
            byte[] bytes108 = seedConverterUniversal(actionsPerformed, tempList);
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
                throw new Exception("The binary key cannot have an odd number of digits: " + hex.Length);

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

        // Reverse the order of bits
        // This method is used since it's faster than other bit-reversal methods
        public static byte ReverseWithLookupTable(byte toReverse)
        {
            return BitReverseTable[toReverse];
        }

        // Construct the list of how many bits represent which parts of the Path to take
        public static List<int> listBuilder()
        {
            int numLocationBits = InteractableConfig.SiteBits;
            int numSpotBits = InteractableConfig.InteractableBits;
            int numActionBits = InteractableConfig.ActionBits;
            int numActions = InteractableConfig.ActionsPerSite;
            int numTotalLocations = InteractableConfig.SitesPerGame;

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

        // Takes the list of actions, converts it back into bytes,
        //  works for seeds of any bit size and configuration
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

            // If the total bits are less than 64, it is easy to find the bytes of the actions
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

                // Reverse the endian of the bytes
                for (int j = 0; j < (bytesPath.Length / 2); j++)
                {
                    byte tmp = bytesPath[j];
                    bytesPath[j] = bytesPath[bytesPath.Length - j - 1];
                    bytesPath[bytesPath.Length - j - 1] = tmp;
                }

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

                count = 0;

                for (int i = 0; i < numLongs; i++)
                {
                    path = 0;
                    numShifts = 0;

                    if (i == 0)
                    {
                        numShifts += varList[0];
                    }
                    else if (i > 0) // If not the first int, add remainder bits from the previous one
                    {
                        if (remainder > 0)
                        {
                            path += (ulong)remainder;
                            path = path << varList[numTraverse];
                            numShifts += varList[numTraverse + 1];
                            numShifts += remainderBits;
                            remainder = 0;
                            remainderBits = 0;
                        }
                    }

                    while (numShifts < 64)
                    {
                        if (numTraverse < actions.Length) // Add actions to the int64
                        {
                            path += (ulong)actions[numTraverse];
                        }
                        if (numTraverse + 1 >= varList.Count) // if there are no more ints in the list, shift to 64 bits
                        {
                            path = path << (64 - numShifts);
                            numShifts += 64;
                        }
                        else if (numShifts + varList[numTraverse + 1] > 64) // if about to overflow 64 bits
                        {
                            int[] partialAction = findLeadingBitValue((64 - numShifts), varList[numTraverse + 1], actions[numTraverse + 1]);
                            remainder = partialAction[1];
                            remainderBits = partialAction[2];
                            path = path << (64 - numShifts);
                            path += (ulong)partialAction[0];
                            numShifts += 64;
                        }
                        else if (numShifts + varList[numTraverse + 1] == 64) // if the bits divide evenly into 64 bits
                        {
                            path = path << varList[numTraverse + 1];
                            path += (ulong)actions[numTraverse + 1];
                            numShifts += varList[numTraverse + 1];
                        }
                        else if ((numTraverse + 1) < varList.Count) // shift the bits of the int64
                        {
                            path = path << varList[numTraverse + 1];
                            numShifts += varList[numTraverse + 1];
                        }
                        else if (numTraverse == varList.Count - 1) // if the list has reached the end
                        {
                            path = path << (64 - numShifts);
                            numShifts += 64;
                        }

                        count++;
                        numTraverse++;
                        if (count > 1000)
                        {
                            numShifts += 64;
                            break;
                        }
                    }

                    byte[] bytesPath = BitConverter.GetBytes(path);
                    byte[] bytesTemp = new byte[bytesPath.Length + bytesFin.Length];

                    // Reverse the endian of the bytes (yes, this is necessary to get the seed out properly)
                    for (int j = 0; j < (bytesPath.Length / 2); j++)
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

            if (totalBits < 64)
            {
                byte[] bytesTemp = new byte[bytesFin.Length - ((64 - totalBits) / 8)];
                System.Buffer.BlockCopy(bytesFin, 0, bytesTemp, 0, bytesTemp.Length);
                bytesFin = bytesTemp;
            }
            else if (totalBits < 128 && totalBits > 64)
            {
                byte[] bytesTemp = new byte[bytesFin.Length - ((128 - totalBits) / 8)];
                System.Buffer.BlockCopy(bytesFin, 0, bytesTemp, 0, bytesTemp.Length);
                bytesFin = bytesTemp;
            }
            else if (totalBits < 192 && totalBits > 128)
            {
                byte[] bytesTemp = new byte[bytesFin.Length - ((192 - totalBits) / 8)];
                System.Buffer.BlockCopy(bytesFin, 0, bytesTemp, 0, bytesTemp.Length);
                bytesFin = bytesTemp;
            }

            return bytesFin;
        }


        // This function is used to handle cases where the bits in a list of actions
        //  do not divide evenly across 64 bit integers. Returns an int array, with
        //  the first int representing the value of the leading bits, the second int
        //  representing the trailing int, and the third int for the number of bits of 
        //  the trailing int
        public static int[] findLeadingBitValue(int leadBits, int totalBits, int value)
        {
            int[] badReturn = new int[3];
            //Debug.Log("leadBits: " + leadBits + " totalBits: " + totalBits + " value: " + value + " 8-total: " + (8-totalBits));
            if (value == 0)
                return badReturn;
            else if (leadBits == 0)
            {
                Debug.Log("Warning: leadBits field of findLeadingBitValue is 0 - check your function call");
                badReturn[0] = 0;
                badReturn[1] = value;
                badReturn[2] = totalBits;
                return badReturn;
            }
            string bits = bitStrings[value];
            string bits2;
            if ((8 - totalBits) + leadBits <= bits.Length)
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

            int[] returnArr = new int[3];
            returnArr[0] = Convert.ToInt32(bits, 2);
            returnArr[1] = Convert.ToInt32(bits2, 2);
            returnArr[2] = totalBits - leadBits;
            return returnArr;
        }
    }
}