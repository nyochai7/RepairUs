using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockObj : MonoBehaviour
{
    public BlockList myList;
    public bool IsDragged { get; set; }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (IsDragged)
        {
            Vector3 w = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            w = new Vector3(w.x, w.y, 0);
            Debug.Log(this.transform.position);
            this.transform.position = w;

            Vector3? possiblePos = null;
            BlockList selectedList = null;

            Debug.Log("Dragging to " + w.ToString());
            possiblePos = findLocationInAllLists(ref selectedList);

            if (possiblePos != null)
            {
                this.transform.position = new Vector3(possiblePos.Value.x + 
                    GetComponent<SpriteRenderer>().size.x / 2.0f,
                    possiblePos.Value.y + GetComponent<SpriteRenderer>().size.y / 2.0f,
                    possiblePos.Value.z);
            }
        }
    }

    Vector3? findLocationInAllLists(ref BlockList outList)
    {
        foreach (BlockList list in MainObject.Get().AllBlockLists)
        {
            Vector3? possiblePos = list.GetPossibleLocation(this.transform.position);
            if (possiblePos != null)
            {
                //Debug.Log("PossiblePos=" + possiblePos.ToString());
                outList = list;
                return possiblePos;
            }
        }

        return null;
    }

    void OnMouseDown()
    {
        Debug.Log("IsDragged=true!");
        this.IsDragged = true;
        if (this.myList != null)
        {
            this.myList.RemoveBlock(this);
            this.myList = null;
        }
    }

    void OnMouseUp()
    {
        this.IsDragged = false;

        Vector3? possiblePos = null;
        BlockList selectedList = null;
        Debug.Log("Releasing...");
        possiblePos = findLocationInAllLists(ref selectedList);

        if (possiblePos != null)
        {
            
            Debug.Log("adding");

            this.transform.position = new Vector3(possiblePos.Value.x +
                GetComponent<SpriteRenderer>().size.x / 2.0f,
                possiblePos.Value.y + GetComponent<SpriteRenderer>().size.y / 2.0f,
                possiblePos.Value.z);

            int? index = selectedList.PositionToIndex(this.transform.position);

            Debug.Log("Index=" + index.ToString());
            // We know index is not null because we just checked location is valid using findLocationInAllLists
            selectedList.blocks[index.Value] = this;

            myList = selectedList;
        } else
        {
            Debug.Log("Not adding");
            this.myList = null;

            // Goodbye Yellow BlockObj
            // Destroy(this.gameObject);
        }
    }
}
