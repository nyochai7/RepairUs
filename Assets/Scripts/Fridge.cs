﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fridge : MonoBehaviour
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
            case OurEvent.FRIDGE_START:
                break;

            case OurEvent.FRIDGE_STOP:
                break;
            default:
                return;
        }
    }
}
