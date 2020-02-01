using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaundryBasket : MonoBehaviour
{
    // Start is called before the first frame update
    public bool hasClothes = false;
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
            case OurEvent.TAKE_CLOTHES:
                this.hasClothes = false;
                break;
            case OurEvent.ADD_CLOTHES_TO_BASKET:
                this.hasClothes = true;
                break;
            default:
                return;
        }
        if (this.hasClothes)
        {
            Sprite basketSprite = Resources.Load<Sprite>("Sprites/laundry_basket1");
            this.GetComponent<SpriteRenderer>().sprite = basketSprite;
            this.GetComponent<SpriteRenderer>().sortingOrder = 1;
        }
    }

    public static bool HasClothes(){
        return GameObject.Find("laundry_basket").GetComponent<LaundryBasket>().hasClothes;
    }
}
