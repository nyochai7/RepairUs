using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shower : MonoBehaviour
{
    // Start is called before the first frame update
    public bool isTaken = false;
    public bool isInUse = false;
    void Start()
    {
        MainObject mainObject = MainObject.Get();
        mainObject.onSomethingHappened += this.listener;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void listener(OurEvent whatHappened, GameObject invoker)
    {
        switch (whatHappened)
        {
            case OurEvent.SHOWER_IS_TAKEN: 
                this.isTaken = true;
                break;
            case OurEvent.USE_SHOWER_STOP: 
                this.isTaken = false;
                this.isInUse = false;
                break;
            case OurEvent.USE_SHOWER_START: 
                this.isInUse = true;
                break;
            default:
                return;
        }
        string toLoad;
        if (this.isInUse)
        {
            toLoad = "Shower_Occupied";
        }
        else
        {
            toLoad = "Shower_Empty";
        }
        Sprite bedSprite = Resources.Load<Sprite>("ScottsSprites/" + toLoad);
        this.GetComponent<SpriteRenderer>().sprite = bedSprite;
        this.GetComponent<SpriteRenderer>().sortingOrder = 1;
    }

    public static bool IsTaken(){

        return !GameObject.Find("shower").GetComponent<Shower>().isTaken;
    }
}
