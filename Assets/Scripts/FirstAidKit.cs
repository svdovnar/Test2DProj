using UnityEngine;

public class FirstAidKit : MonoBehaviour
{
    [SerializeField] private AudioClip healSound; 
    private AudioSource audioSource;
    private bool isCollected = false;
    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !isCollected)
        {
            isCollected = true;
            audioSource.PlayOneShot(healSound);
            GameSession.instance.AddHearts();
            Destroy(gameObject, healSound.length);
        }
    }
}
