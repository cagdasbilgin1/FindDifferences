using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class DifferenceManager : MonoBehaviour
{
    static DifferenceManager _instance;
    public static DifferenceManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new DifferenceManager();
            }
            return _instance;
        }
    }

    [SerializeField] UIController _uiController;
    [SerializeField] Sprite _backgroundSprite;
    [SerializeField] VisualTreeAsset differentItemTemplate;
    [SerializeField] VisualTreeAsset differenceMarkTemplate;
    [SerializeField] List<Difference> differences;
    List<DifferentItem> differentItems;

    int _totalDifferentCount;
    Vector2 _displayedImageSize;
    Vector2 _originalImageSize;
    float _ratioOfImageSize;

    void OnEnable()
    {
        _uiController.OnImageClicked += OnImageClicked;
        _uiController.OnDisplayedImageLoad += OnDisplayedImageLoadEvent;
    }

    void Start()
    {
        var width = _backgroundSprite.rect.width;
        var height = _backgroundSprite.rect.height;

        _originalImageSize = new Vector2(width, height);

        Debug.Log("original width = " + width + " , original height : " + height);

        _uiController.ArrangeImages(_backgroundSprite);
    }

    void OnImageClicked(Vector2 mousePos)
    {
        Difference differencesToRemove = null;
        foreach (var difference in differences)
        {
            var distance = (mousePos - difference.GetCenterOfDifference(_ratioOfImageSize)).magnitude;
            var clickableSizeRadius = difference.ClickableSize * _displayedImageSize.x / 30;

            if (distance <= clickableSizeRadius)
            {
                differencesToRemove = difference;
                _uiController.TopImage.Add(new DiffereceMark(differenceMarkTemplate, difference.GetCenterOfDifference(_ratioOfImageSize), clickableSizeRadius * 2).VisualElement);
                _uiController.BottomImage.Add(new DiffereceMark(differenceMarkTemplate, difference.GetCenterOfDifference(_ratioOfImageSize), clickableSizeRadius * 2).VisualElement);
                break;
            }
        }

        if (differencesToRemove != null)
        {
            differences.Remove(differencesToRemove);
            _totalDifferentCount++;
        }
    }

    void OnDisplayedImageLoadEvent(Vector2 displayedImageSize)
    {
        _displayedImageSize = displayedImageSize;
        PlaceItems();
    }

    void PlaceItems()
    {
        _ratioOfImageSize = _originalImageSize.x / _displayedImageSize.x;

        foreach (var difference in differences)
        {
            if (difference.Sprite1 != null)
            {
                var originalWidthPx = difference.Sprite1.rect.width;
                var originalHeightPx = difference.Sprite1.rect.height;
                var displayedSize = new Vector2(originalWidthPx, originalHeightPx) / _ratioOfImageSize;

                var originalPosition = difference.GetOriginalPositionOfImage(1);
                var displayedPosition = originalPosition / _ratioOfImageSize;

                var color = difference.Image1Color;

                var differentItem = new DifferentItem(differentItemTemplate, displayedPosition, displayedSize, difference.Sprite1, color);
                _uiController.TopImage.Add(differentItem.VisualElement);
            }
            if (difference.Sprite2 != null)
            {
                var originalWidthPx = difference.Sprite2.rect.width;
                var originalHeightPx = difference.Sprite2.rect.height;
                var displayedSize = new Vector2(originalWidthPx, originalHeightPx) / _ratioOfImageSize;

                var originalPosition = difference.GetOriginalPositionOfImage(2);
                var displayedPosition = originalPosition / _ratioOfImageSize;

                var color = difference.Image2Color;

                var differentItem = new DifferentItem(differentItemTemplate, displayedPosition, displayedSize, difference.Sprite2, color);
                _uiController.BottomImage.Add(differentItem.VisualElement);
            }
        }
    }
}
