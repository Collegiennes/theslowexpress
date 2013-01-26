using UnityEngine;
using System.Collections;

public class SpriteAnimation : MonoBehaviour
{
    public int fps = 10;
    int frames;
    float t = 0;

	void Start ()
    {
	    frames = Mathf.RoundToInt(1/renderer.material.mainTextureScale.x);
        t = Random.value * (float)frames/fps;
	}
	
	void Update ()
    {
	    renderer.material.mainTextureOffset =
            new Vector2((Mathf.Floor(t*fps) % frames)/frames, 0);
        t = Mathf.Repeat(t + Time.deltaTime, (float)frames/fps);
	}
}
