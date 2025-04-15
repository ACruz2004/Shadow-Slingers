using System;
using UnityEngine;

public class Instakill : MonoBehaviour
{
    public static event Action onCollected;
    public Gun gunScript;

    void Update()
    {
        transform.localRotation = Quaternion.Euler(270, Time.time * 100f, 0);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            onCollected?.Invoke();
            SoundManager.PlaySound(SoundType.INSTAKILL);
            Destroy(gameObject);
        }
    }
}