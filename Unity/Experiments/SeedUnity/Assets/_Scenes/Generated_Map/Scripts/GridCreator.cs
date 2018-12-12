using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class GridCreator : MonoBehaviour {

    public int xSize, ySize;
    public string gridText;
    public GameObject AllScene;

    public Material metalRed;
    public Transform blockPrefab;
    public Material tile01, tile02, tile03, tile04, tile05, tile06, tile07, tile08;
    public Material tile09, tile10, tile11, tile12, tile13, tile14, tile15, tile16;

    private int zSize = 256;
    private int gridCount = 0;
    private int[] gridNums = new int[256];
    private int[] gridLay = new int[256];
    private int[] smallNums = new int[16];
    private int[] smallLay = new int[16];

    private Vector3[] vertices;
    private Mesh mesh;
    private int seedParsed;


    private void Awake()
    {
        /*
        GameObject block = GameObject.CreatePrimitive(PrimitiveType.Plane);
        block.transform.position = new Vector3(-5, 2, -1);
        block.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
        block.GetComponent<MeshRenderer>().material = metalRed;
        */

        //smallProceed();
        seedParsed = AllScene.GetComponent<AllScene>().getSeed();
        Debug.Log(seedParsed);
        if (seedParsed.GetType() != typeof(int)){
            Debug.Log("Parsed seed is not an int");
            smallProceed(0);
        }
        else{
            smallProceed(seedParsed);
        }
        smallIron();
        //Proceed();
        //arrayCheck();
        //gridIron(1);
        //firstGrid();

    }

    public void smallProceed(int seed = 0)
    {
        /*
            This function is just for showing the proceedural generation.
            Won't be used in the final design.
         */
        if (seed != 0)
        {
            Random.InitState(seed);
        }

        for (int i = 0; i < smallNums.Length; i++)
        {
            smallNums[i] = i + 1;
            smallLay[i] = i + 1;
        }

        if (seed != 0)
        {
            for (int i = 0; i < 16; i++)
            {
                int w = 0;
                while (w == 0)
                {
                    int r = Random.Range(0, 16);
                    if (smallNums[r] != 0)
                    {
                        smallLay[i] = smallNums[r];
                        smallNums[r] = 0;
                        w++;
                    }
                }
            }
        }
    }

    public void smallIron(int first = 0)
    {
        int counter = 0;
        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                Transform block = Instantiate(blockPrefab, new Vector3(0, 0, 0), Quaternion.identity);
                block.transform.position = new Vector3((j * 5 ), 0.1f, (i * 5));
                block.transform.Rotate(0, 180, 0);
                block.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);

                switch(smallLay[counter]){
                    case 1:
                        block.GetComponent<MeshRenderer>().material = tile01;
                        break;
                    case 2:
                        block.GetComponent<MeshRenderer>().material = tile02;
                        break;
                    case 3:
                        block.GetComponent<MeshRenderer>().material = tile03;
                        break;
                    case 4:
                        block.GetComponent<MeshRenderer>().material = tile04;
                        break;
                    case 5:
                        block.GetComponent<MeshRenderer>().material = tile05;
                        break;
                    case 6:
                        block.GetComponent<MeshRenderer>().material = tile06;
                        break;
                    case 7:
                        block.GetComponent<MeshRenderer>().material = tile07;
                        break;
                    case 8:
                        block.GetComponent<MeshRenderer>().material = tile08;
                        break;
                    case 9:
                        block.GetComponent<MeshRenderer>().material = tile09;
                        break;
                    case 10:
                        block.GetComponent<MeshRenderer>().material = tile10;
                        break;
                    case 11:
                        block.GetComponent<MeshRenderer>().material = tile11;
                        break;
                    case 12:
                        block.GetComponent<MeshRenderer>().material = tile12;
                        break;
                    case 13:
                        block.GetComponent<MeshRenderer>().material = tile13;
                        break;
                    case 14:
                        block.GetComponent<MeshRenderer>().material = tile14;
                        break;
                    case 15:
                        block.GetComponent<MeshRenderer>().material = tile15;
                        break;
                    case 16:
                        block.GetComponent<MeshRenderer>().material = tile16;
                        break;
                }
                counter += 1;
            }
        }
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
            for (int j = 0; j < 5; j++)
            {
                //Debug.Log(i);
                //GameObject block = GameObject.CreatePrimitive(PrimitiveType.Plane);
                Transform block = Instantiate(blockPrefab, new Vector3(0, 0, 0), Quaternion.identity);
                block.transform.position = new Vector3((i * 3 + gridCount * 15), 0.2f, (j * 3 + 1));

                //change the scale of the blocks
                //block.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);

                //block.GetComponent<MeshRenderer>().material = metalRed;

                //change the rotation of the blocks - not needed anymore
                //block.transform.Rotate(0, 0, 0);

                // Generate floating text for the block - depricated
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
