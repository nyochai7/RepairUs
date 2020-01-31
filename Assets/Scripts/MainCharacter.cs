using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class MainCharacter : MonoBehaviour, ILocationMonitorable
{

    public long timeStartedCurrTask;
    public Task? currTask;
    public SingleMove currMove;
    public void onMonitorAlertFunc(string name, ILocationMonitorable otherObj)
    {
        if (name == "sink")
        {
            GetComponent<SpriteRenderer>().color = Color.green;
        }
        if (name == "bed")
        {
            this.DoTask(Task.DO_DISHES);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        MainObject.Get().locationManager.monitors.Add(new RadiusRelation(
            "sink", this, new FakeILocationMonitorable(GameObject.Find("sink")), WhoToAlert.OnlyFirst));
        MainObject.Get().locationManager.monitors.Add(new RadiusRelation(
                "bed", this, new FakeILocationMonitorable(GameObject.Find("Bed")), WhoToAlert.OnlyFirst));
    }

    // Update is called once per frame
    void Update()
    {
        if (this.currTask != null){
            MainObject mainObject = MainObject.Get();
            
            if (new System.DateTimeOffset(System.DateTime.UtcNow).ToUnixTimeSeconds() - this.timeStartedCurrTask >= this.currMove.duration){
                mainObject.InvokeEvent(this.currMove.stopEvent);
            }
        }


        if (Input.GetMouseButton(0))
        {
            //Vector3 w = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            //GetComponent<NavMeshAgent2D>().destination = w;
        }
    }

    public void DoTask(Task task){
        this.timeStartedCurrTask = new System.DateTimeOffset(System.DateTime.UtcNow).ToUnixTimeSeconds();
        this.currTask = task;

        MainObject mainObject = MainObject.Get();
        this.currMove = mainObject.allTasks[task][0];
        GetComponent<NavMeshAgent2D>().destination = currMove.goTo;
        mainObject.InvokeEvent(currMove.startEvent);

    }
}
