using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    [SerializeField]
    private int nextLevelIndex;

	public void LoadNextLevel()
    {
        SceneManager.LoadScene(nextLevelIndex);
    }

	public void ResetLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
	}
}
