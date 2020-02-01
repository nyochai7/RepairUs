using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockObj : MonoBehaviour
{
    public BlockList myList;
    public bool IsDragged { get; set; }
    public Task task { get; set; }

    private bool isUndeletable = false;
    public bool IsUndeletable
    {
        get
        {
            return this.isUndeletable;
        }
        set
        {
            this.isUndeletable = value;
            GetComponent<SpriteRenderer>().color = value ? Color.red : Color.green;
        }
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (IsDragged && !Input.GetMouseButton(0))
        {
            OnMouseUp();
        }

        if (IsDragged)
        {
            Vector3 w = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            w = new Vector3(w.x, w.y, 0);
            this.transform.position = w;

            Vector3? possiblePos = null;
            BlockList selectedList = null;

            possiblePos = findLocationInAllLists(ref selectedList);

            if (possiblePos != null && (!IsUndeletable || selectedList == myList))
            {
                this.transform.position = possiblePos.Value;
            }
        }
    }

    /*public Vector3 centerPos(Vector3 orig)
    {
        return new Vector3(orig.x +
                    GetComponent<SpriteRenderer>().size.x / 2.0f,
                    orig.y + GetComponent<SpriteRenderer>().size.y / 2.0f,
                    0);
    }*/

    Vector3? findLocationInAllLists(ref BlockList outList)
    {
        Debug.Log("findLocationInAllLists(" + this.transform.position);
        foreach (BlockList list in MainObject.Get().AllBlockLists)
        {
            Vector3? possiblePos = list.GetPossibleLocation(this.transform.position);
            if (possiblePos != null)
            {
                outList = list;
                Debug.Log("Looks good: " + possiblePos.ToString());
                return possiblePos;
            }
        }


        Debug.Log("Nope");
        return null;
    }

    void OnMouseDown()
    {
        this.IsDragged = true;
        if (this.myList != null && !isUndeletable)
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
        possiblePos = findLocationInAllLists(ref selectedList);

        if (possiblePos != null && (!IsUndeletable || selectedList == myList))
        {
            Debug.Log("A");
            this.transform.position = possiblePos.Value;

            int? index = selectedList.PositionToIndex(this.transform.position);

            selectedList.RemoveBlock(this);
            // We know index is not null because we just checked location is valid using findLocationInAllLists
            selectedList.SetBlock(index.Value, this);

            myList = selectedList;
        }
        else
        {
            Debug.Log("B" + possiblePos.ToString());
            if (IsUndeletable)
            {
                ResetPosByIndex();
            }
            else
            {
                this.myList = null;

                // Goodbye Yellow BlockObj
                Destroy(this.gameObject);
            }
        }
    }

    public void ResetPosByIndex()
    {
        //this.transform.position = centerPos(myList.IndexToPosition(myList.IndexOf(this)));
        this.transform.position = myList.IndexToPosition(myList.IndexOf(this));
    }
}
