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

    /// <summary> Encoder for String Seed to ActionList </summary>
    public class SeedToByte
    {
        public List<int> actionList = new List<int>();

        public static string inputSeed;
        public static string returnSeed;

        public static byte[] inputBytes;
        public static byte[] returnBytes;
        public static int[] actionToDo;
        public static BitArray inputBits;

        // This table is used for reversing the endian of bits
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

        // This table is required for the universal seed converter to handle bit size combinations
        //  that are not divisible by 8
        public static string[] bitStrings =
        {
        "00000000", "00000001", "00000010", "00000011", "00000100",
        "00000101", "00000110", "00000111", "00001000", "00001001",
        "00001010", "00001011", "00001100", "00001101", "00001110",
        "00001111", "00010000", "00010001", "00010010", "00010011",
        "00010100", "00010101", "00010110", "00010111", "00011000",
        "00011001", "00011010", "00011011", "00011100", "00011101",
        "00011110", "00011111", "00100000", "00100001", "00100010",
        "00100011", "00100100", "00100101", "00100110", "00100111",
        "00101000", "00101001", "00101010", "00101011", "00101100",
        "00101101", "00101110", "00101111", "00110000", "00110001",
        "00110010", "00110011", "00110100", "00110101", "00110110",
        "00110111", "00111000", "00111001", "00111010", "00111011",
        "00111100", "00111101", "00111110", "00111111", "01000000",
        "01000001", "01000010", "01000011", "01000100", "01000101",
        "01000110", "01000111", "01001000", "01001001", "01001010",
        "01001011", "01001100", "01001101", "01001110", "01001111",
        "01010000", "01010001", "01010010", "01010011", "01010100",
        "01010101", "01010110", "01010111", "01011000", "01011001",
        "01011010", "01011011", "01011100", "01011101", "01011110",
        "01011111", "01100000", "01100001", "01100010", "01100011",
        "01100100", "01100101", "01100110", "01100111", "01101000",
        "01101001", "01101010", "01101011", "01101100", "01101101",
        "01101110", "01101111", "01110000", "01110001", "01110010",
        "01110011", "01110100", "01110101", "01110110", "01110111",
        "01111000", "01111001", "01111010", "01111011", "01111100",
        "01111101", "01111110", "01111111",
        "10000000", "10000001", "10000010", "10000011", "10000100",
        "10000101", "10000110", "10000111", "10001000", "10001001",
        "10001010", "10001011", "10001100", "10001101", "10001110",
        "10001111", "10010000", "10010001", "10010010", "10010011",
        "10010100", "10010101", "10010110", "10010111", "10011000",
        "10011001", "10011010", "10011011", "10011100", "10011101",
        "10011110", "10011111", "10100000", "10100001", "10100010",
        "10100011", "10100100", "10100101", "10100110", "10100111",
        "10101000", "10101001", "10101010", "10101011", "10101100",
        "10101101", "10101110", "10101111", "10110000", "10110001",
        "10110010", "10110011", "10110100", "10110101", "10110110",
        "10110111", "10111000", "10111001", "10111010", "10111011",
        "10111100", "10111101", "10111110", "10111111", "11000000",
        "11000001", "11000010", "11000011", "11000100", "11000101",
        "11000110", "11000111", "11001000", "11001001", "11001010",
        "11001011", "11001100", "11001101", "11001110", "11001111",
        "11010000", "11010001", "11010010", "11010011", "11010100",
        "11010101", "11010110", "11010111", "11011000", "11011001",
        "11011010", "11011011", "11011100", "11011101", "11011110",
        "11011111", "11100000", "11100001", "11100010", "11100011",
        "11100100", "11100101", "11100110", "11100111", "11101000",
        "11101001", "11101010", "11101011", "11101100", "11101101",
        "11101110", "11101111", "11110000", "11110001", "11110010",
        "11110011", "11110100", "11110101", "11110110", "11110111",
        "11111000", "11111001", "11111010", "11111011", "11111100",
        "11111101", "11111110", "11111111"
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
            {
                Debug.Log("The binary key cannot have an odd number of digits - shortening the string");
                hex = hex.Substring(0, (hex.Length - 1));
            }
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
        public static byte ReverseWithLookupTable(byte toReverse)
        {
            return BitReverseTable[toReverse];
        }

        // Construct the list of how many bits represent which parts of the Path to take,
        //  uses the defaults specified in SeedManager
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

        // Makes a list using values passed in, not the defaults in SeedManager
        public static List<int> customList(int numLocBit, int numSpotBit, int numActBit, int numAct, int numLoc)
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
            int warningCounter = 0;

            for (int i = 0; i < bits.Length; i++)
            {
                if (writeIndex >= (varList.Count))
                    warningCounter++;
                else if (bits[i])
                {
                    // Yes, I know this is reading the bits in reverse, this is intentional, manager wanted it done this way
                    int bitValue = varList[writeIndex] - (valueIndex + 1);
                    value += Convert.ToInt32(Math.Pow(2, bitValue));
                }
                if (writeIndex > (varList.Count - 1))
                    warningCounter++;
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
        //  works for seeds of any bit size < 128
        public static byte[] seedConverterUniversal(int[] actions, List<int> varList)
        {
            if (varList.Count == 0)
                varList = listBuilder();

            byte[] finalBytes = new byte[0];
            int totalBits = 0;

            for (int i = 0; i < varList.Count; i++)
                totalBits += varList[i];

            if (totalBits < 64)
                finalBytes = shortSeed(actions, varList, totalBits);
            else
                finalBytes = longSeed(actions, varList, totalBits);

            // Reverse the order of the bits within each byte (yes, this is necessary)
            for (int i = 0; i < finalBytes.Length; i++)
                finalBytes[i] = ReverseWithLookupTable(finalBytes[i]);

            finalBytes = adjustFinalBytesSize(finalBytes, totalBits);

            return finalBytes;
        }

        // This function is used to handle cases where the bits in a list of actions
        //  do not divide evenly across 64 bit integers. Returns an int array, with
        //  the first int representing the value of the leading bits, the second int
        //  representing the trailing int, and the third int for the number of bits of 
        //  the trailing int
        public static int[] findLeadingBitValue(int leadBits, int totalBits, int value)
        {
            int[] returnArray = new int[3];

            if (value == 0)
                return returnArray;
            else if (leadBits == 0)
            {
                Debug.Log("Warning: leadBits field of findLeadingBitValue is 0 - check your function call");
                return returnArray;
            }

            string bitPart1 = bitStrings[value];
            string bitPart2;

            if ((8 - totalBits) + leadBits <= bitPart1.Length)
            {
                if ((8 - totalBits + leadBits) < 8)
                    bitPart2 = bitPart1.Substring(8 - totalBits + leadBits);
                else
                    bitPart2 = "0";
                bitPart1 = bitPart1.Substring(8 - totalBits, leadBits);
            }
            else
            {
                Debug.Log("Error with 'findLeadingBitValue(), incorrect parameters passed.");
                return returnArray;
            }

            returnArray[0] = Convert.ToInt32(bitPart1, 2);
            returnArray[1] = Convert.ToInt32(bitPart2, 2);
            returnArray[2] = totalBits - leadBits;
            return returnArray;
        }

        // Converts actions into a seed if the total bits of the seed are less than 64 bits
        public static byte[] shortSeed(int[] actions, List<int> varList, int totalBits)
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

            bytesPath = reverseByteEndian(bytesPath);
            byte[] finalBytes = new byte[bytesPath.Length];
            System.Buffer.BlockCopy(bytesPath, 0, finalBytes, 0, bytesPath.Length);

            return finalBytes;
        }

        // Converts actions into a seed if the total bits of the seed are greater than 64 bits
        public static byte[] longSeed(int[] actions, List<int> varList, int totalBits)
        {
            int longIntCount = totalBits / 64;
            int listIndex = 0;
            int bitsShifted = 0;
            int remainder = 0;
            int remainderBits = 0;
            ulong path = 0;
            byte[] finalBytes = new byte[0];

            if (totalBits % 64 > 0)
                longIntCount += 1;

            for (int i = 0; i < longIntCount; i++)
            {
                path = 0;
                bitsShifted = 0;

                if (i == 0)
                    bitsShifted += varList[0];
                else if (i > 0) // If not the first int, add remainder bits from the previous one
                    if (remainder > 0)
                        pathAddRemainder(varList, listIndex, ref bitsShifted,
                                         ref remainder, ref remainderBits, ref path);

                //while (bitsShifted < 64)
                pathAddValues(actions, varList, ref listIndex, ref bitsShifted,
                                 ref remainder, ref remainderBits, ref path);

                finalBytes = convertPathToBytes(finalBytes, path);
            }
            return finalBytes;
        }

        public static byte[] adjustFinalBytesSize(byte[] finalBytes, int totalBits)
        {
            int upperLimit = 0;

            if (totalBits % 64 == 0)
                upperLimit = totalBits;
            else
                upperLimit = ((totalBits / 64) + 1) * 64;

            finalBytes = resizeByteArray(finalBytes, totalBits, upperLimit);

            return finalBytes;
        }

        // Resize the byte array
        public static byte[] resizeByteArray(byte[] finalBytes, int totalBits, int upperLimit)
        {
            byte[] bytesTemp = new byte[finalBytes.Length - ((upperLimit - totalBits) / 8)];
            System.Buffer.BlockCopy(finalBytes, 0, bytesTemp, 0, bytesTemp.Length);
            return bytesTemp;
        }

        // Reverse the endian of bytes in a byte array
        public static byte[] reverseByteEndian(byte[] bytesPath)
        {
            for (int j = 0; j < (bytesPath.Length / 2); j++)
            {
                byte tmp = bytesPath[j];
                bytesPath[j] = bytesPath[bytesPath.Length - j - 1];
                bytesPath[bytesPath.Length - j - 1] = tmp;
            }
            return bytesPath;
        }

        // Add the required values to the path ulong
        public static void pathAddValues(int[] actions, List<int> varList, ref int listIndex,
                                        ref int bitsShifted, ref int remainder,
                                        ref int remainderBits, ref ulong path)
        {
            while (bitsShifted < 64)
            {
                if (listIndex < actions.Length) // Add actions to the int64
                {
                    path += (ulong)actions[listIndex];
                }
                if (listIndex + 1 >= varList.Count) // if there are no more ints in the list, shift to 64 bits
                {
                    path = path << (64 - bitsShifted);
                    bitsShifted += 64;
                }
                else if (bitsShifted + varList[listIndex + 1] > 64) // if about to overflow 64 bits
                {
                    int[] partialAction = findLeadingBitValue((64 - bitsShifted),
                                                              varList[listIndex + 1], actions[listIndex + 1]);
                    remainder = partialAction[1];
                    remainderBits = partialAction[2];
                    path = path << (64 - bitsShifted);
                    path += (ulong)partialAction[0];
                    bitsShifted += 64;
                }
                else if (bitsShifted + varList[listIndex + 1] == 64) // if the bits divide evenly into 64 bits
                {
                    path = path << varList[listIndex + 1];
                    path += (ulong)actions[listIndex + 1];
                    bitsShifted += varList[listIndex + 1];
                }
                else if ((listIndex + 1) < varList.Count) // shift the bits of the int64
                {
                    path = path << varList[listIndex + 1];
                    bitsShifted += varList[listIndex + 1];
                }
                else if (listIndex == varList.Count - 1) // if the list has reached the end
                {
                    path = path << (64 - bitsShifted);
                    bitsShifted += 64;
                }

                listIndex++;
            }
        }

        // Add remainder from previous path ulong into the current path ulong
        public static void pathAddRemainder(List<int> varList, int listIndex, ref int bitsShifted,
                                            ref int remainder, ref int remainderBits, ref ulong path)
        {
            //Debug.Log("Current varList value: " + varList[listIndex] + " varlist length: " + varList.Count + " index: " + listIndex);
            path += (ulong)remainder;
            path = path << varList[listIndex];
            if (varList.Count > listIndex + 1)
                bitsShifted += 0;//varList[listIndex + 1];
            else
                Debug.Log("Can't add bitsShifted by varList[listIndex+1]");
            bitsShifted += remainderBits;
            remainder = 0;
            remainderBits = 0;
        }

        // Convert the path ulong into a byte array
        public static byte[] convertPathToBytes(byte[] finalBytes, ulong path)
        {
            byte[] bytesPath = BitConverter.GetBytes(path);
            byte[] bytesTemp = new byte[bytesPath.Length + finalBytes.Length];

            // Reverse the endian of the bytes (yes, this is necessary)
            bytesPath = reverseByteEndian(bytesPath);

            System.Buffer.BlockCopy(finalBytes, 0, bytesTemp, 0, finalBytes.Length);
            System.Buffer.BlockCopy(bytesPath, 0, bytesTemp, finalBytes.Length, bytesPath.Length);

            finalBytes = new byte[bytesTemp.Length];
            System.Buffer.BlockCopy(bytesTemp, 0, finalBytes, 0, bytesTemp.Length);

            return finalBytes;
        }
    }
}