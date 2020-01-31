using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MoveTiles : MonoBehaviour, IDragHandler, IEndDragHandler
{
    [SerializeField]float minY, maxY;

    void Update()
    {
        Vector3 pos = transform.position;
        pos.y = Mathf.Clamp(pos.y, minY, maxY);
        transform.position = pos;
    }
    public void OnDrag(PointerEventData eventData)
    {
            float newY = Input.mousePosition.y;
            newY -= newY % 150;
            transform.position = new Vector3(transform.position.x, newY, transform.position.z);
    }

    public void OnEndDrag(PointerEventData eventData)
    {

    }
}

class OneTask
{
    public string Name { get; set; }
    public int DUration { get; set; }
}