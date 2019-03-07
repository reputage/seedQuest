using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SeedQuest.Interactables
{
    /// <summary>
    /// Interactable Configurations for SeedQuest
    /// </summary>
    [System.Serializable]
    public class InteractableConfig
    {
        /// <summary> Numer of Bits for Levels/Sites in Game  </summary>
        static public int SiteBits = 1; //4;

        /// <summary> Numer of Levels/Sites in Game  </summary>
        static public int SiteCount { get { return (int)Mathf.Pow(2.0F, SiteBits); } }

        /// <summary> Number of Bits for Interactables per Level  </summary>
        static public int InteractableBits = 3; //5;

        /// <summary> Number of Interactables per Level  </summary>
        static public int InteractableCount { get { return (int)Mathf.Pow(2.0F, InteractableBits); } }

        /// <summary> Number of Bits for Actions per Interactable </summary>
        static public int ActionBits = 2;

        /// <summary> Number of Actions per Interactable </summary>
        static public int ActionCount { get { return (int)Mathf.Pow(2.0F, ActionBits); } }

        /// <summary> Number of Actions to do per Level </summary>
        static public int ActionsPerSite = 4;

        /// <summary> Number of Levels to do per Game </summary>
        static public int SitesPerGame = 2;        

        /// <summary> Nubmer of Actions per Game </summary>
        static public float ActionsPerGame { get { return ActionsPerSite * SitesPerGame; } }
    }
}