using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shoes : MonoBehaviour
{
    // Start is called before the first frame update
    public bool isClean = false;
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

    void listener(OurEvent whatHappened, GameObject invoker)
    {
        switch (whatHappened)
        {
            case OurEvent.CLEAN_SHOES_START:
                break;

            case OurEvent.CLEAN_SHOES_STOP:
                this.isClean = true;
                break;
            case OurEvent.RESET_ALL:
                this.isClean = false;
                break;
            default:
                return;
        }
        string toLoad;
        if (this.isClean)
        {
            toLoad = "";
        }
        else
        {
            toLoad = "Shoes";
        }
        Sprite sprite = Resources.Load<Sprite>("ScottsSprites/" + toLoad);
        this.GetComponent<SpriteRenderer>().sprite = sprite;
        this.GetComponent<SpriteRenderer>().sortingOrder = 1;
    }
}
