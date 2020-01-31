using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sink : MonoBehaviour
{
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

    void listener(OurEvent whatHappened)
    {
        switch(whatHappened)
        {
            case OurEvent.Dishes_Start:
                Debug.Log("Dishes are starting");
                break;

        }


    }
}
