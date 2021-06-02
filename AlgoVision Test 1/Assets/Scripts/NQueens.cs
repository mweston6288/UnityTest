using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NQueens : Algorithm
{
    int n;
    bool solution;
    int [,] internalBoard; // tracks how many queens can move to each tile on the board
    GameObject[,] board;
    // Start is called before the first frame update
    void Start()
    {
        n = 6;
        board = new GameObject[n,n];
        internalBoard = new int[n,n];
        solution = false;
        int i,j;
        for(i = 0; i < n; i++){
            for(j = 0; j < n; j++){
                board[i,j] = GameObject.CreatePrimitive(PrimitiveType.Cube);
                if ((i + j) % 2 == 1){
                    board[i,j].GetComponent<Renderer>().material.color = Color.gray;
                    internalBoard[i,j] = 0;
                }
                board[i,j].transform.position = new Vector3(i, n-j, 0);
            }
        }
        setCam();
        StartCoroutine(build(n, 0));
    }
    public void setCam()
    {
        Camera.main.transform.position = new Vector3(n/2, n/2, n*-1 - 1);
        //Camera.main.farClipPlane = (float)(-1.1*z + 200);
    }
    public IEnumerator build(int size, int column){
        yield return new WaitForSeconds(.01f);

        if (column == size) {
			solution = true;
            yield break;
		}
        int i, j, k;

        for (i = 0; i < n; i++){
            board[i, column].GetComponent<Renderer>().material.color = Color.blue;
            yield return new WaitForSeconds(1f);
            setColor(i, column);
            // a safe space
            if(internalBoard[i, column] == 0){
                // increment the row and column
                for (j = 0; j < size; j++){
                    internalBoard[i, j]++;
                    internalBoard[j, column]++;
                    if ((i + j) % 2 == 1){
                        board[i,j].GetComponent<Renderer>().material.color = new Color(.67f,0,0);
                    }
                    else{
                        board[i,j].GetComponent<Renderer>().material.color = Color.red;
                    }

                    if((j + column) % 2 == 1 ){
                        board[j,column].GetComponent<Renderer>().material.color = new Color(.67f,0,0);

                    }
                    else{
                        board[j,column].GetComponent<Renderer>().material.color = Color.red;
                    }

                }
                // top left diagonal
				for (j = i, k = column;j >= 0 && k>= 0 ;j--,k--){
					internalBoard[j,k]++;
                    if((j + k) % 2 == 1 ){
                        board[j,k].GetComponent<Renderer>().material.color = new Color(.67f,0,0);

                    }
                    else{
                        board[j,k].GetComponent<Renderer>().material.color = Color.red;
                    }
				}
				// bottom left diagonal
				for (j = i, k = column; j >= 0 && k < size; j--, k++) {
					internalBoard[j,k]++;
                    if((j + k) % 2 == 1 ){
                        board[j,k].GetComponent<Renderer>().material.color = new Color(.67f,0,0);

                    }
                    else{
                        board[j,k].GetComponent<Renderer>().material.color = Color.red;
                    }				}
				// top right diagonal
				for (j = i, k = column; j < size && k >= 0; j++, k--) {
					internalBoard[j,k]++;
                    if((j + k) % 2 == 1 ){
                        board[j,k].GetComponent<Renderer>().material.color = new Color(.67f,0,0);

                    }
                    else{
                        board[j,k].GetComponent<Renderer>().material.color = Color.red;
                    }
				}
				// bottom right diagonal
				for (j = i, k = column; j < size && k < size; j++, k++) {
					internalBoard[j,k]++;
                    if((j + k) % 2 == 1 ){
                        board[j,k].GetComponent<Renderer>().material.color = new Color(.67f,0,0);

                    }
                    else{
                        board[j,k].GetComponent<Renderer>().material.color = Color.red;
                    }
				}
                board[i,column].GetComponent<Renderer>().material.color = Color.green;

                yield return build(size, column +1 );
                if (solution)
                    yield break;
                
                for (j = 0; j < size; j++){
                    internalBoard[i, j]--;
                    if (internalBoard[i,j] == 0){
                        if ((i + j) % 2 == 1){
                            board[i,j].GetComponent<Renderer>().material.color = Color.gray;
                        }
                        else{
                            board[i,j].GetComponent<Renderer>().material.color = Color.white;
                        }
                    }
                    internalBoard[j, column]--;
                    if (internalBoard[j,column] == 0){
                        if ((column + j) % 2 == 1){
                            board[j,column].GetComponent<Renderer>().material.color = Color.gray;
                        }
                        else{
                            board[j,column].GetComponent<Renderer>().material.color = Color.white;
                        }
                    }
                }
                // top left diagonal
				for (j = i, k = column;j >= 0 && k>= 0 ;j--,k--){
					internalBoard[j,k]--;
                    if (internalBoard[j,k] == 0){
                        if ((k + j) % 2 == 1){
                            board[j,k].GetComponent<Renderer>().material.color = Color.gray;
                        }
                        else{
                            board[j,k].GetComponent<Renderer>().material.color = Color.white;
                        }
                    }
				}
				// bottom left diagonal
				for (j = i, k = column; j >= 0 && k < size; j--, k++) {
					internalBoard[j,k]--;
                    if (internalBoard[j,k] == 0){
                        if ((k + j) % 2 == 1){
                            board[j,k].GetComponent<Renderer>().material.color = Color.gray;
                        }
                        else{
                            board[j,k].GetComponent<Renderer>().material.color = Color.white;
                        }
                    }
				}
				// top right diagonal
				for (j = i, k = column; j < size && k >= 0; j++, k--) {
					internalBoard[j,k]--;
                    if (internalBoard[j,k] == 0){
                        if ((k + j) % 2 == 1){
                            board[j,k].GetComponent<Renderer>().material.color = Color.gray;
                        }
                        else{
                            board[j,k].GetComponent<Renderer>().material.color = Color.white;
                        }
                    }
				}
				// bottom right diagonal
				for (j = i, k = column; j < size && k < size; j++, k++) {
					internalBoard[j,k]--;
                    if (internalBoard[j,k] == 0){
                        if ((k + j) % 2 == 1){
                            board[j,k].GetComponent<Renderer>().material.color = Color.gray;
                        }
                        else{
                            board[j,k].GetComponent<Renderer>().material.color = Color.white;
                        }
                    }
				}
            }
        }
    }
    // set the color of one of the board pieces
    // if the internalBoard value is greater than 0, that means at least one queen could move there and another queen cannot be placed
    void setColor(int i, int j){
        if (internalBoard[i,j] > 0){
            if((i + j) % 2 == 1 ){
                board[i,j].GetComponent<Renderer>().material.color = new Color(.67f,0,0);
            }
            else{
                board[i,j].GetComponent<Renderer>().material.color = Color.red;
            }           
        }
        else{
             if ((i+j) % 2 == 1){
                board[i,j].GetComponent<Renderer>().material.color = Color.gray;
            }
            else{
                board[i,j].GetComponent<Renderer>().material.color = Color.white;
            }                 
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
