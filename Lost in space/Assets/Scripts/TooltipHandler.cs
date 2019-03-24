using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TooltipHandler : MonoBehaviour
{
    private GameObject tooltip;

    void Start()
    {
        tooltip = GameObject.Find("Tooltip");
        tooltip.SetActive(false);
    }

    public void showTooltip(Transform position, string name, string description, string recipe = "")
    {
        tooltip.transform.GetChild(0).GetComponent<Text>().text = name;
        tooltip.transform.GetChild(1).GetComponent<Text>().text = description;
        tooltip.transform.GetChild(2).GetComponent<Text>().text = recipe;

        //tooltip.transform.position = position.localToWorldMatrix.MultiplyPoint(new Vector2(position.GetComponent<RectTransform>().rect.xMax, position.GetComponent<RectTransform>().rect.yMax));

        tooltip.SetActive(true);
    }

    public void hideTooltip()
    {
        tooltip.SetActive(false);
    }

}
