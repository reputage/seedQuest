using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DebugScreen : MonoBehaviour
{

    public bool debugRandom;
    public bool isDebug;
    public GameObject buttonCard;
    public TextMeshProUGUI counterText;
    public InteractableAutoCounter autoCounter;

    private bool counting;

	private void Update()
	{
        if (counting)
        {
            if (autoCounter.finished)
            {
                setCounterDebugText();
                counting = false;
            }
        }
	}

	public void Back()
    {
        MenuScreenV2.Instance.GoToStart();
    }

    public void GoToEncodeDebugOrdered()
    {
        GameManager.Mode = GameMode.Recall;
        DebugSeedUtility.debugLearnRun = true;
        DebugSeedUtility.debugLearnRand = false;
        MenuScreenV2.Instance.GoToEncodeSeed();
    }

    public void GoToEncodeDebugRand()
    {
        GameManager.Mode = GameMode.Recall;
        DebugSeedUtility.debugLearnRun = true;
        DebugSeedUtility.debugLearnRand = true;
        MenuScreenV2.Instance.GoToEncodeSeed();
    }


    public void autoCountInteractables()
    {
        autoCounter.loadFirstScene();
        GoToWaitingForCounter();
    }

    public void GoToWaitingForCounter()
    {
        buttonCard.SetActive(false);
        counting = true;
        activateCounterText();
    }

    public void setCounterDebugText()
    {
        counterText.text = autoCounter.results;
    }

    public void activateCounterText()
    {
        counterText.gameObject.SetActive(true);
    }

    public static void SetEncodeSeedDebugCanvas()
    {
        //Instance.startDebugCanvas.gameObject.SetActive(true);
    }

}
