using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;
using UnityEngine.Rendering.Universal;

public class scr_Torch : MonoBehaviour
{

    public Transform mainLight;
    public Transform flickerLight;
    Light2D mainLightComponent;
    Light2D flickerLightComponent;


    // Start is called before the first frame update
    void Start()
    {
        mainLightComponent = mainLight.GetComponent<Light2D>();
        flickerLightComponent = flickerLight.GetComponent<Light2D>();

        StartCoroutine(Timer());
    }

    IEnumerator Timer()
    {
        for (; ; ) //this is while(true)
        {
            float randomIntensity = Random.Range(0.25f, 0.4f);
            flickerLightComponent.intensity = randomIntensity;


            float randomTime = Random.Range(0f, 0.2f);
            yield return new WaitForSeconds(randomTime);
        }
    }
}