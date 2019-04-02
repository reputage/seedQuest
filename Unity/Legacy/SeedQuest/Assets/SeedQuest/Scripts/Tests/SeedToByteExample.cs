using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeedToByteExample : MonoBehaviour
{

    private SeedToByte seedToByte = new SeedToByte();

    // To get the int[] with values for site, spot, and action
    public void getDataFromSeed()
    {
        // This will use the default bit values in SeedManager

        string seed = "FFFFFF"; // seed must be in hexidecimal
        int[] interactableData = seedToByte.getActions(seed);

    }

    // To use different bit values, either use another list or make one using customList()
    public void getDataFromSeedCustom()
    {
        string seed = "FFFFFF"; // seed must be in hexidecimal

        int siteBits = 2;
        int spotBits = 3;
        int actionBits = 2;
        int numberOfActions = 2;
        int numberOfSites = 2;

        List<int> customBitValues = SeedToByte.customList(siteBits, spotBits, actionBits, numberOfActions, numberOfSites);
        int[] interactableData = seedToByte.getActions(seed, customBitValues);
    }


    // To get the seed from the player's actions, including site, spot and action values
    public void getSeedFromData()
    {
        int[] playerChoices = new int[24];

        // Get the seed from the player's choices
        string seed = seedToByte.getSeed(playerChoices);

        //Debug.Log("Seed: " + seed);
    }

    public void getSeedFromDataCustom()
    {
        int[] playerChoices = new int[24];

        // using a custom list:
        List<int> customBitValues = SeedToByte.customList(2, 3, 2, 2, 2);
        string seed = seedToByte.getSeed(playerChoices, customBitValues);

        //Debug.Log("Seed: " + seed);
    }

}