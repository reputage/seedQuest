using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;
using UnityEngine;

public class BIP39Processor
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

    public List<int> rebuildWordIndexes(string[] wordsInMnemonicSentence)
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

    public byte[] processWordIndeces(List<int> wordIndex)
    {
        BitArray bits = getBitsFromWords(wordIndex);
        int length = bits.Length - (bits.Length / (entropyMultiple + 1));

        if (length % 8 != 0)
        {
            throw new Exception("Entropy bits less checksum need to be cleanly divisible by " + bitsInByte);
        }

        byte[] entropy = getEntropyFromBits(bits);
        BitArray checksum = new BitArray(bits.Length / (entropyMultiple + 1));
        BitArray checksumActual = new BitArray(bits.Length / (entropyMultiple + 1));

        int index = 0;
        for (int byteIndex = 0; byteIndex < entropy.Length; byteIndex++)
            for (int i = 0; i < bitsInByte; i++)
                index++;

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

    public byte[] processWordIndecesNoChecksum(List<int> wordIndex)
    {
        BitArray bits = getBitsFromWords(wordIndex);
        int length = bits.Length - (bits.Length / (entropyMultiple + 1));

        if (length % 8 != 0)
        {
            throw new Exception("Entropy bits less checksum need to be cleanly divisible by " + bitsInByte);
        }

        return getEntropyFromBits(bits);
    }

    public byte[] processWordShortIndeces(List<int> wordIndex)
    {
        BitArray bits = getBitsFromWords(wordIndex);
        int length = bits.Length - (bits.Length / (entropyMultiple + 1));
        return getEntropyFromBits(bits);
    }

    // Returns a bit array representing the values of the word indeces
    public static BitArray getBitsFromWords(List<int> wordIndex)
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

        return bits;
    }

    public static byte[] getEntropyFromBits(BitArray bits)
    {
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

        for (int i = 0; i < 128; i++)
            finalBits[i] = bits[i];

        for (int i = 0; i < allChecksumBits.Count; i++)
            finalBits[i + 128] = allChecksumBits[i];

        return finalBits;
    }

}
