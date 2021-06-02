using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class InsertionSort : SortingAlgorithm1
{
    [SerializeField] GameObject boxPrefab;
    [SerializeField] GameObject canvas;
    
    public void setup(int size)
    {
        this.size = size;  // 70.2 63.8 -114.5
        arr = new int[size];
        array = new ArrayIndex[size];
        sort();
        setCam();
    }

    // Start is called before the first frame update
    void sort()
    {
        buildArray(boxPrefab, canvas);

        for(int i = 1; i < size; i++)
        {
            for(int j = i-1; j >= 0; j--)
            {
                if(compare(j, j+1, 0) && arr[j] > arr[j+1])
                {
                    swap(j + 1, j, 0);
                    queue.Enqueue(new QueueCommand(2, j, j+1, 0));
                }
                else
                {
                    decompare(j, j+1, 0, 0);
                    break;
                }
                decompare(j, j+1, 0, 0);

            }
        }
        queue.Enqueue(new QueueCommand(6, 0, size - 1, 0, 2));
        queue.Enqueue(new QueueCommand());
    }
    override public IEnumerator extendCommands(QueueCommand q){
        throw new NotImplementedException();
    }
}
