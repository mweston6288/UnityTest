using System.Collections;
using System.Collections.Generic;
using Random = System.Random;
using UnityEngine;
using System;

public class QuickSort : SortingAlgorithm
{
    [SerializeField] GameObject boxPrefab;
    public void setup(int size){
        this.size = size;
        arr = new int[size];
        array = new ArrayIndex[size];
        sort();
        setCam();
    }
    void sort(){
        buildArray(boxPrefab);
        quickSort(0, size - 1);
    }   
    override public IEnumerator extendCommands(short[] command){
        throw new NotImplementedException();
    }
    void quickSort(int low, int high)
        {
            if (low < high)
            {
                q.Enqueue(new short[] { 6, (short)low, (short)high, 0 });

                int split = partition(low, high);
                q.Enqueue(new short[] { 7, (short)low, (short)high, 0});
                q.Enqueue(new short[] { 2, (short)split, 2, 0 });

                quickSort(low, split - 1);
                quickSort(split + 1, high);
            }
            else
            {
                if( low > -1 && low < size)
                {
                    q.Enqueue(new short[] { 2, (short)low, 2, 0 });
                }
                else
                {
                    q.Enqueue(new short[] { 2, (short)high, 2, 0 });
                }
            }
        }

        int partition(int low, int high)
        {
            int i, lowPosition, temp; 
           
            int[] m;
        Random r = new Random();
            if (low >= high)
            {
                return low;
            }

        m = new int[3];
            m[0] = low;
            m[1] = high;
            m[2] = r.Next(low+1, high+1);
            
         if(arr[m[0]] > arr[m[1]])
        {
            temp = m[0];
            m[0] = m[1];
            m[1] = temp;
        }
         if(arr[m[1]] > arr[m[2]])
        {
            temp = m[1];
            m[1] = m[2];
            m[2] = temp;
            if (arr[m[0]] > arr[m[1]])
            {
                temp = m[0];
                m[0] = m[1];
                m[1] = temp;
            }
        }

        swap(low, m[1], 0);
        q.Enqueue(new short[] {1, (short)low, (short)m[1], 0});
            
            
        i = arr[low];
        lowPosition = low;
            low++;

            while (low <= high)
            {
                while (low <= high && compare(low, lowPosition, 5, 0) && arr[low] <= arr[lowPosition])
                    low++;

                while (high >= low && compare(high, lowPosition, 5, 0)  && arr[high] > arr[lowPosition])
                    high--;
                if ( low < high)
                {
                    swap(low, high, 0);
                    q.Enqueue(new short[] {1, (short)low, (short)high, 0});

                }
            }
            // Finally we swap the pivot with the point high was pointing to
            swap(lowPosition, high,0);
            q.Enqueue(new short[] {1, (short)lowPosition, (short)high, 0});

            return high;
        }
/* static int size = 100;
    int leftPointer, rightPointer, split;
    QuickSortPartition head;
    bool partitioning = false;

    public QuickSort() : base(size)
    {
        head = new QuickSortPartition(0, size - 1);
    }

    private class QuickSortPartition

    {
        public int low, high;
        public QuickSortPartition next, prev;
        public QuickSortPartition(int low, int high)
        {
            this.low = low;
            this.high = high;
            next = prev = null;
        }

    }


    // Start is called before the first frame update
    void Start()
    {
        // build the GameObject shape, size, and positions
        for (int i = 0; i < size; i++)
        {
            array[i] = GameObject.CreatePrimitive(PrimitiveType.Cube);
            array[i].transform.position = new Vector3((float)i, .5f * i, 0);
            array[i].transform.localScale = new Vector3(1, i + 1, 1);
        }
    }

    // Update is called once per frame
    void Update()
    {

        if (head == null)
        {
            return;
        }
        // First, have the SortingAlgorithm shuffle the array
        if (!unsorted)
        {
            shuffle();
        }
        else
        {
            // Check if we're in the process of partitioning
            // This triggers if we are not
            if (!partitioning)
            {
                // Check if the partition is only one element
                // If yes, set the left, right, and splitting values
                if (head.low < head.high)
                {
                    partitioning = true;
                    leftPointer = head.low + 1;
                    rightPointer = head.high;
                    split = head.low;
                    array[leftPointer].GetComponent<Renderer>().material.color = Color.red;
                    array[rightPointer].GetComponent<Renderer>().material.color = Color.red;
                    array[split].GetComponent<Renderer>().material.color = Color.red;


                }
                // A catcher line. head.low could potentially be out of bounds so if this happens, just discard the partition
                else if (head.low >= size)
                {
                    head = head.next;
                }
                // If partition is only one item, jump to next partition
                else
                {
                    array[head.low].GetComponent<Renderer>().material.color = Color.green;
                    head = head.next;
                }
            }
            // Enter if we are partitioning
            else
            {
                // Check if the left and right pointers have crossed
                // If we're here, we haven't swapped everything yet
                if (leftPointer <= rightPointer)
                {
                    // check if the number at left is less than split
                    // move up if it is
                    if (array[leftPointer].transform.position.y <= array[split].transform.position.y)
                    {
                        array[leftPointer++].GetComponent<Renderer>().material.color = Color.white;
                        array[leftPointer].GetComponent<Renderer>().material.color = Color.red;
                    }
                    // check if the number at right is greater than split
                    // move down if it is
                    else if (array[rightPointer].transform.position.y >= array[split].transform.position.y)
                    {
                        array[rightPointer--].GetComponent<Renderer>().material.color = Color.white;
                        array[rightPointer].GetComponent<Renderer>().material.color = Color.red;

                    }
                    // if we hit this, that means left and right both found something to swap
                    else
                    {
                        swapPointers();
                    }
                }
                // left and right have crossed meaning all elements have been moved
                // swap split and high, turn off partition, and add to the partition list
                else
                {
                    swapSplitter();
                    if (!isMoving)
                    {
                        array[rightPointer].GetComponent<Renderer>().material.color = Color.green;

                        partitioning = false;

                        QuickSortPartition a, b, temp;
                        a = new QuickSortPartition(head.low, rightPointer - 1);
                        // THis line could potentially cause an OutOfBounds Exception if the split happened at the very last value
                        b = new QuickSortPartition(rightPointer + 1, head.high);
                        temp = head.next;
                        b.next = temp;

                        a.next = b;
                        head = a;
                    }

                }
            }
        }
    }
    // set up the variables in SortingAlgorithm that handle moving objects then call movePieces
    public void swapPointers()
    {
        if (!isMoving)
        {
            left = leftPointer;
            right = rightPointer;
            isMoving = true;
            leftOriginal = array[left].transform.position.x;
            rightOriginal = array[right].transform.position.x;
        }
        movePieces();
    }
    public void swapSplitter()
    {
        if (!isMoving)
        {
            left = split;
            right = rightPointer;
            isMoving = true;
            leftOriginal = array[left].transform.position.x;
            rightOriginal = array[right].transform.position.x;
        }
        movePieces();
    }*/
}
