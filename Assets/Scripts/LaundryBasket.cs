using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaundryBasket : MonoBehaviour
{
    // Start is called before the first frame update
    public bool hasClothes = false;
    void Start()
    {
        this.hasClothes = true;
        MainObject mainObject = MainObject.Get();
        mainObject.onSomethingHappened += this.listener;

    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void listener(OurEvent whatHappened, GameObject invoker)
    {
        switch (whatHappened)
        {
            case OurEvent.TAKE_CLOTHES:
                this.hasClothes = false;
                break;
            case OurEvent.ADD_CLOTHES_TO_BASKET:
                this.hasClothes = true;
                break;
            case OurEvent.RESET_ALL:
                this.hasClothes = true;
                break;
            default:
                return;
        }
        if (!this.hasClothes)
        {
            Sprite basketSprite = Resources.Load<Sprite>("");
            this.GetComponent<SpriteRenderer>().sprite = basketSprite;
            this.GetComponent<SpriteRenderer>().sortingOrder = 1;
        } else {
            Sprite basketSprite = Resources.Load<Sprite>("ScottsSprites/ClothesPile");
            this.GetComponent<SpriteRenderer>().sprite = basketSprite;
            this.GetComponent<SpriteRenderer>().sortingOrder = 1;
        }
    }

    public static bool HasClothes(){
        return GameObject.Find("laundry_basket").GetComponent<LaundryBasket>().hasClothes;
    }
}
