using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugMeshMat : MonoBehaviour {

	// Use this for initialization
	void Start () {
        showMeshInfo();	
	}
	
    private void showMeshInfo() {
        MeshFilter filter = GetComponent<MeshFilter>();
        Mesh mesh = GetComponent<MeshFilter>().mesh;
        Renderer rend = GetComponent<Renderer>();

        Debug.Log(mesh.name + " has " + mesh.subMeshCount + " submeshes!");
    }
}
