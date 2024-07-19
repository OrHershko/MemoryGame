using System;
using System.Collections.Generic;

namespace B24_Ex02_Adi_208295576_Or_322316514
{
    internal class GameLogic
    {
        private GameBoard m_GameBoard;
        
        public void InitGameBoard(int i_Rows, int i_Columns)
        {
            m_GameBoard = new GameBoard(i_Rows, i_Columns);
        }
        
        public int GetBoardNumOfRows()
        {
            return m_GameBoard.NumberOfRows;
        }

        public int GetBoardNumOfCols()
        {
            return m_GameBoard.NumberOfColumns;
        }

        public int GetSquareValueInPairsMatrix(int i_Row, int i_Column)
        {
            return m_GameBoard.GetSquareValueInPairsMatrix(i_Row, i_Column);
        }

        public int GetSquareValueInExposedSquaresMatrix(int i_Row, int i_Column)
        {
            return m_GameBoard.GetSquareValueInExposedSquaresMatrix(i_Row, i_Column);
        }

        public void SetSquareValueInExposedSquaresMatrix(int i_Row, int i_Column, int i_Value)
        {
            m_GameBoard.SetSquareValueInExposedSquaresMatrix(i_Row, i_Column, i_Value);
        }

        public bool IsMoveInBoundaries(int i_RowOfMove, int i_ColumnOfMove)
        {
            return (m_GameBoard.IsColInBoundaries(i_ColumnOfMove) && m_GameBoard.IsRowInBoundaries(i_RowOfMove));
        }

        public bool IsSquareExpose(int i_RowOfMove, int i_ColumnOfMove)
        {
            return (m_GameBoard.GetSquareValueInExposedSquaresMatrix(i_RowOfMove, i_ColumnOfMove) == GameBoard.k_FlipedSquare);
        }

        public bool IsPlayerChosePair(int i_FirstSquareRow, int i_FirstSquareCol, int i_SecondSquareRow, int i_SecondSquareCol)
        {
            bool isPlayerChosePair = false;

            if(m_GameBoard.GetSquareValueInPairsMatrix(i_FirstSquareRow, i_FirstSquareCol)
                == m_GameBoard.GetSquareValueInPairsMatrix(i_SecondSquareRow, i_SecondSquareCol))
            {
                isPlayerChosePair = true;
            }

            return isPlayerChosePair;
        }

        public bool IsAllSquaresExposed()
        {
            return m_GameBoard.IsAllSquaresExposed();
        }
    }
}
