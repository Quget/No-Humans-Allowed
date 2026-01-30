using UnityEngine;

public class Crowd : MonoBehaviour
{
	[SerializeField]
	private float speed = 0.5f;


    // translate crowd over x axis
    private void Update()
	{
        // move crowd to the right over time
        transform.Translate(Vector3.right * speed * Time.deltaTime);
    }
}
