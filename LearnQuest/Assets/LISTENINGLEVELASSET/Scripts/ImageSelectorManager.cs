using System.Collections.Generic;
using UnityEngine;

public class ImageSelectionManager : MonoBehaviour
{
    public int correctIndexAudio1 = 0;
    public int correctIndexAudio2 = 3;

    private int? selectedIndex1 = null;
    private int? selectedIndex2 = null;

    public List<SelectableImage> allSelectableImages;

    public void OnImageSelected(bool isAudio1, int index)
    {
        if (isAudio1)
            selectedIndex1 = index;
        else
            selectedIndex2 = index;
    }

    public bool IsAudio1Correct()
    {
        return selectedIndex1.HasValue && selectedIndex1 == correctIndexAudio1;
    }

    public bool IsAudio2Correct()
    {
        return selectedIndex2.HasValue && selectedIndex2 == correctIndexAudio2;
    }

    public bool HasBothCorrect()
    {
        return IsAudio1Correct() && IsAudio2Correct();
    }

    public void ResetColorsInGroup(bool forAudio1)
    {
        Debug.Log("ResetColorsInGroup llamado para: " + (forAudio1 ? "Audio1" : "Audio2"));

        foreach (var img in allSelectableImages)
        {
            if (img.IsAudio1() == forAudio1)
            {
                img.ResetSelection();
            }
        }
    }

    public void ValidateAudio1()
    {
        foreach (var img in allSelectableImages)
        {
            if (img.IsAudio1())
            {
                if (selectedIndex1.HasValue && img.Index() == selectedIndex1)
                {
                    if (selectedIndex1 == correctIndexAudio1)
                        img.MarkCorrect();
                    else
                        img.ResetSelection();
                }
            }
        }
    }

    public void ValidateAudio2()
    {
        foreach (var img in allSelectableImages)
        {
            if (!img.IsAudio1())
            {
                if (selectedIndex2.HasValue && img.Index() == selectedIndex2)
                {
                    if (selectedIndex2 == correctIndexAudio2)
                        img.MarkCorrect();
                    else
                        img.ResetSelection();
                }
            }
        }
    }
}
