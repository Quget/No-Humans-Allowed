using UnityEngine;

public class MaskSelector : MonoBehaviour
{
    [SerializeField]
    private SelectedMasks selectedMasks;

    void Awake()
    {
        gameObject.SetActive(false);
	}

    public void Open()
    {
        gameObject.SetActive(true);
    }

    public void SelectMask(int race)
    {
        if (selectedMasks.AddMaskSlot((RacesEnumerator)race))
        {
            Close();
		}
	}

    public void Close()
    {
        gameObject.SetActive(false);
	}
}
