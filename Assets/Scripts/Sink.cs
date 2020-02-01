using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Sink : MonoBehaviour
{
    // Start is called before the first frame update
    public int numDishes = 0;
    void Start()
    {
        this.numDishes = 1;
        MainObject mainObject = MainObject.Get();
        mainObject.onSomethingHappened += this.listener;
        //StartCoroutine(waiter());
    }

    // Update is called once per frame
    void Update()
    {

    }
    IEnumerator waiter()
    {
        MainObject mainObject = MainObject.Get();
        yield return new WaitForSeconds(20);
        mainObject.InvokeEvent(OurEvent.RESET_ALL, null);
        yield return new WaitForSeconds(8);
        mainObject.InvokeEvent(OurEvent.LAUNDRY_STOP, null);
    }

    void listener(OurEvent whatHappened, GameObject invoker)
    {
        switch(whatHappened)
        {
            case OurEvent.DISHES_START:
                GetComponent<AudioSource>().Play();
                break;

            case OurEvent.DISHES_STOP:
                GetComponent<AudioSource>().Stop();
                this.numDishes = 0;
                break;

            case OurEvent.EAT_STOP:
                if(this.numDishes != 4)
                {
                    this.numDishes++;
                }
                break;
            case OurEvent.COOK_STOP:
                if (this.numDishes != 4)
                {
                    this.numDishes++;
                }
                break;
            case OurEvent.RESET_ALL:
                this.numDishes = 1;
                break;
            default:
                return;
        }
        string toLoad;
        switch(this.numDishes)
        {
            case 0:
                toLoad = "KitchenSink_Clean";
                break;
            case 1:
                toLoad = "KitchenSink_Dirty1";
                break;
            default:
                toLoad = "KitchenSink_Dirty2";
                break;
        }
        Sprite sinkSprite = Resources.Load<Sprite>("ScottsSprites/" + toLoad);
        this.GetComponent<SpriteRenderer>().sprite = sinkSprite;
        this.GetComponent<SpriteRenderer>().sortingOrder = 1;
    }

    public static bool CheckSink(){
        
        return (GameObject.Find("sink").GetComponent<Sink>().numDishes > 0);
    }
}
