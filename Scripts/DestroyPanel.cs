using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DestroyPanel : MonoBehaviour
{
    public UnityEvent onDestroyed = new UnityEvent();

    private void OnDestroy()
    {
        onDestroyed.Invoke();
    }
}
