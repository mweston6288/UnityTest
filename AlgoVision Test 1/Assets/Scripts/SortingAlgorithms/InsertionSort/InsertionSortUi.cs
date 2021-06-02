using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class InsertionSortUi : MonoBehaviour
{
    [SerializeField] InsertionSort v;

    [SerializeField] Slider speedSlider;

    // Start is called before the first frame update
    void Start()
    {

        //v = gameObject.AddComponent(typeof(InsertionSort)) as InsertionSort;
        v.setup(7);
        v.time = 1;

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
