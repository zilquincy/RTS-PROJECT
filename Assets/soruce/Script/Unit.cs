using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{

    void Start()
    {
        UnitSelectManager.Instance.allUnitsList.Add(gameObject);
    }

    private void OnDestroy()
    {
        UnitSelectManager.Instance.allUnitsList.Remove(gameObject);
    }

}
