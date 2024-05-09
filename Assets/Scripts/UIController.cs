using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;

public class UIController : MonoBehaviour
{
    VisualElement mainMenuPanel;
    VisualElement gamePanel;

    VisualElement topImage;
    VisualElement bottomImage;

    Button settingsBtn;
    Button clueBtn;
    Button playBtn;

    float _imageWidth;
    float _imageHeight;

    public VisualElement TopImage => topImage;
    public VisualElement BottomImage => bottomImage;

    public event Action<Vector2> OnImageClicked;
    public event Action<Vector2> OnDisplayedImageLoad;

    void Awake()
    {
        var root = GetComponent<UIDocument>().rootVisualElement;

        //Panels
        mainMenuPanel = root.Q<VisualElement>("MainMenu-Panel");
        gamePanel = root.Q<VisualElement>("Game-Panel");

        //MainMenu-Panel
        playBtn = mainMenuPanel.Q<Button>("Play-Btn"); ;

        //Game-Panel
        topImage = gamePanel.Q<VisualElement>("Top-Image");
        bottomImage = gamePanel.Q<VisualElement>("Bottom-Image");
        settingsBtn = gamePanel.Q<Button>("Settings-Btn");
        clueBtn = gamePanel.Q<Button>("Clue-Btn");

    }

    void OnEnable()
    {
        //MainMenu-Panel
        playBtn.RegisterCallback<ClickEvent>(OnPlayBtnClicked);

        //Game-Panel
        topImage.RegisterCallback<ClickEvent>(OnTopImageClicked);
        bottomImage.RegisterCallback<ClickEvent>(OnBottomImageClicked);

        StartCoroutine(WaitForDisplayedImageLoad());
    }

    public void ArrangeImages(Sprite sprite)
    {
        topImage.style.backgroundImage = new StyleBackground(sprite);
        bottomImage.style.backgroundImage = new StyleBackground(sprite);
    }

    public Vector2 GetDisplayedImageSize()
    {
        return new Vector2(_imageWidth, _imageHeight);
    }

    IEnumerator WaitForDisplayedImageLoad()
    {
        while (_imageWidth == 0f || _imageHeight == 0f)
        {
            yield return new WaitForEndOfFrame();

            var imageRect = topImage.worldBound;

            _imageWidth = imageRect.width;
            _imageHeight = imageRect.height;
        }
        Debug.Log("Width: " + _imageWidth + "px, Height: " + _imageHeight + "px");
        OnDisplayedImageLoad?.Invoke(new Vector2(_imageWidth, _imageHeight));
    }

    void OnPlayBtnClicked(ClickEvent evt)
    {
        HideAllPanels();
        gamePanel.style.display = DisplayStyle.Flex;
    }

    void OnTopImageClicked(ClickEvent evt)
    {
        var mousePosition = evt.localPosition;
        OnImageClicked?.Invoke(mousePosition);
    }

    void OnBottomImageClicked(ClickEvent evt)
    {
        var mousePosition = evt.localPosition;
        OnImageClicked?.Invoke(mousePosition);
    }

    void HideAllPanels()
    {
        mainMenuPanel.style.display = DisplayStyle.None;
        gamePanel.style.display = DisplayStyle.None;
    }
}
