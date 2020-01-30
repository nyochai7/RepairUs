using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainObject : MonoBehaviour
{
    public static MainObject MainObj;
    public delegate void onSomethingHappenedDeleg(OurEvent whatHappened);
    public event onSomethingHappenedDeleg onSomethingHappened;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void Awake()
    {
        if (MainObj != null)
            GameObject.Destroy(MainObj);
        else
            MainObj = this;

        DontDestroyOnLoad(this);
    }
}
