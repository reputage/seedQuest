using UnityEngine;

namespace SeedQuest.Level
{
    [System.Serializable]
    public class BoundingBox {
        
        public string name;
        public Vector3 center;
        public Vector3 size;

        /// <summary>  Return status of whether an item is within the bounding box </summary>
        public static bool InBounds(Transform item, BoundingBox bounds) {
            if (item == null) return false;

            Vector3 pos = item.position;
            float x0 = bounds.center.x - (bounds.size.x / 2.0f);
            float x1 = bounds.center.x + (bounds.size.x / 2.0f);
            float z0 = bounds.center.z - (bounds.size.z / 2.0f);
            float z1 = bounds.center.z + (bounds.size.z / 2.0f);
            return x0 <= pos.x && pos.x <= x1 && z0 <= pos.z && pos.z <= z1;
        }
    }
}