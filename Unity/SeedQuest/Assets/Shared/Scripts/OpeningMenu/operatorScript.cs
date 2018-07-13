using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class operatorScript : MonoBehaviour {

    /* 
       This script is used for controlling elements in the main menu
       of the start screen in scene 0.
    */

    public GameObject AllScene;
    public GameObject SeedToByte;
    public GameObject SceneChange;

    public GameObject buttonDefault;
    public GameObject buttonMapWord;
    public GameObject buttonGenerate;
    public GameObject buttonBack;
    public GameObject mapInputField;

    public GameObject buttonRehearse;
    public GameObject buttonRecall;
    public GameObject buttonBackRehearse;
    public GameObject buttonRehearseGo;
    public GameObject keySeedInput;


    static string mapWord = "/!0";
    static string keySeed = "/!0";


	void Start () {
        //Debug.Log("Game begin OK");
	}
	
    // For the "default map" button
    public void defaultClick(){
        //Debug.Log("Default button clicked!");
        //UnityEngine.SceneManagement.SceneManager.LoadScene(1);
        SceneChange.GetComponent<SceneChange>().sceneChange(1);
    }

    // For the "Recall mode" button
    public void recallClick()
    {
        Debug.Log("recall button clicked!");
        //UnityEngine.SceneManagement.SceneManager.LoadScene(1);
        SceneChange.GetComponent<SceneChange>().sceneChange(1);
    }

    // For the "rehearse mode" button
    public void rehearseClick()
    {
        Debug.Log("rehearse button clicked!");
        buttonRecall.transform.localPosition = new Vector3(-60, -900, 0);
        buttonRehearse.transform.localPosition = new Vector3(60, -900, 0);

        buttonBackRehearse.transform.localPosition = new Vector3(-20, -50, 0);
        buttonRehearseGo.transform.localPosition = new Vector3(-90, 30, 0);
        keySeedInput.transform.localPosition = new Vector3(120, 30, 0);
        keySeed = "/!0";
    }

    public void backRehearseClick()
    {
        //Debug.Log("Back button clicked!");
        buttonRecall.transform.localPosition = new Vector3(-60, 0, 0);
        buttonRehearse.transform.localPosition = new Vector3(60, 0, 0);

        buttonBackRehearse.transform.localPosition = new Vector3(-20, -900, 0);
        buttonRehearseGo.transform.localPosition = new Vector3(-90, -900, 0);
        keySeedInput.transform.localPosition = new Vector3(120, -900, 0);

    }

    // For the "randomize map" button
    public void randomizeClick()
    {
        //Debug.Log("Seed button clicked!");
        buttonDefault.transform.localPosition = new Vector3(-60, -900, 0);
        buttonMapWord.transform.localPosition = new Vector3(60, -900, 0);
        buttonBack.transform.localPosition = new Vector3(-20, -50, 0);
        buttonGenerate.transform.localPosition = new Vector3(-90, 30, 0);
        mapInputField.transform.localPosition = new Vector3(90, 30, 0);
        mapWord = "/!0";
    }

    // For the back button from the "randomize map" selection
    public void backClick()
    {
        //Debug.Log("Back button clicked!");
        buttonDefault.transform.localPosition = new Vector3(-60, 0, 0);
        buttonMapWord.transform.localPosition = new Vector3(60, 0, 0);
        buttonBack.transform.localPosition = new Vector3(-20, -900, 0);
        buttonGenerate.transform.localPosition = new Vector3(-90, -900, 0);
        mapInputField.transform.localPosition = new Vector3(90, -900, 0);

    }

    // For the "generate" button
    public void generateClick()
    {
        //Debug.Log("Generate button clicked!");
        if (mapWord == "/!0")
        {
            Debug.Log("Invalid map name");
            mapInputField.GetComponent<InputField>().placeholder.GetComponent<Text>().color = Color.red;
        }
        else
        {
            int parsedMapWord = mapWord.GetHashCode();
            Debug.Log(parsedMapWord);
            AllScene.GetComponent<AllScene>().seedChange(parsedMapWord);
            UnityEngine.SceneManagement.SceneManager.LoadScene(1);
        }
    }

    // For the "Go!" button
    public void rehearseGoClick()
    {
        //Debug.Log("Generate button clicked!");
        if (keySeed == "/!0")
        {
            Debug.Log("Invalid map name");
            keySeedInput.GetComponent<InputField>().placeholder.GetComponent<Text>().color = Color.red;
        }
        else
        {
            Debug.Log(keySeed);
            SeedToByte.GetComponent<SeedToByte>().getActions(keySeed);
            UnityEngine.SceneManagement.SceneManager.LoadScene(1);
        }
    }

    // For the "map word" input button
    public void GetMapInput(string mapWordInput) {
        //Debug.Log("Input: " + seedInput);
        mapWord = mapWordInput;
    }

    public void GetSeedInput(string seedWordInput)
    {
        //Debug.Log("Input: " + seedInput);
        keySeed = seedWordInput;
    }
}
