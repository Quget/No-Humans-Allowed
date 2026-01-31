using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SelectedMasks : MonoBehaviour
{
    private int maxMaskCount = 5;

	[SerializeField]
    private MaskSlot maskSlotPrefab;

    [SerializeField]
    private Transform maskSlotGrid;

    [SerializeField]
    private RaceMaskSprites[] raceMaskSprites;

	private List<MaskSlot> masks = new List<MaskSlot>();

    public void SetMaxMaskCount(int count)
    {
        maxMaskCount = count;
    }

    public bool TryUseMask(RacesEnumerator race)
    {
        var mask = masks.FirstOrDefault(x => x.Race == race);
        if(mask != null)
        {
            masks.Remove(mask);
            Destroy(mask.gameObject);
            return true;
		}
        return false;
	}

    public bool AddMaskSlot(RacesEnumerator race)
    {
        MaskSlot maskSlot = Instantiate(maskSlotPrefab, maskSlotGrid);
        masks.Add(maskSlot);
        maskSlot.SetMask(race, GetSpriteByRace(race));

		if (masks.Count >= maxMaskCount)
		{
            Debug.Log("Max masks reached");
			//fire event for playerthat max masks reached
            FindFirstObjectByType<Player>()?.SendMessage("OnMaxMasksReached", SendMessageOptions.DontRequireReceiver);
			return true;
		}
        return false;
	}

    private Sprite GetSpriteByRace(RacesEnumerator race)
    {
        return raceMaskSprites.FirstOrDefault(x => x.Race == race)?.MaskSprite;
	}

    public void Clear()
    {
        foreach (MaskSlot mask in masks)
        {
            Destroy(mask.gameObject);
        }
        masks.Clear();
	}
}

[System.Serializable]
public class RaceMaskSprites
{
    [SerializeField]
    public Sprite MaskSprite;

    [SerializeField]
    public RacesEnumerator Race;
}