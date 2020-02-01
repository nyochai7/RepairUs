using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaundryBasket : MonoBehaviour
{
    // Start is called before the first frame update
    bool isTaken = false;
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
            case OurEvent.TAKE_CLOTHES_STOP:
                this.isTaken = true;
                break;
            default:
                return;
        }
        if (this.isTaken)
        {
            Sprite basketSprite = Resources.Load<Sprite>("Sprites/laundry_basket1");
            this.GetComponent<SpriteRenderer>().sprite = basketSprite;
            this.GetComponent<SpriteRenderer>().sortingOrder = 1;
        }
    }
}
