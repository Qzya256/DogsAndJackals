using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stick : MonoBehaviour
{
    [SerializeField] private SticksTrowing sticksTrowingScript;
    [SerializeField] private SideResults selectedResult;
    [SerializeField] private int selectedVector;
    // Note: the size of the vectorValues and vectorPoints should be the same.
    [SerializeField] private SideResults[] vectorValues;
    [SerializeField] private Vector3[] vectorPoints; // These vectors should be normalized. Might be worth adding a task to Start to ensure they are normalized.
    public bool IsSend;

    void Start () {
        IsSend = true;
    }

	void Update ()
	{
        if (GetComponent<Rigidbody>().IsSleeping() && !IsSend)
        {
            if (selectedResult == SideResults.White)
            {
                sticksTrowingScript.SendPoint(1);
            }
            else if (selectedResult == SideResults.Black)
            {
                sticksTrowingScript.SendPoint(0);
            }
            IsSend = true;
        }
	    float bestDot = -1;
        for(int i = 0; i < vectorPoints.Length; ++i)
        {
            var valueVector = vectorPoints[i];
            // Each side vector is in local object space. We need them in world space for our calculation.
	        var worldSpaceValueVector = this.transform.localToWorldMatrix.MultiplyVector(valueVector);
            // Mathf.Arccos of the dot product can be used to get the angle of difference. You can use this to check for a tilt (perhaps requiring a reroll)
            float dot = Vector3.Dot(worldSpaceValueVector, Vector3.up);
            if (dot > bestDot)
            {
                // The vector with the greatest dot product is the vector in the most "up" direction. This is the current face selected.
                bestDot = dot;
                selectedVector = i;
            }
        }

	    selectedResult = vectorValues[selectedVector];
	}

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        foreach (var valueVector in vectorPoints)
        {
            var worldSpaceValueVector = this.transform.localToWorldMatrix.MultiplyVector(valueVector);
            Gizmos.DrawLine(this.transform.position, this.transform.position + worldSpaceValueVector);
        }
    }
}

// Enum for storing the potential results.
public enum SideResults
{
    White,
    Black,
}