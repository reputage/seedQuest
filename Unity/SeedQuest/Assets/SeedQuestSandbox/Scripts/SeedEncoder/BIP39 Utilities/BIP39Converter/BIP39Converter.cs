using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using SeedQuest.SeedEncoder;
using SeedQuest.Interactables;

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
    private BIP39Processor bipPro = new BIP39Processor();

    // Take a BIP39 sentence, and return the corresponding seedquest actions
    public int[] getActionsFromSentence(string sentence)
    {
        string[] wordArray = sentence.Split(null);

        if (wordArray.Length < 12)
        {
            Debug.Log("Not enough words for 132 bits of entropy.");
            throw new Exception("Less than 12 words in this sentence. Not a valid seed.");
        }

        List<int> indeces = bipPro.rebuildWordIndexes(wordArray);
        byte[] bytes = bipPro.processWordIndecesNoChecksum(indeces);
        int[] actions = seeds.getActionsFromBytes(bytes);

        return actions;
    }

    // Take a short BIP39 sentence (< 12 words), and return the corresponding seedquest actions
    public int[] getActionsFromShortSentence(string sentence)
    {
        string[] wordArray = sentence.Split(null);
        List<int> indeces = bipPro.rebuildWordIndexes(wordArray);
        byte[] bytes = bipPro.processWordShortIndeces(indeces);
        int[] actions = seeds.getActionsFromBytes(bytes);

        return actions;
    }

    // Take a BIP39 sentence, and return the corresponding seedquest actions, checking to 
    //  ensure the checksum matches the rest of the seed phrase
    public int[] getActionsWithChecksum(string sentence)
    {
        string[] wordArray = sentence.Split(null);

        if (wordArray.Length < 12)
        {
            Debug.Log("Not enough words for 132 bits of entropy.");
            throw new Exception("Less than 12 words in this sentence. Not a valid seed.");
        }

        List<int> indeces = bipPro.rebuildWordIndexes(wordArray);
        byte[] bytes = new byte[1];

        try
        {
            bytes = bipPro.processWordIndeces(indeces);
        }
        catch (Exception e)
        {
            Debug.Log("Exception: " + e);
        }

        List<int> wordListSizes = new List<int> { 11, 11, 11, 11, 11, 11, 11, 11, 11, 11, 11, 11 };

        int[] actions = seeds.getActionsFromBytes(bytes);
        return actions;
    }

    // Retrieve a BIP39 seed phrase from player actions 
    public string getSentenceFromActions(int[] actions)
    {
        string seed = seeds.getSeed(actions);
        byte[] seedBytes = HexStringToByteArray(seed);
        BitArray bits = byteToBits(seedBytes);
        string words = convertBitsToWords(bits);
        return words;
    }

    // Retrieve a BIP39 seed phrase from only 128 bits of actions, using a checksum 
    public string getSentence128Bits(byte[] seedBytes)
    {
        BitArray bits = byteToBits(seedBytes);

        if (bits.Count != 128)
        {
            Debug.Log("Invalid actions! This does not represent exactly 128 bits.");
        }

        BitArray bitsWithChecksum = bipPro.appendChecksumBits(bits);

        string words = convertBitsToWords(bitsWithChecksum);
        return words;
    }

    // Retrieve a non-normal sized BIP39 seed phrase based on the sites peere game config variable
    public string getSentenceSiteBased(int[] actions)
    {
        int wordCount = InteractableConfig.SitesPerGame;
        wordCount = wordCount * 2;
        return getSentenceFromShortActions(actions, wordCount);
    }

    // Retrieve a shorter-than-normal BIP39 seed phrase, specifying the number of words desired
    public string getSentenceFromShortActions(int[] actions, int wordCount)
    {
        string words = getSentenceFromActions(actions);
        string[] wordArray = words.Split(null);
        words = "";
        for (int i = 0; i < wordCount; i++)
        {
            if (i == 0)
                words += wordArray[i];
            else
                words += " " + wordArray[i];
        }

        return words;
    }

    // Takes a bitarray, and returns the corresponding BIP39 seed sentence
    public string convertBitsToWords(BitArray bits)
    {
        List<int> wordListSizes = new List<int> { 11, 11, 11, 11, 11, 11, 11, 11, 11, 11, 11, 11 };
        int[] wordIndeces = seeds.bitToActions(bits, wordListSizes);
        List<int> wordIndecesList = new List<int>();

        for (int i = 0; i < wordIndeces.Length; i++)
            wordIndecesList.Add(wordIndeces[i]);

        string words = getMnemonicSentence(wordIndecesList);
        return words;
    }

    // Take a BIP39 sentence, and return the bits as a hex string
    public string getHexFromSentence(string sentence)
    {
        string[] wordArray = sentence.Split(null);
        bool useChecksum = false;

        if (wordArray.Length < 12)
        {
            Debug.Log("Not enough words for 132 bits of entropy.");
            throw new Exception("Less than 12 words in this sentence. Not a valid seed.");
        }

        string hexSeed = convertIndecesToHex(wordArray, useChecksum);
        return hexSeed;
    }

    // Take a BIP39 sentence, and return the bits as a hex string
    public string getHexFromShortSentence(string sentence, int wordCount)
    {
        string[] wordArray = sentence.Split(null);
        bool useChecksum = false;

        string hexSeed = convertIndecesToHex(wordArray, useChecksum, wordCount);
        return hexSeed;
    }

    // Take a BIP39 sentence, and return the bits as a hex string, while regenerating the checksum bits
    public string getHexWithChecksum(string sentence)
    {
        string[] wordArray = sentence.Split(null);
        bool useChecksum = true;

        if (wordArray.Length < 12)
        {
            Debug.Log("Not enough words for 128 bits of entropy.");
            throw new Exception("Less than 12 words in this sentence. Not a valid seed.");
        }

        string hexSeed = convertIndecesToHex(wordArray, useChecksum);
        return hexSeed;
    }

    // Takes a string array for a BIP39 sentence, returns the corresponding hex string
    public string convertIndecesToHex(string[] wordArray, bool useChecksum, int wordCount = 0)
    {
        List<int> indeces = bipPro.rebuildWordIndexes(wordArray);
        byte[] bytes;
        if (wordCount != 0)
        {
            bytes = bipPro.processWordShortIndeces(indeces);
        }
        else if (useChecksum)
            bytes = bipPro.processWordIndeces(indeces);
        else
            bytes = bipPro.processWordIndecesNoChecksum(indeces);
        
        int[] actions = seeds.getActionsFromBytes(bytes);
        string hexSeed = seeds.getSeed(actions);

        return hexSeed;
    }

    // Takes a hex string, returns the corresponding BIP39 sentence
    public string getSentenceFromHex(string hex)
    {
        int[] actions = seeds.getActions(hex);
        string words = getSentenceFromActions(actions);

        return words;
    }

    // Takes a hex string, returns the corresponding BIP39 sentence
    public string getShortSentenceFromHex(string hex, int wordCount)
    {
        int[] actions = seeds.getActions(hex);
        string words = getSentenceFromShortActions(actions, wordCount);

        return words;
    }

    // Take a list of word indeces and return the mnemonic sentence
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
}
