using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class DifferentItem
{
    VisualElement _container;
    VisualElement _differentItem;
    public VisualElement VisualElement => _container;

    public DifferentItem(VisualTreeAsset template, Vector2 position, Vector2 size, Sprite sprite, Color color)
    {
        _container = template.Instantiate();
        _differentItem = _container.Q<VisualElement>("Different-Item");
        _differentItem.style.width = size.x;
        _differentItem.style.height = size.y;
        _differentItem.style.backgroundImage = new StyleBackground(sprite);
        _differentItem.style.unityBackgroundImageTintColor = color;
        _differentItem.style.left = position.x - (size.x / 2);
        _differentItem.style.top = position.y - (size.y / 2);
        _differentItem.style.position = Position.Absolute;
    }
}
