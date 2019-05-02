using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrassSpawner : MonoBehaviour {

    public int count = 100;
    public float itemBounds = 0.2f;
    public Vector2 worldSize = new Vector2(1.0f, 1.0f);
    public GameObject grassObject;

    private List<Vector2> itemOffsetList = new List<Vector2>();
    private int timeoutMaxCount = 10;

	// Use this for initialization
	void Start () {
        for (int i = 0; i < count; i++)
            Create();
    }
	
    private void OnDrawGizmos() {
        Gizmos.DrawWireCube(transform.position, new Vector3(worldSize.x, 1, worldSize.y));

        foreach(Vector2 offset in itemOffsetList) {
            Vector3 position = transform.position;
            position += new Vector3(offset.x, 0, offset.y);
            Gizmos.DrawWireSphere(position, itemBounds / 4.0f);
        }
    }

    private void Create() {
        // Create random angle
        float angle = Random.Range(-180, 180);
        Quaternion rotate = Quaternion.Euler(-90, angle, 0);

        // Create random position within bounds
        Vector3 position = transform.position;
        Vector2 offset = getPositionOffset(); //(Random.insideUnitCircle * worldSize / 2);
        position += new Vector3(offset.x, 0, offset.y);
        itemOffsetList.Add(offset);

        // Create object
        Instantiate(grassObject, position, rotate, transform);
    }

    private Vector2 getPositionOffset() {
        Vector2 offset = Vector2.zero;
        bool found = false;
        int timeoutCount = 0;

        while(!found) {
            if (timeoutCount > timeoutMaxCount)
                break;

            offset = (Random.insideUnitCircle * worldSize / 2);

            if (!isItemCloseToOtherItem(offset))
                found = true;

            timeoutCount++;
        }

        return offset;
    }

    private bool isItemCloseToOtherItem(Vector2 item) {

        foreach (Vector2 otherItem in itemOffsetList) {
            float dist = (item - otherItem).SqrMagnitude();

            if (dist > itemBounds)
                return false;
        }
        return true;
    }
}
