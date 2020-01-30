using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocationManager : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

public abstract class LocationMonitorable
{
    public GameObject gameObj { get; set; }
    public abstract void OnLocationAlert(string name, LocationMonitorable other);
}

public enum WhoToAlert
{
    OnlyFirst,
    Both
}

abstract class RelationToMonitor
{
    public string Name { get; set; }
    public LocationMonitorable First { get; set; }
    public LocationMonitorable Second { get; set; }
    public WhoToAlert whoToAlert { get; set; }
   
    public RelationToMonitor(string name, LocationMonitorable first, LocationMonitorable second, WhoToAlert whoToAlert)
    {
        this.Name = name;
        this.First = first;
        this.Second = second;
        this.whoToAlert = whoToAlert;
    }

    private bool previousValue;

    public void Check()
    {
        bool currentValue = previousValue;

        if (currentValue && !previousValue)
        {
            Alert();
        }

        this.previousValue = currentValue;
    }
    
    protected abstract bool IsActive();
    private void Alert()
    {
        First.OnLocationAlert(Name, Second);
        if (whoToAlert == WhoToAlert.Both)
        {
            Second.OnLocationAlert(Name, First);
        }
    }
}

class RadiusRelation : RelationToMonitor
{
    private const int DEFAULT_RADIUS = 50;
    public int Radius { get; set; }

    public RadiusRelation(string name, LocationMonitorable first, LocationMonitorable second, WhoToAlert whoToAlert):
        base(name, first, second, whoToAlert)
    {
        this.Radius = DEFAULT_RADIUS;
    }

    protected override bool IsActive()
    {
        float distance = Vector3.Distance(
                   this.First.gameObj.GetComponent<Transform>().transform.position,
                   this.Second.gameObj.GetComponent<Transform>().transform.position);

        return distance <= this.Radius;
    }
}