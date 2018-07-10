using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


// Item IDs
// 1 = rock
// 2 = ball
// 3 = drone
// 4 = book

public class InventoryOperator : MonoBehaviour
{
    private bool showing = false; 
    private static int index = 0; // Used to indicate how many items the player has
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

    private static bool[] itemsActive = new bool[16];
    private static int[] itemIDs = new int[16];
    private static string[] itemNames = new string[16];

    void Start()
    {
        item1.SetActive(false);
        item2.SetActive(false);
        item3.SetActive(false);
        item4.SetActive(false);
    }

    void Update()
    {

    }

    // Item IDs
    // 1 = rock
    // 2 = ball
    // 3 = drone
    // 4 = book

    // Adds item to the player's inventory
    public void addItem(int ID, string name)
    {
        itemsActive[index] = true;
        itemIDs[index] = ID;
        itemNames[index] = name;

        getItemIcon(ID);
        activateItem(name);

        index += 1;

        // This will force the inventory window to update if the 
        // item is picked up while the inventory is open
        if (showing)
        {
            show();
        }
    }

    // Activates the inventory and associated UI elements
    public void show()
    {
        showing = true;
        backPanel.SetActive(true);

        if (itemsActive[0] == true)
        {
            item1.SetActive(true);
            drop1.SetActive(true);
        }
        if (itemsActive[1] == true)
        {
            item2.SetActive(true);
            drop2.SetActive(true);
        }
        if (itemsActive[2] == true)
        {
            item3.SetActive(true);
            drop3.SetActive(true);
        }
        if (itemsActive[3] == true)
        {
            item4.SetActive(true);
            drop4.SetActive(true);
        }
    }

    // Deactivates the inventory and associated UI elements
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

    // Used to remove items from inventory, and re-arranges the items as well
    public void dropItem(int itemNum)
    {
        switch (itemNum)
        { 
            case 1:
                if (index > 0)
                {
                    itemIDs[1] = itemIDs[2];
                    icon1.GetComponent<Image>().sprite = icon2.GetComponent<Image>().sprite;
                    item1.GetComponentInChildren<Text>().text = item2.GetComponentInChildren<Text>().text;
                }
                if (index > 1)
                {
                    itemIDs[2] = itemIDs[3];
                    icon2.GetComponent<Image>().sprite = icon3.GetComponent<Image>().sprite;
                    item2.GetComponentInChildren<Text>().text = item3.GetComponentInChildren<Text>().text;
                }
                if (index > 2)
                {
                    itemIDs[3] = itemIDs[4];
                    icon3.GetComponent<Image>().sprite = icon4.GetComponent<Image>().sprite;
                    item3.GetComponentInChildren<Text>().text = item4.GetComponentInChildren<Text>().text;
                }
                break;
            case 2:
                if (index > 1)
                {
                    itemIDs[2] = itemIDs[3];
                    icon2.GetComponent<Image>().sprite = icon3.GetComponent<Image>().sprite;
                    item2.GetComponentInChildren<Text>().text = item3.GetComponentInChildren<Text>().text;
                }
                if (index > 2)
                {
                    itemIDs[3] = itemIDs[4];
                    icon3.GetComponent<Image>().sprite = icon4.GetComponent<Image>().sprite;
                    item3.GetComponentInChildren<Text>().text = item4.GetComponentInChildren<Text>().text;
                }
                break;
            case 3:
                if (index > 2)
                {
                    itemIDs[3] = itemIDs[4];
                    icon3.GetComponent<Image>().sprite = icon4.GetComponent<Image>().sprite;
                    item3.GetComponentInChildren<Text>().text = item4.GetComponentInChildren<Text>().text;
                }
                break;
            case 4:
                break;
            default:
                break;
        }

        itemsActive[index] = false;

        switch (index){
            case 1:
                item1.SetActive(false);
                drop1.SetActive(false);
                break;
            case 2:
                item2.SetActive(false);
                drop2.SetActive(false);
                break;
            case 3:
                item3.SetActive(false);
                drop3.SetActive(false);
                break;
            case 4:
                item4.SetActive(false);
                drop4.SetActive(false);
                break;
            default:
                break;
        } 

        // decrement index
        index -= 1;
    }

    //  figure out which icon is needed for the item 
    public void getItemIcon(int ID)
    {
        switch (ID)
        {
            case 1: // rock
                iconRef = iconRock;
                break;
            case 2: // soccer ball
                iconRef = iconBall;
                break;
            case 3: // drone
                iconRef = iconDrone;
                break;
            case 4: // book
                iconRef = iconBook;
                break;
            default:
                break;
        }
    }

    // Activates item, sets the name of the item
    public void activateItem(string name)
    {
        // Sets the icon for the item in the inventory, and sets the inventory item active
        switch (index)
        {
            case 0:
                item1.GetComponentInChildren<Text>().text = name;
                icon1.GetComponent<Image>().sprite = iconRef;
                break;
            case 1:
                item2.GetComponentInChildren<Text>().text = name;
                icon2.GetComponent<Image>().sprite = iconRef;
                break;
            case 2:
                item3.GetComponentInChildren<Text>().text = name;
                icon3.GetComponent<Image>().sprite = iconRef;
                break;
            case 3:
                item4.GetComponentInChildren<Text>().text = name;
                icon4.GetComponent<Image>().sprite = iconRef;
                break;
            default:
                break;
        }
    }

}
