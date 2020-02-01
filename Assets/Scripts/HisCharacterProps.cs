using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HisCharacterProps : CharacterProps
{
    public override void InitiateLocationListeners()
    {
        base.InitiateLocationListeners();

        MainObject.Get().locationManager.monitors.Add(new RadiusRelation(
            "makeup_table", this, new FakeILocationMonitorable(GameObject.Find("makeup_table")), WhoToAlert.OnlyFirst));
        MainObject.Get().locationManager.monitors.Add(new RadiusRelation(
            "laundry_basket", this, new FakeILocationMonitorable(GameObject.Find("laundry_basket")), WhoToAlert.OnlyFirst));

    }
    public override void onMonitorAlertFunc(string name, ILocationMonitorable otherObj)
    {
        base.onMonitorAlertFunc(name, otherObj);
        if (name == "laundry_basket"){
            if (LaundryBasket.HasClothes()){
                Debug.Log("Laundry basket has clothes");
                MainObject.Get().InvokeEvent(OurEvent.SAY_TYPICAL, this.gameObject);
            } else {
                Debug.Log("Laundry basket has NO clothes");
                MainObject.Get().InvokeEvent(OurEvent.SAY_HAPPY, this.gameObject);
            }
        }

        if (name == "shoes"){
            if (!Shoes.IsClean()){
                Debug.Log("SHOES DIRTY");
                MainObject.Get().InvokeEvent(OurEvent.SAY_TYPICAL, this.gameObject);
            }
        }

        if (name == "makeup_table"){
            if(!MakeupTable.IsClean()){
                Debug.Log("Makeup DIRTY");
                MainObject.Get().InvokeEvent(OurEvent.SAY_TYPICAL, this.gameObject);
            } else {
                Debug.Log("Makeup CLEAN");
                MainObject.Get().InvokeEvent(OurEvent.SAY_HAPPY, this.gameObject);
            }
        }
    }
}
