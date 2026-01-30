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

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
		rigidbody2D = GetComponent<Rigidbody2D>();
        cameraThatFollows.StartFollow(transform);
	}

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.S))
        {
            Dash();
        }
    }

	private void FixedUpdate()
	{
        Movement();
	}

    private void GetNearestLane(bool up)
    {

    }

    private void Dash()
    {
        rigidbody2D.AddForce(Vector2.down * forceMode, ForceMode2D.Impulse);
    }

	private void Movement()
    {
        rigidbody2D.AddForce(Vector2.right * speed,ForceMode2D.Force);
		//rigidbody2D.MovePosition(transform.position + ((Vector3.right * speed) * Time.fixedDeltaTime));
	}
}
