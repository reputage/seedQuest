using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameUI : MonoBehaviour {

    static private GameUI __instance = null;
    static public GameUI instance
    {
        get
        {
            if (__instance == null)
                __instance = GameObject.FindObjectOfType<GameUI>();
            return __instance;
        }
    }

    public void Update()
    {
        if (GameManager.State == GameState.LoadingRecall)
            GameManager.State = GameState.Recall;
    }

    static public bool CursorUI { 
        set {
            GameUI inst = instance;
            CursorUI ele = instance.GetComponentInChildren<CursorUI>(true);
            GameObject gameobj = ele.gameObject;
            instance.GetComponentInChildren<CursorUI>(true).gameObject.SetActive(value); 
        }
    }

    static public bool StartMenuUI {
        set { instance.GetComponentInChildren<StartMenuUI>(true).gameObject.SetActive(value); }
    }

    static public bool DebugCanvas {
        set { instance.GetComponentInChildren<DebugCanvas>(true).gameObject.SetActive(value); }
    }

    static public bool ActionListCanvas {
        set { instance.GetComponentInChildren<ActionListCanvas>(true).gameObject.SetActive(value);  }
    }

    static public bool InteractiableUI {
        set { instance.GetComponentInChildren<InteractableUI>(true).gameObject.SetActive(value);  }
    }

    static public bool PauseMenuCanvas {
        set { instance.GetComponentInChildren<PauseMenuCanvas>(true).gameObject.SetActive(value); }
    }

    static public bool EndGameUI {
        set { instance.GetComponentInChildren<EndGameUI>(true).gameObject.SetActive(value); }
    }

    static public bool CompassUI {
        set { instance.GetComponentInChildren<compassNSEW>(true).gameObject.SetActive(value);  }
    }

    static public bool MinimapUI {
        set { instance.GetComponentInChildren<MinimapObjects>(true).gameObject.SetActive(value); }
    }

    static public bool LoadingScreen {
        set { instance.GetComponentInChildren<LoadingScreen>(true).gameObject.SetActive(value); }
    }

}
