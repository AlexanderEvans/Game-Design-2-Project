using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UISelector : MonoBehaviour
{
    List<Image> iconSlots = new List<Image>();
    List<Sprite> icons = new List<Sprite>();
    RectTransform parentsRectTransform = null;
    float heightDelta = 0;
    float widthDelta = 0;

    int rowCount = 0;
    int colCount = 0;

    List<List<Vector2>> ObjectFramePosition = new List<List<Vector2>>();

    private void Awake()
    {
        //get anchor points
        parentsRectTransform = transform.parent.GetComponent<RectTransform>();

        //calculate rows
        rowCount = (int)(parentsRectTransform.rect.height / 100.0f)-1;
        colCount = (int)(parentsRectTransform.rect.width / 100.0f)-1;
        //generate grid
        heightDelta = parentsRectTransform.rect.height / rowCount;
        widthDelta = parentsRectTransform.rect.width / colCount;

        Vector2 position = new Vector2();
        for (int col = 0; col < colCount; col++)
        {
            ObjectFramePosition.Add(new List<Vector2>());
            position.x = (widthDelta * col) + 50f;
            for (int row = 0; row < rowCount; row++)
            {
                position.y = (heightDelta * row) + 50f;
                ObjectFramePosition[col][row] = position;
            }
        }
    }
}
