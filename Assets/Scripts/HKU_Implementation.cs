using NaughtyAttributes;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using static HKU.HKUApiWrapper;

public class HKU_Implementation : MonoBehaviour
{
    [SerializeField] private int score = 0;
    [SerializeField] private string leaderBoardID = "c7f8b07f-cf97-4ad3-b669-8f59882a65de";

    private GCHandle gch;
    private IntPtr contextPtr = IntPtr.Zero;
    private bool isConfigured = false;
    private string projectID = "e0241dd7-1fa1-48bd-a84a-ae5efee1a8ed";

    [Button]
    public void Initialize()
    {
        // Context for callbacks
        gch = GCHandle.Alloc(this);
        contextPtr = GCHandle.ToIntPtr(gch);

        // Set debug output
#if DEVELOPMENT_BUILD
        DebugOutputDelegate myDebugOutputDelegate = (message, context) =>
        {
            Debug.Log(message);
        };
        SetOutputCallback(myDebugOutputDelegate, IntPtr.Zero);
#endif

        // Register project
        ConfigureProjectCallbackDelegate myConfigureProjectCallbackDelegate = (bool IsSuccess, IntPtr context) =>
        {
            if (IsSuccess)
            {
                Debug.Log("Project configured successfully");
                isConfigured = true;
            }
            else
            {
                Debug.Log("Project configuration failed");
            }
        };

        ConfigureProject(projectID, myConfigureProjectCallbackDelegate, IntPtr.Zero);
    }


    [Button]
    public void StartLoginProcess()
    {
        if (!isConfigured)
        {
            Debug.LogError("Project is not configured. Please configure the project before starting the login process.");
            return;
        }

        // Open login page
        OpenLoginPage();

        // Start polling
        StartPollingProcess();
    }

    private void StartPollingProcess()
    {
        LoginStatusCallbackDelegate myLoginStatusCallbackDelegate = (bool IsSuccess, string userId, IntPtr context) =>
        {
            if (IsSuccess)
            {
                Debug.Log("User logged in successfully with ID: " + userId);
            }
            else
            {
                Debug.Log("User login failed");
            }
        };
        StartPolling(myLoginStatusCallbackDelegate, contextPtr);
    }

    public void StopPollingProcess()
    {
        CancelPolling();
        Debug.Log("Polling stopped");
    }

    [Button]
    public void ActualyUpdateScoreWithInspectorValues()
    {
        UploadScore(leaderBoardID, score);
    }


    public void UploadScore(string leaderboardId, int score)
    {
        UploadLeaderboardScoreCallbackDelegate myUploadScoreCallbackDelegate = (bool IsSuccess, int currentRank, IntPtr context) =>
        {
            if (IsSuccess)
            {
                Debug.Log("Score uploaded successfully. Current Rank: " + currentRank);
            }
            else
            {
                Debug.Log("Failed to upload score");
            }
        };
        UploadLeaderboardScore(leaderboardId, score, myUploadScoreCallbackDelegate, contextPtr);
    }

    [Button]
    public void FetchDebugLeaderBoard()
    {
        FetchLeaderboard(leaderBoardID, 5, GetEntryOptions.Highest);
    }

    public void FetchLeaderboard(string leaderboardId, int amount, GetEntryOptions option)
    {
        IntPtr outArray = IntPtr.Zero;
        LeaderboardCallbackDelegate myLeaderboardCallbackDelegate = (bool isSuccess, IntPtr context) =>
        {
            if (isSuccess)
            {
                HKU.LeaderboardEntry[] entries = MarshalPtrToLeaderboardEntryArray(outArray);
                foreach (var item in entries)
                {
                    Debug.Log(item.PlayerID + " has scored " + item.Score + " position: "+ item.Rank);
                }
                //Debug.Log("Leaderboard fetched successfully");
            }
            else
            {
                Debug.Log("Failed to fetch leaderboard");
            }
        };

        GetLeaderboard(leaderboardId, ref outArray, amount, option, myLeaderboardCallbackDelegate, contextPtr);


        // Don't forget to free the memory
        //FreeMemory(outArray);
    }

    private HKU.LeaderboardEntry[] MarshalPtrToLeaderboardEntryArray(IntPtr ptr)
    {
        int count = 0;
        // First count the number of entries in the array
        while (Marshal.ReadIntPtr(ptr, count * IntPtr.Size) != IntPtr.Zero)
        {
            count++;
        }

        var result = new HKU.LeaderboardEntry[count / 3];
        for (int i = 0; i < count; i += 3)
        {
            IntPtr playerIdPtr = Marshal.ReadIntPtr(ptr, i * IntPtr.Size);
            IntPtr scorePtr = Marshal.ReadIntPtr(ptr, (i + 1) * IntPtr.Size);
            IntPtr rankPtr = Marshal.ReadIntPtr(ptr, (i + 2) * IntPtr.Size);

            result[i / 3] = new HKU.LeaderboardEntry
            {
                PlayerID = Marshal.PtrToStringAnsi(playerIdPtr),
                Score = int.Parse(Marshal.PtrToStringAnsi(scorePtr)),
                Rank = int.Parse(Marshal.PtrToStringAnsi(rankPtr))
            };
        }

        return result;
    }

    private void OnDestroy()
    {
        if (gch.IsAllocated)
        {
            gch.Free();
        }
    }
}

