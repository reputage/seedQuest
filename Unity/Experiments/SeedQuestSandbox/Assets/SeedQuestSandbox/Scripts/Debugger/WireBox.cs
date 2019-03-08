using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SeedQuest.Debugger
{
    public class WireBox
    {
        static public void Render(Vector3 position, Vector3 scale, Transform transform, Material material) {

            if (!material)
            {
                Debug.LogError("Please Assign a material");
                return;
            }

            List<Vector3> points = new List<Vector3>();
            points.Add(new Vector3(position.x + scale.x / 2f, position.y + scale.y / 2f, position.z + scale.z / 2f));
            points.Add(new Vector3(position.x + scale.x / 2f, position.y + scale.y / 2f, position.z - scale.z / 2f));
            points.Add(new Vector3(position.x + scale.x / 2f, position.y - scale.y / 2f, position.z + scale.z / 2f));
            points.Add(new Vector3(position.x + scale.x / 2f, position.y - scale.y / 2f, position.z - scale.z / 2f));
            points.Add(new Vector3(position.x - scale.x / 2f, position.y + scale.y / 2f, position.z + scale.z / 2f));
            points.Add(new Vector3(position.x - scale.x / 2f, position.y + scale.y / 2f, position.z - scale.z / 2f));
            points.Add(new Vector3(position.x - scale.x / 2f, position.y - scale.y / 2f, position.z + scale.z / 2f));
            points.Add(new Vector3(position.x - scale.x / 2f, position.y - scale.y / 2f, position.z - scale.z / 2f));

            int[] lines = { 0, 1, 2, 3, 1, 3, 0, 2, 1, 5, 3, 7, 5, 7, 5, 4, 7, 6, 4, 6, 6, 2, 4, 0 };

            material.SetPass(0);

            GL.PushMatrix();
            GL.MultMatrix(transform.localToWorldMatrix);

            GL.Begin(GL.LINES);
            GL.Color(Color.red);

            for (int i = 0; i < lines.Length; i += 2) {
                GL.Vertex(points[lines[i]]);
                GL.Vertex(points[lines[i + 1]]);
            }

            GL.End();
            GL.PopMatrix();
        }
    }
}