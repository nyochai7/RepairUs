using System;
using UnityEngine;

public class Utils{

    public static Vector3 getPositionByName(string name){
        GameObject go = GameObject.Find(name);

        if (go == null)
        {
            throw new Exception("Couldn't find game object: " + name);
        }

        return go.transform.position;
    }


    public static Rect SRtoRect(SpriteRenderer sr)
    {
        return new Rect(sr.bounds.center.x - sr.bounds.size.x / 2,
             sr.bounds.center.y - sr.bounds.size.y / 2,
             sr.bounds.size.x,
             sr.bounds.size.y);
        //return new Rect(sr.transform.position.x - sr.size.x / 2.0f, sr.transform.position.y - sr.size.y / 2.0f,
        //                      sr.size.x, sr.size.y);
    }

    public static bool PointInSprite(SpriteRenderer spriteObj, Vector3 position)
    {
        return Utils.SRtoRect(spriteObj).Contains(position);
    }

}