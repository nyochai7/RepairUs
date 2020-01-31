using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingleMove
{

    public Vector3 goTo;
    OurEvent startEvent;
    OurEvent stopEvent;
    int duration;

    public SingleMove(Vector3 goTo, OurEvent startEvent, OurEvent stopEvent, int duration){
        this.goTo = goTo;
        this.startEvent = startEvent;
        this.stopEvent = stopEvent;
        this.duration = duration;
    }
}
