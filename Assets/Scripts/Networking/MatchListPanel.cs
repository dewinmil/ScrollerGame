using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking.Match;
public class MatchListPanel : MonoBehaviour {

    [SerializeField] private JoinButton joinButtonPrefab;

    private void Awake()
    {
        AvailableMatchesList.OnAvailableMatchesChanged += AvailableMatchesList_OnAvailableMatchesChanged;
    }

    private void AvailableMatchesList_OnAvailableMatchesChanged(List<MatchInfoSnapshot> matches)
    {
        ClearExistingButtons();
        CreateNewJoinButtons(matches);
    }

    private void ClearExistingButtons()
    {
        var buttons = GetComponentsInChildren<JoinButton>();
        foreach (var button in buttons)
        {
            Destroy(button.gameObject);
        }
    }

    private void CreateNewJoinButtons(List<MatchInfoSnapshot> matches)
    {
        foreach (var match in matches)
        {
            var button = Instantiate(joinButtonPrefab);
            button.Initialize(match, this.transform);
        }
    }

}
