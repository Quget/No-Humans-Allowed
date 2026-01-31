using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    private float speed = 5.0f;

    [SerializeField]
    private float forceMode = 5f;

    [SerializeField]
    private Rigidbody2D rigidbody2D;

    [SerializeField]
    private CameraThatFollowsATransform cameraThatFollows;

	private SelectedMasks selectedMasks;

	private IEnumerable<Lane> lanes;

    private Lane targetLane = null;
    private Lane currentLane = null;
    private bool isMovingToLane = false;
	private const float laneSwitchThreshold = 0.1f;

	private void Awake()
	{
		rigidbody2D = GetComponent<Rigidbody2D>();
		lanes = Object.FindObjectsByType<Lane>(FindObjectsSortMode.None);
		selectedMasks = Object.FindFirstObjectByType<SelectedMasks>();
	}

	// Start is called once before the first execution of Update after the MonoBehaviour is created
	void Start()
    {

	}
	public IEnumerator DelayedStart()
	{
		yield return new WaitForSeconds(1);
		cameraThatFollows.StartFollow(transform);
		cameraThatFollows.ReturnZoom();
	}

	// Update is called once per frame
	void Update()
    {
		if (currentLane != null && currentLane.IsTransformPastLaneEnd(transform))
		{
			End();
			return;
		}

		PlayerInput();

		if (targetLane != null)
        {
            if(Mathf.Abs(transform.position.y - targetLane.transform.position.y) < laneSwitchThreshold)
            {
                rigidbody2D.linearVelocityY = 0f;
				currentLane = targetLane;
				transform.position = new Vector3(transform.position.x, targetLane.transform.position.y, transform.position.z);
				isMovingToLane = false;
			}
		}
    }
	private void FixedUpdate()
	{
		Movement();
	}

    private void PlayerInput()
    {
		if (Input.GetKeyDown(KeyCode.S))
		{
			Dash(false);
		}

		if (Input.GetKeyDown(KeyCode.W))
		{
			Dash(true);
		}
	}

	private Lane FindLane(bool up)
    {
        return lanes.OrderBy(lane => up ? lane.transform.position.y - transform.position.y : transform.position.y - lane.transform.position.y)
                    .Where( lane => lane != targetLane)
					.FirstOrDefault(lane => up ? lane.transform.position.y > transform.position.y : lane.transform.position.y < transform.position.y);
    }

    private void Dash(bool up)
    {
        if (!isMovingToLane)
        {
			var direction = up ? Vector2.up : Vector2.down;
			targetLane = FindLane(up);
			if ((targetLane != null && currentLane != targetLane) || (currentLane == null && targetLane != null))
			{
				rigidbody2D.AddForce(direction * forceMode, ForceMode2D.Impulse);
				isMovingToLane = true;

				//Todo get race of group.
				var groupRace = RacesEnumerator.FishFolk;
				//try use mask
				if (!selectedMasks.TryUseMask(groupRace))
				{
					Die();
				}
			}
		}
	}

	private void End()
	{
		Debug.Log("You win!");
	}

	private void Die()
	{
		Debug.Log("You die");
	}

    private void Movement()
    {
        if(currentLane != null)
        {
			rigidbody2D.AddForce(Vector2.right * speed, ForceMode2D.Force);
		}
	}
	public void OnMaxMasksReached()
	{
		StartCoroutine(DelayedStart());
	}
}
