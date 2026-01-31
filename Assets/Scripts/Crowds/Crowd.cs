    using UnityEngine;

public class Crowd : MonoBehaviour
{
	public float CrowdLength = 1.0f;
	public bool IsWalking = false;
    public bool IsPreviewing = false;

	public RacesEnumerator CrowdRace;
	
	[SerializeField]
	private float speed = 0.5f;

	public static float PreviewSpeedMultiplier = 5;
	
    private bool HasWalked = false;

	private Vector3 initialPosition;


    private void Awake()
    {
        initialPosition = transform.position;
    }

    public void StartWalking()
	{
        if (!HasWalked)
        {
            IsWalking = true;
            HasWalked = true;
        }
	}

	public void StopWalking()
	{
		IsWalking = false;
    }

    public void ResetCrowd()
    {
        transform.position = initialPosition;
		IsWalking = false;
        HasWalked = false;
    }

    // translate crowd over x axis
    private void Update()
	{
		float speeeeed = IsPreviewing ? this.speed * PreviewSpeedMultiplier : this.speed;

        // move crowd to the right over time
        if (IsWalking)
			transform.Translate(speeeeed * Time.deltaTime * Vector3.right);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(transform.position + (Vector3.left  * CrowdLength /2), transform.position + (Vector3.right * CrowdLength /2));
    }
}
