using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class panelEvent : MonoBehaviour
{
    public GameObject info;
    public GameObject panel;
    public GameObject textInfo;
    // Start is called before the first frame update
    void Awake()
    {
        info = transform.GetChild(0).gameObject;
        panel = info.transform.GetChild(0).gameObject;
        textInfo = panel.transform.GetChild(0).gameObject;

        //info.SetActive(false);
        panel.SetActive(false);
        textInfo.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void onPointerEnter()
    {
        //info.SetActive(true);
        Debug.Log("enter");
        panel.SetActive(true);
        textInfo.SetActive(true);
    }

    public void onPointerExit()
    {
        //info.SetActive(false);
        panel.SetActive(false);
        textInfo.SetActive(false);
    }
}
