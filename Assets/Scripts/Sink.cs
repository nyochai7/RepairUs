using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sink : MonoBehaviour
{
    // Start is called before the first frame update
    int num_dishes = 0;
    void Start()
    {
        MainObject mainObject = MainObject.Get();
        mainObject.onSomethingHappened += this.listener;
        StartCoroutine(waiter());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    IEnumerator waiter()
    {
        MainObject mainObject = MainObject.Get();
        yield return new WaitForSeconds(2);
        mainObject.InvokeEvent(OurEvent.EAT_STOP);
        yield return new WaitForSeconds(2);
        mainObject.InvokeEvent(OurEvent.EAT_STOP);
        yield return new WaitForSeconds(2);
        mainObject.InvokeEvent(OurEvent.DISHES_STOP);
    }

    void listener(OurEvent whatHappened)
    {
        switch(whatHappened)
        {
            case OurEvent.DISHES_START:
                Debug.Log("Dishes are starting");
                break;

            case OurEvent.DISHES_STOP:
                Debug.Log("Dishes are done");
                this.num_dishes = 0;
                break;

            case OurEvent.EAT_STOP:
                if(this.num_dishes != 2)
                {
                    this.num_dishes++;
                }
                Debug.Log("More dirt in sink! Num is " + num_dishes.ToString());
                break;
            default:
                return;
        }
        Sprite sinkSprite = Resources.Load<Sprite>("Sprites/sink" + this.num_dishes.ToString());
        this.GetComponent<SpriteRenderer>().sprite = sinkSprite;
        this.GetComponent<SpriteRenderer>().sortingOrder = 1;


    }
}
