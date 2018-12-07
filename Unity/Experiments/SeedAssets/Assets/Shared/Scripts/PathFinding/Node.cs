using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : IHeapItem<Node> {
    
    public bool walkable;
    public Vector3 worldPosition;
    public int gridX;
    public int gridY;

    public int gCost;
    public int hCost;
    public Node parent;
    int heapIndex;

    public Node(bool _walkable, Vector3 _wPos, int _gridX, int _gridY) {
        walkable = _walkable;
        worldPosition = _wPos;
        gridX = _gridX;
        gridY = _gridY;
    }

    public int fCost {
        get {
            return gCost + hCost;
        }
    }

    public int HeapIndex {
        get {
            return heapIndex;
        }
        set {
            heapIndex = value;
        }
    }

    public int CompareTo(Node node) {
        int compare = fCost.CompareTo(node.fCost);
        if (compare == 0)
            compare = hCost.CompareTo(node.hCost);
        return -compare;
    }

}
