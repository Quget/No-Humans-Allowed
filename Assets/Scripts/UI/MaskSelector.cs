using TMPro;
using UnityEngine;

public class MaskSelector : MonoBehaviour
{
    [SerializeField]
    private SelectedMasks selectedMasks;

    [SerializeField]
    private TextMeshProUGUI titleText;

    void Awake()
    {
        gameObject.SetActive(false);
	}

    public void Open()
    {
        UpdateText();
		gameObject.SetActive(true);
    }

    public void SelectMask(int race)
    {
        if (selectedMasks.AddMaskSlot((RacesEnumerator)race))
        {
            Close();
        }
        UpdateText();

	}

    private void UpdateText()
    {
        titleText.text = $"Select a Mask ({selectedMasks.MasksCount}/{LevelManager.Instance.MaxMaskCount})";
	}

    public void Close()
    {
		gameObject.SetActive(false);
	}
}
