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
            "shoes", this, new FakeILocationMonitorable(GameObject.Find("shoes")), WhoToAlert.OnlyFirst));
        MainObject.Get().locationManager.monitors.Add(new RadiusRelation(
            "plant", this, new FakeILocationMonitorable(GameObject.Find("plant")), WhoToAlert.OnlyFirst));
        MainObject.Get().locationManager.monitors.Add(new RadiusRelation(
            "bed", this, new FakeILocationMonitorable(GameObject.Find("Bed")), WhoToAlert.OnlyFirst));
    }
    public virtual void onMonitorAlertFunc(string name, ILocationMonitorable otherObj)
    {
        Debug.Log(name);
        if (name == "shoes")
        {
            //GetComponent<SpriteRenderer>().color = Color.green;
            mc.Happiness--;
            mc.Speak("Fuck shoes");
        }
        if (name == "plant")
        {
            //GetComponent<SpriteRenderer>().color = Color.green;
            mc.Happiness--;
        }
        if (name == "bed"){
        }
    }
}
