using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Toilet : MonoBehaviour
{
    // Start is called before the first frame update
    bool isUp = false;
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
            case OurEvent.LOWER_TOILET_SEAT:
                this.isUp = false;
                break;

            case OurEvent.RAISE_TOILET_SEAT:
                this.isUp = true;
                break;
            default:
                return;
        }
        string toLoad;
        if (this.isUp)
        {
            toLoad = "toilet1";
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
