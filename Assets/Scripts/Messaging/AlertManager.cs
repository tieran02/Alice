using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlertManager
{
    public AlertManager() //Alert manager constructor
    {
        //create new queue of strings
        messages = new Queue<string>(); 
    }

    //Alert manager singleton instance
    public static AlertManager GetInstance()
    {
        //if isntalce doesn't exist create it and return instance
        if (instance == null)
            instance = new AlertManager();
        return instance;
    }
    private static AlertManager instance;

    private Queue<string> messages;

    //Add message to Alert Queue
    public void AddAlert(string message)
    {
        messages.Enqueue(message);
    }

    //Get message from Alert Queue
    public string GetAlert()
    {
        if (HasAlert())
            return messages.Dequeue();
        return string.Empty;
    }

    //Check is there are any alerts
    public bool HasAlert()
    {
        return (messages.Count > 0);
    }
}
