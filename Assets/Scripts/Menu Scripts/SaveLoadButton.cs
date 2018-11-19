using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SaveLoadButton : MonoBehaviour {

    public void Initialize(Transform panelTransform, string name)
    {
        this.GetComponentInChildren<TextMeshProUGUI>().text = name;
        transform.SetParent(panelTransform);
        transform.localScale = Vector3.one;
        transform.localRotation = Quaternion.identity;
        transform.localPosition = Vector3.zero;
    }

    public void SetText()
    {
        FindObjectOfType<TMP_InputField>().text = this.GetComponentInChildren<TextMeshProUGUI>().text;
    }
}
