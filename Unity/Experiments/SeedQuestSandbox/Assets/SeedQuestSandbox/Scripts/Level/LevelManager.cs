using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SeedQuest.Level
{

    public class LevelManager : MonoBehaviour
    {
        static LevelManager instance = null;

        public static LevelManager Instance
        {
            get
            {
                if (instance == null)
                    instance = GameObject.FindObjectOfType<LevelManager>();
                return instance;
            }
        }

        public string levelName;

        static public string LevelName { get { return Instance.levelName; } }

        public int levelIndex = 0;

        static public int LevelIndex { get { return Instance.levelIndex; } }

        /// <summary>  List of Bounds to represent Sites/Zones in a GameLevel </summary>
        public List<BoundingBox> bounds = new List<SeedQuest.Level.BoundingBox>();

        /// <summary>  List of Bounds to represent Sites/Zones in a GameLevel </summary>
        static public List<BoundingBox> Bounds { get { return Instance.bounds; } }

        /// <summary> Reference to player </summary>
        private Transform player;

        public void Start() {

            player = GameObject.FindGameObjectWithTag("Player").transform;
        }

        /// <summary>  Gets the BoundingBox of Site/Zone bound that player is currently in. Returns null if not in one. </summary>
        public static BoundingBox GetBoundingBoxPlayerIsIn() {
            foreach (BoundingBox bb in Instance.bounds) {
                bool inBounds = BoundingBox.InBounds(Instance.player, bb);
                if (inBounds) return bb;
            }

            return null;
        }

        private void OnDrawGizmos() {
            Color[] colors = new Color[6];
            colors[0] = Color.red;
            colors[1] = Color.cyan;
            colors[2] = Color.green;
            colors[3] = new Color(255, 165, 0);
            colors[4] = Color.yellow;
            colors[5] = Color.magenta;

            int count = 0;
            foreach (SeedQuest.Level.BoundingBox bound in bounds) {
                Gizmos.color = colors[count];
                Gizmos.DrawWireCube(bound.center, bound.size);
                count++;
            }
        }
    }
}