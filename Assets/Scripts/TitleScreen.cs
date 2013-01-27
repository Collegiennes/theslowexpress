using UnityEngine;
using System.Collections;

public class TitleScreen : MonoBehaviour
{
    public Renderer splash;
    public Renderer title;

	void Start ()
    {
        StartCoroutine(Enumerate());
	}
	
    IEnumerator Enumerate()
    {
        splash.material.color = new Color(1, 1, 1, 0);
        title.material.color = new Color(1, 1, 1, 0);
        yield return new WaitForSeconds(1);

        while(splash.material.color.a != 1)
        {
            splash.material.color = new Color(1, 1, 1, CoolSmooth.ExpoLinear(
                splash.material.color.a, 1, 0, 1, Time.deltaTime));
            yield return null;
        }
        yield return new WaitForSeconds(1);
        while(splash.material.color.a != 0)
        {
            splash.material.color = new Color(1, 1, 1, CoolSmooth.ExpoLinear(
                splash.material.color.a, 0, 0, 1, Time.deltaTime));
            yield return null;
        }
        while(title.material.color.a != 1)
        {
            title.material.color = new Color(1, 1, 1, CoolSmooth.ExpoLinear(
                title.material.color.a, 1, 0, 1, Time.deltaTime));
            yield return null;
        }
        yield return new WaitForSeconds(1);
        while(title.material.color.a != 0)
        {
            title.material.color = new Color(1, 1, 1, CoolSmooth.ExpoLinear(
                title.material.color.a, 0, 0, 1, Time.deltaTime));
            yield return null;
        }
    }
}
