using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dice : MonoBehaviour
{
    public DiceSide side_Single;
    public DiceSide side_Double;
    public DiceSide side_Triple;
    
    public float throwForceRange;
    public float spinForceRange;
    public float shakePower;

    Rigidbody rb;

    public float totalPower;
    public float stuckThreshold;
    public float onFloorThreshold;
    public float doneRollingThreshold;

    Vector3 targetOffset;
    bool offsetSet;
    Vector3 targetPoint;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void RandomizeSides()
    {
        var allMana = new char[] {
            'r',
            'b',
            'g',
            'w',
            'u'//,
            //'s',
            //'f',
        };
        
        int sides = 6;
        for(int i = 0; i < sides; i++)
        {
            DiceSide sideObj = null;
            char[] manas = new char[0];

            var type = TAGD.ChooseRandom_Weighted(new string[] { "none", "single", "double", "triple" }, new int[] { 2, 8, 3, 1 });
            if(type == "single")
            {
                sideObj = side_Single;
                manas = new char[1];
                manas[0] = TAGD.ChooseRandom_Weighted(allMana, new int[] { 3, 3, 3, 1, 1 });

            }
            else if(type == "double")
            {
                sideObj = side_Double;
                manas = new char[2];
                manas[0] = TAGD.ChooseRandom_Weighted(allMana, new int[] { 3, 3, 3, 1, 1 });
                manas[1] = TAGD.ChooseRandom_Weighted(allMana, new int[] { 3, 3, 3, 1, 1 });
            }
            else if(type == "triple")
            {
                sideObj = side_Triple;
                manas = new char[3];
                manas[0] = TAGD.ChooseRandom_Weighted(allMana, new int[] { 3, 3, 3, 1, 1 });
                manas[1] = TAGD.ChooseRandom_Weighted(allMana, new int[] { 3, 3, 3, 1, 1 });
                manas[2] = TAGD.ChooseRandom_Weighted(allMana, new int[] { 3, 3, 3, 1, 1 });
            }
            if (sideObj != null)
            {
                var side = Instantiate(sideObj, transform);
                side.PutOnSide(i);
                side.SetMana(manas);
            }
        }
    }

    private void Update()
    {
        //if(Input.GetKeyDown(KeyCode.Mouse0))
        //{
        //    RandoToss();
        //}
        TotalEnergy();
    }

    public bool DoneRolling()
    {
        if(TotalEnergy() < doneRollingThreshold && transform.position.y < onFloorThreshold)
        {
            return true;
        }
        return false;
    }

    public bool Stuck()
    {
        if (TotalEnergy() < stuckThreshold && transform.position.y >= onFloorThreshold)
        {
            return true;
        }
        return false;
    }

    float TotalEnergy()
    {
        totalPower = rb.velocity.magnitude;
        totalPower += rb.angularVelocity.magnitude;

        return totalPower;
    }

    public void Nudge()
    {
        RandoToss();
    }

    public void Toss()
    {
        RandoToss();
    }

    public void SetTargetOffset(Vector3 offset)
    {
        targetOffset = offset;
        offsetSet = true;
    }

    public bool HasTargetOffset()
    {
        return offsetSet;
    }

    public void ClearTargetOffset()
    {
        targetOffset = Vector3.zero;
        offsetSet = false;
    }

    public void StartSpin()
    {
        var spin = Random.onUnitSphere * spinForceRange;
        rb.AddTorque(spin, ForceMode.Impulse);
    }

    public void SpinToOffsetPoint(Vector3 p)
    {
        var target = p + targetOffset;
        var forceDir = (target - transform.position).normalized;

        rb.AddForce(forceDir * shakePower, ForceMode.Impulse);
    }

    void RandoToss()
    {
        var force = Random.onUnitSphere;// * throwForceRange; // Random.Range(throwForceRange * 0.1f, throwForceRange);
        if (force.y < 0f) force.y *= -1f;
        if (force.y < 0.2f) force.y += 0.2f;

        force *= throwForceRange;

        var spin = Random.onUnitSphere * spinForceRange; //Random.Range(spinForceRange * 0.1f, spinForceRange);

        rb.AddTorque(spin, ForceMode.Impulse);
        rb.AddForce(force, ForceMode.Impulse);
    }
}


