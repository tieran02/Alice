using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TooltipOnTigger : MonoBehaviour {
    //Tooltip UI Text
    public Text TooltipUIText;
    //Text to show the user
    public string ToolTipText;
    //iss aplication quiting
    private bool applicationIsQuitting;

    void OnTriggerEnter(Collider other)
    {
        //if other is player and tooltip UI text is active show
        if(other.tag == "Player" && TooltipUIText.IsActive())
            TooltipUIText.text = ToolTipText;
    }

    void OnTriggerExit(Collider other)
    {
        //clear tool tip on exit
        if (other.tag == "Player" && TooltipUIText.IsActive())
            TooltipUIText.text = "";
    }

    void OnDisable()
    {
        //if application is not quiting and tooltip is disabled clear tooltip text
        if(!applicationIsQuitting)
            TooltipUIText.text = "";
    }

    void OnApplicationQuit()
    {
        applicationIsQuitting = true;
    }
}
