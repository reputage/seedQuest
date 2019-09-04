using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using SeedQuest.Interactables;

namespace SeedQuest.SeedEncoder
{
    public class SeedConverter
    {
        private SeedToByte converter = new SeedToByte();

        /// <summary> 
        /// Returns list of interactions generated from encoding a seed string
        /// </summary>
        public Interactable[] encodeSeed(string seedString) {
            InteractableID[] ids = getPathIDs(seedString);
            return getInteractablePath(ids); 
        }

        public List<int> encodeActionIDs(string seedString)
        {
            InteractableID[] ids = getPathIDs(seedString);
            return getActionIds(ids);
        }

        public List<int> getActionIds(InteractableID[] pathIDs)
        {
            List<int> tempIDs = new List<int>();
            for (int i = 0; i < pathIDs.Length; i++)
            {
                tempIDs.Add(pathIDs[i].actionID);
            }
            return tempIDs;
        }

        /// <summary> 
        /// Returns recovered seed which as been decodes from a list of interactions
        /// </summary>
        public string DecodeSeed() {
            int[] encodedInteractions = EncodeInteractions();
            return converter.getSeed(encodedInteractions);
        }

        /// <summary> 
        /// Returns list of Interactable IDs generated from encoding a seed string into
        /// a series of interactable actions.
        /// </summary>
        public InteractableID[] getPathIDs(string seedString) {
            int[] actions = converter.getActions(seedString);
            List<InteractableID> locationIDs = new List<InteractableID>();

            int count = 0;
            while (count < actions.Length)
            {
                int siteID = actions[count];

                for (int j = 0; j < InteractableConfig.ActionsPerSite; j++)
                {
                    int spotID = actions[count + (2 * j) + 1];
                    int actionID = actions[count + (2 * j) + 2];
                    locationIDs.Add(new InteractableID(siteID, spotID, actionID));
                }

                count += 1 + 2 * InteractableConfig.ActionsPerSite;
            }

            return locationIDs.ToArray();
        }

        /// <summary>
        /// Return List of Interactables which represents the Path for an encoded seed
        /// </summary>
        private Interactable[] getInteractablePath(InteractableID[] pathIDs) {

            Interactable[,] LUT = new Interactable[InteractableConfig.SiteCount, InteractableConfig.InteractableCount];

            Interactable[] interactables = InteractableManager.InteractableList;
            for (int i = 0; i < interactables.Length; i++)
            {
                int row = interactables[i].ID.siteID;
                int col = interactables[i].ID.spotID;
                if(0 <= row && row < InteractableConfig.SiteCount && 0 <= col && col < InteractableConfig.InteractableCount)
                    LUT[row, col] = interactables[i];
            }

            Interactable[] interactablePath = new Interactable[pathIDs.Length];
            for (int i = 0; i < pathIDs.Length; i++)
            {
                int row = pathIDs[i].siteID;
                int col = pathIDs[i].spotID;
                interactablePath[i] = LUT[row, col];
                if (interactablePath[i] != null)
                    interactablePath[i].ID.actionID = pathIDs[i].actionID;
            }

            return interactablePath;
        }

        /// <summary>
        /// Encodes an interactable log into an encoded array of interactable id information
        /// </summary>
        private int[] EncodeInteractions() {

            Debug.Log("Items in log: " + InteractableLog.Log.Count);
            // Create a copy of the List and gernerate a list with zero fills if not enought log items
            List<InteractableLogItem> log = new List<InteractableLogItem>();
            foreach (InteractableLogItem item in InteractableLog.Log) {
                log.Add(item);
            }
            while(log.Count < InteractableConfig.ActionsPerGame) {
                log.Add(new InteractableLogItem());
            }

            int totalInt = (2 * InteractableConfig.ActionsPerSite) + InteractableConfig.SitesPerGame;
            int counter = 0;
            List<int> actionLog = new List<int>();

            for (int j = 0; j < InteractableConfig.SitesPerGame; j++)
            {
                actionLog.Add(log[counter].SiteIndex);
                for (int i = 0; i < InteractableConfig.ActionsPerSite; i++)
                {
                    actionLog.Add(log[counter].InteractableIndex);
                    actionLog.Add(log[counter].ActionIndex);
                    counter += 1;
                }
            }

            int[] actionArray = actionLog.ToArray();
            return actionArray;
        }

        /// <summary>
        /// Encodes an list of interactables into a seed string
        /// </summary>
        public string InteractableListToSeed(Interactable[] list) {
            if (list.Length == 0)
                return "";

            int totalInt = (2 * InteractableConfig.ActionsPerSite) + InteractableConfig.SitesPerGame;
            int counter = 0;
            List<int> actionLog = new List<int>();

            for (int j = 0; j < InteractableConfig.SitesPerGame; j++)
            {
                actionLog.Add(list[(counter)].ID.siteID);
                for (int i = 0; i < InteractableConfig.ActionsPerSite; i++)
                {
                    actionLog.Add(list[counter].ID.spotID);
                    actionLog.Add(list[counter].ID.actionID);
                    counter += 1;
                }
            }

            int[] actionArray = actionLog.ToArray();

            //return converter.getSeed108(actionArray);
            return converter.getSeed(actionArray);
        }
    }
}