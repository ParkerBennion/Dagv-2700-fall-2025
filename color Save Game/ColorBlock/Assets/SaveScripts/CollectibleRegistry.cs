using System.Collections.Generic;
using System.IO;
using UnityEngine;

public static class CollectibleRegistry
{
    // this does not exist in the unity scene but is called by other scripts and manages the library of collectables (yes collectible is spelled wrong, but it would take too long to rename it all)
    // Dictionary that stores collectible states.
    // Key = unique collectible ID, Value = whether it’s been collected.
    private static readonly Dictionary<string, bool> _state = new();

    // Read-only access so other scripts can inspect (but not modify) the dictionary directly.
    public static IReadOnlyDictionary<string, bool> State => _state;

    // Helper class to make the dictionary serializable by JsonUtility.
    [System.Serializable]
    private class Wrapper 
    { 
        public List<string> keys = new(); 
        public List<bool> values = new(); 
    }

    // -------------------- PUBLIC API --------------------

    // Check if a given collectible has already been collected.
    public static bool IsCollected(string id) =>
        _state.TryGetValue(id, out var v) && v;

    // Mark a collectible as collected (true) or reset it (false).
    public static void SetCollected(string id, bool value) =>
        _state[id] = value;

    // Clear all collectible data in memory.
    public static void ClearAll() => _state.Clear();

    // Save the current collectible states to a JSON file.
    public static void SaveTo(string path)
    {
        var w = new Wrapper();
        foreach (var kv in _state)
        {
            w.keys.Add(kv.Key);
            w.values.Add(kv.Value);
        }

        string json = JsonUtility.ToJson(w, true);
        File.WriteAllText(path, json);
        Debug.Log($"Collectibles saved: {path}");
    }

    // Load collectible states from a JSON file.
    public static void LoadFrom(string path)
    {
        _state.Clear();

        if (!File.Exists(path))
        {
            Debug.Log("No collectibles file yet."); 
            return;
        }

        string json = File.ReadAllText(path);
        var w = JsonUtility.FromJson<Wrapper>(json);

        // Safety check: keys and values should align.
        if (w == null || w.keys.Count != w.values.Count)
        {
            Debug.LogWarning("Bad collectibles JSON."); 
            return;
        }

        // Rebuild the dictionary.
        for (int i = 0; i < w.keys.Count; i++)
            _state[w.keys[i]] = w.values[i];

        Debug.Log($"Collectibles loaded: {path}");
    }
}
