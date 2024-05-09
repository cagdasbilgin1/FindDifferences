using System;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class Difference
{
    [SerializeField] Image image1;
    [SerializeField] Color image1Color = Color.white;
    [SerializeField] Image image2;
    [SerializeField] Color image2Color = Color.white;
    [SerializeField] float clickableSize;

    public Sprite Sprite1 => GetSprite(image1);
    public Sprite Sprite2 => GetSprite(image2);
    public Color Image1Color => image1Color;
    public Color Image2Color => image2Color;

    public float ClickableSize => clickableSize;

    public Vector2 GetCenterOfDifference(float ratioOfImageSize)
    {
        Vector2 anchoredPosition1 = image1 != null ? new Vector2(Mathf.Abs(image1.rectTransform.anchoredPosition.x), Mathf.Abs(image1.rectTransform.anchoredPosition.y)) : Vector2.zero;
        Vector2 anchoredPosition2 = image2 != null ? new Vector2(Mathf.Abs(image2.rectTransform.anchoredPosition.x), Mathf.Abs(image2.rectTransform.anchoredPosition.y)) : Vector2.zero;

        if (image1 == null)
            return anchoredPosition2 / ratioOfImageSize;
        else if (image2 == null)
            return anchoredPosition1 / ratioOfImageSize;

        return (anchoredPosition1 / ratioOfImageSize + anchoredPosition2 / ratioOfImageSize) * 0.5f;
    }

    public Vector2 GetOriginalPositionOfImage(int whichBackground)
    {
        Vector2 anchoredPosition;
        switch (whichBackground)
        {
            case 1:
                anchoredPosition = image1.rectTransform.anchoredPosition;
                break;
            case 2:
                anchoredPosition = image2.rectTransform.anchoredPosition;
                break;
            default:
                anchoredPosition = Vector2.zero;
                break;
        }

        anchoredPosition = new Vector2(Mathf.Abs(anchoredPosition.x), Mathf.Abs(anchoredPosition.y));
        return anchoredPosition;
    }

    Sprite GetSprite(Image image)
    {
        if (image == null) return null;

        return image.sprite;
    }
}
