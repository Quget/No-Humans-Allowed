using UnityEngine;

public class RandomSpriteSetter : MonoBehaviour
{
    [SerializeField]
    private SpriteRenderer spriteRenderer;

    [SerializeField]
    private Sprite[] sprites;

	private void Awake()
	{
		if (sprites != null && sprites.Length > 0 && spriteRenderer != null)
        {
            int randomIndex = Random.Range(0, sprites.Length);
            spriteRenderer.sprite = sprites[randomIndex];
        }
	}
}
