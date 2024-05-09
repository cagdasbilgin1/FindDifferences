using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class DiffereceMark
{
    VisualElement _container;
    VisualElement _differenceMark;
    VisualElement _differenceMarkIn;
    VisualElement _differenceMarkOut;
    public VisualElement VisualElement => _container;

    public DiffereceMark(VisualTreeAsset template, Vector2 position, float sizePx)
    {
        _container = template.Instantiate();
        _differenceMark = _container.Q<VisualElement>("Difference-Mark");
        _differenceMarkIn = _differenceMark.Q<VisualElement>("Difference-Mark-In");
        _differenceMarkOut = _differenceMark.Q<VisualElement>("Difference-Mark-Out");
        ArrangeBorderSize(_differenceMark, sizePx / 50f);
        ArrangeBorderSize(_differenceMarkIn, sizePx / 10f);
        ArrangeBorderSize(_differenceMarkOut, sizePx / 50f);
        _differenceMark.style.width = sizePx;
        _differenceMark.style.height = sizePx;
        _differenceMark.style.left = position.x - (sizePx / 2);
        _differenceMark.style.top = position.y - (sizePx / 2);
        _differenceMark.style.position = Position.Absolute;
    }

    void ArrangeBorderSize(VisualElement visualElement, float width)
    {
        visualElement.style.borderTopWidth = width;
        visualElement.style.borderLeftWidth = width;
        visualElement.style.borderRightWidth = width;
        visualElement.style.borderBottomWidth = width;
    }
}
