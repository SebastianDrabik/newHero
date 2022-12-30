using Discord;
using System.Collections.Generic;
using UnityEngine;

public class DiscordManager : MonoBehaviour
{
    public long appID;
    private State currentState;
    public static DiscordManager Instance;
    [Space]
    public string largeImage = "logo";
    public string largeText = "New Hero();";

    private static bool instanceExists;
    private static bool discordRunning;

    private long time;
    public Discord.Discord discord;

    public enum State
    {
        MENU = 0,
        PLAYING = 1,
        PAUSE = 2,
    }

    private Dictionary<State, string[]> statusMessages = new Dictionary<State, string[]>()
    {
        { State.MENU, new string[2] {"In menu", ""}},
        { State.PLAYING, new string[2] { "In game", "Walking around the world"}},
        { State.PAUSE, new string[2] { "In game", "Paused"}}
    };
    void Awake()
    {
        if (!instanceExists)
        {
            Instance = this;
            instanceExists = true;
            DontDestroyOnLoad(gameObject);
        }
        else if (FindObjectsOfType(GetType()).Length > 1)
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        try
        {
            discord = new Discord.Discord(appID, (System.UInt64) Discord.CreateFlags.NoRequireDiscord);
            time = System.DateTimeOffset.Now.ToUnixTimeMilliseconds();
            UpdateRPC();

            discordRunning = true;
        }
        catch
        {
            discordRunning = false;
        }

    }

    void Update()
    {
        try
        {
            discord.RunCallbacks();
        }
        catch
        {
            Destroy(gameObject);
        }
    }

    void LateUpdate()
    {
        UpdateRPC();
    }

    void UpdateRPC()
    {
        if (!discordRunning) return;
        try
        {
            var activityManager = discord.GetActivityManager();
            string details, state;
            details = statusMessages[currentState][0];
            state = statusMessages[currentState][1];

            var activity = new Discord.Activity
            {
                Details = details,
                State = state,
                Assets =
                {
                    LargeImage = largeImage,
                    LargeText = largeText,
                },
                Timestamps = {
                    Start = time,
                },
            };
            activityManager.UpdateActivity(activity, (res) =>
            {
                if (res != Discord.Result.Ok)
                    Debug.LogWarning("Failed to connect to discrd");
            });
        }
        catch
        {
            Destroy(gameObject);
        }
    }

    public void SetPlaying(State state)
    {
        Debug.Log("Changed discord status");
        currentState = state;
    }

    void OnApplicationQuit()
    {
        discord?.Dispose();
    }
}
