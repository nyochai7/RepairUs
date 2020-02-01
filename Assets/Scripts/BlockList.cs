using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockList : MonoBehaviour
{
    private const int MAX_BLOCKS = 10;
    private const float BLOCK_HEIGHT = 0.5f;
    BlockObj[] blocks;

    private int currentRunningTaskIndex;

    public int CurrentRunningTaskIndex
    {
        get
        {
            return currentRunningTaskIndex;
        }

        set
        {
            if (currentRunningTaskIndex >= 0 && 
                currentRunningTaskIndex < blocks.Length &&
                blocks[currentRunningTaskIndex] != null)
            {
                blocks[currentRunningTaskIndex].IsActive = false;
            }

            currentRunningTaskIndex = value;

            if (currentRunningTaskIndex >= 0 &&
                currentRunningTaskIndex < blocks.Length &&
                blocks[currentRunningTaskIndex] != null)
            {
                blocks[currentRunningTaskIndex].IsActive = true;
            }
        }
    }



    void Awake()
    {


    }

    // Start is called before the first frame update
    void Start()
    {
        MainObject.Get().AllBlockLists.Add(this);
        blocks = new BlockObj[MAX_BLOCKS];
        this.CurrentRunningTaskIndex = blocks.Length;

        /*blocks[0] = Instantiate(MainObject.Get().blockObjPrefab,
                   Vector3.zero,
                   Quaternion.identity).GetComponent<BlockObj>();
        blocks[1] = Instantiate(MainObject.Get().blockObjPrefab,
                           Vector3.zero,
                           Quaternion.identity).GetComponent<BlockObj>();*/
        blocks[9] = Instantiate(MainObject.Get().blockObjPrefab,
                           Vector3.zero,
                           Quaternion.identity).GetComponent<BlockObj>();

        blocks[9].IsUndeletable = true;
        blocks[9].task = Task.EAT;

        foreach (BlockObj block in blocks)
        {
            if (block != null)
            {
                block.myList = this;
                block.ResetPosByIndex();
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log("Mouse: " + Camera.main.ScreenToWorldPoint(Input.mousePosition));

        //PositionToIndex(IndexToPosition(0));
        //PositionToIndex(IndexToPosition(1));
        //PositionToIndex(IndexToPosition(2));

        //Debug.Log("Rect: " + Utils.SRtoRect(this.GetComponent<SpriteRenderer>()));
        /*
        Debug.Log("X" + transform.position.x);
        Debug.Log("Y" + transform.position.y);
        Debug.Log("w" + this.GetComponent<SpriteRenderer>().size.x);
        Debug.Log("h" + this.GetComponent<SpriteRenderer>().size.y);*/

        //Debug.Log("bounds" + this.GetComponent<SpriteRenderer>().bounds);
        //Debug.Log("w" + this.GetComponent<SpriteRenderer>().bounds.size.x);
        //Debug.Log("h" + this.GetComponent<SpriteRenderer>().bounds.size.y);
    }

    public Task? GetNextTask(){
        Debug.Log("task index = " + this.CurrentRunningTaskIndex);
        if (this.CurrentRunningTaskIndex > 0){
            this.CurrentRunningTaskIndex--;
            Debug.Log("Next Task is " + this.blocks[this.CurrentRunningTaskIndex].task + ", index=" + this.CurrentRunningTaskIndex);
            return this.blocks[this.CurrentRunningTaskIndex].task;
        } else {
            return Task.SLEEP;
        }
    }

    public void SetBlock(int index, BlockObj block)
    {
        Debug.Log("Setting block at index " + index);
        this.blocks[index] = block;
    }

    public int IndexOf(BlockObj block)
    {
        for (int i = 0; i < blocks.Length; i++)
        {
            if (blocks[i] == block)
            {
                return i;
            }
        }

        return -1;
    }

    public bool RemoveBlock(BlockObj block)
    {
        for (int i = 0; i < blocks.Length; i++)
        {
            if (blocks[i] == block)
            {
                blocks[i] = null;
                return true;
            }
        }

        return false;
    }

    public Vector3? GetPossibleLocation(Vector3 position)
    {
        int? possibleIndex = this.PositionToIndex(position);

        Debug.Log("PossibleIndex=" + possibleIndex.ToString());
        if (possibleIndex != null)
        {
            Debug.Log("this.blocks=" + this.blocks.ToString());
            //Debug.Log("this.blocks.length=" + this.blocks.Length)
            if (this.blocks[possibleIndex.Value] == null)
            {
                Debug.Log("this.blocks[possibleIndex.Value]=null");

            }
            else
            {
                Debug.Log("this.blocks[possibleIndex.Value]=" + this.blocks[possibleIndex.Value].ToString());

            }

        }

        if (possibleIndex != null && this.blocks[possibleIndex.Value] == null)
        {
            // The conversion back and forth rounds the Y axis value :)
            return this.IndexToPosition(possibleIndex.Value);
        }
        else
        {
            return null;
        }
    }

    public Vector3 IndexToPosition(int i)
    {
        if (i < 0 || i >= this.blocks.Length)
        {
            throw new System.Exception();
        }

        Rect myRect = Utils.SRtoRect(this.GetComponent<SpriteRenderer>());

        //Debug.Log("Index=" + i);
        //Debug.Log("myRect=" + myRect);
        //Debug.Log("newY=" + (myRect.y + i * BLOCK_HEIGHT));

        return new Vector3(myRect.x, myRect.y +
            i * BLOCK_HEIGHT, this.transform.position.z);
    }

    public int? PositionToIndex(Vector3 pos)
    {
        if (!Utils.PointInSprite(this.GetComponent<SpriteRenderer>(), pos))
        {
            Debug.Log("Not in");
            return null;
        }

        float myY = Utils.SRtoRect(this.GetComponent<SpriteRenderer>()).y;

        int index = (int)((pos.y - myY) / BLOCK_HEIGHT);

        if (index < 0 || index >= blocks.Length)
        {
            Debug.Log("Bad index: " + index.ToString());
            //Debug.Log("pos.y: " + pos.y.ToString());
            //Debug.Log("myY: " + myY.ToString());

            return null;
        }

        Debug.Log("Index:" + index.ToString());

        return index;
    }

}
