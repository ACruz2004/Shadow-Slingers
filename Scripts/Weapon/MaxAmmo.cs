using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaxAmmo : MonoBehaviour
{
    public static event Action onCollected;
    public AmmoScript ammoScript;

    void Start()
    {
        if (ammoScript == null)
        {
            ammoScript = FindObjectOfType<AmmoScript>();
        }
    }
    // Update is called once per frame
    void Update()
    {
        transform.localRotation = Quaternion.Euler(270, Time.time * 100f, 0);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            onCollected?.Invoke();
            SoundManager.PlaySound(SoundType.MAXAMMO);
            ammoScript.maxAmmo(32);
            Destroy(gameObject);
        }
    }
}
