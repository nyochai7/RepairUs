using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bed : MonoBehaviour
{
    bool isBedMade = true;
    // Start is called before the first frame update
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
            case OurEvent.MAKE_BED_START:
                Debug.Log("Starting to make bed");
                break;

            case OurEvent.MAKE_BED_STOP:
                Debug.Log("Bed made");
                this.isBedMade = true;
                break;

            case OurEvent.SLEEP_STOP:
                this.isBedMade = false;
                Debug.Log("Bed is unmade");
                break;
            default:
                return;
        }
        string toLoad;
        if (this.isBedMade)
        {
            toLoad = "bed0";
        }
        else
        {
            toLoad = "bed1";
        }
        Sprite bedSprite = Resources.Load<Sprite>("Sprites/" + toLoad);
        this.GetComponent<SpriteRenderer>().sprite = bedSprite;
        this.GetComponent<SpriteRenderer>().sortingOrder = 1;
    }
}
