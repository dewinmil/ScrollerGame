using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking.Match;
using UnityEngine.UI;
using TMPro;

public class JoinButton : MonoBehaviour {

    //private Text buttonText;
    private TextMeshProUGUI buttonText;
    private MatchInfoSnapshot match;

	void Awake () {
        buttonText = GetComponentInChildren<TextMeshProUGUI>();
	}

    public void Initialize(MatchInfoSnapshot match, Transform panelTransform)
    {
        this.match = match;
        buttonText.text = match.name;
        transform.SetParent(panelTransform);
        transform.localScale = Vector3.one;
        transform.localRotation = Quaternion.identity;
        transform.localPosition = Vector3.zero;
    }

    public void JoinMatch()
    {
        if(match.isPrivate == false)
        {
            FindObjectOfType<CustomNetworkManager>().JoinMatch(match);
        }
        else
        {
            FindObjectOfType<CustomNetworkManager>().PasswordPrompt(match);
        }
    }
}
