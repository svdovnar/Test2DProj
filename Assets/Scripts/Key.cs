using System;
using UnityEngine;

public class Key : MonoBehaviour
{
    [SerializeField] private string keyID;
    [SerializeField] private AudioClip keyPickupSound;
    
    private AudioSource audioSource;
    private bool isCollected = false;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (GameSession.instance.IsKeyCollected(keyID))
        {
            Destroy(gameObject);
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !isCollected)
        {
            isCollected = true;
            audioSource.PlayOneShot(keyPickupSound);
            GameSession.instance.CollectKeys(keyID);
            Destroy(gameObject, keyPickupSound.length);
        }
    }
}
