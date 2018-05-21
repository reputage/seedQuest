using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class operatorScript : MonoBehaviour {

    // Use this for initialization

    public GameObject AllScene;
    static string seedWord;

	void Start () {
		
	}
	
    public void defaultClick(){
        Debug.Log("Default button clicked!");
        UnityEngine.SceneManagement.SceneManager.LoadScene(1);
    }

    public void seedClick()
    {
        Debug.Log("Seed button clicked!");
    }

    public void generateClick()
    {
        Debug.Log("Generate button clicked!");
        string testParse = seedWord;
        int parsedSeed = testParse.GetHashCode();
        Debug.Log(parsedSeed);
        AllScene.GetComponent<AllScene>().seedChange(parsedSeed);
        UnityEngine.SceneManagement.SceneManager.LoadScene(1);
    }

    public void GetInput(string seedInput) {
        Debug.Log("Input: " + seedInput);
        seedWord = seedInput;
        Debug.Log("Word: " + seedWord);
    }
}
