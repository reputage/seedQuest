using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Note: Adapted from http://wiki.unity3d.com/index.php/Animating_Tiled_texture

public class AnimatedSpritePlayer : MonoBehaviour {
    
    public int cols = 5;
    public int rows = 7;
    public int spriteCount = 32;
    public float fps = 10f;

    private int index = 0;

    private void Start()
    {
        StartCoroutine(updateImage());
        Vector2 size = new Vector2(1f / cols, 1f / rows);
        Renderer rend = GetComponent<Renderer>();
        rend.materials[0].SetTextureScale("_MainTex", size);
    }

    private IEnumerator updateImage() {
        while(true) {
            index++;
            index = index % (spriteCount);

            Vector2 offset = new Vector2((float)index / cols - (index / cols), 
                                         (index / cols) / (float)rows);

            Renderer rend = GetComponent<Renderer>();
            rend.materials[0].SetTextureOffset("_MainTex", offset);

            yield return new WaitForSeconds(1f / fps);
        }
    }
}
