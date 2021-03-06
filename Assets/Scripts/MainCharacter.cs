﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;

public class MainCharacter : MonoBehaviour, ILocationMonitorable
{
    System.Random rnd = new System.Random();

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
    NavMeshAgent2D navMeshAgent;
    SpriteRenderer spriteRenderer;

    Vector3 prevPos;

    private int happiness;

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
        navMeshAgent = GetComponent<NavMeshAgent2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        MainObject.Get().onSomethingHappened+= MainCharacter_onSomethingHappened;

        Happiness = 50;
    }

    private string RandomString(String[] words)
    {
        return words[rnd.Next(words.Length)];
    }

    private void MainCharacter_onSomethingHappened(OurEvent whatHappened, GameObject invoker)
    {
        string[] ANGRY_WORDS = new string[]
        {
            "WTF!",
            "This sucks",
            "Screw him"
        };

        string[] HAPPY_WORDS = new string[]
        {
            "Beautiful",
            "Yay",
            "Woohoo",
            "Love this"
        };

        string[] TYPICAL_WORDS = new string[]
        {
            "Typical",
            "Bah",
            "Meh",
            "Man.."
        };

        if (invoker == null) {
            return;
        }

        if (invoker.gameObject.GetInstanceID() == this.gameObject.GetInstanceID())
        {
            if (whatHappened == OurEvent.SAY_ANGRY)
            {
                this.Speak(RandomString(ANGRY_WORDS));
                this.Happiness -= 8;
            }
            else if (whatHappened == OurEvent.SAY_HAPPY)
            {
                this.Speak(RandomString(HAPPY_WORDS));
                this.Happiness += 10;
            }
            else if (whatHappened == OurEvent.SAY_TYPICAL)
            {
                this.Speak(RandomString(TYPICAL_WORDS));
                this.Happiness -= 4;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        const float FAST_SPEED = 10;
        const float NORMAL_SPEED = 2.5f;

        navMeshAgent.speed = Utils.IsFastForward() ? FAST_SPEED : NORMAL_SPEED;
        navMeshAgent.angularSpeed = Utils.IsFastForward() ? 200 : 120;
        navMeshAgent.acceleration = Utils.IsFastForward() ? 25 : 8;


        Vector3 relativePos = transform.position - prevPos;
        float angle = Mathf.Atan2(relativePos.y, relativePos.x) * Mathf.Rad2Deg;
        angle += (float)Math.PI / 2.0f;
        //angle += 3f * (float)Math.PI / 4.0f;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        
        prevPos = transform.position;


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

                    float duration = Utils.IsFastForward() ? currSingleMove.duration / 4.0f : currSingleMove.duration;

                    if (new System.DateTimeOffset(System.DateTime.UtcNow).ToUnixTimeSeconds() - this.timeStartedCurrTask > duration)
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
        }
        else if (this.currTask == null)
        {

            Debug.Log("Getting next Task");
            Task? nextTask = blockList.GetNextTask();
            if (nextTask != null)
            {
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

    public void SetCurrTask(GeneralTask[] gt)
    {
        this.currTask = gt;
    }

    public void SetCurrTask(Task task)
    {

        bool isHer = (gameObject.name == "Sabrina");
        Task actualTask;
        switch (task)
        {
            case Task.USE_TOILET:
                if (isHer)
                    actualTask = Task.USE_TOILET_HER;
                else
                    actualTask = Task.USE_TOILET_HIM;
                break;
            case Task.EAT_GOOD_FOOD:
                if (isHer)
                    actualTask = Task.EAT_GOOD_FOOD_HER;
                else
                    actualTask = Task.EAT_GOOD_FOOD_HIM;
                break;

            case Task.EAT_BAD_FOOD:
                if (isHer)
                    actualTask = Task.EAT_BAD_FOOD_HER;
                else
                    actualTask = Task.EAT_BAD_FOOD_HIM;
                break;
            default:
                actualTask = task;
                break;
        }

        this.currTask = MainObject.Get().allTasks[actualTask];
    }
}