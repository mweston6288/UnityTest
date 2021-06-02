using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuickSortUI : MonoBehaviour
{

    [SerializeField] QuickSort v;
    //[SerializeField] Slider speedSlider;

    // Start is called before the first frame update
    void Start()
    {
        //v = gameObject.AddComponent(typeof(BubbleSort)) as BubbleSort;
        v.setup(8);
        v.time = 1;
        StartCoroutine(v.readQueue());
    }

    // Update is called once per frame
    void Update()
    {
        v.time = 1;
    }
}
