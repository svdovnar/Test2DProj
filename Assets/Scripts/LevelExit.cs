using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelExit : MonoBehaviour
{
    [SerializeField] private float reloadTime = 1f;
    [SerializeField] private int keysInNextLevel = 3;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (GameSession.instance.AreAllKeysCollected())
            {
                StartCoroutine(LoadNextLevel());
            }
            else
            {
                return;
            }
        }
    }
    private IEnumerator LoadNextLevel()
    {
        yield return new WaitForSecondsRealtime(reloadTime);
        var currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        var nextSceneIndex = currentSceneIndex + 1;
        if(nextSceneIndex == SceneManager.sceneCountInBuildSettings)
        {
            nextSceneIndex = 0;
        }
        GameSession.instance.ResetKeyCount(keysInNextLevel);
        SceneManager.LoadScene(nextSceneIndex);
    }
}
