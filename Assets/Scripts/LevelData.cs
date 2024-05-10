using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewLevelData", menuName = "Level Data")]
public class LevelData : ScriptableObject
{
    [SerializeField] int _lifeOnStart;
    [SerializeField] Sprite _backgroundSprite;
    [SerializeField] List<DifferenceData> _differenceDatas;
    [SerializeField] Vector2 _originalImageSize;

    public int LifeOnStart => _lifeOnStart;
    public Sprite BackgroundSprite => _backgroundSprite;
    public List<DifferenceData> DifferenceDatas => _differenceDatas;
    public Vector2 OriginalImageSize => _originalImageSize;

    public void Init(int lifeOnStart, Sprite backgroundSprite, List<DifferenceData> differenceDatas, Vector2 originalImageSize)
    {
        _lifeOnStart = lifeOnStart;
        _backgroundSprite = backgroundSprite;
        _differenceDatas = differenceDatas;
        _originalImageSize = originalImageSize;
    }
}