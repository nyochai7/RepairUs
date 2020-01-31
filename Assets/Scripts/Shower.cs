using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shower : MonoBehaviour
{
    // Start is called before the first frame update
    bool isTaken = false;
    bool isInUse = false;
    void Start()
    {
        MainObject mainObject = MainObject.Get();
        mainObject.onSomethingHappened += this.listener;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public bool IsTaken()
    {
        return isTaken;
    }

    void listener(OurEvent whatHappened)
    {
        switch (whatHappened)
        {
            case OurEvent.LOWER_TOILET_SEAT: //change me to GO_SHOWER_START
                this.isTaken = true;
                break;
            case OurEvent.RAISE_TOILET_SEAT: //change me to USE_SHOWER_STOP
                this.isTaken = false;
                break;
            case OurEvent.USE_BATHROOM_START: //change me to USE_SHOWER_START
                this.isInUse = true;
                break;
            case OurEvent.USE_BATHROOM_STOP: //change me to USE_SHOWER_STOP
                this.isInUse = false;
                break;
            default:
                return;
        }
        string toLoad;
        if (this.isInUse)
        {
            toLoad = "shower1";
        }
        else
        {
            toLoad = "toilet0";
        }
        Sprite bedSprite = Resources.Load<Sprite>("Sprites/" + toLoad);
        this.GetComponent<SpriteRenderer>().sprite = bedSprite;
        this.GetComponent<SpriteRenderer>().sortingOrder = 1;
    }
}
