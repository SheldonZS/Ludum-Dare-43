using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class frameControl : MonoBehaviour {

    private RectTransform rt;
    private Image image;

    private void Start()
    {
        rt = GetComponent<RectTransform>();
        image = GetComponent<Image>();
    }

    public void hide()
    {
        image.enabled = false;
    }

    public void setSlot(int x)
    {
        image.enabled = true;

        Vector3 pos = rt.localPosition;
        pos.x = 128 * x - 576;
        rt.localPosition = pos;
    }
}
