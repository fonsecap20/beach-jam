using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalArrow : MonoBehaviour
{
    private ShipController shipController;
    private GameObject goal;
    // Start is called before the first frame update
    void Start()
    {
        shipController = GameObject.FindGameObjectWithTag("Player").GetComponent<ShipController>();
        goal = GameObject.FindGameObjectWithTag("Goal");
        setSize();
    }

    private void setSize()
    {
        float scaleSize = shipController.GetMaxTetherLength() * 0.2875f;
        gameObject.transform.localScale = new Vector3 (scaleSize, scaleSize, 1f);
    }

    // Update is called once per frame
    void Update()
    {
        if(goal != null){
            // float angle = Vector2.Angle(gameObject.transform.position, goal.transform.position);
            float angle = 180/Mathf.PI*Mathf.Atan2(goal.transform.position.y-gameObject.transform.position.y,goal.transform.position.x-gameObject.transform.position.x);
            // Debug.Log("Angle to goal: "+angle);
            gameObject.transform.rotation = Quaternion.Euler(0, 0, angle);
        }
    }
}
