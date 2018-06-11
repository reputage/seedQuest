using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorAnimator : MonoBehaviour {

    Animator animator;

	// Use this for initialization
	void Start () {
        animator = GetComponent<Animator>();
	}

    void OnMouseDown()
    {
        // this object was clicked - do something
        Debug.Log("test");
    }

	// Update is called once per frame
	void Update () {

        if (Input.GetMouseButtonDown(0))
        {

            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit, 100.0f))
            {
                if (hit.transform != null)
                {
                    if(animator.GetBool("Open")) {
                        animator.SetBool("Open", false);
                        animator.SetBool("Close", true);
                    }
                    else {
                        animator.SetBool("Open", true);  
                        animator.SetBool("Close", false);
                    }

                }
            }
        }
	}

    private void PrintName(GameObject obj) {
        print(obj.name);
    }
}
