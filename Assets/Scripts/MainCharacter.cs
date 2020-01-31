using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCharacter : MonoBehaviour, ILocationMonitorable
{
    public void onMonitorAlertFunc(string name, ILocationMonitorable otherObj)
    {
        if (name == "sink")
        {
            GetComponent<SpriteRenderer>().color = Color.green;
        }
        if (name == "bed")
        {
            GetComponent<SpriteRenderer>().color = Color.red;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        MainObject.Get().locationManager.monitors.Add(new RadiusRelation(
            "sink", this, new FakeILocationMonitorable(GameObject.Find("sink")), WhoToAlert.OnlyFirst));
        MainObject.Get().locationManager.monitors.Add(new RadiusRelation(
                "bed", this, new FakeILocationMonitorable(GameObject.Find("Bed")), WhoToAlert.OnlyFirst));
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            //Vector3 w = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            //GetComponent<NavMeshAgent2D>().destination = w;
        }
        MainObject mainObject = MainObject.Get();
        mainObject.InvokeEvent(OurEvent.Dishes_Start);
    }
}
