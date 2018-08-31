using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MinimapObjects : MonoBehaviour {

    public GameObject childObjects;
    public Interactable target;
    public RectTransform waypoint;
    public Transform player;

    public GameStateData gameState;

    private bool checking;

	void Start () 
    {
        deactivate();
        checking = true;

    }

    void Update()
    {
        if (checking)
        {
            if (gameState.inRehersalMode && gameState.isStarted)
            {
                activate();
                checking = false;
            }
            else
            {
                deactivate();
            }
        }

        target = gameState.currentAction;
        calculateWaypointPosition();
    }

    void deactivate()
    {
        childObjects.SetActive(false);
    }

    void activate()
    {
        childObjects.SetActive(true);
    }


    void calculateWaypointPosition()
    {
        Vector3 playerNoY = player.position;
        Vector3 targetNoY = target.transform.position;

        playerNoY.y = 0;
        targetNoY.y = 0;

        // I'm not sure why, but the waypoint won't be in the right spot unless
        //  this calculation is performed. Apparently the scaling for the
        //  x-axis and z-axis are different, so it may be a resolution problem?

        playerNoY.z = playerNoY.z / 25 * 15;
        targetNoY.z = targetNoY.z / 25 * 15;

        Vector3 diff = targetNoY - playerNoY;
        float normDist = Vector3.Distance(player.position, target.transform.position);
        float dist = Vector3.Distance(targetNoY, playerNoY);

        Debug.Log("Dist: " + dist + " normDist: " + normDist + " diff: " + diff);

        if (dist < 15)
        {
            waypoint.anchoredPosition = new Vector3(diff.x / 15 * 120, diff.z / 15 * 120, 0);

        }
        else
        {
            waypoint.anchoredPosition = new Vector3(diff.x / dist * 120, diff.z / (dist) * 120, 0);   
        }
    }

}
