using UnityEngine;

namespace Assets.Code.Database
{
    public enum LoginAttemptResult {Success, WrongPassword, WrongUserName}

    class PlayerProfileManager
    {
        /* SINGLETON */
        private static PlayerProfileManager _instance;
        public static PlayerProfileManager Instance
        {
            get
            {
                return _instance ?? (_instance = new PlayerProfileManager());
            }
        }

        /* PROPERTIES */
        private readonly SqliteDatabase _playerDb;

        public string CurrentPlayer;
        public bool IsLoggedIn
        {
            get { return CurrentPlayer != null; }
        }

        private PlayerProfileManager()
        {
            _playerDb = new SqliteDatabase("player.db");
        }

        public LoginAttemptResult AttemptLogin(string playerName, string playerPassword)
        {
            var data = _playerDb.ExecuteQuery("SELECT PlayerName, PlayerPassword FROM players WHERE PlayerName = '" + playerName + "'");

            if (data == null || data.Columns.Count == 0)
            {
                Debug.Log("NOTE! Login Attempt failed (wrong user name)");
                return LoginAttemptResult.WrongUserName;
            }

            if (data.Columns[1] == playerPassword)
            {
                CurrentPlayer = playerName;
                return LoginAttemptResult.Success;
            }

            Debug.Log("NOTE! Login Attempt failed (wrong password)");
            return LoginAttemptResult.WrongPassword;
        }

        public void CreateAccount(string playerName, string playerPassword, int coins)
        {
            _playerDb.ExecuteNonQuery("INSERT INTO Players (PlayerName, PlayerPassword, Coins) VALUES ('" + playerName + "', " + 
                                                                                                    "'" + playerPassword + "', " +
                                                                                                    "'" + coins + "')");
        }

        public void DeleteAccount(string playerName)
        {
            _playerDb.ExecuteNonQuery("DELETE FROM Players WHERE PlayerName = '" + playerName + "'");
        }

        public int GetCoinsForLoggedInUser()
        {
            if (CurrentPlayer != null)
                return int.Parse(_playerDb.ExecuteQuery("GET Coins WHERE PlayerName = '" + CurrentPlayer + "'").Columns[0]);
            
            Debug.Log("NOTE! Get Coins failed (player is not logged in)");
            return -1;
        }

        public void SetCoinsForLoggedInUser(int coins)
        {
            if(CurrentPlayer != null)
                _playerDb.ExecuteNonQuery("UPDATE Players SET Coins = " + coins + " WHERE PlayerName = '" + CurrentPlayer + "'");
            else
                Debug.Log("NOTE! Set Coins failed (player is not logged in)");
        }
    }
}
