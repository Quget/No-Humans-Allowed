using UnityEngine;

public class CameraStart : MonoBehaviour
{
	[SerializeField]
	private float orthographicSize = 10f;

	private CameraThatFollowsATransform cameraThatFollows;

	private void Awake()
	{
		cameraThatFollows = Object.FindFirstObjectByType<CameraThatFollowsATransform>();
	}

	// Start is called once before the first execution of Update after the MonoBehaviour is created
	void Start()
    {
		cameraThatFollows.StartFollow(transform);
		cameraThatFollows.SetOrthosize(orthographicSize, false);
	}
}
