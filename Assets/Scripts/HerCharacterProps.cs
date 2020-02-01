using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HerCharacterProps : CharacterProps
{
    public override void InitiateLocationListeners()
    {
        base.InitiateLocationListeners();

        MainObject.Get().locationManager.monitors.Add(new RadiusRelation(
            "sink", this, new FakeILocationMonitorable(GameObject.Find("sink")), WhoToAlert.OnlyFirst));
        MainObject.Get().locationManager.monitors.Add(new RadiusRelation(
            "plant", this, new FakeILocationMonitorable(GameObject.Find("plant")), WhoToAlert.OnlyFirst));
        MainObject.Get().locationManager.monitors.Add(new RadiusRelation(
            "bed", this, new FakeILocationMonitorable(GameObject.Find("Bed")), WhoToAlert.OnlyFirst));
    }
    public override void onMonitorAlertFunc(string name, ILocationMonitorable otherObj)
    {
        base.onMonitorAlertFunc(name, otherObj);
        Debug.Log("Alert name=" + name);
        if (name =="sink"){
            if (!Sink.CheckSink()){
                Debug.Log("SINK DIRTY");
                MainObject.Get().InvokeEvent(OurEvent.SAY_ANGRY, this.gameObject);
            } else {
                Debug.Log("SINK CLEAN");
                MainObject.Get().InvokeEvent(OurEvent.SAY_HAPPY, this.gameObject);
            }
        }
        if (name == "bed"){
            if (Bed.IsMade()){
                Debug.Log("BED MADE");
                MainObject.Get().InvokeEvent(OurEvent.SAY_HAPPY, this.gameObject);
            }
        }
    }
}
