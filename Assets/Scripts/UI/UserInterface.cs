using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UserInterface : MonoBehaviour
{
	[SerializeField]
	private MaskSelector maskSelector;

	[SerializeField]
	private GameObject maskSelectorButton;

	[SerializeField]
	private Button PreviewButton;

	public void OpenMaskSelector()
	{
		maskSelectorButton.SetActive(false);
		PreviewButton.gameObject.SetActive(false);
		maskSelector.Open();
	}

	public void StartPreview()
	{
		Debug.Log("Start preview");
		MasterLane.Instance.StartAutoPlay();
	}

    public void Update()
    {
        // makes sure you cannot start previewing whisle already previewing :D
  //      if (MasterLane.Instance.IsAutoPlaying)
		//	PreviewButton.interactable = false;
		//else 
		//	PreviewButton.interactable = true;
    }
}
