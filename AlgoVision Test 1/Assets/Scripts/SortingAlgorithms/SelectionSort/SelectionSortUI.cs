using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
public class SelectionSortUI : MonoBehaviour
{

    [SerializeField] SelectionSort v;
    [SerializeField] Slider speedSlider;

    // Start is called before the first frame update
    void Start()
    {
        //v = gameObject.AddComponent(typeof(BubbleSort)) as BubbleSort;
        v.Setup(8);
        v.time = 1;
        StartCoroutine(v.readQueue());
    }

    // Update is called once per frame
    void Update()
    {
        v.time = 1;
    }
}
