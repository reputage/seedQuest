using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cloudGenerator : MonoBehaviour {

    public Transform cloudPrefab;
    public bool generating;


    void Start () {
        generating = false;
	}
	

    void Update () 
    {
        Vector3 position = gameObject.transform.position;

        if (generating)
        {
            Instantiate(cloudPrefab, position, Quaternion.identity);
            Instantiate(cloudPrefab, position, Quaternion.identity);
        }
		
	}

    public void startGenerate()
    {
        generating = true;
    }

    public void stopGenerate()
    {
        generating = false;    
    }
}
