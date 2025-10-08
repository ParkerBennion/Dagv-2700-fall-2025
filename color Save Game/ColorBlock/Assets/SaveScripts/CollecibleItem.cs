using UnityEngine;
using System.Collections.Generic;

[DisallowMultipleComponent]
public class CollectibleItem : MonoBehaviour
{
    // this script is nice as every object will manage itself, this script talks to gameDataManager and CollectibleRegisty to add itself check itself and hideiself if its collected.
    // Each collectible has a permanent, unique ID string.
    // This is what the CollectibleRegistry uses as the key when saving/loading.
    // Using GUIDs prevents name collisions between different scene objects.
    [SerializeField] private string uniqueId = System.Guid.NewGuid().ToString();

    // A static list that tracks every CollectibleItem currently active in the scene.
    // GameDataManager uses this to refresh visibility when resetting.
    public static readonly List<CollectibleItem> All = new();

    private void Awake()
    {
        // Register this item in the global list (for reset visibility etc.)
        if (!All.Contains(this)) All.Add(this);

        // Immediately hide/show based on whether this item has already been collected.
        ApplyVisibility();
    }

    private void OnDestroy()
    {
        // Unregister from the global list when destroyed.
        All.Remove(this);
    }

    // Called when the player collects this item (for example, by trigger or interaction).
    public void Collect()
    {
        // Record in the registry that this ID is now collected.
        CollectibleRegistry.SetCollected(uniqueId, true);

        // Update visibility — usually hides the object so it disappears from the scene.
        ApplyVisibility();

        // NOTE: We don't automatically save here.
        // The player or game system must explicitly call GameDataManager.SaveCollectibles()
        // or SaveAll() to persist the change to disk.
    }

    // Resets this item’s collected state (used by reset buttons or debug commands).
    public void ResetThis()
    {
        CollectibleRegistry.SetCollected(uniqueId, false);
        ApplyVisibility();
    }

    // Updates visibility in the scene to match collected state.
    public void ApplyVisibility()
    {
        bool isCollected = CollectibleRegistry.IsCollected(uniqueId);
        gameObject.SetActive(!isCollected); // Hide if collected, show if not
    }
}
