using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeedConverter {

    private SeedToByte converter = new SeedToByte();

    /// <summary> 
    /// Returns list of interactions generated from encoding a seed string
    /// </summary>
    public Interactable[] encodeSeed(string seedString)
    {
        return getInteractablePath(getPathIDs(seedString));
    }

    /// <summary> 
    /// Returns recovered seed which as been decodes from a list of interactions
    /// </summary>
    public string DecodeSeed(InteractableLog log)
    {
        int[] encodedInteractions = EncodeInteractions(log);
        return converter.getSeed(encodedInteractions);
    }

    /// <summary> 
    /// Returns list of Interactable IDs generated from encoding a seed string into
    /// a series of interactable actions.
    /// </summary>
    private InteractableID[] getPathIDs(string seedString) {
        int[] actions = converter.getActions(seedString);
        List<InteractableID> locationIDs = new List<InteractableID>();

        int count = 0;
        while (count < actions.Length)
        {
            int siteID = actions[count];

            for (int j = 0; j < SeedManager.ActionCount; j++)
            {
                int spotID = actions[count + (2 * j) + 1];
                int actionID = actions[count + (2 * j) + 2];
                locationIDs.Add(new InteractableID(siteID, spotID, actionID));
            }

            count += 1 + 2 * SeedManager.ActionCount;
        }

        return locationIDs.ToArray();
    }

    /// <summary>
    /// Return List of Interactables which represents the Path for an encoded seed
    /// </summary>
    private Interactable[] getInteractablePath(InteractableID[] pathIDs) {

        int siteCount = (int)Mathf.Pow(2.0F, SeedManager.SiteBits);
        int spotCount = (int)Mathf.Pow(2.0F, SeedManager.SpotBits);
        Interactable[,] LUT = new Interactable[siteCount, spotCount];

        Interactable[] interactables = InteractableManager.InteractableList;
        for (int i = 0; i < interactables.Length; i++) {
            int row = interactables[i].ID.siteID;
            int col = interactables[i].ID.spotID;
            LUT[row, col] = interactables[i];
        }

        Interactable[] interactablePath = new Interactable[pathIDs.Length];
        for (int i = 0; i < pathIDs.Length; i++) {
            int row = pathIDs[i].siteID;
            int col = pathIDs[i].spotID;
            interactablePath[i] = LUT[row, col];
        }

        return interactablePath;
    }

    /// <summary>
    /// Encodes an interactable log into an encoded array of interactable id information
    /// </summary>
    private int[] EncodeInteractions(InteractableLog log)
    {
        int totalInt = (2 * SeedManager.ActionCount) + SeedManager.SiteCount;
        int counter = 0;
        List<int> actionLog = new List<int>();

        for (int j = 0; j < SeedManager.SiteCount; j++)
        {
            actionLog.Add(log.interactableLog[(counter)].ID.siteID);
            for (int i = 0; i < SeedManager.ActionCount; i++)
            {
                actionLog.Add(log.interactableLog[counter].ID.spotID);
                actionLog.Add(log.actionLog[counter]);
                counter += 1;
            }
        }

        int[] actionArray = actionLog.ToArray();
        return actionArray;
    }
}