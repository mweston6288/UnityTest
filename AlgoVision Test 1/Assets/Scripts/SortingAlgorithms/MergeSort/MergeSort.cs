using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MergeSort : SortingAlgorithmWithAuxArray
{
    [SerializeField] GameObject boxPrefab;
    // To reduce runtime, we'll use the same auxArray for everything. 
    // This value tells us which index is the last possible index of the leftAuxArray
    int midSplit;

    public void setup(int size){
        this.size = size;
        arr = new int[size];
        auxArr = new int[size];
        array = new ArrayIndex[size];
        auxArray = new ArrayIndex[size];
        sort();
        setAuxCam();

    }
    void sort(){
        buildArray(boxPrefab);
        buildAuxArray();

        midSplit = (size - 2)/ 2 + 1;
        mergeSort(0, size - 1);
        q.Enqueue(new short[] {8, 0, 0, 0});
    }   

    private void merge(int low, int middle, int high)
    {
        int i, j, k;
        int n1 = middle - low + 1;
        int n2 = high - middle;

        //int[] leftArray = new int[n1];
        //int[] rightArray = new int[n2];
        
        for (i = 0; i < n1; i++)
        {
            auxArr[i] = arr[low+i];
            q.Enqueue(new short[] {10, (short)i, (short)(low + i), 1});
            //leftArray[i] = array[low + i];
        }

        for (j = 0; j < n2; j++)
        {
            auxArr[j + midSplit] = arr[middle + 1 + j];
            q.Enqueue(new short[] {10, (short)(j+midSplit), (short)(middle+1+j), 1});

            //rightArray[j] = array[middle + 1 + j];
        }

        i = 0;
        j = midSplit;
        k = low; 

        while (i < n1 && j < midSplit + n2)
        {
            if (compare(i, j, 4, 1) && auxArr[i] <= auxArr[j])
            //if (leftArray[i] <= rightArray[j])
            {
                arr[k] = auxArr[i];
                q.Enqueue(new short[] {10, (short)k, (short)i, 0});
                q.Enqueue(new short[] {2, (short)i, 4, 1});

                i++;
            }
            else
            {
                arr[k] = auxArr[j];
                q.Enqueue(new short[] {10, (short)k, (short)j, 0});
                q.Enqueue(new short[] {2, (short)j, 4, 1});
                j++;
            }
            k++; 
        }

        while (i < n1)
        {
            arr[k] = auxArr[i];
            q.Enqueue(new short[] {10, (short)k, (short)i, 0});
            q.Enqueue(new short[] {2, (short)i, 4, 1});
            i++;
            k++;
        }
        while (j < n2+ midSplit)
        {
            arr[k] = auxArr[j];
            q.Enqueue(new short[] {10, (short)k, (short)j, 0});
            q.Enqueue(new short[] {2, (short)j, 4, 1});
            j++;
            k++;
        }
        q.Enqueue(new short[]{9, 0, 0, 0});
    }

    private void mergeSort(int low, int high){
        if (low < high)
        {
            int med = (low + high - 1) / 2;

            mergeSort(low, med);
            mergeSort(med + 1, high);

            q.Enqueue(new short[] { 6, (short)low, (short)high , 0});
            merge(low, med, high);
            q.Enqueue(new short[] { 7, (short)low, (short)high,0 });
        }

    }
}

