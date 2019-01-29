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
        /// <summary> Numer of Levels/Sites in Game  </summary>
        static public int LevelCount = 16;
        /// <summary> Number of Interactables per Level  </summary>
        static public int InteractableCount = 32;
        /// <summary> Number of Actions per Interactable </summary>
        static public int ActionCount = 4;
        /// <summary> Number of Actions to do per Level </summary>
        static public int ActionsDoTo = 4;
        /// <summary> Number of Levels to do per Game </summary>
        static public int LevelDoTo = 2;
    }
}