using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Random = System.Random;

public class AVL : Algorithm
{
    protected int treeDepth; // lowest level of tree
    protected int size;
    protected Queue q = new Queue();
    protected Random r = new Random();
    protected AVLNode[] Nodetree;
    protected int[] inttree;
    protected int[] heights;
    protected static float[] Xcoords;
    protected static float[] Ycoords;

    protected class AVLNode
    {
        public GameObject o;
        public int value, I;
        public LineRenderer parentEdge;

        public AVLNode(int value, int I)
        {
            this.value = value;
            this.I = I;
        }

        public void updateCoords()
        {
            this.o.transform.position = new Vector3(Xcoords[this.I], Ycoords[this.I], 0);
        }
    }

    protected class AVLCommand
    {
        public short commandId;
        public int arg1, arg2;

        public AVLCommand(short commandId, int a1, int a2)
        {
            this.commandId = commandId;
            this.arg1 = a1;
            this.arg2 = a2;
        }
    }

    void updateDepth()
    {
        treeDepth++;
        int[] newtree = new int[inttree.Length * 2 + 1];
        int[] newHeights = new int[newtree.Length];

        for(int i = 0; i < newtree.Length; i++)
        {
            newtree[i] = -1;
            newHeights[i] = 0;
        }

        for(int i = 0; i < inttree.Length; i++)
        {
            newtree[i] = inttree[i];
            newHeights[i] = heights[i];
        }

        inttree = newtree;
        heights = newHeights;
    }


    void setCoords()
    {
        if(treeDepth % 2 == 0) // if the tree depth is even
        {
            for(int i = 0; i < treeDepth; i++)
            {
                float y = (float)(treeDepth-1) / (float)2 - (float)i;
                int n = -1 * (int)(Math.Pow(2, i) - 1);
                int d = (int)(Math.Pow(2, i + 1));

                for(int j = (int)Math.Pow(2, i) - 1; j < (int)Math.Pow(2, i+1) - 1; j++)
                {
                    float x = (float)n/5 * (float)Xcoords.Length / (float)d;
                    n = n + 2;

                    Xcoords[j] = x; 
                    Ycoords[j] = 3*y;
                }
            }
        }
        else // if the tree depth is odd
        {
            for (int i = 0; i < treeDepth; i++)
            {
                float y = (float)(treeDepth-1) / (float)2 - (float)i;
                int n = -1 * (int)(Math.Pow(2, i)-1);
                int d = (int)(Math.Pow(2, i + 1));

                for (int j = (int)Math.Pow(2, i) - 1; j < (int)Math.Pow(2, i + 1) - 1; j++)
                {
                    float x = (float)n/5 * (float)Xcoords.Length / (float)d;
                    n = n + 2;

                    Xcoords[j] = x;
                    Ycoords[j] = 3*y;
                }
            }
        }
    }

    int leftCI(int i) => 2 * i + 1;
    int rightCI(int i) => 2 * i + 2;
    int parentI(int i) => i % 2 == 0 ? (i - 2) / 2 : (i - 1) / 2;


    void shiftUp(int i, int d)
    {
        if(!(i < inttree.Length) || inttree[i] == -1)
        {
            return;
        }

        inttree[d] = inttree[i];
        inttree[i] = -1;
        q.Enqueue(new AVLCommand(3, i, d));

        shiftUp(leftCI(i), leftCI(d));
        shiftUp(rightCI(i), rightCI(d));
    }

    void shiftDown(int i, int d)
    {
        if (!(i < inttree.Length) || inttree[i] == -1)
        {
            return;
        }

        while(d >= inttree.Length)
        {
            updateDepth();
        }


        shiftDown(leftCI(i), leftCI(d));
        shiftDown(rightCI(i), rightCI(d));

        inttree[d] = inttree[i];
        inttree[i] = -1;
        q.Enqueue(new AVLCommand(3, i, d));

    }

    void movetree(int i, int d)
    {
        if(d > i)
        {
            shiftDown(i, d);
        }
        else
        {
            shiftUp(i, d);
        }
    }

    void updateHeights(int i)
    {
        if(i >= inttree.Length)
        {
            return;
        }
        if(inttree[i] == -1)
        {
            heights[i] = 0;
            return;
        }

        updateHeights(leftCI(i));
        updateHeights(rightCI(i));

        int left = 0;
        int right = 0;

        if(leftCI(i) < heights.Length)
        {
            updateHeights(leftCI(i));
            left = heights[leftCI(i)];
        }
        if (rightCI(i) < heights.Length)
        {
            updateHeights(rightCI(i));
            right = heights[rightCI(i)];
        }

        heights[i] = max(left, right) + 1;
    }

    /*
    
        y                           x
       / \                         / \
      x   3     ===> right        1   y
     / \        <=== left            / \  
    1   2                           2   3

    */

    void lRotate(int I)
    {
        int x = I;
        int t1 = leftCI(x);
        int y = rightCI(x);
        int t2 = leftCI(y);
        int t3 = rightCI(y);

        q.Enqueue(new AVLCommand(4, t1, 6));

        q.Enqueue(new AVLCommand(2, x, 5));

        q.Enqueue(new AVLCommand(4, t2, 4));

        q.Enqueue(new AVLCommand(2, y, 3));
        
        q.Enqueue(new AVLCommand(4, t3, 2));
        
        q.Enqueue(new AVLCommand(-1, 0, 0));


        // move tree1 downleft
        movetree(t1, leftCI(t1));
        q.Enqueue(new AVLCommand(-1, 0, 0));



        // move x downleft
        inttree[leftCI(x)] = inttree[x];
        inttree[x] = -1;
        q.Enqueue(new AVLCommand(3, x, leftCI(x)));
        q.Enqueue(new AVLCommand(-1, 0, 0));


        // move tree2 left 1
        movetree(t2, t2 - 1);
        q.Enqueue(new AVLCommand(-1, 0, 0));



        // move move y up
        inttree[parentI(y)] = inttree[y];
        inttree[y] = -1;
        q.Enqueue(new AVLCommand(3, y, parentI(y)));
        q.Enqueue(new AVLCommand(-1, 0, 0));


        // move tree 3 up
        movetree(t3, parentI(t3));
        q.Enqueue(new AVLCommand(-1, 0, 0));


        q.Enqueue(new AVLCommand(4, I, 0));
        q.Enqueue(new AVLCommand(-1, 0, 0));
        updateHeights(I);
    }

    void rRotate(int I)
    {
        int y = I;
        int x = leftCI(y);
        int t3 = rightCI(y);
        int t1 = leftCI(x);
        int t2 = rightCI(x);

        q.Enqueue(new AVLCommand(4, t3, 6));
        q.Enqueue(new AVLCommand(2, y, 5));
        q.Enqueue(new AVLCommand(4, t2, 4));
        q.Enqueue(new AVLCommand(2, x, 3));
        q.Enqueue(new AVLCommand(4, t1, 2));
        q.Enqueue(new AVLCommand(-1, 0, 0));

        // move tree3 downright
        movetree(t3, rightCI(t3));
        q.Enqueue(new AVLCommand(-1, 0, 0));


        // move y downright
        inttree[rightCI(y)] = inttree[y];
        inttree[y] = -1;
        q.Enqueue(new AVLCommand(3, y, rightCI(y)));
        q.Enqueue(new AVLCommand(-1, 0, 0));


        // movetree2 right 1
        movetree(t2, t2 + 1);
        q.Enqueue(new AVLCommand(-1, 0, 0));



        // move x up
        inttree[parentI(x)] = inttree[x];
        inttree[x] = -1;
        q.Enqueue(new AVLCommand(3, x, parentI(x)));
        q.Enqueue(new AVLCommand(-1, 0, 0));


        // move tree1 up
        movetree(t1, parentI(t1));
        q.Enqueue(new AVLCommand(-1, 0, 0));


        q.Enqueue(new AVLCommand(4, I, 0));
        q.Enqueue(new AVLCommand(-1, 0, 0));
        updateHeights(I);
    }

    private int max(int a, int b) => a > b ? a : b;

    void printIntTree()
    {
        for (int i = 0; i < treeDepth; i++)
        {
            String output = "";

            for (int j = (int)Math.Pow(2, i) - 1; j < (int)Math.Pow(2, i + 1) - 1; j++)
            {
                if(inttree[j] == -1)
                {
                    output = output + "n, ";
                }
                else
                {
                    output = output + inttree[j].ToString() + ", ";
                }
                
            }

            Debug.Log("Depth " + i + "\t" + output);
        }

    }

    void Start()
    {
        size = 25;
        string order = "";
        treeDepth = (int)(Math.Ceiling(0.0001 + Math.Log(size) / Math.Log(2)) + 2);

        inttree =  new int[(int)Math.Pow(2, treeDepth) - 1];
        heights =  new int[(int)Math.Pow(2, treeDepth) - 1];
        /*Nodetree = new AVLNode[(int)Math.Pow(2, treeDepth) - 1];
        Xcoords = new float[(int)Math.Pow(2, treeDepth) - 1];
        Ycoords = new float[(int)Math.Pow(2, treeDepth) - 1];*/

        for (int i = 0; i < inttree.Length; i++)
        {
            inttree[i] = -1;
            heights[i] = 0;            
        }

        // setCoords();

        for(int i = 0; i < size; i++)
        {
            int ins =  r.Next(1, 101);
            insert(ins, 0);
            
            order = order + ins.ToString() + " ";            
            printIntTree();
        }

        Nodetree = new AVLNode[inttree.Length*2+1];
        Xcoords = new float[Nodetree.Length];
        Ycoords = new float[Nodetree.Length];
        setCoords();
        
        Debug.Log(order);
        Debug.Log(Nodetree.Length);
        StartCoroutine(readQueue(0.1f));
    }

    void insert(int key, int I)
    {
        if(I > inttree.Length)
        {
            updateDepth();
        }
        if (inttree[I] == -1)
        {
            inttree[I] = key;
            heights[I] = 1;
            q.Enqueue(new AVLCommand(0, I, key));
            if(I != 0)
            {
                q.Enqueue(new AVLCommand(1, I, parentI(I)));
            }
            q.Enqueue(new AVLCommand(-1, 0, 0));
            return;
        }
        q.Enqueue(new AVLCommand(2, I, 1));
        q.Enqueue(new AVLCommand(-1, 0, 0));
        if (inttree[I] > key)
        {
            insert(key, leftCI(I));
        }
        else
        {
            insert(key, rightCI(I));
        }

        q.Enqueue(new AVLCommand(2, I, 0));
        q.Enqueue(new AVLCommand(-1, 0, 0));

        heights[I] = max(heights[leftCI(I)], heights[rightCI(I)]) + 1;

        int balance = heights[leftCI(I)] - heights[rightCI(I)];

        if (balance > 1)
        {
            if (key > inttree[leftCI(I)])
            {
                lRotate(leftCI(I));                
            }

            rRotate(I);
            return;

        }
        else if (balance < -1)
        {
            if (key <= inttree[rightCI(I)])
            {
                rRotate(rightCI(I));
            }

            lRotate(I);
        }

        return;
    }

    void dunkNode(int n)
    {
        Destroy(Nodetree[n].o);
        Destroy(Nodetree[n].parentEdge);
        Nodetree[n] = null;
    }

    void colorTree(int i, int c)
    {
        if( !(i < inttree.Length)  || Nodetree[i] == null)
        {
            return;
        }
        switch (c)
        {
            case 0:
                Nodetree[i].o.GetComponent<Renderer>().material.color = Color.white;
                break;

            case 1:
                Nodetree[i].o.GetComponent<Renderer>().material.color = Color.red;
                break;

            case 2:
                Nodetree[i].o.GetComponent<Renderer>().material.color = new Color(0.945f, 0.518f, 0.031f, 1.0f);
                    break;

            case 3:
                Nodetree[i].o.GetComponent<Renderer>().material.color = Color.yellow;
                break;

            case 4:
                Nodetree[i].o.GetComponent<Renderer>().material.color = Color.green;
                break;

            case 5:
                Nodetree[i].o.GetComponent<Renderer>().material.color = Color.blue;
                break;

            case 6:
                Nodetree[i].o.GetComponent<Renderer>().material.color = Color.magenta;
                    break;

            default:
                Nodetree[i].o.GetComponent<Renderer>().material.color = Color.black;
                    break;

        }

        colorTree(leftCI(i), c);
        colorTree(rightCI(i), c);
    }

    public IEnumerator readQueue(float time)
    {
        foreach (AVLCommand instr in q)
        {
            Debug.Log(instr.commandId + "\t" + instr.arg1 + "\t" + instr.arg2);
            switch (instr.commandId)
            {
                case -1:
                    yield return new WaitForSeconds(time);
                    break;

                case 0: // create a node, (0, index, value)
                    Nodetree[instr.arg1] = new AVLNode(instr.arg2, instr.arg1);
                    Nodetree[instr.arg1].o = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                  //  Nodetree[instr.arg1].o.transform.localScale = new Vector3(1f, 1f, (float)instr.arg2 / 10);
                  //  Nodetree[instr.arg1].o.tag = instr.arg2.ToString();
                    Nodetree[instr.arg1].updateCoords();
                    break;

                case 1: // link two nodes, (1, child, parent)
                    Nodetree[instr.arg1].parentEdge = new GameObject("Line").AddComponent(typeof(LineRenderer)) as LineRenderer;
                    Nodetree[instr.arg1].parentEdge.GetComponent<LineRenderer>().startColor = Color.black;
                    Nodetree[instr.arg1].parentEdge.GetComponent<LineRenderer>().endColor = Color.black;
                    Nodetree[instr.arg1].parentEdge.GetComponent<LineRenderer>().startWidth = .05f;
                    Nodetree[instr.arg1].parentEdge.GetComponent<LineRenderer>().endWidth = .05f;
                    Nodetree[instr.arg1].parentEdge.GetComponent<LineRenderer>().positionCount = 2;
                    Nodetree[instr.arg1].parentEdge.GetComponent<LineRenderer>().useWorldSpace = true;
                    Nodetree[instr.arg1].parentEdge.SetPosition(0, new Vector3(Xcoords[instr.arg1], Ycoords[instr.arg1], 0)); //x,y and z position of the starting point of the line
                    Nodetree[instr.arg1].parentEdge.SetPosition(1, new Vector3(Xcoords[instr.arg2], Ycoords[instr.arg2], 0));
                    break;

                case 2: // color node (2, node, color), 0 = white, 1 = red, 2 = orange, 3 = yellow, 4 = green, 5 = blue, 6 = purble
                    if(Nodetree[instr.arg1] != null)
                    {
                        switch (instr.arg2)
                        {
                            case 0:
                                Nodetree[instr.arg1].o.GetComponent<Renderer>().material.color = Color.white;
                                break;

                            case 1:
                                Nodetree[instr.arg1].o.GetComponent<Renderer>().material.color = Color.red;
                                break;

                            case 2:
                                Nodetree[instr.arg1].o.GetComponent<Renderer>().material.color = new Color(0.945f, 0.518f, 0.031f, 1.0f);
                                break;

                            case 3:
                                Nodetree[instr.arg1].o.GetComponent<Renderer>().material.color = Color.yellow;
                                break;

                            case 4:
                                Nodetree[instr.arg1].o.GetComponent<Renderer>().material.color = Color.green;
                                break;

                            case 5:
                                Nodetree[instr.arg1].o.GetComponent<Renderer>().material.color = Color.blue;
                                break;

                            case 6:
                                Nodetree[instr.arg1].o.GetComponent<Renderer>().material.color = Color.magenta;
                                break;

                            default:
                                Nodetree[instr.arg1].o.GetComponent<Renderer>().material.color = Color.black;
                                break;

                        }
                    }
                    

                    break;
                    


                case 3: // move node (3, node, destination)
                    Debug.Log(instr.arg1 + "\t" + instr.arg2);
                    Nodetree[instr.arg2] = new AVLNode(Nodetree[instr.arg1].value, instr.arg2);
                    Nodetree[instr.arg2].o = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                   // Nodetree[instr.arg2].o.transform.localScale = new Vector3(1f, 1f, (float)Nodetree[instr.arg2].value / 10);
                    Nodetree[instr.arg2].o.GetComponent<Renderer>().material.color = Nodetree[instr.arg1].o.GetComponent<Renderer>().material.color;
                    //Nodetree[instr.arg1].o.tag = instr.arg2.ToString();
                    Nodetree[instr.arg2].updateCoords();

                    Destroy(Nodetree[instr.arg1].o);
                    Destroy(Nodetree[instr.arg1].parentEdge);
                    Nodetree[instr.arg1] = null;

                    if (instr.arg2 != 0)
                    {
                        Nodetree[instr.arg2].parentEdge = new GameObject("Line").AddComponent(typeof(LineRenderer)) as LineRenderer;
                        Nodetree[instr.arg2].parentEdge.GetComponent<LineRenderer>().startColor = Color.black;
                        Nodetree[instr.arg2].parentEdge.GetComponent<LineRenderer>().endColor = Color.black;
                        Nodetree[instr.arg2].parentEdge.GetComponent<LineRenderer>().startWidth = .05f;
                        Nodetree[instr.arg2].parentEdge.GetComponent<LineRenderer>().endWidth = .05f;
                        Nodetree[instr.arg2].parentEdge.GetComponent<LineRenderer>().positionCount = 2;
                        Nodetree[instr.arg2].parentEdge.GetComponent<LineRenderer>().useWorldSpace = true;
                        Nodetree[instr.arg2].parentEdge.SetPosition(0, new Vector3(Xcoords[instr.arg2], Ycoords[instr.arg2], 0)); 
                        Nodetree[instr.arg2].parentEdge.SetPosition(1, new Vector3(Xcoords[parentI(instr.arg2)], Ycoords[parentI(instr.arg2)], 0));
                    }

                    yield return new WaitForSeconds(time);
                    break;

                case 4: // color tree(4, index, color)
                    colorTree(instr.arg1, instr.arg2);
                    break;

                default:
                    yield return new WaitForSeconds(time);
                    break;
            }
        }
    }
}





/*
    public void setCam()
    {
        float z = -1*(float)Math.Ceiling(size * Math.Tan(112*Math.PI/360)/2);
        Camera.main.transform.position = new Vector3(0, 5, (float)(z));
        Camera.main.farClipPlane = (float)(-1.1 * z + 200);
    }

    void resetPositions(AVLNode root)
    {
        // for each TreeNode in the array, check if it has a left or right child and adjust their locations

        if (root.children[0] != null && root.children[0].o != null && root.children[0].parentEdge != null)
        {
            root.children[0].o.transform.position = new Vector3(root.o.transform.position.x - root.children[0].childVolumes[1] - 1, root.children[0].o.transform.position.y, 0);
            root.children[0].parentEdge.SetPosition(0, new Vector3(root.o.transform.position.x, root.o.transform.position.y, 0)); //x,y and z position of the starting point of the line
            root.children[0].parentEdge.SetPosition(1, new Vector3(root.children[0].o.transform.position.x, root.children[0].o.transform.position.y, 0)); //x,y and z position of the starting point of the line
            resetPositions(root.children[0]);
        }
        if (root.children[1] != null && root.children[1].o != null && root.children[1].parentEdge != null)
        {
            root.children[1].o.transform.position = new Vector3(root.o.transform.position.x + root.children[1].childVolumes[0] + 1, root.children[1].o.transform.position.y, 0);
            root.children[1].parentEdge.SetPosition(0, new Vector3(root.o.transform.position.x, root.o.transform.position.y, 0)); //x,y and z position of the starting point of the line
            root.children[1].parentEdge.SetPosition(1, new Vector3(root.children[1].o.transform.position.x, root.children[1].o.transform.position.y, 0)); //x,y and z position of the starting point of the line
            resetPositions(root.children[1]);
        }
    }

    public IEnumerator readQueue(float time)
    {
        foreach (AVLCommand instr in q)
        {
            switch (instr.commandId)
            {
                case 0: // create a node
                    instr.node1.o = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                    instr.node1.o.transform.position = new Vector3(0, 10 - 2 * instr.node1.depth);
                    break;
                case 1: // color a node red
                    instr.node1.o.GetComponent<Renderer>().material.color = Color.red;
                    yield return new WaitForSeconds(time);

                    break;
                case 2: // color a node white
                    instr.node1.o.GetComponent<Renderer>().material.color = Color.white;
                    break;
                case 3: // link node1 and node2
                    instr.node1.parentEdge = new GameObject("Line").AddComponent(typeof(LineRenderer)) as LineRenderer;
                    instr.node1.parentEdge.GetComponent<LineRenderer>().startColor = Color.black;
                    instr.node1.parentEdge.GetComponent<LineRenderer>().endColor = Color.black;
                    instr.node1.parentEdge.GetComponent<LineRenderer>().startWidth = .05f;
                    instr.node1.parentEdge.GetComponent<LineRenderer>().endWidth = .05f;
                    instr.node1.parentEdge.GetComponent<LineRenderer>().positionCount = 2;
                    instr.node1.parentEdge.GetComponent<LineRenderer>().useWorldSpace = true;
                    resetPositions(root);
                    yield return new WaitForSeconds(time);
                    break;
                case 4: // increment node1.childVolumes[insrt.additionalInfo]
                    instr.node1.childVolumes[instr.additionalInfo]++;
                    break;
                case 5: // set node1.childVolumes[x] to additional info
                    instr.node1.childVolumes[1]++;
                    break;
                case 6:
                    yield return new WaitForSeconds(time);
                    break;
                case 7: // left rotate
                    break;
                case 8: // right rotate
                    break;
                default:
                    break;
            }
        }
    }

    void Start()
    {
        treeDepth = 0;
        size = 8;
        root = null;
        Random r = new Random();
        int[] a = new int[size];

        for(int i = 0; i < size; i++)
        {
            a[i] = i + 1;
        }
        for(int i = 0; i < size; i++)
        {
            int temp1 = r.Next(i, size);
            int temp2 = a[i];
            a[i] = a[temp1];
            a[temp1] = temp2; 
        }

        Debug.Log(a);
        foreach (int i in a)
        {
            root = insertNode(root, null, i, 0);
        }
        //q.Enqueue(new AVLCommand(0, root, null, 0));
        preOrderPrint(root);
        setCam();
        StartCoroutine(readQueue(.1f));
    }

    private int getBalance(AVLNode node) => node == null ? 0 : height(node.children[0]) - height(node.children[1]);

    private int max(int a, int b) => a > b ? a : b;

    private int height(AVLNode node) => node == null ? 0 : node.height;

    private AVLNode updateDepth(AVLNode root, int depth)
    {
        if(root == null)
        {
            return root;
        }
        root.depth = depth;
        root.children[0] = updateDepth(root.children[0], depth + 1);
        root.children[1] = updateDepth(root.children[1], depth + 1);
        return root;
    }
    
    private int fetchChildren(AVLNode root) => root == null ? 0 : fetchChildren(root.children[0]) + fetchChildren(root.children[1]) + 1;

    private AVLNode updateChildren(AVLNode root)
    {
        if(root == null)
        {
            return root;
        }

        root.children[0] = updateChildren(root.children[0]);
        root.children[1] = updateChildren(root.children[1]);

        if(root.children[0] != null)
        {
            root.childVolumes[0] = root.children[0].childVolumes[0] + root.children[0].childVolumes[1] + 1;
        }

        if (root.children[1] != null)
        {
            root.childVolumes[1] = root.children[1].childVolumes[0] + root.children[1].childVolumes[1] + 1;
        }

        return root;
    }

    private void preOrderPrint(AVLNode root)
    {
        if(root == null)
        {
            return;
        }
        Debug.Log("Key: " + root.value + "  \theight: " + root.height + "  \tDepth: " + root.depth);

        if (root.children[0] != null)
        {
            int leftC = fetchChildren(root.children[0]);
            
            for(int i = 0; i < leftC; i++)
            {
                //q.Enqueue(new AVLCommand(4, root, null, 0));
            }

            //q.Enqueue(new AVLCommand(0, root.children[0], null, 0));
            //q.Enqueue(new AVLCommand(3, root.children[0], root, 0));
            preOrderPrint(root.children[0]);
        }

        if (root.children[1] != null)
        {
            int rightC = fetchChildren(root.children[1]);

            for (int i = 0; i < rightC; i++)
            {
                //q.Enqueue(new AVLCommand(4, root, null, 1));
            }

            //q.Enqueue(new AVLCommand(0, root.children[1], null, 0));
            //q.Enqueue(new AVLCommand(3, root.children[1], root, 0));
            preOrderPrint(root.children[1]);
        }
    }
    

    protected AVLNode rRotate(AVLNode root)
    {
        int rootDepth = root.depth;
        AVLNode newRoot = root.children[0];
        AVLNode newRightLeft = newRoot.children[1];

        newRoot.children[1] = root;
        root.children[0] = newRightLeft;

        root.height = max(height(root.children[0]), height(root.children[1])) + 1;
        newRoot.height = max(height(newRoot.children[0]), height(newRoot.children[1])) + 1;

        newRoot = updateDepth(newRoot, rootDepth);

        return newRoot;
    }

    protected AVLNode lRotate(AVLNode root)
    {
        int rootDepth = root.depth;
        AVLNode newRoot = root.children[1];
        AVLNode newLeftRight = newRoot.children[0];

        newRoot.children[0] = root;
        root.children[1] = newLeftRight;

        root.height = max(height(root.children[0]), height(root.children[1])) + 1;
        newRoot.height = max(height(newRoot.children[0]), height(newRoot.children[1])) + 1;

        newRoot = updateDepth(newRoot, rootDepth);

        return newRoot;
    }

    private AVLNode insertNode(AVLNode root, AVLNode parent, int key, int depth)
    {
        if(root == null) // no node found with key, make new node
        {
            if(depth > treeDepth)
            {
                treeDepth = depth;
                setCam();
            }
            AVLNode newNode = new AVLNode(key, depth, 1, 2);

            q.Enqueue(new AVLCommand(0, newNode, null, 0));
            if(parent != null)
            {
                q.Enqueue(new AVLCommand(3, newNode, parent, 0));
            }

            return newNode;
        }

        q.Enqueue(new AVLCommand(1, root, null, 0));

        if (key < root.value) // inserted key is less than node's key, go left
        {
            q.Enqueue(new AVLCommand(4, root, null, 0));
            root.children[0] = insertNode(root.children[0], root, key, depth+1);
        }
        else if(key > root.value) // inserted key is more than node's key, go right
        {
            q.Enqueue(new AVLCommand(4, root, null, 1));
            root.children[1] = insertNode(root.children[1], root, key, depth + 1);
        }

        q.Enqueue(new AVLCommand(2, root, null, 0));

        root.height = max(height(root.children[0]), height(root.children[1])) + 1; // update height

        int balance = getBalance(root); // get the balance of the node;

        // rotations

        if(balance > 1)
        {
            if(key > root.children[0].value)
            {
                root.children[0] = lRotate(root.children[0]);
            }

            return rRotate(root);
        }
        else if(balance < -1)
        {
            if (key < root.children[1].value)
            {
                root.children[1] = rRotate(root.children[1]);
            }

            return lRotate(root);
        }

        return root;
    }
    */






/*
 // extends the TreeNode class by adding AVL elements
    // NOTES: AVLNode.children is inherited but 
    protected class AVLNode : TreeNode{
        public short balanceFactor;
        public int height;
        public override AVLNode[] children;
        public AVLNode(int value, int depth, int NoOfChildren) : base(value, depth, NoOfChildren)
        {
            balanceFactor = 0;
            height = 1;
        }
    }
    
    /* stuff to keep track of for AVLNodes:
     * GameObject o: the sphere in the visualizer
     * AVLNode children[]: 0 is the node to the left, 1 to the right
     * int value: the key
     * int depth: 
     //

// Start is called before the first frame update
void Start()
{
    depth = 0; // unecessary
    size = 20; // # of nodes in AVL tree
               // set the root node
    root = new AVLNode(r.Next(100), 0, 2); // making root of the tree
    Debug.Log(root.GetType());
    q.Enqueue(new QueueCommand(0, root, null, 0)); // queues visualization of root
                                                   // set all the other nodes
    for (int i = 0; i < size; i++)
    {
        root = addNode((AVLNode)root, null, r.Next(100), 0); //inserts new node
        q.Enqueue(new QueueCommand(6, null, null, 0)); // wait
    }
    StartCoroutine(readQueue(.5f));  // starts visualization queue      
}
// add a node to the tree
// recursively call the function until we hit the point it gets added
protected AVLNode addNode(AVLNode root, AVLNode parent, int x, int depth)
{
    // build the node and its line renderer
    if (root == null)
    {
        // increment deepest depth
        if (depth > this.depth)
        { // if the depth of the new node is deeper than the depth of the tree
            this.depth = depth; // update tree's depth
            setCam(); // update camera
        }
        AVLNode node = new AVLNode(x, depth, 2); // make and insert new node
        q.Enqueue(new QueueCommand(0, node, null, 0)); // queue creation of new node
        q.Enqueue(new QueueCommand(3, node, parent, 0)); // link new node to tree

        return node; // return the new node
    }
    else
    {
        q.Enqueue(new QueueCommand(1, root, null, 0)); // queue highlight of new node
        if (x < root.value) // if the new key is less than the key of the node
        {
            q.Enqueue(new QueueCommand(4, root, null, 0)); // increases the stored number of left children

            root.children[0] = addNode((AVLNode)(root.children[0]), root, x, depth + 1); // continues insertion down left child
            if (root.children[1] == null) // if the node has no right children
            {
                root.balanceFactor = -1; // set the node's balance to -1
            }
            else // else set the node's balance to the
            {
                root.balanceFactor = (short)((AVLNode)(root.children[1]).height - (AVLNode)(root.children[0]).height); // 
            }
            root.childEdges[0] = root.children[0].parentEdge;

        }
        else
        {
            q.Enqueue(new QueueCommand(4, root, null, 1));

            if (root.children[0] == null)
            {
                root.balanceFactor = 1;
            }
            else
            {
                root.balanceFactor = (short)(root.children[0].height - root.children[1].height);
            }
            root.childEdges[1] = root.children[1].parentEdge;
        }
        q.Enqueue(new QueueCommand(2, root, null, 0));
        if (Math.Abs(root.balanceFactor) > 1)
        {

        }
        return root;
    }
}

// Update is called once per frame
void Update()
{

}
*/


//  
