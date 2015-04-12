
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

        private PlayerProfileManager()
        {
            _playerDb = new SqliteDatabase("player.db");
        }

        public LoginAttemptResult AttemptLogin(string playerName, string playerPassword)
        {
            var data = _playerDb.ExecuteQuery("SELECT PlayerName, PlayerPassword FROM players WHERE PlayerName = " + playerName);

            if (data.Columns.Count == 0)
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

        public void ClaimCoins(int coins)
        {
            if(CurrentPlayer != null)
                _playerDb.ExecuteNonQuery("UPDATE Players SET Coins = " + coins + " WHERE PlayerName = " + CurrentPlayer);
            else
                Debug.Log("NOTE! Claim Coins failed (player is not logged in)");
        }
    }
}
