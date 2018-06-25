using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class InventoryOperator : MonoBehaviour
{
    public GameObject backPanel;
    public GameObject item1;
    public GameObject item2;
    public GameObject item3;
    public GameObject item4;
    public GameObject icon1;
    public GameObject icon2;
    public GameObject icon3;
    public GameObject icon4;
    public GameObject drop1;
    public GameObject drop2;
    public GameObject drop3;
    public GameObject drop4;

    public Sprite iconRock;
    public Sprite iconBall;
    public Sprite iconDrone;
    public Sprite iconBook;
    public Sprite iconRef;

    private static bool item1Active = false;
    private static bool item2Active = false;
    private static bool item3Active = false;
    private static bool item4Active = false;

    private static int item1ID;
    private static int item2ID;
    private static int item3ID;
    private static int item4ID;

    private static string item1Name;
    private static string item2Name;
    private static string item3Name;
    private static string item4Name;

    private bool showing = false;
    private static int index = 0;

    // Use this for initialization

    void Start()
    {
        item1.SetActive(false);
        item2.SetActive(false);
        item3.SetActive(false);
        item4.SetActive(false);
        //DontDestroyOnLoad(gameObject);

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void addItem(int ID, string name)
    {

        // 100001 = rock
        // 100002 = ball
        // 100003 = drone
        // 100004 = book
        switch (ID)
        {
            case 100001: // rock
                // do something
                iconRef = iconRock;
                break;
            case 100002: // soccer ball
                iconRef = iconBall;
                break;
            case 100003:
                iconRef = iconDrone;
                break;
            case 100004:
                iconRef = iconBook;
                break;
            default:
                break;
        }

        switch (index)
        {
            case 0:
                item1Active = true;
                item1ID = ID;
                item1.GetComponentInChildren<Text>().text = name;
                icon1.GetComponent<Image>().sprite = iconRef;
                break;
            case 1:
                item2Active = true;
                item2ID = ID;
                item2.GetComponentInChildren<Text>().text = name;
                icon2.GetComponent<Image>().sprite = iconRef;
                break;
            case 2:
                item3Active = true;
                item3ID = ID;
                item3.GetComponentInChildren<Text>().text = name;
                icon3.GetComponent<Image>().sprite = iconRef;
                break;
            case 3:
                item4Active = true;
                item4ID = ID;
                item4.GetComponentInChildren<Text>().text = name;
                icon4.GetComponent<Image>().sprite = iconRef;
                break;
            default:
                break;
        }

        index += 1;
        if (showing)
        {
            show();
        }

    }


    public void show()
    {

        showing = true;
        backPanel.SetActive(true);

        if (item1Active == true)
        {
            item1.SetActive(true);
            drop1.SetActive(true);
        }
        if (item2Active == true)
        {
            item2.SetActive(true);
            drop2.SetActive(true);
        }
        if (item3Active == true)
        {
            item3.SetActive(true);
            drop3.SetActive(true);
        }
        if (item4Active == true)
        {
            item4.SetActive(true);
            drop3.SetActive(true);
        }

    }

    public void hide()
    {
        showing = false;
        backPanel.SetActive(false);
        item1.SetActive(false);
        item2.SetActive(false);
        item3.SetActive(false);
        item4.SetActive(false);
        drop1.SetActive(false);
        drop2.SetActive(false);
        drop3.SetActive(false);
        drop4.SetActive(false);

    }


    /*
    for (int i = 0; i == index; i++;)
    {
        if (i == 1)
        {
            item1ID = item2ID;
            iconRef = icon1.GetComponent<Image>().sprite = iconRef;
        }
    }
    */

    public void dropItem(int itemNum)
    {
        //spawn item on ground
        //move lower-level items to higher-level slots

        //deactivate item# and drop#
        switch (itemNum)
        { 
            case 1:
                if (index > 0)
                {
                    item1ID = item2ID;
                    icon1.GetComponent<Image>().sprite = icon2.GetComponent<Image>().sprite;
                    item1.GetComponentInChildren<Text>().text = item2.GetComponentInChildren<Text>().text;
                }
                if (index > 1)
                {
                    item2ID = item3ID;
                    icon2.GetComponent<Image>().sprite = icon3.GetComponent<Image>().sprite;
                    item2.GetComponentInChildren<Text>().text = item3.GetComponentInChildren<Text>().text;
                }
                if (index > 2)
                {
                    item3ID = item4ID;
                    icon3.GetComponent<Image>().sprite = icon4.GetComponent<Image>().sprite;
                    item3.GetComponentInChildren<Text>().text = item4.GetComponentInChildren<Text>().text;
                }
                break;
            case 2:
                if (index > 1)
                {
                    item2ID = item3ID;
                    icon2.GetComponent<Image>().sprite = icon3.GetComponent<Image>().sprite;
                    item2.GetComponentInChildren<Text>().text = item3.GetComponentInChildren<Text>().text;
                }
                if (index > 2)
                {
                    item3ID = item4ID;
                    icon3.GetComponent<Image>().sprite = icon4.GetComponent<Image>().sprite;
                    item3.GetComponentInChildren<Text>().text = item4.GetComponentInChildren<Text>().text;
                }
                break;
            case 3:
                if (index > 2)
                {
                    item3ID = item4ID;
                    icon3.GetComponent<Image>().sprite = icon4.GetComponent<Image>().sprite;
                    item3.GetComponentInChildren<Text>().text = item4.GetComponentInChildren<Text>().text;
                }
                break;
            case 4:
                //item4.SetActive(false);
                //drop4.SetActive(false);
                break;
            default:
                break;
        
        }

        switch (index){
            case 1:
                item1.SetActive(false);
                drop1.SetActive(false);
                item1Active = false;
                break;
            case 2:
                item2.SetActive(false);
                drop2.SetActive(false);
                item2Active = false;
                break;
            case 3:
                item3.SetActive(false);
                drop3.SetActive(false);
                item3Active = false;
                break;
            case 4:
                item4.SetActive(false);
                drop4.SetActive(false);
                item4Active = false;
                break;
            default:
                break;
        } 

        // decrement index
        index -= 1;
    }

}
