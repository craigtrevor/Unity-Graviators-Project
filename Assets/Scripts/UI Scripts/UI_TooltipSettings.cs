using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_TooltipSettings : MonoBehaviour {

    [SerializeField]
    UI_MatchMixerLevels matchMixerLevels;

    [SerializeField]
    Toggle tooltipToggle;

    [SerializeField]
    bool tooltipValue;

    private void Start()
    {
        matchMixerLevels = GameObject.Find("Scene Checker").GetComponent<UI_MatchMixerLevels>();
        tooltipToggle.GetComponent<Toggle>();
        tooltipValue = matchMixerLevels.tooltipActivated;
        tooltipToggle.isOn = matchMixerLevels.tooltipActivated;
    }

    public void ToggleChanged(bool isTooltip)
    {
        if (isTooltip)
        {
            tooltipValue = true;
        }

        else if (!isTooltip)
        {
            tooltipValue = false;
        }
    }

    public void SendMessage()
    {
        matchMixerLevels.SendMessage("UpdateTValue", tooltipValue);
    }
}
