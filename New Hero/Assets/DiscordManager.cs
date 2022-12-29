using Discord;
using UnityEngine;

public class DiscordManager : MonoBehaviour
{
    public long appID;
    [Space]
    public string details = "In the menus";
    public string state = "";
    [Space]
    public string largeImage = "logo";
    public string largeText = "New Hero();";

    private static bool instanceExists;

    private long time;
    public Discord.Discord discord;

    void Awake()
    {
        if (!instanceExists)
        {
            instanceExists = true;
            DontDestroyOnLoad(gameObject);
        }
        else if (FindObjectsOfType(GetType()).Length > 1)
        {
            Destroy(gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        discord = new Discord.Discord(appID, (System.UInt64)Discord.CreateFlags.NoRequireDiscord);
        time = System.DateTimeOffset.Now.ToUnixTimeMilliseconds();

        UpdateRPC();
    }

    // Update is called once per frame
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

    void OnApplicationQuit()
    {
        discord?.Dispose();
    }
}
