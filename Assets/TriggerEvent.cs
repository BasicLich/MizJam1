using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TriggerEvent : MonoBehaviour
{
    public UnityEvent myEvent;
    public bool destroyOnEnter = false;
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Player>() != null)
        {
            myEvent.Invoke();
            if (destroyOnEnter)
            {
                Destroy(gameObject);
            }
        }
    }
}
