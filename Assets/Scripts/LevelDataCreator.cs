using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class LevelDataCreator : MonoBehaviour
{
    [SerializeField] int _levelNumber;
    [SerializeField] int _lifeOnStart;
    [SerializeField] Image _background;
    [SerializeField] List<Difference> _differences;
    Vector2 _originalImageSize;
    List<DifferenceData> _differenceDatas;

    public void CreateLevelData()
    {
        string directoryPath = "Assets/LevelData";
        if (!AssetDatabase.IsValidFolder(directoryPath))
        {
            AssetDatabase.CreateFolder("Assets", "LevelData");
            Debug.Log("LevelData folder created at: " + directoryPath);
        }

        var levelData = ScriptableObject.CreateInstance<LevelData>();

        var width = _background.sprite.rect.width;
        var height = _background.sprite.rect.height;
        _originalImageSize = new Vector2(width, height);

        _differenceDatas = new();
        foreach ( var difference in _differences)
        {
            Sprite sprite1 = null;
            Sprite sprite2 = null;
            Vector2 sprite1OriginalSize = Vector2.zero;
            Vector2 sprite2OriginalSize = Vector2.zero;
            Vector2 sprite1OriginalPosition = Vector2.zero;
            Vector2 sprite2OriginalPosition = Vector2.zero;
            Color sprite1Color = Color.white;
            Color sprite2Color = Color.white;


            if (difference.Image1 != null)
            {
                sprite1 = difference.Image1.sprite;
                sprite1OriginalSize = new Vector2(sprite1.rect.width, sprite1.rect.height);
                var sprite1AnchoredPosition = difference.Image1.rectTransform.anchoredPosition;
                sprite1OriginalPosition = new Vector2(Mathf.Abs(sprite1AnchoredPosition.x), Mathf.Abs(sprite1AnchoredPosition.y));
                sprite1Color = difference.Image1Color;
            }
            if (difference.Image2 != null)
            {
                sprite2 = difference.Image2.sprite;
                sprite2OriginalSize = new Vector2(sprite2.rect.width, sprite2.rect.height);
                var sprite2AnchoredPosition = difference.Image2.rectTransform.anchoredPosition;
                sprite2OriginalPosition = new Vector2(Mathf.Abs(sprite2AnchoredPosition.x), Mathf.Abs(sprite2AnchoredPosition.y));
                sprite2Color = difference.Image2Color;
            }

            var cliclableSize = difference.ClickableSize;

            var differenceData = new DifferenceData(sprite1, sprite1OriginalSize, sprite1OriginalPosition, sprite1Color, sprite2, sprite2OriginalSize, sprite2OriginalPosition, sprite2Color, cliclableSize);
            _differenceDatas.Add(differenceData);
        }

        levelData.Init(_lifeOnStart, _background.sprite, _differenceDatas, _originalImageSize);

        var path = directoryPath + $"/Level{_levelNumber}.asset";
        AssetDatabase.CreateAsset(levelData, path);
        AssetDatabase.SaveAssets();

        Debug.Log("Level data saved at: " + path);
    }

    public void OnSaveBtnClicked()
    {
        CreateLevelData();
    }
}
