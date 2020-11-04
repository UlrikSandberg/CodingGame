using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class Matrix
{
    // Hold the mathematical matrix as 2D array
    private float[,] _matrix;

    public int Rows => _matrix.GetUpperBound(0) + 1;
    public int Columns => _matrix.GetUpperBound(1) + 1;

    public float this[int row, int col]
    {
        get
        {
            //Validates that neither row nor column is out of range of the mathematical _matrix.
            ValidateIndex(row, col);
            return _matrix[row, col];
        }
        set
        {
            //Validate that neither row nor column is out of range of the mathematical _matrix;    
            ValidateIndex(row, col);
            //Check for a null value or overflow condition on the value to be inserted
            if (double.IsInfinity(value) || double.IsNaN(value))
            {
                throw new MatrixError("Trying to assign invalud number to matrix: "
                                      + value);
            }

            _matrix[row, col] = value;
        }
    }

    //Construct new empty matrix
    public Matrix(int rows, int cols)
    {
        EnsureMatrixValidity(rows, cols);
        _matrix = new float[rows, cols];
    }

    // Construct matrix from other matrix.
    public Matrix(float[,] source)
    {
        //Construct a new matrix from the dimensions of the source
        _matrix = new float[source.GetUpperBound(0) + 1, source.GetUpperBound(1) + 1];

        for (int i = 0; i < Rows; i++)
        {
            for (int j = 0; j < Columns; j++)
            {
                _matrix[i, j] = source[i, j];
            }
        }
    }

    public void Add(int row, int col, float value)
    {
        ValidateIndex(row, col);
        _matrix[row, col] += value;
    }

    public void Set(int row, int col, float value)
    {
        ValidateIndex(row, col);
        _matrix[row, col] = value;
    }

    public void Clear()
    {
        for (int r = 0; r < Rows; r++)
        {
            for (int c = 0; c < Columns; c++)
            {
                _matrix[r, c] = 0;
            }
        }
    }

    private void ValidateIndex(int row, int col)
    {
        if (row >= Rows || row < 0)
        {
            throw new MatrixError($"The row: {row} is out of range. Number of rows: {Rows}");
        }

        if (col >= Columns || col < 0)
        {
            throw new MatrixError($"The col: {col} is out of range. Number of cols: {Columns}");
        }
    }

    private void EnsureMatrixValidity(int rows, int cols)
    {
        if (rows < 1 || cols < 1)
        {
            throw new MatrixError($"Tried to instantiate matrix of Rows:{rows}, Cols:{cols} caused an error. Rows and cols must be above 0");
        }
    }

    private void EnsureMatrixValidity(float[,] matrix)
    {
        if ((matrix.GetUpperBound(0) + 1) < 1 || (matrix.GetUpperBound(1) + 1) < 1)
        {
            throw new MatrixError($"Tried to instantiate matrix of Rows:{(matrix.GetUpperBound(0) + 1)}, Cols:{matrix.GetUpperBound(1) + 1} caused an error. Rows and cols must be above 0");
        }
    }

    public override string ToString()
    {
        var builder = new StringBuilder();

        builder.Append("\n");
        for (int r = 0; r < Rows; r++)
        {
            for (int c = 0; c < Columns; c++)
            {
                builder.Append(_matrix[r, c]);
                builder.Append("\t");
            }

            builder.Append("\n");
        }

        return builder.ToString();
    }
}