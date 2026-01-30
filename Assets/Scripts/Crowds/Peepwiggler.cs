using UnityEngine;

public class PeepWiggler : MonoBehaviour
{
    [SerializeField]
    private float RotationWiggleAmount = 5f;

    [SerializeField]
    private float TranslationWiggleAmount = 5f;

    private float randomOffset;

    private Vector3 positionOrigin;

    private void Start()
    {
        randomOffset = Random.value * 10;
        positionOrigin = transform.localPosition;
    }
    
    private void Update()
    {
        RotationWiggle();
        TranslationWiggle();
    }

    private void RotationWiggle()
    {
        float wiggle = Mathf.Sin(Time.time * 10f + randomOffset) * RotationWiggleAmount *10;
        transform.localRotation = Quaternion.Euler(0, 0, wiggle);
    }
    
    private void TranslationWiggle()
    {
        float wiggleX = Mathf.Sin(Time.time * 10f + randomOffset) * (TranslationWiggleAmount / 100f);
        float wiggleY = Mathf.Sin(Time.time * 10f + randomOffset) * (TranslationWiggleAmount / 100f);

        transform.localPosition = positionOrigin + new Vector3(wiggleX, wiggleY, -0.1f);
    }
}
