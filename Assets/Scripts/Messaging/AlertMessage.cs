using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AlertMessage : MonoBehaviour {
    //How long to display a message
    public float DisplayTime = 2.0f;
    //UI text component
    private Text alertText;
    //current Alert message
    private string currentAlert;

    void Awake()
    {
        //set alert text component 
        alertText = GetComponent<Text>();
        currentAlert = string.Empty;
    }

    // Update is called once per frame
    void Update()
    {
        //check if alert manager instance has an alert and current alert is empty
        if (AlertManager.GetInstance().HasAlert() && currentAlert == string.Empty)
        {
            //get current alert and set the UI text to the alert message
            currentAlert = AlertManager.GetInstance().GetAlert();
            alertText.text = currentAlert;
            //reset the alert
            StartCoroutine(ResetAlert());
        }
	}

    IEnumerator ResetAlert()
    {
        //wait for the display time then clear the alert
        yield return new WaitForSeconds(DisplayTime);
        currentAlert = string.Empty;
        alertText.text = currentAlert;
    }
}
