using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cloudGenerator : MonoBehaviour {

    public int cloudIter;
    public Transform cloudPrefab;

	// Use this for initialization
	void Start () {
        cloudIter = 0;
	}
	
	// Update is called once per frame
	void Update () 
    {
        cloudIter += 1;
        if (cloudIter % 2 == 0)
        {
            cloudIter = 0;
            Instantiate(cloudPrefab, new Vector3(2.0F, 0, 0), Quaternion.identity);
        }
		
	}
}
