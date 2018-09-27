using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MinimapObjects : MonoBehaviour {

    public Interactable target;
    public RectTransform waypoint;

    public int xOffset;
    public int yOffset;

    public float xScale;
    public float yScale;

	void Update()
    {
        target = PathManager.PathTarget;
        calculateWaypointPosition();
    }

    void calculateWaypointPosition()
    {
        Vector3 playerNoY = PlayerManager.Position;
        Vector3 targetNoY = target.transform.position;

        playerNoY.y = 0;
        targetNoY.y = 0;

        // I'm not sure why, but the waypoint won't be in the right spot unless
        //  this calculation is performed. Apparently the scaling for the
        //  x-axis and z-axis are different, so it may be a resolution problem?

        playerNoY.z = playerNoY.z / 25 * 15;
        targetNoY.z = targetNoY.z / 25 * 15;

        Vector3 diff = targetNoY - playerNoY;
        float normDist = Vector3.Distance(PlayerManager.Position, target.transform.position);
        float dist = Vector3.Distance(targetNoY, playerNoY);

        //Debug.Log("Dist: " + dist + " normDist: " + normDist + " diff: " + diff);

        if (dist < 15)
        {
            Vector3 wayPos = new Vector3((diff.x / 15 * 120 * xScale + xOffset) * Screen.width / 1024f, (diff.z / 15 * 120 * yScale + yOffset) * Screen.height / 768f, 0);
            waypoint.anchoredPosition = wayPos;
        }
        else
        {
            Vector3 wayPos = new Vector3((diff.x / dist * 120 * xScale + xOffset) * Screen.width / 1024f, (diff.z / (dist) * 120 * yScale + yOffset) * Screen.height / 768f, 0);
            waypoint.anchoredPosition = wayPos;
        }
    }


}