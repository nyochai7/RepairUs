using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class MainCharacter : MonoBehaviour, ILocationMonitorable
{

    public float radiusToObj = 1;
    public long timeStartedCurrTask;
    public GeneralTask[] currTask;
    public GeneralTask currMove;
    int moveIndex;

    [SerializeField]
    DynamicFace face;

    public void onMonitorAlertFunc(string name, ILocationMonitorable otherObj)
    {
        if (name == "sink")
        {
            GetComponent<SpriteRenderer>().color = Color.green;
        }
        if (name == "bed")
        {
            this.DoTask(Task.SHOWER_NOW);
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
        if (this.currTask != null && this.currMove != null){
            MainObject mainObject = MainObject.Get();
            
            if (this.currMove is SingleMove){
                SingleMove currSingleMove = (SingleMove)this.currMove;

                if (Vector3.Distance(this.gameObject.transform.position, currSingleMove.goTo) < radiusToObj)
                {
                    mainObject.InvokeEvent(currSingleMove.startEvent);
                    Debug.Log("Sent Start Event");

                    if (new System.DateTimeOffset(System.DateTime.UtcNow).ToUnixTimeSeconds() - this.timeStartedCurrTask > currSingleMove.duration){
                        mainObject.InvokeEvent(currSingleMove.stopEvent);
                        Debug.Log("Sent Stop Event for move index " + this.moveIndex);
                        this.GetNextMove();
                    }

                }
            } else if (this.currMove is ConditionalTask){
                
                ConditionalTask currConditionalTask = (ConditionalTask)this.currMove;
                if (currConditionalTask.conditionFunc()){
                    Debug.Log("Condition was true");
                    mainObject.InvokeEvent(currConditionalTask.trueEvent);
                    if (currConditionalTask.trueTask != null){
                        this.DoTask(currConditionalTask.trueTask.Value);
                    } else {
                        this.currTask = null;
                        this.currMove = null;
                    }
                    
                } else {
                    Debug.Log("Condition was FALSE");
                    
                    mainObject.InvokeEvent(currConditionalTask.falseEvent);
                    if (currConditionalTask.falseTask!= null){
                        this.DoTask(currConditionalTask.falseTask.Value);
                    } else {
                        this.currTask = null;
                        this.currMove = null;
                    }
                }
            }
        }

        this.face.CurrentSpriteIndex = (this.face.CurrentSpriteIndex + 1) % this.face.SpritesCount;


        if (Input.GetMouseButton(0))
        {
            Vector3 w = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            GetComponent<NavMeshAgent2D>().destination = w;
        }
    }

    public void DoTask(Task task){

        this.timeStartedCurrTask = new System.DateTimeOffset(System.DateTime.UtcNow).ToUnixTimeSeconds();
        MainObject mainObject = MainObject.Get();
        Debug.Log("Task is " + task);
        this.currTask = mainObject.allTasks[task];
        this.currMove = this.currTask[0];
        this.moveIndex = 0;
        if (this.currMove is SingleMove){

            SingleMove currSingleMove = (SingleMove)this.currMove;
            GetComponent<NavMeshAgent2D>().destination = currSingleMove.goTo;
        }
    }

    void GetNextMove(){
        if (this.moveIndex < this.currTask.Length - 1){
            this.moveIndex++;
            this.currMove = this.currTask[moveIndex];
        } else {
            this.currMove = null;
            this.currTask = null;
        }
    }
}
