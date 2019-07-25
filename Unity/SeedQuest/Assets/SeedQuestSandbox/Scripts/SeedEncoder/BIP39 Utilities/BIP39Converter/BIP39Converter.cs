using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using SeedQuest.SeedEncoder;

public class BIP39Converter
{
    private string[] englishWordList = EnglishWordList.words;
        
    public const string saltHeader = "mnemonic";
    public const int minIterations = 2048;
    public const int hLen = 64;
    public const int bitsInByte = 8;
    public const int bitGroupSize = 11;
    public const int minimumEntropyBits = 128;
    public const int maximumEntropyBits = 8192;
    public const int entropyMultiple = 32;
    private Int32 dkLen;
    private SeedToByte seeds = new SeedToByte();

    public int[] getActionsFromSentence(string sentence)
    {
        int[] actions = new int[1];
        string[] wordArray = sentence.Split(null);

        if (wordArray.Length < 12)
        {
            Debug.Log("Not enough words for 132 bits of entropy.");
            throw new Exception("Less than 12 words in this sentence. Not a valid seed.");
        }

        List<int> indeces = rebuildWordIndexes(wordArray);
        byte[] bytes = processWordIndecesNoChecksum(indeces);
        List<int> wordListSizes = new List<int> { 11, 11, 11, 11, 11, 11, 11, 11, 11, 11, 11, 11 };

        actions = seeds.getActionsFromBytes(bytes);
        return actions;
    }

    public string getIndecesFromSentence(string sentence)
    {
        int[] actions = new int[1];
        string[] wordArray = sentence.Split(null);

        if (wordArray.Length < 12)
        {
            Debug.Log("Not enough words for 132 bits of entropy.");
            throw new Exception("Less than 12 words in this sentence. Not a valid seed.");
        }
        List<int> indeces = rebuildWordIndexesDebug(wordArray);
        string indecesStr = "";

        for (int i = 0; i < indeces.Count; i++)
        {
            indecesStr += indeces[i] + " ";
        }

        return indecesStr;
    }

    public int[] getActionsWithChecksum(string sentence)
    {
        int[] actions = new int[1];
        string[] wordArray = sentence.Split(null);

        if (wordArray.Length < 12)
        {
            Debug.Log("Not enough words for 132 bits of entropy.");
            throw new Exception("Less than 12 words in this sentence. Not a valid seed.");
        }

        List<int> indeces = rebuildWordIndexes(wordArray);
        byte[] bytes = new byte[1];

        try
        {
            bytes = processWordIndeces(indeces);
        }
        catch (Exception e)
        {
            Debug.Log("Exception: " + e);
        }

        List<int> wordListSizes = new List<int> { 11, 11, 11, 11, 11, 11, 11, 11, 11, 11, 11, 11 };

        actions = seeds.getActionsFromBytes(bytes);
        return actions;
    }

    public string getSentenceFromActions(int[] actions)
    {
        string seed = seeds.getSeed(actions);
        byte[] seedBytes = HexStringToByteArray(seed);
        BitArray bits = byteToBits(seedBytes);
        List<int> wordListSizes = new List<int> { 11, 11, 11, 11, 11, 11, 11, 11, 11, 11, 11, 11 };
        int[] wordIndeces = seeds.bitToActions(bits, wordListSizes);
        List<int> wordIndecesList = new List<int>();

        for (int i = 0; i < wordIndeces.Length; i++)
            wordIndecesList.Add(wordIndeces[i]);

        string words = getMnemonicSentence(wordIndecesList);
        return words;
    }

    public string getSentenceFromActionsDebug(int[] actions)
    {
        string seed = seeds.getSeed(actions);
        byte[] seedBytes = HexStringToByteArray(seed);
        BitArray bits = byteToBits(seedBytes);

        List<int> wordListSizes = new List<int> { 11, 11, 11, 11, 11, 11, 11, 11, 11, 11, 11, 11 };
        int[] wordIndeces = seeds.bitToActions(bits, wordListSizes);
        List<int> wordIndecesList = new List<int>();

        for (int i = 0; i < wordIndeces.Length; i++)
            wordIndecesList.Add(wordIndeces[i]);

        string words = getMnemonicSentence(wordIndecesList);
        return words;
    }

    public string getSentence128Bits(byte[] seedBytes)
    {
        BitArray bits = byteToBits(seedBytes);

        if (bits.Count != 128)
        {
            Debug.Log("Invalid actions! This does not represent exactly 128 bits.");
        }

        BitArray bitsWithChecksum = appendChecksumBits(bits);

        List<int> wordListSizes = new List<int> { 11, 11, 11, 11, 11, 11, 11, 11, 11, 11, 11, 11 };
        int[] wordIndeces = seeds.bitToActions(bitsWithChecksum, wordListSizes);
        List<int> wordIndecesList = new List<int>();

        for (int i = 0; i < wordIndeces.Length; i++)
            wordIndecesList.Add(wordIndeces[i]);

        string words = getMnemonicSentence(wordIndecesList);
        return words;
    }

    public string getHexFromSentence(string sentence)
    {
        string[] wordArray = sentence.Split(null);

        if (wordArray.Length < 12)
        {
            Debug.Log("Not enough words for 128 bits of entropy.");
            throw new Exception("Less than 12 words in this sentence. Not a valid seed.");
        }

        List<int> indeces = rebuildWordIndexes(wordArray);
        byte[] bytes = processWordIndecesNoChecksum(indeces);
        List<int> wordListSizes = new List<int> { 11, 11, 11, 11, 11, 11, 11, 11, 11, 11, 11, 11 };
        int[] actions = seeds.getActionsFromBytes(bytes);

        string hexSeed = seeds.getSeed(actions);
        return hexSeed;
    }

    public string getHexWithChecksum(string sentence)
    {
        string[] wordArray = sentence.Split(null);

        if (wordArray.Length < 12)
        {
            Debug.Log("Not enough words for 128 bits of entropy.");
            throw new Exception("Less than 12 words in this sentence. Not a valid seed.");
        }

        List<int> indeces = rebuildWordIndexes(wordArray);
        byte[] bytes = processWordIndeces(indeces);
        List<int> wordListSizes = new List<int> { 11, 11, 11, 11, 11, 11, 11, 11, 11, 11, 11, 11 };
        int[] actions = seeds.getActionsFromBytes(bytes);

        string hexSeed = seeds.getSeed(actions);
        return hexSeed;
    }

    public string getSentenceFromHex(string hex)
    {
        int[] actions = seeds.getActions(hex);
        string words = getSentenceFromActions(actions);

        return words;
    }

    private int processBitsToInt(BitArray bits)
    {
        int number = 0;
        int base2Divide = 1024; 

        foreach (bool b in bits)
        {
            if (b)
            {
                number = number + base2Divide;
            }

            base2Divide = base2Divide / 2;
        }

        return number;
    }

    private string getMnemonicSentence(List<int> wordIndexList)
    {
        if (wordIndexList.Contains(-1))
        {
            Debug.Log("Error! Invalid word indicies.");
        }

        string mSentence = "";

        for (int i = 0; i < wordIndexList.Count; i++)
        {
            mSentence += englishWordList[wordIndexList[i]];
            if (i + 1 < wordIndexList.Count)
            {
                mSentence += " ";
            }
        }

        return mSentence;
    }

    private List<int> rebuildWordIndexes(string[] wordsInMnemonicSentence)
    {
        List<int> wordIndexList = new List<int>();
        string[] wordList = englishWordList;

        foreach (string s in wordsInMnemonicSentence)
        {
            int idx = -1;

            if (!wordList.Contains(s))
            {
                throw new Exception("Word " + s + " is not in the wordlist for language " + "english" + " cannot continue to rebuild entropy from wordlist");
            }
            else
            {
                idx = Array.IndexOf(wordList, s);
            }

            wordIndexList.Add(idx);
        }

        return wordIndexList;
    }

    private List<int> rebuildWordIndexesDebug(string[] wordsInMnemonicSentence)
    {
        List<int> wordIndexList = new List<int>();
        string[] wordList = englishWordList;

        foreach (string s in wordsInMnemonicSentence)
        {
            int idx = -1;

            if (!wordList.Contains(s))
            {
                throw new Exception("Word " + s + " is not in the wordlist for language " + "english" + " cannot continue to rebuild entropy from wordlist");
            }
            else
            {
                idx = Array.IndexOf(wordList, s);
            }
            wordIndexList.Add(idx);
        }

        return wordIndexList;
    }

    // Note to self: this funciton should be complete, but needs testing
    private byte[] processWordIndeces(List<int> wordIndex)
    {
        if (wordIndex.Contains(-1))
        {
            throw new Exception("the wordlist index contains -1. Invalid indexes.");
        }

        BitArray bits = new BitArray(wordIndex.Count * bitGroupSize);
        int bitIndex = 0;

        for (int i = 0; i < wordIndex.Count; i++)
        {
            double wordindex = (double)wordIndex[i];

            for (int biti = 0; biti < 11; biti++)
            {
                bits[bitIndex] = false;

                if (wordindex % 2 == 1)
                {
                    bits[bitIndex] = true;
                }

                wordindex = Math.Floor(wordindex / (double)2);

                bitIndex++;
            }

            bool temp = bits.Get(bitIndex - (bitGroupSize));
            bits.Set(bitIndex - (bitGroupSize), bits.Get(bitIndex - 1));
            bits.Set(bitIndex - 1, temp);
            temp = bits.Get(bitIndex - (bitGroupSize - 1));
            bits.Set(bitIndex - (bitGroupSize - 1), bits.Get(bitIndex - 2));
            bits.Set(bitIndex - 2, temp);
            temp = bits.Get(bitIndex - (bitGroupSize - 2));
            bits.Set(bitIndex - (bitGroupSize - 2), bits.Get(bitIndex - 3));
            bits.Set(bitIndex - 3, temp);
            temp = bits.Get(bitIndex - (bitGroupSize - 3));
            bits.Set(bitIndex - (bitGroupSize - 3), bits.Get(bitIndex - 4));
            bits.Set(bitIndex - 4, temp);
            temp = bits.Get(bitIndex - (bitGroupSize - 4));
            bits.Set(bitIndex - (bitGroupSize - 4), bits.Get(bitIndex - 5));
            bits.Set(bitIndex - 5, temp);
        }

        int length = bits.Length - (bits.Length / (entropyMultiple + 1));

        if (length % 8 != 0)
        {
            throw new Exception("Entropy bits less checksum need to be cleanly divisible by " + bitsInByte);
        }

        byte[] entropy = new byte[length / bitsInByte];
        BitArray checksum = new BitArray(bits.Length / (entropyMultiple + 1));
        BitArray checksumActual = new BitArray(bits.Length / (entropyMultiple + 1));

        int index = 0;

        //get entropy bytes
        for (int byteIndex = 0; byteIndex < entropy.Length; byteIndex++)
        {
            for (int i = 0; i < bitsInByte; i++)
            {
                int bitIdx = index % bitsInByte;
                byte mask = (byte)(1 << bitIdx);
                entropy[byteIndex] = (byte)(bits.Get(index) ? (entropy[byteIndex] | mask) : (entropy[byteIndex] & ~mask));
                index++;
            }
        }

        //get remaining bits as checksum bits
        int csindex = 0;

        while (index < bits.Length)
        {
            checksum.Set(csindex, bits.Get(index));
            csindex++;
            index++;
        }

        //calculate checksum of our entropy bytes
        BitArray allChecksumBits = new BitArray(swapEndianBytes(Sha256Process(swapEndianBytes(entropy), 0, entropy.Length))); 

        for (int i = 0; i < checksumActual.Length; i++)
        {
            checksumActual.Set(i, allChecksumBits.Get(i));
        }

        //compare calculated checksum to derived checksum
        foreach (bool b in checksumActual.Xor(checksum))
        {
            if (b)
            {
                throw new Exception("Checksum does not match derived checksum.");
            }
        }

        return entropy;
    }

    private byte[] processWordIndecesNoChecksum(List<int> wordIndex)
    {
        if (wordIndex.Contains(-1))
            throw new Exception("the wordlist index contains -1. Invalid indexes.");

        BitArray bits = new BitArray(wordIndex.Count * bitGroupSize);
        int bitIndex = 0;

        for (int i = 0; i < wordIndex.Count; i++)
        {
            double wordindex = (double)wordIndex[i];
            for (int biti = 0; biti < 11; biti++)
            {
                bits[bitIndex] = false;

                if (wordindex % 2 == 1)
                    bits[bitIndex] = true;

                wordindex = Math.Floor(wordindex / (double)2);
                bitIndex++;
            }

            bool temp = bits.Get(bitIndex - (bitGroupSize));
            bits.Set(bitIndex - (bitGroupSize), bits.Get(bitIndex - 1));
            bits.Set(bitIndex - 1, temp);
            temp = bits.Get(bitIndex - (bitGroupSize - 1));
            bits.Set(bitIndex - (bitGroupSize - 1), bits.Get(bitIndex - 2));
            bits.Set(bitIndex - 2, temp);
            temp = bits.Get(bitIndex - (bitGroupSize - 2));
            bits.Set(bitIndex - (bitGroupSize - 2), bits.Get(bitIndex - 3));
            bits.Set(bitIndex - 3, temp);
            temp = bits.Get(bitIndex - (bitGroupSize - 3));
            bits.Set(bitIndex - (bitGroupSize - 3), bits.Get(bitIndex - 4));
            bits.Set(bitIndex - 4, temp);
            temp = bits.Get(bitIndex - (bitGroupSize - 4));
            bits.Set(bitIndex - (bitGroupSize - 4), bits.Get(bitIndex - 5));
            bits.Set(bitIndex - 5, temp);
        }

        int length = bits.Length - (bits.Length / (entropyMultiple + 1));

        if (length % 8 != 0)
        {
            throw new Exception("Entropy bits less checksum need to be cleanly divisible by " + bitsInByte);
        }

        byte[] entropy = new byte[17];
        int index = 0;

        for (int byteIndex = 0; byteIndex < entropy.Length; byteIndex++)
        {
            for (int i = 0; i < bitsInByte; i++)
            {
                if (index < bits.Length)
                {
                    int bitIdx = index % bitsInByte;
                    byte mask = (byte)(1 << bitIdx);
                    entropy[byteIndex] = (byte)(bits.Get(index) ? (entropy[byteIndex] | mask) : (entropy[byteIndex] & ~mask));
                }

                index++;
            }
        }

        return entropy;
    }

    public Byte[] mergeByteArrays(Byte[] source1, Byte[] source2)
    {
        Byte[] buffer = new Byte[source1.Length + source2.Length];
        System.Buffer.BlockCopy(source1, 0, buffer, 0, source1.Length);
        System.Buffer.BlockCopy(source2, 0, buffer, source1.Length, source2.Length);

        return buffer;
    }

    public static byte[] swapEndianBytes(byte[] bytes)
    {
        byte[] output = new byte[bytes.Length];
        int index = 0;

        foreach (byte b in bytes)
        {
            byte[] ba = { b };
            BitArray bits = new BitArray(ba);

            int newByte = 0;
            if (bits.Get(7)) newByte++;
            if (bits.Get(6)) newByte += 2;
            if (bits.Get(5)) newByte += 4;
            if (bits.Get(4)) newByte += 8;
            if (bits.Get(3)) newByte += 16;
            if (bits.Get(2)) newByte += 32;
            if (bits.Get(1)) newByte += 64;
            if (bits.Get(0)) newByte += 128;

            output[index] = Convert.ToByte(newByte);
            index++;
        }

        return output;
    }

    //  Convert string to byte array
    public byte[] seedToByte(string seedString)
    {
        byte[] seedByte = HexStringToByteArray(seedString);
        return seedByte;
    }

    // Convert byte array to bit array
    public BitArray byteToBits(byte[] bytes)
    {
        var returnBits = new BitArray(bytes);
        return returnBits;
    }

    public byte[] HexStringToByteArray(string hex)
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
    public int GetHexVal(char hex)
    {
        int val = (int)hex;
        return val - (val < 58 ? 48 : (val < 97 ? 55 : 87));
    }

    public static byte[] Sha256Process(byte[] input, int offset, int length)
    {
        var algorithm = new Sha256Digest();
        Byte[] firstHash = new Byte[algorithm.GetDigestSize()];
        algorithm.BlockUpdate(input, offset, length);
        algorithm.DoFinal(firstHash, 0);
        return firstHash;
    }

    public BitArray appendChecksumBits(BitArray bits)
    {
        if (bits.Count != 128)
        {
            Debug.Log("Bit array does not contain exactly 128 bits!");
            return bits;
        }

        int length = bits.Length - (bits.Length / (entropyMultiple + 1));

        //get entropy bytes
        byte[] entropy = new byte[length / bitsInByte];
        int index = 0;
        for (int byteIndex = 0; byteIndex < entropy.Length; byteIndex++)
        {
            for (int i = 0; i < bitsInByte; i++)
            {
                int bitIdx = index % bitsInByte;
                byte mask = (byte)(1 << bitIdx);
                entropy[byteIndex] = (byte)(bits.Get(index) ? (entropy[byteIndex] | mask) : (entropy[byteIndex] & ~mask));
                index++;
            }
        }

        BitArray allChecksumBits = new BitArray(swapEndianBytes(Sha256Process(swapEndianBytes(entropy), 0, entropy.Length)));
        BitArray finalBits = new BitArray(128 + allChecksumBits.Count);

        for (int i = 0; i < 128; i ++)
        {
            finalBits[i] = bits[i];
        }
        for (int i = 0; i < allChecksumBits.Count; i++)
        {
            finalBits[i + 128] = allChecksumBits[i];
        }

        return finalBits;
    }

}
