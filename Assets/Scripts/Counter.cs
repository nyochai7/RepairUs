﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Counter : MonoBehaviour
{
    // Start is called before the first frame update
    public int numFood = 0;
    void Start()
    {
        MainObject mainObject = MainObject.Get();
        mainObject.onSomethingHappened += this.listener;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void listener(OurEvent whatHappened, GameObject invoker)
    {
        switch (whatHappened)
        {
            case OurEvent.COOK_START:
                break;

            case OurEvent.PUT_FOOD_DOWN_START:
                this.numFood = 2;
                break;

            case OurEvent.EAT_COUNTER_FOOD:
                MainObject.Get().InvokeEvent(OurEvent.SAY_HAPPY, invoker);
                if(this.numFood != 0) //if this is 0 some other action needs to happen (eat cornflakes?)
                {
                    this.numFood--;
                }
                break;
            case OurEvent.RESET_ALL:
                this.numFood = 0;
                break;
            default:
                return;
        }
        Sprite counterSprite = Resources.Load<Sprite>("ScottsSprites/kitchen_counter" + this.numFood.ToString());
        this.GetComponent<SpriteRenderer>().sprite = counterSprite;
        this.GetComponent<SpriteRenderer>().sortingOrder = 1;
    }

    public static bool hasCookedFood(){
        return GameObject.Find("kitchen_counter").GetComponent<Counter>().numFood > 0;
    }
}
