using UnityEngine;

public class  MaskSlot : MonoBehaviour
{

    [SerializeField]
    private UnityEngine.UI.Image maskImage;

    public RacesEnumerator Race;

	// Start is called once before the first execution of Update after the MonoBehaviour is created
	void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetMask(RacesEnumerator race, Sprite sprite)
    {
        maskImage.sprite = sprite;
		Race = race;
	}
}
