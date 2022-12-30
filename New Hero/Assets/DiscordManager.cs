using Discord;
using UnityEngine;

public class DiscordManager : MonoBehaviour
{
    public long appID;
    private State currentState;
    public static DiscordManager Instance;
    [Space]
    private string details_menu = "In the menus";
    private string state_menu = "";
    private string details_playing = "In game";
    private string state_playing = "Walking around the world";
    private string details_pause = "In game";
    private string state_pause = "Paused";
    [Space]
    public string largeImage = "logo";
    public string largeText = "New Hero();";

    private static bool instanceExists;

    private long time;
    public Discord.Discord discord;

    public enum State
    {
        MENU = 0,
        PLAYING = 1,
        PAUSE = 2,
    }

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
        discord = new Discord.Discord(appID, (System.UInt64)Discord.CreateFlags.NoRequireDiscord);
        time = System.DateTimeOffset.Now.ToUnixTimeMilliseconds();

        UpdateRPC();
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
        try
        {
            var activityManager = discord.GetActivityManager();
            string details, state;
            switch (currentState)
            {
                case State.MENU:
                    details = details_menu;
                    state = state_menu;
                    break;
                case State.PLAYING:
                    details = details_playing;
                    state = state_playing;
                    break;
                case State.PAUSE:
                    details = details_pause;
                    state = state_pause;
                    break;
                default:
                    details = "404";
                    state = "404";
                    break;
            }

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
                }
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
