using UnityEngine;
using UnityEngine.Events;
[CreateAssetMenu]
public class GameAction : ScriptableObject
{
    /// <summary>
    /// Invoking raise will execute all the events added by any gameActionResponses tied to this Scriptable Object Gameaction
    /// </summary>
    public UnityAction raise;
    
    public void RaiseAction()
    {
        raise?.Invoke();
    }
}
