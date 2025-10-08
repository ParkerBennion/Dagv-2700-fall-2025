using UnityEngine;


public class SaveButtons : MonoBehaviour
{
    public void SaveAll() => GameDataManager.Instance?.SaveAll();
    public void SavePlayer() => GameDataManager.Instance?.SavePlayerData();
    public void SaveCollectibles() => GameDataManager.Instance?.SaveCollectibles();
    public void ResetCollectibles() => GameDataManager.Instance?.ResetAllCollectibles();
    public void loadCollectibles() => GameDataManager.Instance?.LoadCollectibles();
}