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
            "table", this, new FakeILocationMonitorable(GameObject.Find("table")), WhoToAlert.OnlyFirst));
        MainObject.Get().locationManager.monitors.Add(new RadiusRelation(
            "plant", this, new FakeILocationMonitorable(GameObject.Find("plant")), WhoToAlert.OnlyFirst));
        MainObject.Get().locationManager.monitors.Add(new RadiusRelation(
            "bed", this, new FakeILocationMonitorable(GameObject.Find("Bed")), WhoToAlert.OnlyFirst));
        MainObject.Get().locationManager.monitors.Add(new RadiusRelation(
            "both_eating", (ILocationMonitorable)GameObject.Find("Sabrina").GetComponent<MainCharacter>(), 
            GameObject.Find("Chad").GetComponent<MainCharacter>(), WhoToAlert.Both));
        MainObject.Get().locationManager.monitors.Add(new RadiusRelation(
            "both_tving", (ILocationMonitorable)GameObject.Find("Sabrina").GetComponent<MainCharacter>(), 
            GameObject.Find("Chad").GetComponent<MainCharacter>(), WhoToAlert.Both));
        
    }
    public virtual void onMonitorAlertFunc(string name, ILocationMonitorable otherObj)
    {
        if (name == "both_eating"){

            MainCharacter sab = GameObject.Find("Sabrina").GetComponent<MainCharacter>();
            MainCharacter chad = GameObject.Find("Chad").GetComponent<MainCharacter>();
            if ((sab.blockList.CurrTask == Task.EAT_GOOD_FOOD || sab.blockList.CurrTask == Task.EAT_BAD_FOOD) && 
                (chad.blockList.CurrTask == Task.EAT_GOOD_FOOD || chad.blockList.CurrTask == Task.EAT_BAD_FOOD)){
                Debug.Log("EATING TOGETHER1");
                MainObject.Get().InvokeEvent(OurEvent.SAY_HAPPY, this.gameObject);
            }
        }

        if (name == "both_tving"){

            MainCharacter sab = GameObject.Find("Sabrina").GetComponent<MainCharacter>();
            MainCharacter chad = GameObject.Find("Chad").GetComponent<MainCharacter>();
            if ((sab.blockList.CurrTask == Task.WATCH_TV) && 
                (chad.blockList.CurrTask == Task.WATCH_TV)){
                
                MainObject.Get().InvokeEvent(OurEvent.SAY_HAPPY, this.gameObject);
            }
        }
    }
}
