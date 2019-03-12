using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MNKBuilder : MonoBehaviour {
	private int amount=5;
    private double[,] matrix;
    private int[] mask;
    private int basis = 4;
	public DotController d1, d2, d3, d4, d5;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
 
	private double[,] MakeSystem(double[,] xyTable, int basis)
    {
        double[,] matrix = new double[basis, basis + 1];
        for (int i = 0; i < basis; i++)
        {
            for (int j = 0; j < basis; j++)
            {
            matrix[i, j] = 0;
            }
        }
        for (int i = 0; i < basis; i++)
        {
            for (int j = 0; j < basis; j++)
            {
                double sumA = 0, sumB = 0;
                for (int k = 0; k < xyTable.Length / 2; k++)
                {
                    sumA += Math.Pow(xyTable[0, k], i) * Math.Pow(xyTable[0, k], j);
                    sumB += xyTable[1, k] * Math.Pow(xyTable[0, k], i);
                }
                matrix[i, j] = sumA;
                matrix[i, basis] = sumB;
            }
        }
        return matrix;
    }

    private double[] Gauss(int rowCount, int colCount){
        int i;
        mask = new int[colCount - 1];
        for (i = 0; i < colCount - 1; i++)
            mask[i] = i;
        if (GaussDirectPass(rowCount, colCount))
        {
            double[] answer = GaussReversePass(colCount, rowCount);
            return answer;
        }
        else return null;
    }
 
    private bool GaussDirectPass(int rowCount, int colCount){
        int i, j, k, maxId, tmpInt;
        double maxVal, tmpDouble;
        for (i = 0; i < rowCount; i++)
        {
            maxId = i;
            maxVal = matrix[i, i];
            for (j = i + 1; j < colCount - 1; j++)
                if(Math.Abs(maxVal)<Math.Abs(matrix[i, j]))
            {
                maxVal = matrix[i, j];
                maxId = j;
            }
            if (maxVal == 0) return false;
            if (i != maxId)
            {
                for (j = 0; j < rowCount; j++)
                {
                    tmpDouble = matrix[j, i];
                    matrix[j, i] = matrix[j, maxId];
                    matrix[j, maxId] = tmpDouble;
                }
                tmpInt = mask[i];
                mask[i] = mask[maxId];
                mask[maxId] = tmpInt;
            }
            for (j = 0; j < colCount; j++) matrix[i, j] /= maxVal;
            for (j = i + 1; j < rowCount; j++)
            {
                double tempMn = matrix[j, i];
                for (k = 0; k < colCount; k++)
                    matrix[j, k] -= matrix[i, k] * tempMn;
            }
        }
        return true;
    }
 
    private double[] GaussReversePass(int colCount, int rowCount){
        int i,j,k;
        for (i = rowCount - 1; i >= 0; i--)
            for (j = i - 1; j >= 0; j--)
            {
                double tempMn = matrix[j,i];
                for (k = 0; k < colCount; k++)
                    matrix[j, k] -= matrix[i, k] * tempMn;
            }
        double[] answer = new double[rowCount];
        for (i = 0; i < rowCount; i++) answer[mask[i]] = matrix[i, colCount - 1];
        return answer;
    }
 
    public void LeastSquares(){
        double[] result = BuildCoefs();
        string str = "";
        for (int i = 0; i < basis; i++)
        {
            str += result[i] + "x^" + i;
            if(i!=basis-1)
                str+=" + ";
        }
        print(str);
    }

    private double[] BuildCoefs(){
        double[,] xyTable = new double[2, 5];
		xyTable[0, 0] = 1;
		xyTable[0, 1] = toY(d1.transform.position.y);
		xyTable[0, 1] = 2;
		xyTable[1, 1] = toY(d2.transform.position.y);
		xyTable[0, 2] = 3;
		xyTable[1, 2] = toY(d3.transform.position.y);
		xyTable[0, 3] = 4;
		xyTable[1, 3] = toY(d4.transform.position.y);
		xyTable[0, 4] = 5;
		xyTable[1, 4] = toY(d5.transform.position.y);
        this.matrix = MakeSystem(xyTable, basis);
        return Gauss(basis, basis + 1);
    }

	double toY(float y){
		return 10 * (y+2.05/6.45);
	}
}