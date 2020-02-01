using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;

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
    public BlockList blockList;
    [SerializeField]
    DynamicFace face;

    [SerializeField]
    public GameObject happyFacesPrefab;

    [SerializeField]
    public GameObject sadFacesPrefab;

    [SerializeField]
    public GameObject speechBubblePrefab;

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
            int newValue = value;
            if (newValue < 0)
            {
                newValue = 0;
            }
            if (newValue > 99)
            {
                newValue = 99;
            }

            if (newValue != happiness)
            {
                ShowSmallFaces(newValue > happiness);
            }

            happiness = newValue;

            face.CurrentSpriteIndex = happiness / 20;
        }
    }

    private void ShowSmallFaces(bool isHappy)
    {
        const float SMILE_FACE_TIME = 2;

        Vector3 pos = transform.position;
        pos.z = -4;

        GameObject obj = Instantiate(isHappy ? happyFacesPrefab : sadFacesPrefab,
                          pos,
                           Quaternion.identity);
        obj.transform.parent = this.transform;

        Destroy(obj, SMILE_FACE_TIME);
    }

    public void onMonitorAlertFunc(string name, ILocationMonitorable otherObj)
    {
        charProps.onMonitorAlertFunc(name, otherObj);
    }

    public void Speak(string text)
    {
        const float SPEECH_BUBBLE_TIME = 4;

        GameObject obj = Instantiate(speechBubblePrefab,
                           transform.position,
                           Quaternion.identity);

        obj.transform.parent = this.transform;
        Vector3 pos = obj.transform.position;
        pos.y += 0.5f;
        pos.x += 0.35f;
        pos.z = -4;
        obj.transform.position = pos;
        //obj.transform.position.y -= 1;
        TextMeshPro tm = obj.transform.Find("A/SpeechText").GetComponent<TextMeshPro>();
        tm.SetText(text);

        Destroy(obj, SPEECH_BUBBLE_TIME);
    }

    // Start is called before the first frame update
    void Start()
    {
        // MainObject.Get().InvokeEvent(OurEvent.ADD_CLOTHES_TO_BASKET, this);
        charProps = GetComponent<CharacterProps>();

        MainObject.Get().locationManager.monitors.Add(new RadiusRelation(
            "sink", this, new FakeILocationMonitorable(GameObject.Find("sink")), WhoToAlert.OnlyFirst));
        MainObject.Get().locationManager.monitors.Add(new RadiusRelation(
            "bed", this, new FakeILocationMonitorable(GameObject.Find("Bed")), WhoToAlert.OnlyFirst));
        MainObject.Get().locationManager.monitors.Add(new RadiusRelation(
            "laundry_basket", this, new FakeILocationMonitorable(GameObject.Find("laundry_basket")), WhoToAlert.OnlyFirst));                
        MainObject.Get().locationManager.monitors.Add(new RadiusRelation(
            "laundry_basket", this, new FakeILocationMonitorable(GameObject.Find("laundry_basket")), WhoToAlert.OnlyFirst));
        MainObject.Get().locationManager.monitors.Add(new RadiusRelation(
            "laundry_basket", this, new FakeILocationMonitorable(GameObject.Find("laundry_basket")), WhoToAlert.OnlyFirst));    
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
                if (!this.alreadyMoved)
                {
                    GetComponent<NavMeshAgent2D>().destination = currSingleMove.goTo;
                    this.alreadyMoved = true;
                }
                if (Vector3.Distance(this.gameObject.transform.position, currSingleMove.goTo) < radiusToObj)
                {
                    if (!this.sentStartForMove)
                    {
                        this.sentStartForMove = true;
                        Debug.Log("Seding Start");
                        mainObject.InvokeEvent(currSingleMove.startEvent, this.gameObject);
                    }

                    if (new System.DateTimeOffset(System.DateTime.UtcNow).ToUnixTimeSeconds() - this.timeStartedCurrTask > currSingleMove.duration)
                    {
                        if (!this.sentStopForMove)
                        {
                            this.sentStopForMove = true;
                            Debug.Log("Seding Stop");
                            mainObject.InvokeEvent(currSingleMove.stopEvent, this.gameObject);
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
                    mainObject.InvokeEvent(currConditionalTask.trueEvent, this.gameObject);
                    if (currConditionalTask.trueTask != null)
                    {
                        this.SetCurrTask(currConditionalTask.trueTask.Value);
                        this.DoTask();
                    }
                    else
                    {
                        this.currTask = null;
                        this.currMove = null;
                    }
                }
                else
                {
                    mainObject.InvokeEvent(currConditionalTask.falseEvent, this.gameObject);
                    if (currConditionalTask.falseTask != null)
                    {
                        this.SetCurrTask(currConditionalTask.falseTask.Value);
                        this.DoTask();
                    }
                    else
                    {
                        this.currTask = null;
                        this.currMove = null;
                    }
                }
            }
        } else if (this.currTask == null){

            Debug.Log("Getting next Task");
            Task? nextTask = blockList.GetNextTask();
            if (nextTask != null){
                this.SetCurrTask(nextTask.Value);
                Debug.Log("Next task is not null");    
                Debug.Log("Task is " + nextTask.Value);
                this.DoTask();
            }
        }

        // if (Input.GetMouseButton(0))
        // {
        //     Vector3 w = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        //     GetComponent<NavMeshAgent2D>().destination = w;
        // }
    }

    public void DoTask()
    {
        this.timeStartedCurrTask = new System.DateTimeOffset(System.DateTime.UtcNow).ToUnixTimeSeconds();
        MainObject mainObject = MainObject.Get();
        this.currMove = this.currTask[0];
        this.moveIndex = 0;
    }

    void GetNextMove()
    {
        Debug.Log("getting next move");
        this.sentStartForMove = false;
        this.sentStopForMove = false;
        this.alreadyMoved = false;
        if (this.moveIndex < this.currTask.Length - 1)
        {
            Debug.Log("Got next!");

            this.moveIndex++;
            this.currMove = this.currTask[moveIndex];
        }
        else
        {
            Debug.Log("No more moves");
            this.currMove = null;
            this.currTask = null;
        }
    }

    public void SetCurrTask(GeneralTask[] gt){
        this.currTask = gt;
    }

    public void SetCurrTask(Task task){
        this.currTask = MainObject.Get().allTasks[task];
    }
}