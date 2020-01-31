using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MainObject : MonoBehaviour
{
    public static int defaultDuration = 5;
    public Dictionary<Task, GeneralTask[]> allTasks = new Dictionary<Task, GeneralTask[]>();
    public LocationManager locationManager { get; set; }
    public event Action<OurEvent> onSomethingHappened;
    // Start is called before the first frame update
    void Start()
    {
        this.InitiateAllTasks();
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

    public void InitiateAllTasks(){
        allTasks.Add(Task.DO_DISHES, new SingleMove[]{
            new SingleMove(Utils.getPositionByName("sink"), OurEvent.DISHES_START, OurEvent.DISHES_STOP, defaultDuration)
        });

        allTasks.Add(Task.MAKE_BED, new SingleMove[]{
            new SingleMove(Utils.getPositionByName("bed"), OurEvent.MAKE_BED_START, OurEvent.MAKE_BED_STOP, defaultDuration)
        });

        allTasks.Add(Task.SLEEP, new SingleMove[]{
            new SingleMove(Utils.getPositionByName("bed"), OurEvent.SLEEP_START, OurEvent.SLEEP_STOP, defaultDuration)
        });

        allTasks.Add(Task.DO_LAUNDRY, new SingleMove[]{
            new SingleMove(Utils.getPositionByName("laundry"), OurEvent.LAUNDRY_START, OurEvent.LAUNDRY_STOP, defaultDuration)
        });

        allTasks.Add(Task.EAT, new SingleMove[]{
            new SingleMove(Utils.getPositionByName("stove"), OurEvent.GET_FOOD_START, OurEvent.GET_FOOD_STOP, defaultDuration),
            new SingleMove(Utils.getPositionByName("table"), OurEvent.EAT_START, OurEvent.EAT_STOP, defaultDuration)
        });

        allTasks.Add(Task.RAISE_TOILET_SEAT, new SingleMove[]{
            new SingleMove(Utils.getPositionByName("toilet"), OurEvent.RAISE_TOILET_SEAT, OurEvent.DO_NOTHING, defaultDuration)
        });

        allTasks.Add(Task.LOWER_TOILET_SEAT, new SingleMove[]{
            new SingleMove(Utils.getPositionByName("toilet"), OurEvent.LOWER_TOILET_SEAT, OurEvent.DO_NOTHING, defaultDuration)
        });

        allTasks.Add(Task.GO_BATHROOM, new SingleMove[]{
            new SingleMove(Utils.getPositionByName("toilet"), OurEvent.GO_BATHROOM_START, OurEvent.GO_BATHROOM_STOP, defaultDuration)
        });

        allTasks.Add(Task.COOK, new SingleMove[]{
            new SingleMove(Utils.getPositionByName("stove"), OurEvent.COOK_START, OurEvent.COOK_STOP, defaultDuration)
        });

        allTasks.Add(Task.SHOWER, new SingleMove[]{
            new SingleMove(Utils.getPositionByName("bathroom"), OurEvent.CHECK_SHOWER, OurEvent.DO_NOTHING, defaultDuration)
        });

    }
}
