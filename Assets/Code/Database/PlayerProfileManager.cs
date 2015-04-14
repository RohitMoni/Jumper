using System.Linq;
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
            _playerDb.ExecuteScript("CREATE TABLE IF NOT EXISTS 'Players' (PlayerName varchar(255), PlayerPassword varchar(255), Coins int)");
        }

        public void ReadAllInPlayers()
        {
            var data = _playerDb.ExecuteQuery("SELECT * FROM Players");

            foreach (var row in data.Rows)
                foreach(var item in row)
                    Debug.Log("row item : " + item);
            foreach (var column in data.Columns)
                Debug.Log("Columns: " + column);
        }

        public LoginAttemptResult AttemptLogin(string playerName, string playerPassword)
        {
            var data = _playerDb.ExecuteQuery("SELECT PlayerName, PlayerPassword FROM Players WHERE PlayerName = '" + playerName + "'");

            if (data == null || data.Rows.Count == 0)
            {
                Debug.Log("NOTE! Login Attempt failed (wrong user name)");
                return LoginAttemptResult.WrongUserName;
            }

            var fuckthis = data.Rows[0].ToList()[1];

            if (fuckthis.Value.ToString() == playerPassword)
            {
                CurrentPlayer = playerName;
                return LoginAttemptResult.Success;
            }

            Debug.Log("NOTE! Login Attempt failed (wrong password)");
            return LoginAttemptResult.WrongPassword;
        }

        public void CreateAccount(string playerName, string playerPassword, int coins)
        {
            _playerDb.ExecuteNonQuery("INSERT INTO Players VALUES ('" + playerName + "', " + "'" + playerPassword + "', " + "'" + coins + "');");
        }

        public void DeleteAccount(string playerName)
        {
            _playerDb.ExecuteNonQuery("DELETE FROM Players WHERE PlayerName = '" + playerName + "'");
        }

        public int GetCoinsForLoggedInUser()
        {
            if (CurrentPlayer != null)
            {
                return
                    (int)
                        _playerDb.ExecuteQuery("SELECT Coins FROM Players WHERE PlayerName = '" + CurrentPlayer + "'")
                            .Rows[0].ToList()[0].Value;
            }

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
