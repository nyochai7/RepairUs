using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class MainCharacter : MonoBehaviour, ILocationMonitorable
{

    public float radiusToObj = 3f;
    public long timeStartedCurrTask;
    public GeneralTask[] currTask;
    public GeneralTask currMove;
    int moveIndex;
    bool sentStartForMove = false;
    bool sentStopForMove = false;
    bool alreadyMoved = false;

    [SerializeField]
    DynamicFace face;

    CharacterProps charProps;

    private int happiness = 50;

    public int Happiness
    {
        get
        {
            return happiness;
        }

        set
        {
            happiness = value;
            if (happiness < 0)
            {
                happiness = 0;
            }
            if (happiness > 99)
            {
                happiness = 99;
            }
            face.CurrentSpriteIndex = happiness / 20;
        }
    }

    public void onMonitorAlertFunc(string name, ILocationMonitorable otherObj)
    {
        charProps.onMonitorAlertFunc(name, otherObj);

        if (name == "sink")
        {
            GetComponent<SpriteRenderer>().color = Color.green;
            this.DoTask(Task.EAT);

        }
        if (name == "bed")
        {
            this.DoTask(Task.USE_TOILET_HER);

        }
    }

    // Start is called before the first frame update
    void Start()
    {
        charProps = GetComponent<CharacterProps>();

        MainObject.Get().locationManager.monitors.Add(new RadiusRelation(
            "sink", this, new FakeILocationMonitorable(GameObject.Find("sink")), WhoToAlert.OnlyFirst));
        MainObject.Get().locationManager.monitors.Add(new RadiusRelation(
                "bed", this, new FakeILocationMonitorable(GameObject.Find("Bed")), WhoToAlert.OnlyFirst));
    }

    // Update is called once per frame
    void Update()
    {
        if (this.currTask != null && this.currMove != null)
        {
            MainObject mainObject = MainObject.Get();

            if (this.currMove is SingleMove)
            {
                SingleMove currSingleMove = (SingleMove)this.currMove;
                if (!this.alreadyMoved){
                    GetComponent<NavMeshAgent2D>().destination = currSingleMove.goTo;
                    this.alreadyMoved = true;
                }
                if (Vector3.Distance(this.gameObject.transform.position, currSingleMove.goTo) < radiusToObj)
                {
                    if (!this.sentStartForMove)
                    {
                        this.sentStartForMove = true;
                        Debug.Log("Seding Start");
                        mainObject.InvokeEvent(currSingleMove.startEvent);
                    }

                    if (new System.DateTimeOffset(System.DateTime.UtcNow).ToUnixTimeSeconds() - this.timeStartedCurrTask > currSingleMove.duration)
                    {
                        if (!this.sentStopForMove)
                        {
                            this.sentStopForMove = true;
                            Debug.Log("Seding Stop");
                            mainObject.InvokeEvent(currSingleMove.stopEvent);
                            this.GetNextMove();
                        }
                    }



                }
            }
            else if (this.currMove is ConditionalTask)
            {
                ConditionalTask currConditionalTask = (ConditionalTask)this.currMove;
                if (currConditionalTask.conditionFunc())
                {
                    Debug.Log("Condition was true");
                    mainObject.InvokeEvent(currConditionalTask.trueEvent);
                    if (currConditionalTask.trueTask != null)
                    {
                        this.DoTask(currConditionalTask.trueTask.Value);
                    }
                    else
                    {
                        this.currTask = null;
                        this.currMove = null;
                    }
                }
                else
                {
                    mainObject.InvokeEvent(currConditionalTask.falseEvent);
                    if (currConditionalTask.falseTask != null)
                    {
                        this.DoTask(currConditionalTask.falseTask.Value);
                    }
                    else
                    {
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

    public void DoTask(Task task)
    {

        this.timeStartedCurrTask = new System.DateTimeOffset(System.DateTime.UtcNow).ToUnixTimeSeconds();
        MainObject mainObject = MainObject.Get();
        Debug.Log("Task is " + task);
        this.currTask = mainObject.allTasks[task];
        this.currMove = this.currTask[0];
        this.moveIndex = 0;
    }

    void GetNextMove(){
        Debug.Log("getting next move");
        if (this.moveIndex < this.currTask.Length - 1){
            Debug.Log("Got next!");
            this.sentStartForMove = false;
            this.sentStopForMove = false;
            this.alreadyMoved = false;
            this.moveIndex++;
            this.currMove = this.currTask[moveIndex];
        } else {
            Debug.Log("No more moves");
            this.sentStartForMove = false;
            this.sentStopForMove = false;
            this.alreadyMoved = false;
            this.currMove = null;
            this.currTask = null;
        }
    }
}