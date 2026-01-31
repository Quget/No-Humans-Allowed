    using UnityEngine;
using UnityEngine.UI;

public class UserInterface : MonoBehaviour
{
    [SerializeField]
    private MaskSelector maskSelector;

    [SerializeField]
    private GameObject maskSelectorButton;

	// Start is called once before the first execution of Update after the MonoBehaviour is created
	void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OpenMaskSelector()
    {
		maskSelectorButton.SetActive(false);
        maskSelector.Open();
	}
}
