using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using SeedQuest.Interactables;

public class InteractableAutoCounter : MonoBehaviour
{
    public bool checkInteractableCount;
    public bool finished;
    public string results;
    public string problemScenes = "";

    private int sceneIndex = 0;
    private int success = 0;
    private int failure = 0;
    private int updateDelay;
    private int waitCheck;

    void Start()
    {

    }

    void Update()
    {

        if (checkInteractableCount && updateDelay >= 15)
        {
            updateDelay = 0;
            countAllInteractables();
        }

        updateDelay++;
    }

    public void loadFirstScene()
    {
        checkInteractableCount = true;
        updateDelay = 0;
        waitCheck = 0;
        LevelSetManager.AddLevel(0);
        if (!GameManager.V2Menus)
            MenuScreenManager.Instance.state = MenuScreenStates.Debug;
        SceneManager.LoadScene(DebugSeedUtility.sceneIndeces[0]);
    }

    public void countAllInteractables()
    {
        int count = interactableCount();

        if (count > 0)
            Debug.Log("Current interactable count: " + count);
        
        if (count == 16 && sceneIndex < 16)
        {
            Debug.Log("16 interactables found in this scene!");
            success++;
            waitCheck = 0;
            sceneIndex++;
            if (sceneIndex < 16)
            {
                loadNextScene();
            }
        }
        else if (count < 16 && count > 0 && waitCheck < 5 && sceneIndex < 16)
        {
            Debug.Log("Going to wait for a second to see if more interactables load");
            waitCheck++;
        }
        else if (sceneIndex < 16)
        {
            Debug.Log("Unfortunately, could not find 16 interactables in scene: " + 
                      DebugSeedUtility.sceneIndeces[sceneIndex] + " Interactable count: " + count);
            problemScenes += "\n" + DebugSeedUtility.sceneIndeces[sceneIndex] + " Interactable count: " + count;

            failure++;
            waitCheck = 0;
            sceneIndex++;
            if (sceneIndex < 16)
            {
                loadNextScene();
            }
        }

        if (sceneIndex >= 16)
        {
            Debug.Log("Finished checking all scenes. \nScenes with 16 interactables: " + success);
            Debug.Log("Scenes without 16 interactables: " + failure);

            results = "Scenes with 16 interactables: " + success + "\nScenes without 16 interactables: " + failure + "\n";
            results += "Problematic scenes:" + problemScenes;
            finished = true;
        }
    }

    public void loadNextScene()
    {
        LevelSetManager.AddLevel(sceneIndex);
        if(!GameManager.V2Menus)
            MenuScreenManager.Instance.state = MenuScreenStates.Debug;
        SceneManager.LoadScene(DebugSeedUtility.sceneIndeces[sceneIndex]);
    }


    public int interactableCount()
    {
        int counter = 0;
        Interactable[] items = FindObjectsOfType<Interactable>();
        counter = items.Length;

        return counter;
    }


}
