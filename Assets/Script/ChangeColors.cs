using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeColors : MonoBehaviour
{
    [SerializeField] private float _timer;
    void Update()
    {
        ChangeColorToGrey();
    }
    void ChangeColorToGrey()
    {
        _timer -= 1f*Time.deltaTime;
        if (_timer <= 0)
        {
            _timer = 0;
            this.gameObject.GetComponent<Image>().color = Color.grey;
        }
    }
}
