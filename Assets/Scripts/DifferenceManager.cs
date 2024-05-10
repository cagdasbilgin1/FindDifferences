using System;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.UIElements;
using static UnityEngine.InputManagerEntry;

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
    [SerializeField] int _lifeOnStart;
    [SerializeField] Sprite _backgroundSprite;
    [SerializeField] Sprite _findIndicatorCheckMark;
    [SerializeField] Sprite _findIndicatorQuestionMark;
    [SerializeField] Sprite _heart;
    [SerializeField] Sprite _emptyHeart;
    [SerializeField] VisualTreeAsset _differentItemTemplate;
    [SerializeField] VisualTreeAsset _differenceMarkTemplate;
    [SerializeField] VisualTreeAsset _lifeIndicatorTemplate;
    [SerializeField] VisualTreeAsset _findIndicatorTemplate;
    [SerializeField] List<Difference> _differences;
    List<DifferentItem> _differentItems;
    List<VisualElement> _lifeIndicators;
    List<VisualElement> _findIndicators;
    List<Difference> _currentDifferences;


    int _countOfDifferenceFound;
    int _currentLife;
    Vector2 _displayedImageSize;
    Vector2 _originalImageSize;
    float _ratioOfImageSize;

    void OnEnable()
    {
        _uiController.OnImageClicked += OnImageClicked;
        _uiController.OnDisplayedImageLoad += OnDisplayedImageLoadEvent;
        _uiController.OnGetLiveBtnClickedAct += OnLiveBtnClickedEvent;
        _uiController.OnRestartBtnClickedAct += OnRestartBtnClickedEvent;
    }

    void Start()
    {
        var width = _backgroundSprite.rect.width;
        var height = _backgroundSprite.rect.height;

        _originalImageSize = new Vector2(width, height);
        _currentLife = _lifeOnStart;

        Debug.Log("original width = " + width + " , original height : " + height);

        _uiController.ArrangeImages(_backgroundSprite);
    }

    void OnImageClicked(Vector2 mousePos)
    {
        Difference differencesToRemove = null;
        foreach (var difference in _currentDifferences)
        {
            var distance = (mousePos - difference.GetCenterOfDifference(_ratioOfImageSize)).magnitude;
            var clickableSizeRadius = difference.ClickableSize * _displayedImageSize.x / 30;

            if (distance <= clickableSizeRadius)
            {
                differencesToRemove = difference;
                _uiController.TopImage.Add(new DiffereceMark(_differenceMarkTemplate, difference.GetCenterOfDifference(_ratioOfImageSize), clickableSizeRadius * 2).VisualElement);
                _uiController.BottomImage.Add(new DiffereceMark(_differenceMarkTemplate, difference.GetCenterOfDifference(_ratioOfImageSize), clickableSizeRadius * 2).VisualElement);
                break;
            }
        }

        if (differencesToRemove != null)
        {
            _currentDifferences.Remove(differencesToRemove);
            MarkIndicatorToGreenCheck(_findIndicators[_countOfDifferenceFound]);
            _countOfDifferenceFound++;

            if(_currentDifferences.Count <= 0)
            {
                OnAllDifferencesFound();
            }
        }
        else
        {
            _currentLife--;
            OneLifeGone(_lifeIndicators[_currentLife]);
        }
    }

    void OnDisplayedImageLoadEvent(Vector2 displayedImageSize)
    {
        _displayedImageSize = displayedImageSize;
        PlaceItems();
        PlaceLifeIndicators();
        PlaceFindIndicators();
    }

    void PlaceItems()
    {
        _ratioOfImageSize = _originalImageSize.x / _displayedImageSize.x;
        _currentDifferences = new List<Difference>(_differences);

        foreach (var difference in _currentDifferences)
        {
            if (difference.Sprite1 != null)
            {
                var originalWidthPx = difference.Sprite1.rect.width;
                var originalHeightPx = difference.Sprite1.rect.height;
                var displayedSize = new Vector2(originalWidthPx, originalHeightPx) / _ratioOfImageSize;

                var originalPosition = difference.GetOriginalPositionOfImage(1);
                var displayedPosition = originalPosition / _ratioOfImageSize;

                var color = difference.Image1Color;

                var differentItem = new DifferentItem(_differentItemTemplate, displayedPosition, displayedSize, difference.Sprite1, color);
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

                var differentItem = new DifferentItem(_differentItemTemplate, displayedPosition, displayedSize, difference.Sprite2, color);
                _uiController.BottomImage.Add(differentItem.VisualElement);
            }
        }
    }

    void PlaceLifeIndicators()
    {
        _lifeIndicators = new();
        for (int i = 0; i < _lifeOnStart; i++)
        {
            var lifeIndicator = _lifeIndicatorTemplate.Instantiate();
            lifeIndicator.AddToClassList("life-indicator");
            _uiController.LifeIndicatorsArea.Add(lifeIndicator);
            _lifeIndicators.Add(lifeIndicator);
        }
    }

    void PlaceFindIndicators()
    {
        _findIndicators = new();
        var differenceCount = _differences.Count;
        for (int i = 0; i < differenceCount; i++)
        {
            var findIndicator = _findIndicatorTemplate.Instantiate();
            findIndicator.AddToClassList("find-indicator");
            _uiController.FindIndicatorsArea.Add(findIndicator);
            _findIndicators.Add(findIndicator);
        }
    }

    void MarkIndicatorToGreenCheck(VisualElement indicator)
    {
        var graphic = indicator.Q<VisualElement>("Indicator-Graphic");
        graphic.style.backgroundImage = new StyleBackground(_findIndicatorCheckMark);
    }

    void OnAllDifferencesFound()
    {
        _uiController.OpenGamePanelPopup("Level-Up-Popup");
    }

    void OneLifeGone(VisualElement indicator)
    {
        var graphic = indicator.Q<VisualElement>("Indicator-Graphic");
        graphic.style.backgroundImage = new StyleBackground(_emptyHeart);

        if (_currentLife <= 0)
        {
            _uiController.OpenGamePanelPopup("Get-Live-Popup");
            return;
        }
    }

    void OnLiveBtnClickedEvent()
    {
        ResetLives();
    }

    void ResetLives()
    {
        _currentLife = _lifeOnStart;
        foreach (var indicator in _lifeIndicators)
        {
            var graphic = indicator.Q<VisualElement>("Indicator-Graphic");
            graphic.style.backgroundImage = new StyleBackground(_heart);
        }
    }

    void ResetFinds()
    {
        foreach(var indicator in _findIndicators)
        {
            var graphic = indicator.Q<VisualElement>("Indicator-Graphic");
            graphic.style.backgroundImage = new StyleBackground(_findIndicatorQuestionMark);
        }

        _uiController.TopImage.Clear();
        _uiController.BottomImage.Clear();
    }

    void OnRestartBtnClickedEvent()
    {
        _countOfDifferenceFound = 0;
        ResetLives();
        ResetFinds();
        PlaceItems();
    }
}
