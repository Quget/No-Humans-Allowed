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

	[SerializeField]
	private AudioSource dieSound;

	private SelectedMasks selectedMasks;

    private bool isDead;

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
        if (isDead) return;

        UpdateMasterLanePosition();

        if (currentLane != null && MasterLane.Instance.IsTransformPastLaneEnd(transform))
        {
            End();
            return;
        }

        // FIX: Only check for crowd death if we AREN'T currently dashing/switching
        if (!isMovingToLane && currentLane != null)
        {
            var crowdInfo = currentLane.GetCrowdAtPosition(currentLane.GetLanePosition(transform.position));
            if (!crowdInfo.Item1)
            {
                Die();
                return;
            }
        }

        PlayerInput();

        if (targetLane != null && isMovingToLane)
        {
			//Aggressive check to see if we've reached the target lane
			var currentY = currentLane == null ? transform.position.y : currentLane.transform.position.y;
			bool goingLaneUp = targetLane.transform.position.y > currentY;
            if((goingLaneUp && transform.position.y >= targetLane.transform.position.y - laneSwitchThreshold) ||
               (!goingLaneUp && transform.position.y <= targetLane.transform.position.y + laneSwitchThreshold))
			{
                rigidbody2D.linearVelocityY = 0f;
                currentLane = targetLane; // Successfully switched!
                transform.position = new Vector3(transform.position.x, targetLane.transform.position.y, transform.position.z);
                isMovingToLane = false;
                SetIdleSprite();
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
        return lanes.OrderBy(lane => Vector3.Distance(transform.position, lane.transform.position))
                    .ThenBy(lane => up ? lane.transform.position.y - transform.position.y : transform.position.y - lane.transform.position.y)
                    .Where( lane => lane != targetLane)
					.FirstOrDefault(lane => up ? lane.transform.position.y > transform.position.y : lane.transform.position.y < transform.position.y);
    }

    private void Dash(bool up)
    {
        if (!isMovingToLane)
        {
            targetLane = FindLane(up);
            if (targetLane != null)
            {
                isMovingToLane = true; 
                var direction = up ? Vector2.up : Vector2.down;
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
        if (isDead) return;
        isDead = true;
        Debug.Log("You die");
        rigidbody2D.linearVelocity = Vector2.zero; // Stop moving
        dieSound?.Play();
        // Maybe trigger a reload or game over UI here

        // make the player spin
        rigidbody2D.freezeRotation = false;
        rigidbody2D.AddTorque(10f, ForceMode2D.Impulse);
        rigidbody2D.AddForce(Vector2.up * 5f, ForceMode2D.Impulse);

        Invoke(nameof(StopDeathSpin), 1f);
    }

    private void StopDeathSpin()
    {
        
        rigidbody2D.linearDamping = 10f;
        rigidbody2D.angularDamping = 10f;
    }

    private void UpdateMasterLanePosition()
	{
		MasterLane lane = MasterLane.Instance;

		float progress = lane.GetLanePosition(transform.position);
		lane.SetCurrentProgress(progress);

		if (lane != null && currentLane != null)
		{
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

        // Clear vertical velocity
        rigidbody2D.linearVelocity = new Vector2(rigidbody2D.linearVelocity.x, 0);
        rigidbody2D.AddForce(direction * forceMode, ForceMode2D.Impulse);
        SetSprite(dashSprite);
        
        // CHECK MASK HERE
        float progressAtTarget = targetLane.GetLanePosition(transform.position);
        var groupRace = targetLane.GetCrowdAtPosition(progressAtTarget);

        // If there's no crowd to move into, OR we fail to use the mask, we die.
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
