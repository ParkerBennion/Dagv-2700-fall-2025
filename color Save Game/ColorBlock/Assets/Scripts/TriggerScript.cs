using System;
using UnityEngine;
using UnityEngine.Events;

public class TriggerScript : MonoBehaviour
{
    public UnityEvent enterEvent;
    public bool requireTag;
    public string tagName;
    
    private void OnTriggerEnter(Collider other)
    {
        if(requireTag && !other.CompareTag(tagName))
            return;
        enterEvent.Invoke();
    }
}