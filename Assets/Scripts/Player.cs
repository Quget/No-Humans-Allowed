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
	private float equipDuration = 0.15f;

	[SerializeField]
    private Rigidbody2D rigidbody2D;

	[SerializeField]
	private SpriteRenderer spriteRenderer;

	[SerializeField]
	private Sprite idleSprite;

	[SerializeField]
	private Sprite dashSprite;

	[SerializeField]
	private Sprite equipSprite;

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

	public IEnumerator DelayedStart()
	{
		yield return new WaitForSeconds(1);
		cameraThatFollows.StartFollow(transform);
		cameraThatFollows.ReturnZoom();
	}

	// Update is called once per frame
	void Update()
    {
		if (currentLane != null && MasterLane.Instance.IsTransformPastLaneEnd(transform))
		{
			End();
			return;
		}

		if (currentLane != null && !currentLane.GetCrowdAtPosition(currentLane.GetLanePosition(transform.position)).Item1)
		{
			Die();
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
				SetIdleSprite();
			}
		}

        UpdateMasterLanePosition();
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
				StartCoroutine(DashCoroutine(direction));
			}
		}
	}

	public void End()
	{
		Debug.Log("You win!");
	}

	private void Die()
	{
		Debug.Log("You die");
	}

	private void UpdateMasterLanePosition()
	{
		MasterLane lane = MasterLane.Instance;

		if (lane != null && currentLane != null)
		{
			float progress = currentLane.GetLanePosition(transform.position);
			lane.SetCurrentProgress(progress);
        }
    }

    private void Movement()
    {
        if(currentLane != null)
        {
			rigidbody2D.AddForce(Vector2.right * speed, ForceMode2D.Force);
		}
	}

	private IEnumerator DashCoroutine(Vector2 direction)
	{
		SetSprite(equipSprite);
		yield return new WaitForSeconds(equipDuration);
		rigidbody2D.AddForce(direction * forceMode, ForceMode2D.Impulse);
		isMovingToLane = true;
		SetSprite(dashSprite);

		//Todo get race of group.
		var groupRace = targetLane.GetCrowdAtPosition(targetLane.GetLanePosition(transform.position));
        //try use mask
        if (!groupRace.Item1 || !selectedMasks.TryUseMask(groupRace.Item2.CrowdRace))
		{
			Die();
		}
	}

	private void SetIdleSprite()
	{
		SetSprite(idleSprite);
	}

	private void SetSprite(Sprite sprite)
	{
		if (spriteRenderer != null)
		{
			spriteRenderer.sprite = sprite;
		}
	}

	public void OnMaxMasksReached()
	{
		StartCoroutine(DelayedStart());
	}
}
