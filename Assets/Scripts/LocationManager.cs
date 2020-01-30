using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocationManager : MonoBehaviour
{
    public List<RelationToMonitor> monitors = new List<RelationToMonitor>();

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        foreach (RelationToMonitor rtm in monitors)
        {
            rtm.Check();
        }
    }
}


public interface ILocationMonitorable
{
    void onMonitorAlertFunc(string name, ILocationMonitorable otherObj);
    T GetComponent<T>();
}

public enum WhoToAlert
{
    OnlyFirst,
    Both
}

public abstract class RelationToMonitor
{
    public string Name { get; set; }
    public ILocationMonitorable First { get; set; }
    public ILocationMonitorable Second { get; set; }
    public WhoToAlert whoToAlert { get; set; }
   
    public RelationToMonitor(string name, ILocationMonitorable first, ILocationMonitorable second, WhoToAlert whoToAlert)
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
        First.onMonitorAlertFunc(Name, Second);
        if (whoToAlert == WhoToAlert.Both)
        {
            Second.onMonitorAlertFunc(Name, First);
        }
    }
}

public class RadiusRelation : RelationToMonitor
{
    private const int DEFAULT_RADIUS = 50;
    public int Radius { get; set; }

    public RadiusRelation(string name, ILocationMonitorable first, ILocationMonitorable second, WhoToAlert whoToAlert):
        base(name, first, second, whoToAlert)
    {
        this.Radius = DEFAULT_RADIUS;
    }

    protected override bool IsActive()
    {
        float distance = Vector3.Distance(
                   this.First.GetComponent<Transform>().transform.position,
                   this.Second.GetComponent<Transform>().transform.position);

        return distance <= this.Radius;
    }
}