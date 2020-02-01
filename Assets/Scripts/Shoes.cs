using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shoes : MonoBehaviour
{
    // Start is called before the first frame update
    bool isClean = false;
    void Start()
    {

        MainObject mainObject = MainObject.Get();
        mainObject.onSomethingHappened += this.listener;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public static bool IsClean()
    {
        return GameObject.Find("shoes").GetComponent<Shoes>().isClean;
    }

    void listener(OurEvent whatHappened)
    {
        switch (whatHappened)
        {
            case OurEvent.CLEAN_SHOES_START:
                break;

            case OurEvent.CLEAN_SHOES_STOP:
                this.isClean = true;
                break;
            default:
                return;
        }
        string toLoad;
        if (this.isClean)
        {
            toLoad = "shoes0";
        }
        else
        {
            toLoad = "shoes1";
        }
        Sprite sprite = Resources.Load<Sprite>("Sprites/" + toLoad);
        this.GetComponent<SpriteRenderer>().sprite = sprite;
        this.GetComponent<SpriteRenderer>().sortingOrder = 1;
    }
}
