using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameUI : MonoBehaviour {
    public static GameUI instance = null;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);

    }

    public void Update()
    {
        if (GameManager.State == GameState.LoadingRecall)
            GameManager.State = GameState.Recall;
    }

    static public bool CursorUI { 
        set { instance.GetComponentInChildren<CursorUI>(true).gameObject.SetActive(value); }
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


}
