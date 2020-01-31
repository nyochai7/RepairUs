using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MainObject : MonoBehaviour
{

    public Dictionary<string, SingleMove[]> allTasks = new Dictionary<string, SingleMove[]>();
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

    void InitiateAllTasks(){
        allTasks.Add("DO_DISHES", new SingleMove[]{
            new SingleMove(Utils.getPositionByName("sink"), OurEvent.DISHES_START, OurEvent.DISHES_STOP, 10)
        });

        allTasks.Add("MAKE_BED", new SingleMove[]{
            new SingleMove(Utils.getPositionByName("bed"), OurEvent.MAKE_BED_START, OurEvent.MAKE_BED_STOP, 10)
        });

        allTasks.Add("SLEEP", new SingleMove[]{
            new SingleMove(Utils.getPositionByName("bed"), OurEvent.SLEEP_START, OurEvent.SLEEP_STOP, 10)
        });

        // allTasks.Add("DO_LAUNDRY", new SingleMove(Utils.getPositionByName("laundry"), OurEvent.LAUNDRY_START, OurEvent.LAUNDRY_STOP, 10));
        // allTasks.Add("EAT", new SingleMove(Utils.getPositionByName("stove"), OurEvent.EAT_START, OurEvent.EAT_STOP, 10));
        // allTasks.Add
}
}
