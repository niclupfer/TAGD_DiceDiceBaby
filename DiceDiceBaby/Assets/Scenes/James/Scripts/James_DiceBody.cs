using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class James_DiceBody : MonoBehaviour
{
    public int numOfFaces = 6;
    public Side upwardsFace;
    public Side[] faceList;

    void Start()
    {
        if(faceList == null)
            faceList = new Side[numOfFaces];
    }

    private void OnDrawGizmos()
    {
        foreach(Side s in faceList)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawRay(this.transform.position, transform.TransformDirection(s.diceFaceNormal));
        }
    }

    private void Update()
    {
        upwardsFace = CheckWhichSideIsUp();
    }

    public Side CheckWhichSideIsUp()
    {
        Side upSide = faceList[1];
        foreach(Side s in faceList)
        {
            Vector3 up = Vector3.up;
            up = up - transform.TransformDirection(s.diceFaceNormal);

            if (upSide.diceFaceNormal.magnitude > up.magnitude)
                upSide = s;
        }

        return upSide;
    }
}

[System.Serializable]
public class Side
{
    public James_DiceFace diceFace;
    public Vector3 diceFaceNormal;
}
