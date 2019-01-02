using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SaveDids 
{

    // Adds a did with a label name to the dictionary of user dids, then saves the data in a file
    static public void saveDidData(Dictionary<string, string> userDids, string name, string did)
    {
        if (userDids.ContainsKey(name))
        {
            Debug.Log("Error: A did with that name already exists");
            return;
        }
        else if (name == "" || did == "")
        {
            Debug.Log("Error: Name or did seems to be empty");
            return;
        }
        else
        {
            userDids.Add(name, did);
        }
        SaveSettings.saveSettings();
    }

    // Save seed data - necessary to rehearse the seed's route later
    //  Note to self: should probably encrypt this in some way, just so it isn't stored in plain text
    static public void saveSeedData(Dictionary<string, string> userSeeds, string name, string seed)
    {
        if (userSeeds.ContainsKey(name))
        {
            Debug.Log("Error: A seed with that name already exists");
            return;
        }
        else if (name == "" || seed == "")
        {
            Debug.Log("Error: Name or seed seems to be empty");
            return;
        }
        else
        {
            userSeeds.Add(name, seed);
        }
        SaveSettings.saveSettings();
    }

    // Remove the seed data from the saved data file - this will render rehearse mode 
    //  impossible for the chosen seed once this is performed
    static public void removeSeedData(Dictionary<string, string> userSeeds, string name)
    {
        if (name == "")
        {
            Debug.Log("Error: Name of seed seems to be empty");
            return;
        }
        else if (userSeeds.ContainsKey(name))
        {
            // Note to self: add function here to "purge" the original seed data
            userSeeds.Remove(name);
        }
        SaveSettings.saveSettings();
    }

    // Encrypt the seed before saving to file
    static public string xorSeed(string seed, string pad)
    {
        // encrypt the seed in some way
        // seed = encryption(seed);
        if(seed.Length <= 0)
        {
            Debug.Log("Error: Seed length is zero!");
            return seed;
        }
        if(seed.Length > pad.Length)
        {
            Debug.Log("Error: Encryption pad is not long enough for this seed!");
            return seed;
        }
        else
        {
            char[] seedArray = seed.ToCharArray();
            char[] padArray = pad.ToCharArray();
            char[] result = new char[seedArray.Length];

            for (int i = 0; i < result.Length; i++)
            {
                result[i] = (char)(seedArray[i] ^ padArray[i]);
            }
            return result.ToString();
        }
    }


    static public void testXorSeed()
    {
        string seed = "qwertyuiopasdfghjklzxcvbnm";
        string empty = "";
        string pad1 = "mnbvcxzasdfghjpoiuyqwertlk";
        string pad2 = "poiuytrewqlkjhgfdsa";

        string xorSeed1 = xorSeed(seed, pad1);
        string xorSeed2 = xorSeed(xorSeed1, pad1);

        string xorFail1 = xorSeed(seed, pad2);
        string xorFail2 = xorSeed(empty, pad1);

        if (xorSeed1 == xorSeed2)
        {
            Debug.Log("xorSeed test for seed encryption and decryption passed");
        }
        else
        {
            Debug.Log("xorSeed test for seed encryption and decryption failed");    
        }

        if (xorFail1 == seed)
        {
            Debug.Log("xorSeed test for handling short pad passed");
        }
        else
        {
            Debug.Log("xorSeed test for handling short pad failed");
        }

        if(xorFail2 == empty)
        {
            Debug.Log("xorSeed test for handling empty seed passed");
        }
        else
        {
            Debug.Log("xorSeed test for handling empty seed failed");    
        }
    }
}
