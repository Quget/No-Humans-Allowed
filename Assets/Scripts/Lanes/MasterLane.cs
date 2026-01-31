using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;

public class MasterLane : MonoBehaviour
{
	public UnityEvent IsAtEnd;
	public float CurrentProgress = 0f;
	
	public float LaneLength = 20f;

	public static MasterLane Instance;

	public bool IsAutoPlaying = false;

	private Crowd[] allCrowds;

	[SerializeField]
	private float ReplaySpeed = 10;

    private void Awake()
    {
        Instance = this;
        allCrowds = FindObjectsByType<Crowd>(FindObjectsSortMode.None);

    }

    public float GetLanePosition(Vector2 worldPosition)
    {
		return worldPosition.x;
    }

    public void SetCurrentProgress(float progress)
	{
		if(!IsAutoPlaying)
			CurrentProgress = progress;
	}

	public void StartAutoPlay()
	{
		IsAutoPlaying = true;

        // set all crowds to previewing
        foreach (Crowd crowd in allCrowds)
			crowd.IsPreviewing = true;
	}

    private void Update()
    {
		if (IsAutoPlaying)
		{
			// 2 needs to be replaced by player speed
			CurrentProgress += ReplaySpeed * Time.deltaTime;
		}

		if(CurrentProgress >= LaneLength)
		{
			if (IsAutoPlaying) // reset crowds and progress
			{
				IsAutoPlaying = false;
				
				for (int i = 0; i < allCrowds.Length; i++)
				{
					allCrowds[i].IsPreviewing = false;
					allCrowds[i].ResetCrowd();
					allCrowds[i].StopWalking();
                }

				CurrentProgress = 0;
            }
            IsAtEnd.Invoke();
        }
    }

    public bool IsTransformPastLaneEnd(Transform otherTransform)
    {
        return otherTransform.position.x > transform.position.x + LaneLength;
    }

    private void OnDrawGizmos()
	{
		Gizmos.color = Color.green;
		Gizmos.DrawLine(transform.position, transform.position + transform.right * LaneLength);

        // draw some notches every meter to make it more visible
		for(int i = 0; i <= LaneLength; i++)
			Gizmos.DrawSphere(transform.position + transform.right * i, 0.1f);

		Gizmos.color = Color.red;
		Gizmos.DrawCube(transform.position + transform.right * CurrentProgress, new(0.4f, 0.8f, 0.1f));

    }
}
