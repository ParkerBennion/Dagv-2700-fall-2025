using UnityEngine;


[CreateAssetMenu(fileName = "NewPlayerData", menuName = "Save System/Player Data")]
public class DataBlockSO : ScriptableObject
{
    // this is an object and can be created in the editor.
    //just holds data
    [Header("Events")] public GameAction onValueChanged;


    [Header("Basic Info")]
    public string playerName = "Player";


    [Header("Stats")]
    public int coins = 0;


    public void NotifyChanged() => onValueChanged?.RaiseAction();


    public void AddCoins(int amount)
    {
        coins += amount;
        NotifyChanged();
    }
}