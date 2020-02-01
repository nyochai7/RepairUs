using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Toilet : MonoBehaviour
{
    // Start is called before the first frame update
    public bool isUp = false;
    public bool isTaken = false;
    void Start()
    {
        MainObject mainObject = MainObject.Get();
        mainObject.onSomethingHappened += this.listener;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public static bool IsTaken(){
        return !GameObject.Find("Toilet_Down").GetComponent<Toilet>().isTaken;
    }

    void listener(OurEvent whatHappened, GameObject invoker)
    {
        switch (whatHappened)
        {
            case OurEvent.LOWER_TOILET_SEAT:
                this.isUp = false;
                break;
            case OurEvent.RAISE_TOILET_SEAT:
                this.isUp = true;
                break;
            case OurEvent.USE_BATHROOM_START:
                this.isTaken = true;
                break;
            case OurEvent.USE_BATHROOM_STOP:
                this.isTaken = false;
                break;
            case OurEvent.RESET_ALL:
                this.isUp = false;
                break;
            default:
                return;
        }
        string toLoad;
        if (this.isUp)
        {
            toLoad = "Toilet_Up";
        }
        else
        {
            toLoad = "Toilet_Seat";
        }
        Sprite bedSprite = Resources.Load<Sprite>("ScottsSprites/" + toLoad);
        this.GetComponent<SpriteRenderer>().sprite = bedSprite;
        this.GetComponent<SpriteRenderer>().sortingOrder = 1;
    }
}
