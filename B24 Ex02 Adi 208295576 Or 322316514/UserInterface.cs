using System;
using System.Collections.Generic;

namespace B24_Ex02_Adi_208295576_Or_322316514
{
    internal class UserInterface
    {
        private Player m_FirstPlayer = new Player();
        private Player m_SecondPlayer = new Player();
        private GameLogic m_GameLogic = new GameLogic();
        private bool m_IsGameOn = true;
        private readonly char[] r_Letters = { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R' };
        public const int k_AsciiValueOfA = 65;
        public const string k_PlayAnotherGame = "1";
        public const string k_ExitGame = "2";
        private bool m_IsThereAnotherGame = true;
        private bool m_FirstPlayerTurn = true;
        private bool m_IsPlayerPressedQ = false;
        private Dictionary<string, int> m_ComputerExposedSquaresMemory = new Dictionary<string, int>();
        private Queue<string> m_ComputerMovesQueue = new Queue<string>();
        private Stack<string> m_FlipedSquaresSaver = new Stack<string>();

        public void RunGame()
        {
            printStartOfGame();
            setUsersData();
            
            while(m_IsThereAnotherGame == true && m_IsPlayerPressedQ == false)
            {
                setGameBoard();
                gameLoop();

                if(m_IsPlayerPressedQ == false)
                {
                    m_IsThereAnotherGame = isThereAnotherGame();
                }
            }
        }

        private bool isThereAnotherGame()
        {
            Console.WriteLine("Press 1 to start a new game or press 2 to exit.");
            string userChoice = Console.ReadLine();
            
            while(userChoice != k_PlayAnotherGame && userChoice != k_ExitGame)
            {
                Console.WriteLine("Invalid input, Press only 1 or 2.");
                userChoice = Console.ReadLine();
            }

            if(userChoice == k_PlayAnotherGame)
            {
                m_IsGameOn = true;
                m_FirstPlayer.Score = 0;
                m_SecondPlayer.Score = 0;
                m_FirstPlayerTurn = true;
                Ex02.ConsoleUtils.Screen.Clear();
            }
            else
            {
                m_IsThereAnotherGame = false;
            }

            return (userChoice == k_PlayAnotherGame);
        }

        private void printStartOfGame()
        {
            Console.WriteLine("Hello, welcome to our Memory Game!");
            Console.WriteLine("**********************************");
            Console.Write(Environment.NewLine);
        }

        private void setUsersData() 
        {
            setPlayerOne();
            chooseGameType();
            setPlayerTwo();
        }

        private void setPlayerOne()
        {
            Console.WriteLine("Please enter the first player name: ");
            m_FirstPlayer.Name = Console.ReadLine();
            m_FirstPlayer.PlayerType = Player.ePlayerType.HumanPlayer;
        }

        private void chooseGameType()
        {
            Console.WriteLine("For two players game press 1, and to play against the computer press 2.");
            string playerType;

            while (true)
            {
                playerType = Console.ReadLine();

                if (isPlayerTypeValid(playerType) == true)
                {
                    break;
                }
                else
                {
                    Console.WriteLine("Invalid input, please enter 1 or 2 only.");
                }
            }

            m_SecondPlayer.PlayerType = (Player.ePlayerType)int.Parse(playerType);
        }

        private bool isPlayerTypeValid(string i_PlayerType)
        {
            return (i_PlayerType == "1" ||  i_PlayerType == "2");
        }

        private void setPlayerTwo()
        {
            if(m_SecondPlayer.PlayerType == Player.ePlayerType.HumanPlayer)
            {
                Console.WriteLine("Please enter the second player name: ");
                m_SecondPlayer.Name = Console.ReadLine();
            }
            else
            {
                m_SecondPlayer.Name = "Computer";
            }
        }

        private void setGameBoard()
        {
            while(true)
            {
                Console.WriteLine("Enter the preferred number of rows between 4 to 6 for the board size:") ;
                string numberOfRowsStr = Console.ReadLine();

                if (isBoardBoundariesValid(numberOfRowsStr) == false)
                {
                    continue;
                }

                Console.WriteLine("Enter the preferred number of columns between 4 to 6 for the board size:");
                string numberOfColsStr = Console.ReadLine();

                if (isBoardBoundariesValid(numberOfColsStr) == true)
                {
                    if(isEvenNumberOfSquares(numberOfRowsStr, numberOfColsStr) == true)
                    {
                        m_GameLogic.InitGameBoard(int.Parse(numberOfRowsStr), int.Parse(numberOfColsStr));
                        break;
                    }
                }
            }
        }

        private bool isBoardBoundariesValid(string i_BoundaryStr)
        {
            bool isBoundaryValid = false;

            if(i_BoundaryStr == "4" || i_BoundaryStr == "5" || i_BoundaryStr == "6") 
            { 
                isBoundaryValid = true;
            }
            else
            {
                Console.WriteLine("Invalid input, must be a number between 4 to 6.");
            }

            return isBoundaryValid;
        }

        private bool isEvenNumberOfSquares(string i_NumberOfRowsStr, string i_NumberOfColsStr)
        {
           bool isEvenNumberOfSquares = false;

            if(int.TryParse(i_NumberOfRowsStr, out int rows) && int.TryParse(i_NumberOfColsStr, out int cols))
            {
               
                if ((rows * cols) % 2 == 0)
                {
                    isEvenNumberOfSquares = true;
                }
                else
                {
                    Console.WriteLine("Invalid input, must be an even number of squares.");
                }
            }
            
            return isEvenNumberOfSquares;
        }

        private void gameLoop()
        {
            while (m_IsGameOn == true)
            {
                printGameBoard();
                playerTurn();
                checkIfGameFinished();
            }
        }

        private void checkIfGameFinished()
        {
            if(m_GameLogic.IsAllSquaresExposed() == true)
            {
                printEndOfGameMessage();
                m_IsGameOn = false;
            }
        }

        private void printEndOfGameMessage()
        {
            Console.Write("Game over. ");

            if (m_FirstPlayer.Score == m_SecondPlayer.Score)
            {
                Console.Write("It's a tie!");
            }
            else
            {
                string winnerName = m_FirstPlayer.Score > m_SecondPlayer.Score
                    ? m_FirstPlayer.Name : m_SecondPlayer.Name;
                Console.Write($"{winnerName} is the winner!");
            }

            Console.Write(Environment.NewLine);
        }

        private void printGameBoard()
        {
            int numberOfCols = m_GameLogic.GetBoardNumOfCols();
            int numberOfRows = m_GameLogic.GetBoardNumOfRows();

            Ex02.ConsoleUtils.Screen.Clear();
            printLineOfLetters(numberOfCols);
            printSeparatorLine(numberOfCols);
            printRowsOfBoard(numberOfRows, numberOfCols);
            printPlayersScore();
        }

        private void printPlayersScore()
        {
            Console.WriteLine($"{m_FirstPlayer.Name}'s score: {m_FirstPlayer.Score}");
            Console.WriteLine($"{m_SecondPlayer.Name}'s score: {m_SecondPlayer.Score}");
            Console.Write(Environment.NewLine);
        }

        private void printLineOfLetters(int i_NumberOfCols)
        {
            Console.Write("    ");
   
            for(int i = 0; i < i_NumberOfCols; i++)
            {
                Console.Write(r_Letters[i]);
                Console.Write("   ");
            }

            Console.Write(Environment.NewLine);
        }

        private void printSeparatorLine(int i_NumberOfCols)
        {
            Console.Write("  =");

            for (int i = 0; i < i_NumberOfCols; i++)
            {
                Console.Write("====");
                
            }

            Console.Write(Environment.NewLine);
        }

        private void printRowsOfBoard(int i_NumberOfRows, int i_NumberOfCols)
        {
            for(int i = 0;i < i_NumberOfRows; i++)
            {
                Console.Write((i + 1) + " |");

                for (int j = 0; j < i_NumberOfCols; j++)
                {
                    if(m_GameLogic.GetSquareValueInExposedSquaresMatrix(i,j) == 0)
                    {
                        Console.Write("   |");
                    }
                    else
                    {
                        Console.Write(" " + r_Letters[m_GameLogic.GetSquareValueInPairsMatrix(i,j)] + " |");
                    }
                }

                Console.Write(Environment.NewLine);
                printSeparatorLine(i_NumberOfCols);
                Console.Write(Environment.NewLine);
            }
        }

        private void playerTurn()
        {
            printTurnMassage();
            flipSquere();
            if(m_IsGameOn == true)
            {
                flipSquere();
            }
            if (m_IsGameOn == true)
            {
                playerTurnResult();
            }
        }

        private void printTurnMassage()
        {
            string name = m_FirstPlayerTurn ? m_FirstPlayer.Name : m_SecondPlayer.Name;

            Console.WriteLine($"{name}'s turn.");
        }

        private bool isMoveValid(int i_RowOfMove, int i_ColumnOfMove)
        {
            bool isMoveValid = false;

            if (m_GameLogic.IsMoveInBoundaries(i_RowOfMove, i_ColumnOfMove) == true)
            {
                if (m_GameLogic.IsSquareExpose(i_RowOfMove, i_ColumnOfMove) == false)
                {
                    isMoveValid = true;
                }
                else
                {
                    Console.WriteLine("Invalid input, the square is already exposed.");
                }
            }
            else
            {
                Console.WriteLine("Invalid input, the move must be in the board boundaries.");
            }

            return isMoveValid;
        }

        private void flipSquere()
        {
            while (true)
            {
                string playerMove = getPlayerMove();

                if(playerMove == "Q" || playerMove == "q")
                {
                    m_IsGameOn = false;
                    m_IsPlayerPressedQ = true;
                    break;
                }

                if (isInputLetterAndNumber(playerMove) == true)
                {
                    int columnOfMove = char.ToUpper(playerMove[0]) - k_AsciiValueOfA;
                    int rowOfMove = int.Parse(playerMove[1].ToString()) - 1;

                    if (isMoveValid(rowOfMove, columnOfMove) == true)
                    {
                        m_GameLogic.SetSquareValueInExposedSquaresMatrix(rowOfMove, columnOfMove, GameBoard.k_FlipedSquare);
                        m_FlipedSquaresSaver.Push(playerMove);
                        printGameBoard();
                        break;
                    }
                }
            }
        }

        private string getPlayerMove()
        {
            string moveStr;

            if(m_FirstPlayerTurn == true || m_SecondPlayer.PlayerType == Player.ePlayerType.HumanPlayer)
            {
                Console.WriteLine("Please choose a square location on the board or enter Q to exit:");
                moveStr = Console.ReadLine();
            }
            else
            {
                moveStr = computerMoveGenerator();
                Console.WriteLine($"The computer choice is: {moveStr}");
                System.Threading.Thread.Sleep(2000);
            }

            return moveStr;
        }

        private string computerMoveGenerator()
        {
            string NextMoveStr;
            List<string> possibleMoves;

            if (m_ComputerMovesQueue.Count > 0)
            {
                NextMoveStr =  m_ComputerMovesQueue.Dequeue();
            }
            else if (isThereMoveToExposePair(out NextMoveStr) == false)
            {
                possibleMoves = makePossibleMovesList();
                NextMoveStr = chooseRandomMoveFromPossibleMovesList(possibleMoves);
            }

            return NextMoveStr;
        }

        private string chooseRandomMoveFromPossibleMovesList(List<string> i_PossibleMoves)
        {
            Random random = new Random();
            int randomIndex = random.Next(i_PossibleMoves.Count);

            return i_PossibleMoves[randomIndex];
        }

        private bool isThereMoveToExposePair(out string o_NextMoveStr)
        {
            bool isThereMoveToExposePair = false;
            o_NextMoveStr = null;

            foreach (var firstMove in m_ComputerExposedSquaresMemory)
            {
                foreach (var secondMove in m_ComputerExposedSquaresMemory)
                {
                    if (firstMove.Key != secondMove.Key && firstMove.Value == secondMove.Value)
                    {
                        m_ComputerMovesQueue.Enqueue(secondMove.Key);
                        o_NextMoveStr = firstMove.Key;
                        isThereMoveToExposePair = true;
                        break;
                    }
                }

                if(isThereMoveToExposePair == true)
                {
                    break;
                }
            }

            return isThereMoveToExposePair;
        }

        private void updateComputerExposedSquaresMemory(ref string io_MoveStr, int i_Row, int i_Col)
        {
            int squareValue = m_GameLogic.GetSquareValueInPairsMatrix(i_Row, i_Col);

            io_MoveStr = $"{char.ToUpper(io_MoveStr[0])}{io_MoveStr[1]}";

            if (m_ComputerExposedSquaresMemory.ContainsKey(io_MoveStr) == false)
            {
                m_ComputerExposedSquaresMemory.Add(io_MoveStr, squareValue);
            }
        }

        private List<string> makePossibleMovesList()
        {
            List<string> possibleMoves = new List<string>();

            for (int row = 0; row < m_GameLogic.GetBoardNumOfRows(); row++)
            {
                for (int col = 0; col < m_GameLogic.GetBoardNumOfCols(); col++)
                {
                    if (m_GameLogic.GetSquareValueInExposedSquaresMatrix(row, col) == GameBoard.k_UnflipedSquare)
                    {
                        char letter = (char)(k_AsciiValueOfA + col);
                        int digit = row + 1;
                        possibleMoves.Add($"{letter}{digit}");
                    }
                }
            }

            return possibleMoves;
        }

        private bool isInputLetterAndNumber(string i_PlayerInput)
        {
            bool isLetterAndNumber = false;

            if (i_PlayerInput.Length == 2)
            {
                char firstChar = i_PlayerInput[0];
                char secondChar = i_PlayerInput[1];
                isLetterAndNumber = char.IsLetter(firstChar) && char.IsDigit(secondChar);
            }

            if (isLetterAndNumber == false)
            {
                Console.WriteLine("Invalid input, the first character must be a letter and the second character must be a number.");
            }

            return isLetterAndNumber;
        }

        private void playerTurnResult()
        {
            int firstSquareRow;
            int firstSquareCol;
            int secondSquareRow;
            int secondSquareCol;
            string firstMoveStr = getSquareLocationFromFlipedStackSaver(out firstSquareRow, out firstSquareCol);
            string secondMoveStr = getSquareLocationFromFlipedStackSaver(out secondSquareRow, out secondSquareCol);

            updateComputerExposedSquaresMemory(ref firstMoveStr, firstSquareRow, firstSquareCol);
            updateComputerExposedSquaresMemory(ref secondMoveStr, secondSquareRow, secondSquareCol);

            if (m_GameLogic.IsPlayerChosePair(firstSquareRow, firstSquareCol, secondSquareRow, secondSquareCol) == true)
            {
                m_ComputerExposedSquaresMemory.Remove(firstMoveStr);
                m_ComputerExposedSquaresMemory.Remove(secondMoveStr);
                Player currentPlayer = m_FirstPlayerTurn ? m_FirstPlayer : m_SecondPlayer;
                currentPlayer.Score++;
            }
            else
            {
                m_GameLogic.SetSquareValueInExposedSquaresMatrix(firstSquareRow, firstSquareCol, GameBoard.k_UnflipedSquare);
                m_GameLogic.SetSquareValueInExposedSquaresMatrix(secondSquareRow, secondSquareCol, GameBoard.k_UnflipedSquare);
                m_FirstPlayerTurn = !m_FirstPlayerTurn;
                System.Threading.Thread.Sleep(2000);
            }
        }

        private string getSquareLocationFromFlipedStackSaver(out int o_Row, out int o_Column)
        {
            string squareLocationStr = m_FlipedSquaresSaver.Pop();

            o_Column = char.ToUpper(squareLocationStr[0]) - k_AsciiValueOfA;
            o_Row = int.Parse(squareLocationStr[1].ToString()) - 1;

            return squareLocationStr;
        }
    }
}
