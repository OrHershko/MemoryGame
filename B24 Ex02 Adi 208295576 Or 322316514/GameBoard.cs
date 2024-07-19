using System;
using System.Collections.Generic;

namespace B24_Ex02_Adi_208295576_Or_322316514
{
    internal class GameBoard
    {
        private readonly int r_NumberOfRows;
        private readonly int r_NumberOfColumns;
        private readonly int[,] r_PairsMatrix;
        private int[,] m_ExposedSquaresMatrix;
        public const int k_FlipedSquare = 1;
        public const int k_UnflipedSquare = 0;

        public GameBoard(int i_NumberOfRows, int i_NumberOfColumns)
        {
            r_NumberOfRows = i_NumberOfRows;
            r_NumberOfColumns = i_NumberOfColumns;
            m_ExposedSquaresMatrix = new int[i_NumberOfRows, i_NumberOfColumns];
            r_PairsMatrix = new int[i_NumberOfRows, i_NumberOfColumns];
            initPairsMatrix();
        }

        public int NumberOfRows
        {
            get { return r_NumberOfRows; }
        }

        public int NumberOfColumns
        {
            get { return r_NumberOfColumns; }
        }

        public int GetSquareValueInPairsMatrix(int i_Row, int i_Column)
        {
            return r_PairsMatrix[i_Row, i_Column];
        }

        public int GetSquareValueInExposedSquaresMatrix(int i_Row, int i_Column)
        {
            return m_ExposedSquaresMatrix[i_Row, i_Column];
        }

        public void SetSquareValueInExposedSquaresMatrix(int i_Row, int i_Column, int i_Value)
        {
            m_ExposedSquaresMatrix[i_Row, i_Column] = i_Value;
        }

        private void initPairsMatrix()
        {
            List<int> pairsValues = initPairsValuesList();
            int pairsIndex = 0;

            for(int row = 0;  row < r_NumberOfRows; row++) 
            {
                for(int col = 0; col < r_NumberOfColumns; col++)
                {
                    r_PairsMatrix[row,col] = pairsValues[pairsIndex];
                    pairsIndex++;
                }
            }
        }

        private List<int> initPairsValuesList()
        {
            int numberOfSquares = r_NumberOfRows * r_NumberOfColumns;
            int numberOfPairs = numberOfSquares / 2;
            List<int> pairsValues = new List<int>();
            Random random = new Random();

            for (int i = 0; i < numberOfPairs; i++)
            {
                pairsValues.Add(i);
                pairsValues.Add(i);
            }

            for (int i = pairsValues.Count - 1; i > 0; i--)
            {
                int j = random.Next(i + 1);
                swapInPairsList(ref pairsValues, i, j);
            }

            return pairsValues;
        }

        private void swapInPairsList(ref List<int> io_PairsValues, int i_FirstIndex, int i_SecondIndex)
        {
            int valueToSwap = io_PairsValues[i_FirstIndex];

            io_PairsValues[i_FirstIndex] = io_PairsValues[i_SecondIndex];
            io_PairsValues[i_SecondIndex] = valueToSwap;
        }

        public bool IsRowInBoundaries(int i_RowOfMove)
        {
            return (i_RowOfMove >= 0 && i_RowOfMove < r_NumberOfRows);
        }

        public bool IsColInBoundaries(int i_ColumnOfMove)
        {
            return (i_ColumnOfMove >= 0 && i_ColumnOfMove < r_NumberOfColumns);
        }

        public bool IsAllSquaresExposed()
        {
            bool isAllSquaresExposed = true;

            for(int i = 0; (i < r_NumberOfRows) && (isAllSquaresExposed == true); i++)
            {
                for(int j = 0; j < r_NumberOfColumns; j++)
                {
                    if (m_ExposedSquaresMatrix[i, j] == k_UnflipedSquare)
                    {
                        isAllSquaresExposed = false;
                        break;
                    }
                }
            }

            return isAllSquaresExposed;
        }
    }
}
