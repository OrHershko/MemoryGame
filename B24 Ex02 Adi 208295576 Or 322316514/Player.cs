
namespace B24_Ex02_Adi_208295576_Or_322316514
{
    internal class Player
    {
        private string m_Name;
        private int m_Score;
        private ePlayerType m_PlayerType;

        public enum ePlayerType
        {
            HumanPlayer = 1,
            Computer = 2,
        }

        public string Name
        { 
            get { return m_Name; } 
            set { m_Name = value; }
        }

        public int Score
        {
            get { return m_Score; }
            set { m_Score = value; }
        }

        public ePlayerType PlayerType 
        { 
            get { return m_PlayerType; } 
            set { m_PlayerType = value; }
        }
    }
}
