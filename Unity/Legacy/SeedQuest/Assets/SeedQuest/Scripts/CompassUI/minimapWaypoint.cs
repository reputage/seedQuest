using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class minimapWaypoint : MonoBehaviour {

    public Interactable target;
    public RectTransform waypoint;

    public int xOffset;
    public int yOffset;

    public float xScale;
    public float yScale;

    void Update()
    {
        target = PathManager.PathTarget;
        waypoint = GetComponent<RectTransform>();

        xOffset = -150;
        yOffset = 150;
        xScale = 1;
        yScale = 1;

        calculateWaypointPosition();
    }

    // Calculate the position of the minimap waypoint, and update the waypoint's position
    void calculateWaypointPosition()
    {
        Vector3 playerNoY = PlayerManager.Position;
        Vector3 targetNoY = target.transform.position;

        playerNoY.y = 0;
        targetNoY.y = 0;

        playerNoY.z = playerNoY.z / 25 * 15;
        targetNoY.z = targetNoY.z / 25 * 15;

        Vector3 diff = targetNoY - playerNoY;
        float normDist = Vector3.Distance(PlayerManager.Position, target.transform.position);
        float dist = Vector3.Distance(targetNoY, playerNoY);

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
