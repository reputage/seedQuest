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
        static public int ActionsPerSite = 3; //4;

        /// <summary> Number of Levels to do per Game </summary>
        static public int SitesPerGame = 2;        

        /// <summary> Number of Actions per Game </summary>
        static public float ActionsPerGame { get { return ActionsPerSite * SitesPerGame; } }

        /// <summary>  Number of Bits used to encode a seed with given config parameters  </summary>
        static public int BitEncodingCount { get => (SiteBits + (InteractableBits + ActionBits) * ActionsPerSite) * SitesPerGame; } 

        /// <summary>  Hex string length for a Seed (includes parital hex character) </summary>
        static public float SeedHexSize { get => BitEncodingCount / 4.0f; } 

        /// <summary> Hex string length for a Seed  </summary>
        static public int SeedHexLength { get => Mathf.CeilToInt(BitEncodingCount / 4.0f); }
    }
}