using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BlockObj : MonoBehaviour
{
    public BlockList myList;
    public bool IsDragged { get; set; }
    public Task task { get; set; }

    SpriteRenderer lockSpriteRend;

    public bool LockVisible
    {
        set
        {
            if (lockSpriteRend != null)
            {
                Color c = lockSpriteRend.color;
                c.a = value ? 1f : 0f;
                lockSpriteRend.color = c;
            }
        }
    }
    private bool isUndeletable = false;
    public bool IsUndeletable
    {
        get
        {
            return this.isUndeletable || this.IsActive;
        }
        set
        {
            this.isUndeletable = value;
            LockVisible = value;
        }
    }

    private bool isActive = false;

    public bool IsActive
    {

        get
        {
            return isActive;
        }

        set
        {
            isActive = value;
            GetComponent<SpriteRenderer>().color = isActive ? Color.red : new Color(0x8B / 255f, 0xE5 / 255f, 0x38 / 255f);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        TextMeshPro tm = transform.Find("A/Title").GetComponent<TextMeshPro>();
        tm.SetText(Utils.GetTaskTitle(task));

        lockSpriteRend = transform.Find("lock").GetComponent<SpriteRenderer>();
        LockVisible = this.IsUndeletable;
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
        if (this.myList != null && !IsUndeletable)
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
            this.transform.position = possiblePos.Value;

            int? index = selectedList.PositionToIndex(this.transform.position);

            selectedList.RemoveBlock(this);
            // We know index is not null because we just checked location is valid using findLocationInAllLists
            selectedList.SetBlock(index.Value, this);

            myList = selectedList;
        }
        else
        {
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
