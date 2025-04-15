using System;
using UnityEngine;

public class DoublePoints : MonoBehaviour
{
    public static event Action onCollected;

    void Update()
    {
        transform.localRotation = Quaternion.Euler(0, Time.time * 100f, 0);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            onCollected?.Invoke();
            SoundManager.PlaySound(SoundType.DOUBLEPOINTS);
            Destroy(gameObject);
        }
    }
}