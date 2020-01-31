using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockList : MonoBehaviour
{
    private const int MAX_BLOCKS = 3;
    private const int BLOCK_HEIGHT = 1;
    public BlockObj[] blocks = new BlockObj[MAX_BLOCKS];
    // Start is called before the first frame update
    void Start()
    {
        MainObject.Get().AllBlockLists.Add(this);
    }

    // Update is called once per frame
    void Update()
    {

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

    public static bool PointInSprite(SpriteRenderer spriteObj, Vector3 position)
    {
        return SRtoRect(spriteObj).Contains(position);
    }

    public Vector3? GetPossibleLocation(Vector3 position)
    {
        int? possibleIndex = this.PositionToIndex(position);

        if (possibleIndex != null && this.blocks[possibleIndex.Value] == null)
        {
            // The conversion back and forth rounds the Y axis value :)
            return this.IndexToPosition(possibleIndex.Value);
        } else
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

        Rect myRect = SRtoRect(this.GetComponent<SpriteRenderer>());

        return new Vector3(myRect.x, myRect.y +
            i * BLOCK_HEIGHT, this.transform.position.z);
    }

    public int? PositionToIndex(Vector3 pos)
    {
        if (!PointInSprite(this.GetComponent<SpriteRenderer>(), pos))
        {
            return null;
        }

        float myY = SRtoRect(this.GetComponent<SpriteRenderer>()).y;
        // Debug.Log("pos y:" + pos.y.ToString());
        Debug.Log("transform y:" + myY.ToString());

        return (int)((pos.y - myY) / BLOCK_HEIGHT);
    }

    public static Rect SRtoRect(SpriteRenderer sr)
    {
        return new Rect(sr.transform.position.x - sr.size.x / 2.0f, sr.transform.position.y - sr.size.y / 2.0f,
                              sr.size.x, sr.size.y);
    }
}
