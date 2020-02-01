using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterProps : MonoBehaviour, ILocationMonitorable
{
    MainCharacter mc;
    // Start is called before the first frame update
    void Start()
    {
        mc = GetComponent<MainCharacter>();

        InitiateLocationListeners();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public virtual void InitiateLocationListeners()
    {
        MainObject.Get().locationManager.monitors.Add(new RadiusRelation(
            "sink", this, new FakeILocationMonitorable(GameObject.Find("sink")), WhoToAlert.OnlyFirst));
        MainObject.Get().locationManager.monitors.Add(new RadiusRelation(
            "bed", this, new FakeILocationMonitorable(GameObject.Find("Bed")), WhoToAlert.OnlyFirst));
    }
    public virtual void onMonitorAlertFunc(string name, ILocationMonitorable otherObj)
    {
        if (name == "sink")
        {
            mc.GetComponent<SpriteRenderer>().color = Color.green;
        }
        if (name == "bed")
        {
            MainObject.Get().InvokeEvent(OurEvent.EAT_STOP);
            MainObject.Get().InvokeEvent(OurEvent.EAT_STOP);
            MainObject.Get().InvokeEvent(OurEvent.EAT_STOP);
            MainObject.Get().InvokeEvent(OurEvent.EAT_STOP);

            mc.DoTask(Task.DO_DISHES);
        }
    }
}
