using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class BubbleSortUI : MonoBehaviour
{

    [SerializeField] BubbleSort v;
    [SerializeField] Slider speedSlider;

    // Start is called before the first frame update
    void Start()
    {
        //v = gameObject.AddComponent(typeof(BubbleSort)) as BubbleSort;
        v.setup(7);
        StartCoroutine(v.readQueue());
    }

    // Update is called once per frame
    void Update()
    {
        v.time = speedSlider.value;
    }

    public void pause()
    {
        Time.timeScale = 0;
    }
    public void play()
    {
        Time.timeScale = 1;
    }
}

