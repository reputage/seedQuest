using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class operatorScript : MonoBehaviour {

    // Use this for initialization

    public GameObject AllScene;
    public GameObject buttonDefault;
    public GameObject buttonSeed;
    public GameObject buttonGenerate;
    public GameObject buttonBack;
    public GameObject inputField;

    static string seedWord = "/!0";

	void Start () {
        //Debug.Log("Game begin OK");
	}
	
    public void defaultClick(){
        //Debug.Log("Default button clicked!");
        UnityEngine.SceneManagement.SceneManager.LoadScene(1);
    }

    public void seedClick()
    {
        //Debug.Log("Seed button clicked!");
        buttonDefault.transform.localPosition = new Vector3(-60, -900, 0);
        buttonSeed.transform.localPosition = new Vector3(60, -900, 0);
        buttonBack.transform.localPosition = new Vector3(-20, -50, 0);
        buttonGenerate.transform.localPosition = new Vector3(-90, 30, 0);
        inputField.transform.localPosition = new Vector3(90, 30, 0);
        seedWord = "/!0";
    }

    public void backClick()
    {
        //Debug.Log("Back button clicked!");
        buttonDefault.transform.localPosition = new Vector3(-60, 0, 0);
        buttonSeed.transform.localPosition = new Vector3(60, 0, 0);
        buttonBack.transform.localPosition = new Vector3(-20, -900, 0);
        buttonGenerate.transform.localPosition = new Vector3(-90, -900, 0);
        inputField.transform.localPosition = new Vector3(90, -900, 0);

    }

    public void generateClick()
    {
        //Debug.Log("Generate button clicked!");
        if (seedWord == "/!0")
        {
            Debug.Log("No input given");
            inputField.GetComponent<InputField>().placeholder.GetComponent<Text>().color = Color.red;
        }
        else
        {
            int parsedSeed = seedWord.GetHashCode();
            Debug.Log(parsedSeed);
            AllScene.GetComponent<AllScene>().seedChange(parsedSeed);
            UnityEngine.SceneManagement.SceneManager.LoadScene(1);
        }
    }

    public void GetInput(string seedInput) {
        //Debug.Log("Input: " + seedInput);
        seedWord = seedInput;
    }

}
