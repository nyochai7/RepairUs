using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BookTable : MonoBehaviour
{
    // Start is called before the first frame update
    public bool isBook = true;
    void Start()
    {

        MainObject mainObject = MainObject.Get();
        mainObject.onSomethingHappened += this.listener;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public static bool IsBookThere()
    {
        return GameObject.Find("book").GetComponent<BookTable>().isBook;
    }

    void listener(OurEvent whatHappened)
    {
        switch (whatHappened)
        {
            case OurEvent.TAKE_BOOK_START:
                break;
            case OurEvent.TAKE_BOOK_STOP:
                this.isBook = false;
                break;
            default:
                return;
        }
        string toLoad;
        if (this.isBook)
        {
            toLoad = "book0";
        }
        else
        {
            toLoad = "book1";
        }
        Sprite sprite = Resources.Load<Sprite>("Sprites/" + toLoad);
        this.GetComponent<SpriteRenderer>().sprite = sprite;
        this.GetComponent<SpriteRenderer>().sortingOrder = 1;
    }
}
