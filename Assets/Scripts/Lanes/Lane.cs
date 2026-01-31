using System.Collections.Generic;
using UnityEngine;

public class Lane : MonoBehaviour
{
	public List<Crowd> CrowdsAssignedToLane = new();
	public List<OccupationZone> Occupation = new();

	[SerializeField]
	private float laneLength = 10;


	private void Awake()
	{
		if (CrowdsAssignedToLane.Count <= 0)
			Debug.LogWarning($"YOU FORGOT TO ASSIGN ANY CROWDS TO THIS LANE!!!!! '{gameObject.name}' 😡😡😡😡", gameObject);

		// add each crowd to this lane's list
		foreach (Crowd crowd in CrowdsAssignedToLane)
			Occupation.Add(new(0, 0, crowd));
	}

	private void Update()
	{
		if (CrowdsAssignedToLane.Count <= 0)
			return; // so lonely without crowds :(

		// updates the occupation based on the crowds currently assigned to this lane :D
		foreach(var occ in Occupation)
			UpdateOccupation(occ);
	}

	private void UpdateOccupation(OccupationZone occ)
	{
		// get the position of the crowd along the lane
		float lanePosition = GetLanePosition(occ.crowd.transform.position);
		occ.start = lanePosition - occ.crowd.CrowdLength / 2;
		occ.end = lanePosition + occ.crowd.CrowdLength / 2;


        // if crowd is at end stop it from going past lane length
		if (occ.end > laneLength)
			occ.crowd.StopWalking();
    }

    /// <summary>
    /// Finds out if a given position along the lane is currently occupied by a crowd
    /// </summary>
    /// <param name="position"></param>
    /// <returns></returns>
    public (bool, Crowd) GetCrowdAtPosition(float position)
	{
		foreach(var occ in Occupation)
		{
			if (position >= occ.start && position <= occ.end)
				return (true, occ.crowd);
		}
		return (false, null);
	}

	

    /// <summary>
    /// gets the position along the lane (0 to laneLength) for a given world position
    /// </summary>
    public float GetLanePosition(Vector2 position)
	{
		Vector2 laneRight = transform.right;
		Vector2 laneOrigin = transform.position;
		Vector2 toPosition = position - laneOrigin;
		float projectedLength = Vector2.Dot(toPosition, laneRight.normalized);
		
		return projectedLength;
	}

	private void OnDrawGizmos()
	{
		Gizmos.color = Color.blue;

		Gizmos.DrawLine(transform.position, transform.position + transform.right * laneLength);

		// draw little spheres every meter to make it more visible
		for(int i = 0; i <= laneLength; i++)
			Gizmos.DrawSphere(transform.position + transform.right * i, 0.1f);


		// this is to visualize the currently occupied areas for debugging. 
		Vector3 littleZOffset = new Vector3(0, 0, 0.01f);

		foreach(var occ in Occupation)
		{
			Gizmos.color = Color.green;
			Gizmos.DrawCube(transform.position + littleZOffset + transform.right * occ.start, Vector3.one * 0.1f);

			Gizmos.color = Color.yellowGreen;
			Gizmos.DrawLine(transform.position + littleZOffset + transform.right * occ.start, transform.position +littleZOffset + transform.right * occ.end);

			Gizmos.color = Color.orange;
			Gizmos.DrawCube(transform.position + littleZOffset + transform.right * occ.end, Vector3.one * 0.1f);
		}
	}

	public bool IsTransformPastLaneEnd(Transform otherTransform)
	{
		return otherTransform.position.x > transform.position.x + laneLength;
	}
}

public class OccupationZone
{
	public float start;
	public float end;
	public Crowd crowd;
	public OccupationZone(float start, float end, Crowd crowd)
	{
		this.start = start;
		this.end = end;
		this.crowd = crowd;
	}
}