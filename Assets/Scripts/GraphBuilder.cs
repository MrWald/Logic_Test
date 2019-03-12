using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class GraphBuilder : MonoBehaviour 
{
    private double[,] matrix;

    private int amount=5;

    private int[] mask;

    private int basis = 3;

    public UnityEngine.Object DotPrefabMNK, DotPrefabLagrange;

    public DotController[] dots;

    public string Basis{ get; set; }

    public void BuildGraphs()
    {
        double[,] xyTable = new double[2, amount];
        for(int i=0; i<amount; i++)
        {
            xyTable[0,i] = i+1;
            xyTable[1,i] = toY(dots[i].transform.position.y);
        }
        DrawGraphMNK(BuildCoefs(xyTable));
        DrawGraphLagrange(xyTable);
    }

    private void DrawGraphMNK(double[] coeffs)
    {
        for(double x=0.1;x<5.9;x+=0.1)
        {
            double y = 0;
            for(int i=0;i<basis;++i)
            {
                y+=Math.Pow(x, i)*coeffs[i];
            }
            Instantiate(DotPrefabMNK, new Vector2(toXReverse(x), toYReverse(y)), Quaternion.identity);
        }
    }

    private void DrawGraphLagrange(double[,] xyValues)
    {
        for(double x=0.1;x<5.9;x+=0.1)
        {
            double y = InterpolateLagrangePolynomial(x, xyValues, amount);
            Instantiate(DotPrefabLagrange, new Vector2(toXReverse(x), toYReverse(y)), Quaternion.identity);
        }
    }

    private double[] BuildCoefs(double[,] xyTable)
    {
        Int32.TryParse(Basis, out basis);
        if(basis<0)
            throw new Exception("Basis can't be less than 0!");
        basis++;
        this.matrix = MakeSystem(xyTable, basis);
        return Gauss(basis, basis + 1);
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

    private double[] GaussReversePass(int colCount, int rowCount)
    {
        int i,j,k;
        for (i = rowCount - 1; i >= 0; i--)
        {
            for (j = i - 1; j >= 0; j--)
            {
                double tempMn = matrix[j,i];
                for (k = 0; k < colCount; k++)
                    matrix[j, k] -= matrix[i, k] * tempMn;
            }
        }
        double[] answer = new double[rowCount];
        for (i = 0; i < rowCount; i++)
            answer[mask[i]] = matrix[i, colCount - 1];
        return answer;
    }

    double InterpolateLagrangePolynomial (double x, double[,] xyValues, int size)
	{
		double lagrangePol = 0;
		for (int i = 0; i < size; i++)
		{
				double basicsPol = 1;
				for (int j = 0; j < size; j++)
				{
					if (j != i)
						basicsPol *= (x - xyValues[0, j])/(xyValues[0, i] - xyValues[0, j]);
				}
				lagrangePol += basicsPol * xyValues[1, i];
		}
		return lagrangePol;
	}

	double toY(float y)
    {
		return 10 * ((y+2.05)/6.45);
	}

    float toXReverse(double x)
    {
        return (float)x*(1.4f) - 5.25f;
    }

    float toYReverse(double y)
    {
        return (float)y*0.645f - 2.05f;
    }
}