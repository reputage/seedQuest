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
        sumTest(ref passed, testShortSeedPhrases());
        sumTest(ref passed, testGetSentenceShort());
        sumTest(ref passed, testGetSentenceSitesBased());

        string passedString = "Successfully passed " + passed[0] + " of " + passed[1] + " BIP39 tests.";
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

        //Debug.Log("Checksum sentence: " + ChecksumSentence);

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
        string sentence = bpc.getSentenceFromActions(actions);

        if (sentence == input)
            passed[0] = 1;
        else
        {
            Debug.Log("BIP39 custom conversion test (sentence -> actions -> sentence) failed." +
                      "\nSentence: " + input + "\nRecovered: " + sentence + "\n Value: " + iter);
        }

        return passed;
    }

    // Test to ensure the conversion proccess is functional with all words in the BIP39 word list
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

    // Test to ensure seed phrases shorter than 12 words can be succesfully recoveered
    public int[] testShortSeedPhrases()
    {
        int[] passed = new int[2];
        passed[1] = 7;

        string testWords2 = "ugly call";
        string testWords3 = "armed canvas hand";
        string testWords4 = "ugly call give address";
        string testWords5 = "armed canvas hand burst grunt";
        string testWords6 = "ugly call give address amount venture";
        string testWords8 = "armed canvas hand burst grunt leopard wall garlic";
        string testWords10 = "ugly call give address amount venture misery dose quick spoil";

        int[] actions2 = bpc.getActionsFromShortSentence(testWords2);
        int[] actions3 = bpc.getActionsFromShortSentence(testWords3);
        int[] actions4 = bpc.getActionsFromShortSentence(testWords4);
        int[] actions5 = bpc.getActionsFromShortSentence(testWords5);
        int[] actions6 = bpc.getActionsFromShortSentence(testWords6);
        int[] actions8 = bpc.getActionsFromShortSentence(testWords8);
        int[] actions10 = bpc.getActionsFromShortSentence(testWords10);

        string sentence2 = bpc.getSentenceFromShortActions(actions2, 2);
        string sentence3 = bpc.getSentenceFromShortActions(actions3, 3);
        string sentence4 = bpc.getSentenceFromShortActions(actions4, 4);
        string sentence5 = bpc.getSentenceFromShortActions(actions5, 5);
        string sentence6 = bpc.getSentenceFromShortActions(actions6, 6);
        string sentence8 = bpc.getSentenceFromShortActions(actions8, 8);
        string sentence10 = bpc.getSentenceFromShortActions(actions10, 10);

        if (sentence2 == testWords2)
            passed[0] += 1;
        else
            Debug.Log("BIP39 full conversion test for two words failed " + sentence2);

        if (sentence3 == testWords3)
            passed[0] += 1;
        else
            Debug.Log("BIP39 full conversion test for three words failed " + sentence3);

        if (sentence4 == testWords4)
            passed[0] += 1;
        else
            Debug.Log("BIP39 full conversion test for four words failed " + sentence4);

        if (sentence5 == testWords5)
            passed[0] += 1;
        else
            Debug.Log("BIP39 full conversion test for five words failed " + sentence5);

        if (sentence6 == testWords6)
            passed[0] += 1;
        else
            Debug.Log("BIP39 full conversion test for six words failed " + sentence6);

        if (sentence8 == testWords8)
            passed[0] += 1;
        else
            Debug.Log("BIP39 full conversion test for eight words failed " + sentence8);

        if (sentence10 == testWords10)
            passed[0] += 1;
        else
            Debug.Log("BIP39 full conversion test for ten words failed " + sentence10);

        return passed;
    }

    // Test to make sure a BIP39 sentence can be retrieved from a list of actions
    public int[] testGetSentenceShort()
    {
        int[] passed = new int[2];
        passed[1] = 2;

        string testWords = "ugly call give address amount venture";
        int[] actions = bpc.getActionsFromShortSentence(testWords);

        int[] subActionArray = new int[21];
        for (int i = 0; i < subActionArray.Length; i++)
        {
            subActionArray[i] = actions[i];
        }

        string words = bpc.getSentenceFromShortActions(actions, 6);

        if (words == "ugly call give address amount venture")
            passed[0] = 1;
        else
            Debug.Log("BIP39 test converting short actions (6 words) to sentence failed");


        testWords = "venture";
        actions = bpc.getActionsFromShortSentence(testWords);
        subActionArray = new int[7];
        for (int i = 0; i < subActionArray.Length; i++)
            subActionArray[i] = actions[i];
        words = bpc.getSentenceFromShortActions(actions, 1);

        if (words == "venture")
            passed[0] += 1;
        else
            Debug.Log("BIP39 test converting short actions (1 word) to sentence failed");


        return passed;
    }

    // Test to make sure retrieving a short seed phrase based on number of sites pere game is functional
    public int[] testGetSentenceSitesBased()
    {
        int[] passed = new int[2];
        passed[1] = 1;

        string testingHex = "3720B091810D8127C55630F55DD2275C05";
        int[] actions = seeds.getActions(testingHex);
        string words = bpc.getSentenceSiteBased(actions);

        if (words == "ugly call give address amount venture misery dose quick spoil weekend inspire")
            passed[0] = 1;
        else
            Debug.Log("BIP39 test converting actions to sentence failed");

        return passed;
    }
}
