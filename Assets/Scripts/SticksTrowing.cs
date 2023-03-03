using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SticksTrowing : MonoBehaviour
{
    [SerializeField] private GameControl gameControlScript;
    [SerializeField] private Transform[] sticks;
    [SerializeField] private Button trowButton;
    private Vector3[] defoultPositionSticks = new Vector3[4];
    [SerializeField] private float trowForce;
    [SerializeField] private float rotateForce;
    private int totalPoint;
    private int pointSenderCount;
    [SerializeField] private TextMeshPro totalPoitnText;

    private void Start()
    {
        for (int i = 0; i < sticks.Length; i++)
        {
          defoultPositionSticks[i] = sticks[i].position;
        }
    }
    public void SendPoint(int point)
    {
        pointSenderCount++;
        totalPoint += point;
        if (pointSenderCount == 4 && totalPoint != 0)
        {
            totalPoitnText.text = totalPoint.ToString() + " Points";
            gameControlScript.isMovingFigure = true;
            gameControlScript.SetStepsCount(totalPoint);
        }
        if (pointSenderCount == 4 && totalPoint == 0)
        {
            gameControlScript.isMovingFigure = true;
            totalPoint = 5;
            totalPoitnText.text = "5 Points";
            gameControlScript.SetStepsCount(totalPoint);
        }
    }

    public void TrowStiks()
    {
            totalPoitnText.text = null;
            trowButton.interactable = false;
            for (int i = 0; i < sticks.Length; i++)
            {
                sticks[i].position = defoultPositionSticks[i];
            }
            pointSenderCount = 0;
            totalPoint = 0;

            for (int i = 0; i < sticks.Length; i++)
            {
                sticks[i].GetComponent<Stick>().IsSend = false;
            }
            for (int i = 0; i < sticks.Length; i++)
            {
                sticks[i].GetComponent<Rigidbody>().AddForce(Vector3.up * trowForce * Random.Range(5, 10), ForceMode.Impulse);
                sticks[i].GetComponent<Rigidbody>().AddTorque(sticks[i].right * rotateForce * Random.Range(5, 10), ForceMode.Impulse);
            }
    }
}
