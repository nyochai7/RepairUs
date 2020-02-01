using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TV : MonoBehaviour
{
    // Start is called before the first frame update
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

    public static bool IsInUse()
    {
        return GameObject.Find("tv").GetComponent<TV>().isInUse;
    }

    void listener(OurEvent whatHappened, GameObject invoker)
    {
        switch (whatHappened)
        {
            case OurEvent.WATCH_TV_START:
                this.isInUse = true;
                break;
            case OurEvent.WATCH_TV_STOP:
                this.isInUse = false;
                break;
            default:
                return;
        }
        string toLoad;
        if (this.isInUse)
        {
            toLoad = "Tv_on";
        }
        else
        {
            toLoad = "Tv_off";
        }
        Sprite sprite = Resources.Load<Sprite>("Sprites/" + toLoad);
        this.GetComponent<SpriteRenderer>().sprite = sprite;
        this.GetComponent<SpriteRenderer>().sortingOrder = 1;
    }
}
