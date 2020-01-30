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

public abstract class LocationMonitorable : GameObject
{
    public abstract void onLocationAlert(string name, ILocationMonitorable other);
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
        First.onLocationAlert(name, Second);
        if (whoToAlert == WhoToAlert.Both)
        {
            Second.onLocationAlert(name, First);
        }
    }
}

class RadiusRelation : RelationToMonitor
{
    private const int MIN_RADIUS = 50;

    protected override bool IsActive()
    {
        float distance = Vector3.Distance(
                   this.First.GetComponent<Transform>().transform.position,
                   this.Second.GetComponent<Transform>().transform.position);

        return distance <= MIN_RADIUS;
    }
}