using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectedIconInfo : MonoBehaviour {

    public Unit Parent;

    public void ShowInfo()
    {
        GameManager.Instance.ShowUnitInfo(Parent);
    }

    public void SideInfo()
    {
        GameManager.Instance.SizeText.gameObject.SetActive(false);
    }

    public void OnDestroy()
    {
        if(Parent == GameManager.Instance.unitForInfo)
        {
            GameManager.Instance.SizeText.gameObject.SetActive(false);
        }
    }
}
