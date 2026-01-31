using UnityEngine;

[CreateAssetMenu(fileName = "LevelScriptableObject", menuName = "Scriptable Objects/LevelScriptableObject")]
public class LevelScriptableObject : ScriptableObject
{
    [SerializeField]
    private GameObject levelPrefab;
    public GameObject LevelPrefab => levelPrefab;


}
