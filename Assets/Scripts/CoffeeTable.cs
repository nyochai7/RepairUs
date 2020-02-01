using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoffeeTable : MonoBehaviour
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
        return GameObject.Find("coffee_table").GetComponent<CoffeeTable>().isClean;
    }

    void listener(OurEvent whatHappened, GameObject invoker)
    {
        switch (whatHappened)
        {
            case OurEvent.CLEAN_COFFEE_TABLE_START:
                this.isClean = true;
                break;

            case OurEvent.CLEAN_COFFEE_TABLE_STOP:
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
            toLoad = "CoffeeTable_Clean";
        }
        else
        {
            toLoad = "CoffeeTable_Messy";
        }
        Sprite sprite = Resources.Load<Sprite>("ScottsSprites/" + toLoad);
        this.GetComponent<SpriteRenderer>().sprite = sprite;
        this.GetComponent<SpriteRenderer>().sortingOrder = 1;
    }
}
