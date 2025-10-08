using UnityEngine;
using TMPro;
using System.Collections.Generic;


public class PlayerProfileLoader : MonoBehaviour
{
    [SerializeField] private TMP_Dropdown profileDropdown;


    private readonly List<string> _names = new();


    private void Start()
    {
        RefreshList();
        profileDropdown.onValueChanged.AddListener(OnSelected);
    }


    public void RefreshList()
    {
        _names.Clear();
        profileDropdown.ClearOptions();
        var profiles = Resources.LoadAll<DataBlockSO>("Profiles");
        foreach (var p in profiles)
        {
            _names.Add(p.playerName);
        }
        profileDropdown.AddOptions(_names);
        if (_names.Count > 0) OnSelected(0);
    }


    private void OnSelected(int index)
    {
        var profiles = Resources.LoadAll<DataBlockSO>("Profiles");
        if (index < 0 || index >= profiles.Length) return;
        var selected = profiles[index];
        GameDataManager.Instance.SetActiveProfile(selected);


// Optional: notify UI via the profile's own event
        selected.NotifyChanged();
    }
}