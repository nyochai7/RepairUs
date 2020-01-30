using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCharacter : MonoBehaviour, ILocationMonitorable
{
    public void onMonitorAlertFunc(string name, ILocationMonitorable otherObj)
    {
        if (name == "test")
        {
            Debug.Log("Test!");
            GetComponent<SpriteRenderer>().color = Color.green;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            Vector3 w = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            GetComponent<NavMeshAgent2D>().destination = w;
        }
    }
}
