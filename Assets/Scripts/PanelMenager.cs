using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PanelMenager : MonoBehaviour
{
    public static GameObject BottomPanel;
    public static GameObject BarrackPanel;
    public static GameObject LightWarriorCostPanel;

    public static GameObject resourcesPanel;
    [SerializeField, Range(50f, 90f)] float BottomPanelSizeX = 72f;
    [SerializeField, Range(5f, 30f)] float BottomPanelSizeY = 16.5f;

    private void Start()
    {
        BarrackPanel = GameObject.Find("BarrackPanel");
        BarrackPanel.SetActive(false);

        BottomPanel = GameObject.Find("BottomPanel");
        BottomPanel.SetActive(true);

        resourcesPanel = GameObject.Find("ResourcesPanel");
        resourcesPanel.SetActive(true);

      
    }

    private void Update()
    {
        TrySetUiSize();
    }

    private void TrySetUiSize()
    {
        var screenY = new Vector2(Screen.width / 100f * BottomPanelSizeX, Screen.height / 100f * BottomPanelSizeY);
        BottomPanel.GetComponent<RectTransform>().sizeDelta = screenY;
    }

}
