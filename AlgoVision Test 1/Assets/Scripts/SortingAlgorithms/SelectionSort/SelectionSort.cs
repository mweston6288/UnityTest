using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SelectionSort : SortingAlgorithm
{
    [SerializeField] GameObject boxPrefab;

    public void Setup(int size)
    {
        this.size = size;  // 70.2 63.8 -114.5
        arr = new int[size];
        array = new ArrayIndex[size];
        Sort();
        setCam();
    }
    // Start is called before the first frame update
    void Sort()
    {
        int smallest;
        buildArray(boxPrefab);

        for(int i = 0; i < size-1; i++)
        {
            smallest = i;

            for(int j = i+1; j < size; j++)
            {
                if(compare(j, smallest, 4, 0) && arr[j] < arr[smallest])
                {
                    smallest = j;
                }
            }
            swap(smallest, i, 0);
            q.Enqueue(new short[] {1, (short)i, (short)smallest, 0});
            q.Enqueue(new short[] { 2, (short)i, 2, 0 });
        }
        q.Enqueue(new short[] { 2, (short)(size-1), 2, 0 });

    }

    override public IEnumerator extendCommands(short[] command){
        throw new NotImplementedException();
    }
}
