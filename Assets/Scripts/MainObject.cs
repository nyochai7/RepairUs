using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MainObject : MonoBehaviour
{
    public LocationManager locationManager { get; set; }
    public event Action<OurEvent> onSomethingHappened;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log("Checking all");
        locationManager.CheckAll();
    }

    void Awake()
    {
        locationManager = new LocationManager();
    }

    public static MainObject Get()
    {
        MainObject mainObject = null;
        GameObject MO = GameObject.Find("MO");
        if (MO != null)
        {
            mainObject = MO.GetComponent<MainObject>();
        }
        return mainObject;
    }

    public void InvokeEvent(OurEvent eventToInvoke)
    {
        if (this.onSomethingHappened != null)
        {
            this.onSomethingHappened(eventToInvoke);
        }
    }
}
