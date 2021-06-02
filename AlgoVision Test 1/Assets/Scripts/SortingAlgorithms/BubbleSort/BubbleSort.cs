using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class BubbleSort : SortingAlgorithm1
{
    [SerializeField] GameObject boxPrefab;
    [SerializeField] GameObject canvas;
    public void setup(int size){
        this.size = size;
        arr = new int[size];
        array = new ArrayIndex[size];
        sort();
        setCam();
    }

    void sort(){
        buildArray(boxPrefab, canvas);
        int i,j;
        for (i = 0; i < size; i++){
            for(j = 0; j < size - i - 1; j++){
                if (compare(j, j+1, 0) && arr[j] > arr[j+1]){
                    swap(j, j+1, 0);
                    queue.Enqueue(new QueueCommand(2, j, j+1, 0));
                }
                decompare(j, j+1, 0, 0);
            }
            queue.Enqueue(new QueueCommand(3, j, (short)0, 2));
            queue.Enqueue(new QueueCommand());

        }

    }
    override public IEnumerator extendCommands(QueueCommand q){
        throw new NotImplementedException();
    }
    
    /*
    int i, j = 0;
    public BubbleSort2(): base(20)
    {

    }

    // Start is called before the first frame update
    void Start()
    {
        // build the GameObject shape, size, and positions
        for (i = 0; i < 20; i++)
        {
            array[i] = GameObject.CreatePrimitive(PrimitiveType.Cube);
            array[i].transform.position = new Vector3((float)i, .5f * i, 0);
            array[i].transform.localScale = new Vector3(1, i + 1, 1);
        }
        i = 0;
    }

    // Update is called once per frame
    void Update()
    {
        // First, have the SortingAlgorithm shuffle the array
        if (!unsorted)
        {
            shuffle();
        }
        // THen sort it
        else
        {
            // Represents the outer for-loop in bubble sort
            if (i< 20)
            {
                // represents the inner for-loop in bubble sort
                if (j < 19- i)
                {
                    array[j].GetComponent<Renderer>().material.color = Color.red;
                    array[j+1].GetComponent<Renderer>().material.color = Color.red;

                    // use the height of each GameObject to determine their value
                    if (array[j].transform.position.y > array[j + 1].transform.position.y)
                    {
                        sort();
                    }
                    // Only increment j (and move to next "loop") when the objects have been swapped
                    if (!isMoving)
                    {
                        array[j].GetComponent<Renderer>().material.color = Color.white;
                        array[j + 1].GetComponent<Renderer>().material.color = Color.white;
                        j++;
                    }
                }
                // j has reach the end of its loop so now we go to the next iteration in the outer "loop"
                else
                {
                    // show this index is sorted
                    array[j].GetComponent<Renderer>().material.color = Color.green;
                    i++;
                    j = 0;
                }
            }

        }
    }

    // set up the variables in SortingAlgorithm that handle moving objects then call movePieces
    public void sort()
    {
        if (!isMoving)
        {
            left = j;
            right = j + 1;
            isMoving = true;
            leftOriginal = array[left].transform.position.x;
            rightOriginal = array[right].transform.position.x;

        }
        movePieces();
    }*/
}
