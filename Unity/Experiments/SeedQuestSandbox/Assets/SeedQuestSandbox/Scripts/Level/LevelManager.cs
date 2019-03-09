using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

using SeedQuest.Interactables;

namespace SeedQuest.Level
{
    public class LevelManager : MonoBehaviour {
        static LevelManager instance = null;

        public static LevelManager Instance {
            get  {
                if (instance == null)
                    instance = GameObject.FindObjectOfType<LevelManager>();
                return instance;
            }
        }

        /// <summary> Level Name </summary>
        public string levelName;

        /// <summary> Level Name </summary>
        static public string LevelName { get => Instance.levelName;  }

        /// <summary> Level Index Offset for use in InteractablePath calculations </summary>
        public int levelIndex = 0;

        /// <summary> Level Index Offset for use in InteractablePath calculations </summary>
        static public int LevelIndex { get => Instance.levelIndex; }

        public string levelSelectScene = "SceneSelect";

        static string LevelSelectScene { get => Instance.levelSelectScene; }

        /// <summary> MultiLevelGame Flag important for InteractablePath calculations </summary>
        public bool isMultiLevelGame = false;

        /// <summary> MultiLevelGame Flag important for InteractablePath calculations </summary>
        static public bool IsMultiLevelGame { get => Instance.isMultiLevelGame; }

        /// <summary>  List of Bounds to represent Sites/Zones in a GameLevel </summary>
        public List<BoundingBox> bounds = new List<BoundingBox>();

        /// <summary>  List of Bounds to represent Sites/Zones in a GameLevel </summary>
        static public List<BoundingBox> Bounds { get => Instance.bounds; }

        /// <summary> Reference to player </summary>
        private Transform player;

        public void Awake() {
            GameManager.State = GameState.Play;

            var playerObject = GameObject.FindGameObjectWithTag("Player");
            if (playerObject != null)
                player = playerObject.transform;
        }

        public void Start() {
            if (InteractableManager.InteractableList.Length == 0)
                return;

            InteractablePathManager.SetupInteractablePathIDs();

            if (!isMultiLevelGame)
                InteractablePathManager.InitalizePathAndLog();
            else if (InteractablePathManager.IsPathInitialized)
                InteractablePathManager.InitalizePathAndLogForMultiLevelGame();
            else
                InteractablePathManager.InitalizePathAndLog();
        }

        private void Update() {
            ListenForKeyDown();
        }

        public void ListenForKeyDown() {
            if (!isMultiLevelGame)
                return;

            bool goSceneSelect = InteractablePath.PathLevelComplete || InteractablePath.Instance.nextIndex == 0;
            if (goSceneSelect && InputManager.GetKeyDown(KeyCode.H)) {
                SceneManager.LoadScene(levelSelectScene);
            }
        }

        public void GoToSceneSelect() {
            SceneManager.LoadScene(levelSelectScene);
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
            foreach (BoundingBox bound in bounds) {
                Gizmos.color = colors[count];
                Gizmos.DrawWireCube(bound.center, bound.size);
                count++;
            }
        }
    }
}