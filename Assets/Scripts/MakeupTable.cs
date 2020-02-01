using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MakeupTable : MonoBehaviour
{
    // Start is called before the first frame update

    public bool isClean = false;
    void Start()
    {
        MainObject mainObject = MainObject.Get();
        this.isClean = false;
        mainObject.onSomethingHappened += this.listener;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public static bool IsClean()
    {
        return GameObject.Find("makeup_table").GetComponent<MakeupTable>().isClean;
    }

    void listener(OurEvent whatHappened, GameObject invoker)
    {
        switch (whatHappened)
        {
            case OurEvent.CLEAN_MAKEUP_TABLE_START:
                GetComponent<AudioSource>().Play();
                break;

            case OurEvent.CLEAN_MAKEUP_TABLE_STOP:
                GetComponent<AudioSource>().Stop();
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
            toLoad = "BathroomSink_Clean";
        }
        else
        {
            toLoad = "BathroomSink_Messy";
        }
        Sprite sprite = Resources.Load<Sprite>("ScottsSprites/" + toLoad);
        this.GetComponent<SpriteRenderer>().sprite = sprite;
        this.GetComponent<SpriteRenderer>().sortingOrder = 1;
    }
}
