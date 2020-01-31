using System;
using UnityEngine;

public class Utils{

    public static Vector3 getPositionByName(string name){
        return GameObject.Find(name).transform.position;
    }
}