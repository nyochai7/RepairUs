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

    void listener(OurEvent whatHappened)
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
            default:
                return;
        }
        string toLoad;
        if (this.isClean)
        {
            toLoad = "makeup_table0";
        }
        else
        {
            toLoad = "makeup_table1";
        }
        Sprite sprite = Resources.Load<Sprite>("Sprites/" + toLoad);
        this.GetComponent<SpriteRenderer>().sprite = sprite;
        this.GetComponent<SpriteRenderer>().sortingOrder = 1;
    }
}
