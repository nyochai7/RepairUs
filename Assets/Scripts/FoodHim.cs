using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodHim : MonoBehaviour
{
    // Start is called before the first frame update
    int foodType = 0;
    void Start()
    {
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
            case OurEvent.FRIDGE_STOP_HIM:
                this.foodType = 2;
                break;

            case OurEvent.EAT_START_HIM:
                if (this.foodType == 0)
                    this.foodType = 1;
                loadSprite();
                break;

            case OurEvent.EAT_STOP_HIM:
                this.foodType = 0;
                loadSprite();
                break;
            default:
                return;
        }


    }
    private void loadSprite()
    {
        string spriteName = "";
        switch (this.foodType)
        {
            case 1:
                spriteName = "ScottsSprites/Dinner_Yum";
                break;

            case 2:
                spriteName = "ScottsSprites/Dinner_Gross";
                break;
            case 0:
                spriteName = "";
                break;
        }
        Sprite sprite = Resources.Load<Sprite>(spriteName);
        this.GetComponent<SpriteRenderer>().sprite = sprite;
    }
}




