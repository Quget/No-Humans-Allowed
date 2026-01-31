using Unity.VisualScripting;
using UnityEngine;

public class Lane : MonoBehaviour
{
	[SerializeField]
	private float laneLength = 10;


	private void OnDrawGizmos()
	{
		Gizmos.color = Color.blue;

		Gizmos.DrawLine(transform.position, transform.position + transform.right * laneLength);

        // draw little spheres every meter to make it more visible
		for(int i = 0; i <= laneLength; i++)
		{
			Gizmos.DrawSphere(transform.position + transform.right * i, 0.1f);
		}
    }

	public bool IsTransformPastLaneEnd(Transform otherTransform)
	{
		return otherTransform.position.x > transform.position.x + laneLength;
	}
}
