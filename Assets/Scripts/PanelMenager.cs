using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PanelMenager : MonoBehaviour
{
    public static PanelMenager panelMenager;
    
    public GameObject BottomPanel;
    public GameObject resourcesPanel;
    public GameObject BarrackPanel;
    public GameObject ArcherSchoolPanel;
    public GameObject WoodCutterPanel;
    public GameObject BuildPanel;


    private void Awake()
    {
        panelMenager = this;
    }


}
