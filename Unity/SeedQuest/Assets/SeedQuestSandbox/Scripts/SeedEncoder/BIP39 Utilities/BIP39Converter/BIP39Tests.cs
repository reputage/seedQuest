using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SeedQuest.SeedEncoder;


public class BIP39Tests
{
    private SeedToByte seeds = new SeedToByte();
    private BIP39Converter bpc = new BIP39Converter();

    // Run all tests at once
    public string runAllTests()
    {
        int[] passed = new int[2];

        sumTest(ref passed, testGetSentence());
        sumTest(ref passed, testGetActions());
        sumTest(ref passed, testFullConversion());
        sumTest(ref passed, testHexConversion());
        sumTest(ref passed, testChecksumConversion());
        sumTest(ref passed, testAllSeedWords());

        string passedString = "Successfully passed " + passed[0] + " of " + passed[1] + " tests.";
        Debug.Log(passedString);
        return passedString;
    }

    // This function helps make the test running code a bit cleaner
    public void sumTest(ref int[] passed, int[] testPassed)
    {
        if (passed.Length < 2 || testPassed.Length < 2)
            Debug.Log("Error summing test results: int[] shorter than two elements");

        passed[0] += testPassed[0];
        passed[1] += testPassed[1];
    }

    // Test to make sure a BIP39 sentence can be retrieved from a list of actions
    public int[] testGetSentence()
    {
        int[] passed = new int[2];
        passed[1] = 1;

        string testingHex = "3720B091810D8127C55630F55DD2275C05";
        int[] actions = seeds.getActions(testingHex);
        string words = bpc.getSentenceFromActions(actions);

        if (words == "ugly call give address amount venture misery dose quick spoil weekend inspire")
            passed[0] = 1;
        else
            Debug.Log("BIP39 test converting actions to sentence failed");

        return passed;
    }

    // Test to make sure actions can be retrieved from a BIP39 sentence
    public int[] testGetActions()
    {
        int[] passed = new int[2];
        passed[1] = 1;

        string testingHex = "3720B091810D8127C55630F55DD2275C05";
        string testWords = "ugly call give address amount venture misery dose quick spoil weekend inspire";
        int[] actions = bpc.getActionsFromSentence(testWords);
        string seed = seeds.getSeed(actions);

        if (testingHex == seed)
            passed[0] = 1;
        else
            Debug.Log("BIP39 test converting sentence to actions failed");

        return passed;
    }

    public int[] testHexConversion()
    {
        int[] passed = new int[2];
        passed[1] = 2;

        string hex = "3720B091810D8127C55630F55DD2275C05";
        string hardSentence = "ugly call give address amount venture misery dose quick spoil weekend inspire";
        string sentence = bpc.getSentenceFromHex(hex);

        if (hex == bpc.getHexFromSentence(sentence))
            passed[0] += 1;
        else
            Debug.Log("BIP39 test converting hex to BIP39 sentence and back failed");
        if (hardSentence == sentence)
            passed[0] += 1;
        else
            Debug.Log("BIP39 test converting sentence to hex and back failed");

        return passed;
    }

    public int[] testChecksumConversion()
    {
        int[] passed = new int[2];
        passed[1] = 1;

        string testingHex = "3720B091810D8127C55630F55DD2275C";
        string testWords = "ugly call give address amount venture misery dose quick spoil weekend insane";

        byte[] seedBytes = bpc.HexStringToByteArray(testingHex);
        string ChecksumSentence = bpc.getSentence128Bits(seedBytes);

        Debug.Log("Checksum sentence: " + ChecksumSentence);

        if (testWords == ChecksumSentence)
            passed[0] = 1;
        else
            Debug.Log("BIP39 test converting sentence to actions with checksum failed");

        return passed;
    }

    // Test converting from a BIP39 sentence to actions and back
    public int[] testFullConversion()
    {
        int[] passed = new int[2];
        passed[1] = 1;

        string testWords = "ugly call give address amount venture misery dose quick spoil weekend inspire";
        int[] actions = bpc.getActionsFromSentence(testWords);
        string sentence = bpc.getSentenceFromActions(actions);

        if (sentence == testWords)
            passed[0] = 1;
        else
            Debug.Log("BIP39 full conversion test (sentence -> actions -> sentence) failed");

        return passed;
    }

    // Test converting from a BIP39 sentence to actions and back
    public int[] testFullConversion(string input, int iter)
    {
        int[] passed = new int[2];
        passed[1] = 1;

        int[] actions = bpc.getActionsFromSentence(input);
        List<int> wordListSizes = new List<int> { 11, 11, 11, 11, 11, 11, 11, 11, 11, 11, 11, 11 };
        string sentence = bpc.getSentenceFromActionsDebug(actions);

        if (sentence == input)
            passed[0] = 1;
        else
        {
            Debug.Log("BIP39 custom conversion test (sentence -> actions -> sentence) failed." +
                      "\nSentence: " + input + "\nRecovered: " + sentence + "\n Value: " + iter);
        }

        return passed;
    }

    public int[] testAllSeedWords()
    {
        int[] passed = new int[2];
        passed[1] = 0;

        for (int i = 1; i < EnglishWordList.words.Length; i++) 
        {
            string sentence = EnglishWordList.words[i];
            sentence += " " + EnglishWordList.words[i]; 
            sentence += " " + EnglishWordList.words[i]; 
            sentence += " " + EnglishWordList.words[i]; 
            sentence += " " + EnglishWordList.words[i]; 
            sentence += " " + EnglishWordList.words[i]; 
            sentence += " " + EnglishWordList.words[i]; 
            sentence += " " + EnglishWordList.words[i]; 
            sentence += " " + EnglishWordList.words[i]; 
            sentence += " " + EnglishWordList.words[i];
            sentence += " " + EnglishWordList.words[i]; 
            sentence += " " + EnglishWordList.words[i];

            sumTest(ref passed, testFullConversion(sentence, i));
        }

        return passed;
    }
}
