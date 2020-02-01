using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HisCharacterProps : CharacterProps
{
    public override void InitiateLocationListeners()
    {
        base.InitiateLocationListeners();

        MainObject.Get().locationManager.monitors.Add(new RadiusRelation(
            "table", this, new FakeILocationMonitorable(GameObject.Find("table")), WhoToAlert.OnlyFirst));
        MainObject.Get().locationManager.monitors.Add(new RadiusRelation(
            "laundry_basket", this, new FakeILocationMonitorable(GameObject.Find("laundry_basket")), WhoToAlert.OnlyFirst));
        MainObject.Get().locationManager.monitors.Add(new RadiusRelation(
            "bed", this, new FakeILocationMonitorable(GameObject.Find("Bed")), WhoToAlert.OnlyFirst));
    }
    public override void onMonitorAlertFunc(string name, ILocationMonitorable otherObj)
    {
        base.onMonitorAlertFunc(name, otherObj);
        if (name == "laundry_basket"){
            if (LaundryBasket.HasClothes()){
                MainObject.Get().InvokeEvent(OurEvent.SAY_TYPICAL, this.gameObject);
            } else {
                MainObject.Get().InvokeEvent(OurEvent.SAY_HAPPY, this.gameObject);
            }
        }

        if (name == "shoes"){
            if (!Shoes.IsClean()){
                MainObject.Get().InvokeEvent(OurEvent.SAY_TYPICAL, this.gameObject);
            }
        }

        if (name == "makeup_table"){
            if(!MakeupTable.IsClean()){
                MainObject.Get().InvokeEvent(OurEvent.SAY_TYPICAL, this.gameObject);
            } else {
                MainObject.Get().InvokeEvent(OurEvent.SAY_HAPPY, this.gameObject);
            }
        }
    }
}
