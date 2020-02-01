﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bed : MonoBehaviour
{
    public bool isBedMade = true;
    // Start is called before the first frame update
    void Start()
    {
        MainObject mainObject = MainObject.Get();
        mainObject.onSomethingHappened += this.listener;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public static bool IsMade()
    {
        return GameObject.Find("Bed").GetComponent<Bed>().isBedMade;
    }

    void listener(OurEvent whatHappened)
    {
        switch (whatHappened)
        {
            case OurEvent.MAKE_BED_START:
                break;

            case OurEvent.MAKE_BED_STOP:
                this.isBedMade = true;
                break;

            case OurEvent.SLEEP_STOP:
                this.isBedMade = false;
                break;
            default:
                return;
        }
        string toLoad;
        if (this.isBedMade)
        {
            toLoad = "bed0";
        }
        else
        {
            toLoad = "bed1";
        }
        Sprite bedSprite = Resources.Load<Sprite>("Sprites/" + toLoad);
        this.GetComponent<SpriteRenderer>().sprite = bedSprite;
        this.GetComponent<SpriteRenderer>().sortingOrder = 1;
    }
}
