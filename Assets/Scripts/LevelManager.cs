using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    [SerializeField]
    private int nextLevelIndex;

    [SerializeField]
    public int MaxMaskCount = 8;

	public static LevelManager Instance;

    public void Awake()
    {
        Instance = this;
	}

	public void LoadNextLevel()
    {
        SceneManager.LoadScene(nextLevelIndex);
    }

	public void ResetLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
	}
}
