using System.Collections.Generic;
using UnityEngine;

public class CrowdTrigger : MonoBehaviour
{
	[SerializeField]
	private List<Crowd> crowdsToTrigger = new();

	private MasterLane masterLane;

	

	private void Awake()
	{
		if (crowdsToTrigger.Count <= 0)
			Debug.LogWarning($"EY MAAT! YOU FORGOT TO ASSIGN CROWDS TO THIS TRIGGER!!! '{gameObject.name}' 😡😡😡😡", gameObject);
	}

    private void Start()
    {
        masterLane = MasterLane.Instance;
    }

    public void Update()
    {
        // check if master lane's current progress is greater than this trigger's position along the master lane
		float triggerPosition = masterLane.GetLanePosition(transform.position);
		if (masterLane.CurrentProgress >= triggerPosition)
			TriggerCrowdsToWalk();
    }

    public void TriggerCrowdsToWalk()
	{
		foreach(var crowd in crowdsToTrigger)
			crowd.StartWalking();
    }

    private void OnDrawGizmos()
	{
        // draw a cube to represent the trigger
		Gizmos.color = Color.yellow;
		Gizmos.DrawCube(transform.position, new Vector3(0.5f, 0.5f, 0.5f));
		Gizmos.DrawLine(transform.position, transform.position + Vector3.up * 10f);

    }
}
