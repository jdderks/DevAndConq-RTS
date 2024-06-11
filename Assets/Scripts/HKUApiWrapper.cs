using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace HKU
{

    public class LeaderboardEntry
    {
        public string PlayerID { get; set; }
        public int Score { get; set; }
        public int Rank { get; set; }
    }

    public class HKUApiWrapper
    {
        public enum GetEntryOptions
        {
            Highest,
            AroundMe,
            AtRank,
            Friends // Not implemented
        };

        [DllImport("HKU_SDK", CallingConvention = CallingConvention.Cdecl)]
        public static extern void GetUsers(IntPtr callback, IntPtr context);
        [DllImport("HKU_SDK", CallingConvention = CallingConvention.Cdecl)]
        public static extern void ConfigureProject(string project_ID, ConfigureProjectCallbackDelegate callback, IntPtr context);
        [DllImport("HKU_SDK", CallingConvention = CallingConvention.Cdecl)]
        public static extern void OpenLoginPage();
        [DllImport("HKU_SDK", CallingConvention = CallingConvention.Cdecl)]
        public static extern void StartPolling(LoginStatusCallbackDelegate callback, IntPtr context);
        [DllImport("HKU_SDK", CallingConvention = CallingConvention.Cdecl)]
        public static extern void CancelPolling();
        [DllImport("HKU_SDK", CallingConvention = CallingConvention.Cdecl)]
        public static extern void Logout(LogoutCallbackDelegate callback, IntPtr context);
        [DllImport("HKU_SDK", CallingConvention = CallingConvention.Cdecl)]
        public static extern void GetUser(string user_ID, GetUserCallbackDelegate callback, IntPtr context);
        [DllImport("HKU_SDK", CallingConvention = CallingConvention.Cdecl)]
        public static extern void UploadLeaderboardScore(string leaderboard, int score, UploadLeaderboardScoreCallbackDelegate callback, IntPtr context);
        [DllImport("HKU_SDK", CallingConvention = CallingConvention.Cdecl)]
        public static extern void GetLeaderboard(string leaderboard_Id, ref IntPtr outArray, int amount, GetEntryOptions option, LeaderboardCallbackDelegate callback, IntPtr context);
        [DllImport("HKU_SDK", CallingConvention = CallingConvention.Cdecl)]
        public static extern void GetLeaderboardsForProject(ref IntPtr outArray, GetLeaderboardsForProjectCallbackDelegate callback, IntPtr context);
        [DllImport("HKU_SDK", CallingConvention = CallingConvention.Cdecl)]
        public static extern void FreeMemory(IntPtr ptr);
        [DllImport("HKU_SDK", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SetOutputCallback(DebugOutputDelegate callback, IntPtr context);

        // General Callbacks
        public delegate void UsersCallbackDelegate(IntPtr users, int length, IntPtr context);
        public delegate void GetUserCallbackDelegate(IntPtr username, int length, IntPtr context);
        public delegate void ConfigureProjectCallbackDelegate(bool IsSuccess, IntPtr context);

        // Account
        public delegate void LoginStatusCallbackDelegate(bool IsSuccess, string Id, IntPtr context);
        public delegate void LogoutCallbackDelegate(bool IsSuccess, IntPtr context);

        // Leaderboard
        public delegate void UploadLeaderboardScoreCallbackDelegate(bool IsSucces, int currentRank, IntPtr context);
        public delegate void LeaderboardCallbackDelegate(bool isSuccess, IntPtr context);
        public delegate void GetLeaderboardsForProjectCallbackDelegate(bool isSuccess, IntPtr context);

        // Debug
        public delegate void DebugOutputDelegate(string message, IntPtr context);
    }
}
