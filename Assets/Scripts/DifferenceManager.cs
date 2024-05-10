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
    [SerializeField] Sprite _findIndicatorCheckMark;
    [SerializeField] Sprite _findIndicatorQuestionMark;
    [SerializeField] Sprite _heart;
    [SerializeField] Sprite _emptyHeart;
    [SerializeField] VisualTreeAsset _differentItemTemplate;
    [SerializeField] VisualTreeAsset _differenceMarkTemplate;
    [SerializeField] VisualTreeAsset _lifeIndicatorTemplate;
    [SerializeField] VisualTreeAsset _findIndicatorTemplate;
    [SerializeField] LevelData _levelData;

    List<VisualElement> _lifeIndicators;
    List<VisualElement> _findIndicators;
    List<DifferenceData> _currentDifferenceDatas;
    int _countOfDifferenceFound;
    int _currentLife;
    Vector2 _displayedImageSize;
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
        _currentLife = _levelData.LifeOnStart;
        _uiController.ArrangeImages(_levelData.BackgroundSprite);
    }

    void OnImageClicked(Vector2 mousePos)
    {
        DifferenceData differencesToRemove = null;
        foreach (var difference in _currentDifferenceDatas)
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
            _currentDifferenceDatas.Remove(differencesToRemove);
            MarkIndicatorToGreenCheck(_findIndicators[_countOfDifferenceFound]);
            _countOfDifferenceFound++;

            if(_currentDifferenceDatas.Count <= 0)
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
        _ratioOfImageSize = _levelData.OriginalImageSize.x / _displayedImageSize.x;
        _currentDifferenceDatas = new List<DifferenceData>(_levelData.DifferenceDatas);

        PlaceItems();
        PlaceLifeIndicators();
        PlaceFindIndicators();
    }

    void PlaceItems()
    {
        foreach (var differenceData in _levelData.DifferenceDatas)
        {
            if (differenceData.Sprite1 != null)
            {
                var displayedSize = differenceData.Sprite1OriginalSize / _ratioOfImageSize;
                var displayedPosition = differenceData.Sprite1OriginalPosition / _ratioOfImageSize;
                var color = differenceData.Image1Color;

                var differentItem = new DifferentItem(_differentItemTemplate, displayedPosition, displayedSize, differenceData.Sprite1, color);
                _uiController.TopImage.Add(differentItem.VisualElement);
            }
            if (differenceData.Sprite2 != null)
            {
                var displayedSize = differenceData.Sprite2OriginalSize / _ratioOfImageSize;
                var displayedPosition = differenceData.Sprite2OriginalPosition / _ratioOfImageSize;
                var color = differenceData.Image2Color;

                var differentItem = new DifferentItem(_differentItemTemplate, displayedPosition, displayedSize, differenceData.Sprite2, color);
                _uiController.BottomImage.Add(differentItem.VisualElement);
            }
        }
    }

    void PlaceLifeIndicators()
    {
        _lifeIndicators = new();
        for (int i = 0; i < _levelData.LifeOnStart; i++)
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
        var differenceCount = _levelData.DifferenceDatas.Count;
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
        _currentLife = _levelData.LifeOnStart;
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
        _currentDifferenceDatas = new List<DifferenceData>(_levelData.DifferenceDatas);
        ResetLives();
        ResetFinds();
        PlaceItems();
    }
}
