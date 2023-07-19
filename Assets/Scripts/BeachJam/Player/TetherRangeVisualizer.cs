using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TetherRangeVisualizer : MonoBehaviour
{
    private ShipController shipController;
    void Start()
    {
        shipController = GameObject.FindGameObjectWithTag("Player").GetComponent<ShipController>();
        setSize();
    }

    private void setSize()
    {
        float scaleSize = shipController.GetMaxTetherLength() * 2f;
        gameObject.transform.localScale = new Vector3 (scaleSize, scaleSize, 1f);
    }
}
