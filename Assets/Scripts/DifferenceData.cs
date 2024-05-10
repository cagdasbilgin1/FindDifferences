using System;
using UnityEngine;

[Serializable]
public class DifferenceData
{
    [SerializeField] Sprite _sprite1;
    [SerializeField] Color _sprite1Color = Color.white;
    [SerializeField] Vector2 _originalSprite1Size;
    [SerializeField] Vector2 _originalSprite1Position;
    [SerializeField] Sprite _sprite2;
    [SerializeField] Color _sprite2Color = Color.white;
    [SerializeField] Vector2 _originalSprite2Size;
    [SerializeField] Vector2 _originalSprite2Position;
    [SerializeField] float _clickableSize;

    public Sprite Sprite1 => _sprite1;
    public Sprite Sprite2 => _sprite2;
    public Color Image1Color => _sprite1Color;
    public Color Image2Color => _sprite2Color;

    public Vector2 Sprite1OriginalSize => _originalSprite1Size;
    public Vector2 Sprite2OriginalSize => _originalSprite2Size;

    public Vector2 Sprite1OriginalPosition => _originalSprite1Position;
    public Vector2 Sprite2OriginalPosition => _originalSprite2Position;

    public float ClickableSize => _clickableSize;


    public DifferenceData(Sprite sprite1, Vector2 sprite1OriginalSize, Vector2 sprite1OriginalPos, Color sprite1Color,
                     Sprite sprite2, Vector2 sprite2OriginalSize, Vector2 sprite2OriginalPos, Color sprite2Color,
                     float clickableSize)
    {
        _sprite1 = sprite1;
        _originalSprite1Size = sprite1OriginalSize;
        _originalSprite1Position = sprite1OriginalPos;
        _sprite1Color = sprite1Color;

        _sprite2 = sprite2;
        _originalSprite2Size = sprite2OriginalSize;
        _originalSprite2Position = sprite2OriginalPos;
        _sprite2Color = sprite2Color;

        _clickableSize = clickableSize;
    }

    public Vector2 GetCenterOfDifference(float ratioOfImageSize)
    {
        if (_sprite1 == null)
            return _originalSprite2Position / ratioOfImageSize;
        else if (_sprite2 == null)
            return _originalSprite1Position / ratioOfImageSize;

        return (_originalSprite1Position / ratioOfImageSize + _originalSprite2Position / ratioOfImageSize) * 0.5f;
    }
}
