using System.IO;
using UnityEngine;


public class GameDataManager : MonoBehaviour
{
    // this is a static item and only one can exist in a scene.
    public static GameDataManager Instance { get; private set; }

    //in unity formating
    [Header("Active Profile")]
    public DataBlockSO activeProfile;

    //in unity formating
    [Header("Optional: hook this to auto-save")]
    public GameAction saveGameAction;

    //the folder where unity saves things (declared later)
    private string playerDataFolder;
    private string collectiblesFilePath; // unified store for collectible booleans


    private void Awake()
    {
        //this ensures that only one instance of this object exists at a time to prevent errors.
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
        
        //dont destroy makes this item persists across scenes.
        DontDestroyOnLoad(gameObject);

        //identifies and adds a player data folder in your computers designated place to save unity data.
        string root = Application.persistentDataPath;
        playerDataFolder = Path.Combine(root, "PlayerData");
        Directory.CreateDirectory(playerDataFolder);

        // makes a json file to save the collectables data
        collectiblesFilePath = Path.Combine(root, "collectibles.json");

        //loads collectibles based on file.
        LoadCollectibles();
        
        foreach (var item in CollectibleItem.All)
            item.ApplyVisibility();
        
        //optional subscription to a game action if you want to call an action to save.
        if (saveGameAction != null)
            saveGameAction.raise += SaveAll; // anyone can RaiseAction() to save
    }


// -------- Profiles --------
    public void SetActiveProfile(DataBlockSO profile)
    {
        Debug.LogWarning("set active profile");
        activeProfile = profile;
        LoadPlayerData();
    }

    // this is a property when you call playerjsonpath it will run this to get the file path of your active profile... ask chat to explain if you are having trouble reading this.
    string PlayerJsonPath => activeProfile == null
        ? null
        : Path.Combine(playerDataFolder, $"{activeProfile.playerName}.json");


    public void SavePlayerData()
    {
        //this method checks, then saves the profile when called. it also prints the path it saves to if you were curious where this is on your hard drive.
        if (activeProfile == null) { Debug.LogWarning("No active profile set."); return; }
        string path = PlayerJsonPath;
        string json = JsonUtility.ToJson(activeProfile, prettyPrint: true);
        File.WriteAllText(path, json);
        Debug.Log($"Player data saved: {path}");
    }


    public void LoadPlayerData()
    {
        // loads profile look in documentation to see what the json utility methods do... or ask chat.
        if (activeProfile == null) { Debug.LogWarning("No active profile set."); return; }
        string path = PlayerJsonPath;
        if (!File.Exists(path)) { Debug.Log("No player JSON yet. Using asset defaults."); return; }
        string json = File.ReadAllText(path);
        JsonUtility.FromJsonOverwrite(json, activeProfile);
        Debug.Log($"Player data loaded: {path}");
        activeProfile.NotifyChanged();
    }


// -------- Collectibles --------
    public void SaveCollectibles()
    //tells the collectibleregistry script where to save and save it.
    {
        CollectibleRegistry.SaveTo(collectiblesFilePath);
    }
    
    

    public void LoadCollectibles()
    {
        CollectibleRegistry.LoadFrom(collectiblesFilePath);
        
        // this only works because all collectable items add themselves to a static list (see CollecibleItem line 15) all items in this list call the apply visibility method.
        foreach (var item in CollectibleItem.All)
            item.ApplyVisibility();
    }


    public void ResetAllCollectibles()
    {
        CollectibleRegistry.ClearAll();
        // Optionally refresh in-scene items now:
        foreach (var item in CollectibleItem.All)
            item.ApplyVisibility();
    }


// -------- One-click convenience --------
    public void SaveAll()
    {
        SavePlayerData();
        SaveCollectibles();
        Debug.Log("All data saved (player + collectibles).");
    }
}