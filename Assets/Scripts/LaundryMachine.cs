using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaundryMachine : MonoBehaviour
{
    // Start is called before the first frame update
    public bool isFull = false;
    void Start()
    {
        MainObject mainObject = MainObject.Get();
        mainObject.onSomethingHappened += this.listener;

    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void listener(OurEvent whatHappened)
    {
        switch (whatHappened)
        {
            case OurEvent.LAUNDRY_START: 
                this.isFull = true;
                GetComponent<AudioSource>().Play();
                break;
            case OurEvent.LAUNDRY_STOP:
                this.isFull = false;
                GetComponent<AudioSource>().Stop();
                break;
            default:
                return;
        }
        string toLoad;
        if (this.isFull)
        {
            toLoad = "laundry_machine1";
        }
        else
        {
            toLoad = "laundry_machine0";
        }
        Sprite bedSprite = Resources.Load<Sprite>("Sprites/" + toLoad);
        this.GetComponent<SpriteRenderer>().sprite = bedSprite;
        this.GetComponent<SpriteRenderer>().sortingOrder = 1;
    }
}
