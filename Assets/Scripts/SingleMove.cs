using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;

public class SingleMove : GeneralTask
{
    public Vector3 goTo;
    public OurEvent startEvent;
    public OurEvent stopEvent;
    public int duration;

    public SingleMove(Vector3 goTo, OurEvent startEvent, OurEvent stopEvent, int duration){
        this.goTo = goTo;
        this.startEvent = startEvent;
        this.stopEvent = stopEvent;
        this.duration = duration;
    }
}
