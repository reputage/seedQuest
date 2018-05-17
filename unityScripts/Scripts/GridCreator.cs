using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class GridCreator : MonoBehaviour {

    public int xSize, ySize;
    public string gridText;

    private int zSize = 256;
    private int gridCount = 0;
    private int[] gridNums = new int[256];
    private int[] gridLay = new int[256];

    private Vector3[] vertices;
    private Mesh mesh;

    private void Awake()
    {
        Proceed();
        //arrayCheck();
        gridIron(1);
        //firstGrid();
    }

    public void Proceed()
    {
        /*
            This function generates an array with random numbers
            representing the map indexes of the world.
            To be used for proceedural generation of the 
            world map from a seed.

         */
        for (int i = 0; i < gridNums.Length; i++)
        {
            gridNums[i] = i+1;
            gridLay[i] = 0;
        }

        for (int i = 0; i < zSize; i++)
        {
            /*
                for each element in the 2D grid, choose a random number between 1 and 256
                when the number is selected, remove it from the possible list 
                so that future spaces don't re-use the same element
            */
            if (i <= 250) //For efficiency purposes, once most RNs have been used, use another loop to finish the array generation
            {
                int w = 0;
                while (w == 0) // Ranomly fill in the empty array, without repeating numbers
                {
                    int r = Random.Range(0, 256);
                    if (gridNums[r] != 0)
                    {
                        gridLay[i] = gridNums[r];
                        gridNums[r] = 0;
                        w++;
                    }
                }
            }
            else
            {
                for (int j = 0; j < zSize; j++)
                {
                    if (gridNums[j] != 0)
                    {
                        gridLay[i] = gridNums[j];
                        gridNums[j] = 0;
                        j += 256;
                    }
                }
            }
        }
    }

    private void arrayCheck()
    {
        for (int i = 0; i < gridNums.Length; i++)
        {
            //Debug.Log(gridNums[i]);
            //Debug.Log(gridLay[i]);   
            gridText = string.Join(",", new List<int>(gridLay).ConvertAll(j => j.ToString()).ToArray());
        }
        Debug.Log(gridText);
    }

    public void gridIron(int first = 0)
    {

        for (int i = 0; i < 5; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                //Debug.Log(i);
                GameObject block = GameObject.CreatePrimitive(PrimitiveType.Plane);
                block.transform.position = new Vector3((i * 3 + gridCount * 15), 0.2f, (j * 3 + 1));

                //change the rotation of the blocks - not needed anymore
                //block.transform.Rotate(0, 0, 0);

                //change the scale of the blocks
                block.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);

                /*
                GameObject text = new GameObject();

                int ij = (i + j * 5);
                Debug.Log(ij);
                TextMesh t = text.AddComponent<TextMesh>();

                if (first == 0)
                {
                    t.text = gridLay[ij].ToString();
                }
                else 
                {
                    t.text = ij.ToString();
                }

                t.fontSize = 15;
                t.transform.position = new Vector3((i * 3f - 1.2f + gridCount * 15), 0, (j * 3 + 2));
                t.color = new Color(0f, 0f, 0f);
                */
            }
        }
        gridCount += 1;
    }

}
