using UnityEngine;

public class PeepWiggler : MonoBehaviour
{
    [SerializeField]
    private float RotationWiggleAmount = 5f;

    [SerializeField]
    private float TranslationWiggleAmount = 5f;

    [Header("Dynamic Wiggle Settings")]
    [SerializeField]
    private float baseWiggleSpeed = 2f;
    [SerializeField]
    private float speedMultiplier = 5f;

    private float randomOffset;
    private Vector3 positionOrigin;
    private float currentWiggleTimer;

    // Tracking variables
    private Vector3 lastPosition;
    private float currentVelocityMagnitude;

    private void Start()
    {
        randomOffset = Random.value * 10;
        positionOrigin = transform.localPosition;

        lastPosition = transform.position;
    }

    private void Update()
    {
        CalculateManualVelocity();

        float dynamicSpeed = baseWiggleSpeed + (currentVelocityMagnitude * speedMultiplier);

        // Accumulate timer
        currentWiggleTimer += Time.deltaTime * dynamicSpeed;

        RotationWiggle();
        TranslationWiggle();
    }

    private void CalculateManualVelocity()
    {
        Vector3 deltaPosition = transform.position - lastPosition;

        if (Time.deltaTime > 0)
            currentVelocityMagnitude = deltaPosition.magnitude / Time.deltaTime;

        lastPosition = transform.position;
    }

    private void RotationWiggle()
    {
        float wiggle = Mathf.Sin(currentWiggleTimer + randomOffset) * RotationWiggleAmount;
        transform.localRotation = Quaternion.Euler(0, 0, wiggle);
    }

    private void TranslationWiggle()
    {
        float wiggleX = Mathf.Cos(currentWiggleTimer + randomOffset) * (TranslationWiggleAmount / 100f);
        float wiggleY = Mathf.Sin(currentWiggleTimer + randomOffset) * (TranslationWiggleAmount / 100f);

        transform.localPosition = positionOrigin + new Vector3(wiggleX, wiggleY, -0.1f);
    }
}