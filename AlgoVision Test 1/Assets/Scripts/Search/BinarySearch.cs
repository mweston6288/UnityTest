using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BinarySearch : Algorithm
{
    int size;
    int searchToken;
    ArrayIndex[] array;

    Queue<QueueCommand> queue = new Queue<QueueCommand>();
    class ArrayIndex{
        public int value;
        public GameObject Object;
        public ArrayIndex(int value, int index){
            this.value = value;
            Object = GameObject.CreatePrimitive(PrimitiveType.Cube);
            Object.transform.position = new Vector3(index, 0, 0);
            if (index % 2 == 1){
                Object.GetComponent<Renderer>().material.color = Color.gray;
            }
        }

    }

    class QueueCommand{
        public short commandId;
        public int index1;
        public int index2;
        public short additionalInfo;
        public QueueCommand(short commandId, int index1, short additionalInfo){
            this.commandId = commandId;
            this.index1 = index1;
            this.additionalInfo = additionalInfo;
        }
        public QueueCommand(short commandId, int index1, int index2, short additionalInfo){
            this.commandId = commandId;
            this.index1 = index1;
            this.index2 = index2;
            this.additionalInfo = additionalInfo;
        }
    }
    
    // Start is called before the first frame update
    void Start()
    {
        int i;
        size = 10;
        array = new ArrayIndex[size];
        for(i = 0; i < size; i++){
            array[i] = new ArrayIndex(r.Next(21), i);
        }
        sort();
        for(i = 0; i < size; i++){
            Debug.Log(array[i].value);
        }
        searchToken = r.Next(21);
        search();
        StartCoroutine(readQueue());
    }
    void sort(){
        int i,j;
        for(i = 1; i < size; i++){
            for(j = i; j > 0; j--){
                if(array[j].value < array[j-1].value){
                    swap(j, j-1);
                }
            }
        }
    }
    void swap(int a, int b){
        int temp;
        temp = array[a].value;
        array[a].value = array[b].value;
        array[b].value = temp;
    }
    void search(){
        int min, max, middle;
        min = 0;
        max = size - 1;
        middle = max / 2;

        while(max - min > 0){
            queue.Enqueue(new QueueCommand(1, middle, 1));
            queue.Enqueue(new QueueCommand(0, -1,-1));

            if(searchToken < array[middle].value){
                Debug.Log(searchToken + " less than "+array[middle].value);
                queue.Enqueue(new QueueCommand(2, middle, max, -1));
                queue.Enqueue(new QueueCommand(0, -1,-1));
                max = middle -1;
                middle = (max+min)/2;
                continue;
            }
            else if(searchToken > array[middle].value){
                Debug.Log(searchToken + " greater than "+array[middle].value);
                queue.Enqueue(new QueueCommand(2, min, middle, -1));
                queue.Enqueue(new QueueCommand(0, -1,-1));
                min = middle + 1;
                middle = (max+min)/2;
                continue;
            }
            else{
                queue.Enqueue(new QueueCommand(1, middle, 2));
                Debug.Log(searchToken + " at "+middle);
                return;
            }
        }

        if(max == min && array[max].value == searchToken){
            queue.Enqueue(new QueueCommand(1, max, 1));
            queue.Enqueue(new QueueCommand(0, -1,-1));
            queue.Enqueue(new QueueCommand(1, max, 2));
            Debug.Log(searchToken + " in array");
        }
        else {
            Debug.Log(searchToken + " not in array");

        }
    }
    public IEnumerator readQueue(){
        foreach(QueueCommand q in queue){
            switch(q.commandId){
                case 0:
                    yield return new WaitForSeconds(1);
                    break;
                case 1:
                    changeColor(q.index1, q.additionalInfo);
                    break;
                case 2:
                    for(int i = q.index1; i<= q.index2; i++ ){
                        array[i].Object.GetComponent<Renderer>().material.color = Color.black;
                    }
                    break;
            }
        }
    }
    void changeColor(int index, short colorId){
        switch(colorId){
            case 1:
                array[index].Object.GetComponent<Renderer>().material.color = Color.red;
                break;
            case 2:
                array[index].Object.GetComponent<Renderer>().material.color = Color.green;
                break;
            case 3:
                array[index].Object.GetComponent<Renderer>().material.color = Color.black;
                break;
        }
    }
}
