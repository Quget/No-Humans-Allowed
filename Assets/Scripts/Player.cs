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

    private IEnumerable<Lane> lanes;

    private Lane targetLane = null;

	// Start is called once before the first execution of Update after the MonoBehaviour is created
	void Start()
    {
		rigidbody2D = GetComponent<Rigidbody2D>();
        lanes = Object.FindObjectsOfType<Lane>();
		cameraThatFollows.StartFollow(transform);
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.S))
        {
            Dash(false);
        }

        if (Input.GetKeyDown(KeyCode.W))
        {
            Dash(true);
        }

        if(targetLane != null)
        {
            if(Mathf.Abs(transform.position.y - targetLane.transform.position.y) < 0.1f)
            {
                rigidbody2D.linearVelocityY = 0f;
			}
		}
    }

    private Lane FindLane(bool up)
    {
        return lanes.OrderBy(lane => up ? lane.transform.position.y - transform.position.y : transform.position.y - lane.transform.position.y)
                    .Where( lane => lane != targetLane)
					.FirstOrDefault(lane => up ? lane.transform.position.y > transform.position.y : lane.transform.position.y < transform.position.y);
    }

    private void FixedUpdate()
	{
        Movement();
	}

    private void Dash(bool up)
    {
        var direction = up ? Vector2.up : Vector2.down;
        targetLane = FindLane(up);
        
		if (targetLane != null)
        {
            rigidbody2D.AddForce(direction * forceMode, ForceMode2D.Impulse);
        }
    }


    private void Movement()
    {
        rigidbody2D.AddForce(Vector2.right * speed,ForceMode2D.Force);
	}
}
