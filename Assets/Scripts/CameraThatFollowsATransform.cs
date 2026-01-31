using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraThatFollowsATransform : MonoBehaviour
{
	private Transform transformToFollow = null;

	[SerializeField]
	private float dampTime = 0.15f;

	private Vector3 velocity = Vector3.zero;

	private bool isFollowing = false;

	private float startZoom = 0f;

	public void Awake()
	{
		SetUp();
	}

	public void SetUp()
	{
		startZoom = Camera.main.orthographicSize;
	}

	public void SetOrthosize(float targetZoom, bool instant = false)
	{
		if (instant)
		{
			Camera.main.orthographicSize = targetZoom;
			return;
		}

		StartCoroutine(Zoom(targetZoom));
	}
	public void ReturnZoom()
	{
		StartCoroutine(Zoom(startZoom));
	}

	private IEnumerator Zoom(float targetZoom)
	{
		while (Camera.main.orthographicSize != targetZoom)
		{
			Camera.main.orthographicSize = Mathf.MoveTowards(Camera.main.orthographicSize, targetZoom, 5 * Time.deltaTime);
			yield return new WaitForEndOfFrame();
		}
	}


	public void StartFollow(Transform transformToFollow)
	{
		this.transformToFollow = transformToFollow;
		isFollowing = true;
	}
	public void StopFollow()
	{
		isFollowing = false;
	}

	public void Update()
	{
		if (isFollowing)
			SmoothDampFollow();
	}

	private void SmoothDampFollow()
	{
		if (transformToFollow == null)
			return;

		Vector3 point = Camera.main.WorldToViewportPoint(transformToFollow.position);
		Vector3 delta = transformToFollow.position - Camera.main.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, point.z));
		Vector3 destination = transform.position + delta;
		transform.position = Vector3.SmoothDamp(transform.position, destination, ref velocity, dampTime);
	}
}