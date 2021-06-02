using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class Prim : Graph
{
    GameObject boxPrefab;
    int main;
    protected static List head;
    // Start is called before the first frame update
    protected class PrimVertex : Vertex{
        public bool visited;
        public PrimVertex(int value, GameObject boxPrefab) : base(value, boxPrefab){

        }
    }    
    protected class List{
        public Edge edge;
        public List next;
        public List(Edge edge){
            this.edge = edge;
            next = null;
        }
        public void insert(Edge e){
            List temp1, temp2;
            temp1 = head;
            if (temp1 == null){
                temp1 = new List(e);
                return;
            }
            while (temp1.next != null && temp1.next.edge.weight < e.weight){
                temp1 = temp1.next;
            }
            temp2 = temp1.next;
            temp1.next = new List(e);
            temp1.next.next = temp2;
        }
    }
    void Start()
    {
        vertices = new PrimVertex[vertex];
        for(int i = 0; i < vertex; i++){
            vertices[i] = new PrimVertex(i, boxPrefab);
        }
        for(int i = 0; i < edge; i++){
            edges[i] = new Edge(i, r.Next(1,21));
        }
        //setCam();
        main = r.Next(vertex);
        //PrimAlgorithm();
        BreadthFirstSearch(main);
        StartCoroutine(readQueue());        
    }
    void PrimAlgorithm(){
        head = new List(null); // junk data to initialize

        // We can guarantee no vertices have been visited yet so don't check for that
        foreach(Edge e in vertices[main].neighborEdges){
            head.insert(e);
        }
        // Lock the main and remove the junk data
        ((PrimVertex)vertices[main]).visited = true;
        head = head.next;
        queue.Enqueue(new QueueCommand(1,main,-1, 4));
        // Make writing easier by setting references to precast values
        PrimVertex a,b;

        while (head != null){
            a = (PrimVertex)vertices[head.edge.i];
            b = (PrimVertex)vertices[head.edge.j];
            if (a.visited && b.visited){
                head = head.next;
                continue;
            }
            foreach(Edge e in a.neighborEdges){
                if (!a.visited){
                    head.insert(e);
                }
            }
            foreach(Edge e in b.neighborEdges){
                if (!b.visited){
                    head.insert(e);
                }
            }
            if(a.value != main){
                queue.Enqueue(new QueueCommand(1,a.value,-1,1));

            }
            if (b.value != main){
                queue.Enqueue(new QueueCommand(1,b.value,-1,1));

            }

            a.visited = true;
            b.visited = true;
            queue.Enqueue(new QueueCommand(3, head.edge.id, 1));
            queue.Enqueue(new QueueCommand(0,-1,-1));

            head = head.next;
        }
    }
    protected override void extendCommands(QueueCommand command)
    {
        throw new System.NotImplementedException();
    }
    protected override void extendVertexColors(int vertex, short colorId)
    {
        vertices[vertex].o.GetComponent<Renderer>().material.color = Color.green;
    }
}
