using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightFlicker : MonoBehaviour
{
    [Range(0,1)]
    //How often does the light violently change
    public float Jitter = 0.7f;
    //strengh of the jitter
    public float JitterStrength = 5.0f;
    //Intensity
    public float Intensity = 4f;
    //How low should the Intensity be multiplied by
    public float LowModifer = -0.1f;
    //How high should the Intensity be multiplied by
    public float HighModifer = 0.1f;

    private Light flickeringLight;
    private float intialIntensity;
    private float targetLightLevel;

    // Use this for initialization
    void Awake ()
    {
        //get the light and set the intialIntensity
        flickeringLight = GetComponent<Light>();
        intialIntensity = flickeringLight.intensity;
    }

    void Start()
    {
        //set a random target light level
        targetLightLevel = intialIntensity + Random.Range(LowModifer, HighModifer);
    }

    // Update is called once per frame
    void Update ()
    {
        //check if current intensity is roughtly the target light level
        if (Mathf.Abs(flickeringLight.intensity - targetLightLevel) <= 0.01f)
        {
            //set targetLightLevel to a random amount
            targetLightLevel = intialIntensity + Random.Range(LowModifer, HighModifer);
        }
        //time step
        float t = Time.deltaTime * Intensity;
        //random value between 0-1
        float rand = Random.value;
        //check if rand is less than jitter and if so make the time step multiply by jitterstrength
        if (rand < Jitter)
            t *= JitterStrength;
        //lerp the lights intensity to target level using the timestep
        flickeringLight.intensity = Mathf.Lerp(flickeringLight.intensity, targetLightLevel, t);
	}
}
