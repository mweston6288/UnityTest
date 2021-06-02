using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DijkstraUI : MonoBehaviour
{
    [SerializeField] Dijkstra a;

    [SerializeField] Slider speedSlider;

    // Start is called before the first frame update
    void Start()
    {
        a.Setup(3);
        StartCoroutine(a.readQueue());
    }

    // Update is called once per frame
    void Update()
    {
        // temp until I figure out how to set up a slider
        a.time = 1;//speedSlider.value;
    }
}
