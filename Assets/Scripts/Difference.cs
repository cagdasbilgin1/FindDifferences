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

    public Image Image1 => image1;
    public Image Image2 => image2;
    public Color Image1Color => image1Color;
    public Color Image2Color => image2Color;
    public float ClickableSize => clickableSize;
}
