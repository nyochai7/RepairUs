﻿﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MainObject : MonoBehaviour
{
    public static int defaultDuration = 2;
    public Dictionary<Task, GeneralTask[]> allTasks = new Dictionary<Task, GeneralTask[]>();
    public LocationManager locationManager { get; set; }
    public event Action<OurEvent> onSomethingHappened;

    public List<BlockList> AllBlockLists = new List<BlockList>();

    [SerializeField]
    public GameObject blockObjPrefab;

    // Start is called before the first frame update
    void Start()
    {
        this.InitiateAllTasks();
    }

    // Update is called once per frame
    void Update()
    {
        if (locationManager != null){
            locationManager.CheckAll();
        }
        
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

    public void InvokeEvent(OurEvent? eventToInvoke)
    {
        if (this.onSomethingHappened != null && eventToInvoke != null)
        {
            this.onSomethingHappened(eventToInvoke.Value);
        }
    }

    public void InitiateAllTasks(){
        allTasks.Add(Task.DO_DISHES, new GeneralTask[]{
            new SingleMove(Utils.getPositionByName("sink"), OurEvent.GO_TO_SINK, OurEvent.DO_NOTHING, defaultDuration),
            new ConditionalTask(Sink.CheckSink, Task.WASH_DISHES, null, OurEvent.MAKE_BED_START, null)
        });

        allTasks.Add(Task.WASH_DISHES, new GeneralTask[]{
            new SingleMove(Utils.getPositionByName("sink"), OurEvent.DISHES_START, OurEvent.DISHES_STOP, defaultDuration)
        });

        allTasks.Add(Task.SHOWER, new GeneralTask[]{
            new SingleMove(Utils.getPositionByName("makeup_table0"), OurEvent.GO_TO_SHOWER, OurEvent.DO_NOTHING, defaultDuration),
            new ConditionalTask(Shower.IsTaken, Task.USE_SHOWER, null, OurEvent.SHOWER_IS_TAKEN, OurEvent.SAY_TYPICAL)
        });

        allTasks.Add(Task.USE_SHOWER, new GeneralTask[]{
            new SingleMove(Utils.getPositionByName("shower"), OurEvent.USE_SHOWER_START, OurEvent.USE_SHOWER_STOP, defaultDuration)
        });

        allTasks.Add(Task.MAKE_BED, new GeneralTask[]{
            new SingleMove(Utils.getPositionByName("Bed"), OurEvent.MAKE_BED_START, OurEvent.MAKE_BED_STOP, defaultDuration)
        });

        allTasks.Add(Task.SLEEP, new GeneralTask[]{
            new SingleMove(Utils.getPositionByName("Bed"), OurEvent.SLEEP_START, OurEvent.SLEEP_STOP, defaultDuration)
        });

        allTasks.Add(Task.DO_LAUNDRY, new GeneralTask[]{
            new SingleMove(Utils.getPositionByName("laundry_machine0"), OurEvent.LAUNDRY_START, OurEvent.LAUNDRY_STOP, defaultDuration)
        });

        allTasks.Add(Task.EAT, new GeneralTask[]{
            new SingleMove(Utils.getPositionByName("kitchen_counter"), OurEvent.GET_FOOD_START, OurEvent.GET_FOOD_STOP, defaultDuration),
            new ConditionalTask(Counter.hasCookedFood, Task.EAT_GOOD_FOOD, Task.EAT_BAD_FOOD, OurEvent.EAT_COUNTER_FOOD, OurEvent.SAY_ANGRY)
        });

        allTasks.Add(Task.EAT_GOOD_FOOD, new GeneralTask[]{
            new SingleMove(Utils.getPositionByName("table"), OurEvent.EAT_START, OurEvent.EAT_STOP, defaultDuration)
        });

        allTasks.Add(Task.EAT_BAD_FOOD, new GeneralTask[]{
            new SingleMove(Utils.getPositionByName("table"), OurEvent.FRIDGE_START, OurEvent.FRIDGE_STOP, defaultDuration), //need to change to fridge
            new SingleMove(Utils.getPositionByName("table"), OurEvent.EAT_START, OurEvent.EAT_STOP, defaultDuration)

        });
        allTasks.Add(Task.RAISE_TOILET_SEAT, new GeneralTask[]{
            new SingleMove(Utils.getPositionByName("Toilet_Down"), OurEvent.RAISE_TOILET_SEAT, OurEvent.DO_NOTHING, defaultDuration)
        });

        allTasks.Add(Task.LOWER_TOILET_SEAT, new GeneralTask[]{
            new SingleMove(Utils.getPositionByName("Toilet_Down"), OurEvent.LOWER_TOILET_SEAT, OurEvent.DO_NOTHING, defaultDuration)
        });

        allTasks.Add(Task.USE_TOILET, new GeneralTask[]{
            new SingleMove(Utils.getPositionByName("makeup_table0"), OurEvent.GO_TO_BATHROOM, OurEvent.DO_NOTHING, defaultDuration),
            new ConditionalTask(Toilet.IsTaken, Task.ACTUALLY_USE_TOILET, null, null, OurEvent.SAY_ANGRY)
        });

        allTasks.Add(Task.COOK, new GeneralTask[]{
            new SingleMove(Utils.getPositionByName("kitchen_counter"), OurEvent.COOK_START, OurEvent.COOK_STOP, defaultDuration)
        });
    }
}