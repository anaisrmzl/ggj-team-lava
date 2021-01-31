using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIDisplayEnd : MonoBehaviour
{
    [SerializeField]
    private Collector collector;
    [SerializeField]
    private int max_feathers;

    void Start()
    {
        
    }

    public void ShowOverview()
    {
        //max_feathers = 10;
        //feathers.text = "Feathers: " + collector.Feathers.ToString() + "/" + max_feathers;
        //birdCaught.text = "Bird captured " + timesCaught + "times";
    }

}
