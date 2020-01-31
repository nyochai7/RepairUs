using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laundry_Basket : MonoBehaviour
{
    // Start is called before the first frame update
    bool isFull = true;
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
            case OurEvent.RAISE_TOILET_SEAT: //change me to TAKE_LAUNDRY_START
                this.isFull = false;
                break;
            default:
                return;
        }
        if (!this.isFull)
        {
            Sprite basketSprite = Resources.Load<Sprite>("Sprites/laundry_basket1");
            this.GetComponent<SpriteRenderer>().sprite = basketSprite;
            this.GetComponent<SpriteRenderer>().sortingOrder = 1;
        }
    }
}
