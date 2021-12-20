using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorHumanoid : MonoBehaviour
{
    [Header("Main")]
    [Range(-1, 1)]
    public int mirrored = 1;
    [Space]
    [Header("Lower Body")]
    [SerializeField]
    Transform rightLeg;
    [SerializeField]
    Transform rightLegTarget;
    [SerializeField]
    Transform rightLegSolver;
    [SerializeField]
    Transform rightTargetParent;
    RaycastHit2D [] rightLegHit = new RaycastHit2D [1];
    [SerializeField]
    Transform leftLeg;
    [SerializeField]
    Transform leftLegTarget;
    [SerializeField]
    Transform leftLegSolver;
    [SerializeField]
    Transform leftTargetParent;
    RaycastHit2D [] leftLegHit = new RaycastHit2D [1];
    public float legLength = 1;
    public float kneeLength = 1;
    public float maxLegLength = 1.8f;
    public float legsMoveRotationScaler = 3;
    public Vector3 legsCurlPosRight = new Vector3(0.3f, -0.65f, 0);
    public Vector3 legsCurlPosLeft = new Vector3(-0.3f, -0.65f, 0);
    [Space]
    [Header("Upper Body")]
    [SerializeField]
    Transform head;
    public float maxHeadDegree = 50;
    public float minHeadDegree = -45;
    public bool rightHandOccupied = false;
    public bool leftHandOccupied = false;
    public float handsTargetRotationScaler = 3;
    [SerializeField]
    Transform weaponParent;
    public Weapon weapon;
    [SerializeField]
    Transform rightHandSolver;
    [SerializeField]
    Transform rightHandIdleTarget;
    [SerializeField]
    Transform rightHandIdleTrashTarget;
    [SerializeField]
    Transform rightHandTargetParent;
    [SerializeField]
    Transform leftHandSolver;
    [SerializeField]
    Transform leftHandIdleTarget;
    [SerializeField]
    Transform leftHandIdleTrashTarget;
    [SerializeField]
    Transform leftHandTargetParent;

    public Transform ammoPoint;

    public bool grounded = false;
    public int maxJumps = 1;
    public int jumps = 0;

    float recoilGrad = 0;

    public Logic logic;
    int rightLegArrLength;
    int leftLegArrLength;

    ContactFilter2D walkableFilter;
    private void Awake()
    {
        weapon.animator = this;
        walkableFilter = Manager.instance.walkableFilter;
    }
    //Legs movement
    #region
    void LegsKneelingWalk()
    {
        /*
        //Move Right Leg
        rightLeg.up = -(rightLegTarget.position - rightLeg.position);
        if (rightLegArrLength > 0)
        {
            if (Vector2.Distance(rightLeg.position, rightLegTarget.position) < maxLegLength)
            {
                rightLegSolver.position = rightLegTarget.position;
            }
            else if (Vector2.Distance(rightLeg.position, rightLegHit[0].point) > 0)
            {
                rightLegSolver.position = rightLegHit[0].point;
            }
        }
        else
        {
            if (Vector2.Distance(rightLeg.position, rightLegTarget.position) < maxLegLength)
            {
                rightLegSolver.position = rightLegTarget.position;
            }
            else
            {
                rightLeg.localRotation = Quaternion.identity;
            }
        }


        //Move Left Leg

        leftLeg.up = -(leftLegTarget.position - leftLeg.position);

        if (leftLegArrLength > 0)
        {
            if (Vector2.Distance(leftLeg.position, leftLegTarget.position) < maxLegLength)
            {
                leftLegSolver.position = leftLegTarget.position;
            }
            else if (Vector2.Distance(leftLeg.position, leftLegHit[0].point) > 0)
            {
                leftLegSolver.position = leftLegHit[0].point;
            }
        }
        else
        {
            if (Vector2.Distance(leftLeg.position, leftLegTarget.position) < maxLegLength)
            {
                leftLegSolver.position = leftLegTarget.position;
            }
            else
            {
                leftLeg.localRotation = Quaternion.identity;
            }
        }
        */

        //Move Right Leg
        rightLeg.up = -(rightLegTarget.position - rightLeg.position);

        if (rightLegArrLength > 0)
        {
            if (Vector2.Distance(rightLeg.position, rightLegTarget.position) < Vector2.Distance(rightLeg.position, rightLegHit[0].point))
            {
                rightLegSolver.position = rightLegTarget.position;
            }
            else
            {
                rightLegSolver.position = rightLegHit[0].point;
            }
        }


        //Move Left Leg
        leftLeg.up = -(leftLegTarget.position - leftLeg.position);

        if (leftLegArrLength > 0)
        {
            if (Vector2.Distance(leftLeg.position, leftLegTarget.position) < Vector2.Distance(leftLeg.position, leftLegHit[0].point))
            {
                leftLegSolver.position = leftLegTarget.position;
            }
            else
            {
                leftLegSolver.position = leftLegHit[0].point;
            }
        }
    }
    void LegsKneelingCrouch()
    {
        //Move Right Leg
        rightLeg.up = -(rightLegTarget.position - rightLeg.position);

        if (rightLegArrLength > 0)
        {
            if (Vector2.Distance(rightLeg.position, rightLegHit[0].point) > 0)
            {
                rightLegSolver.position = rightLegHit[0].point;
            }
        }
        else
        {
            if (Vector2.Distance(rightLeg.position, rightLegTarget.position) < maxLegLength)
            {
                rightLegSolver.position = rightLegTarget.position;
            }
            else
            {
                rightLeg.localRotation = Quaternion.identity;
            }
        }


        //Move Left Leg
        leftLeg.up = -(leftLegTarget.position - leftLeg.position);

        if (leftLegArrLength > 0)
        {
            if (Vector2.Distance(leftLeg.position, leftLegHit[0].point) > 0)
            {
                leftLegSolver.position = leftLegHit[0].point;
            }
        }
        else
        {
            if (Vector2.Distance(leftLeg.position, leftLegTarget.position) < maxLegLength)
            {
                leftLegSolver.position = leftLegTarget.position;
            }
            else
            {
                leftLeg.localRotation = Quaternion.identity;
            }
        }
    }
    public void StepOnGround()
    {
        // Step Right Leg
        if (Vector2.Distance(rightLeg.position, rightLegTarget.position) < Vector2.Distance(rightLeg.position, rightLegHit[0].point))
        {
            rightLeg.transform.rotation = Quaternion.Euler(0, 0, 0);

            if (rightLegArrLength > 0)
            {
                rightLegSolver.position = rightLegHit[0].point;
            }
        }

        // Step Left Leg
        if (Vector2.Distance(leftLeg.position, leftLegTarget.position) < Vector2.Distance(leftLeg.position, leftLegHit[0].point))
        {
            leftLeg.transform.rotation = Quaternion.Euler(0, 0, 0);

            if (leftLegArrLength > 0)
            {
                leftLegSolver.position = leftLegHit[0].point;
            }
        }
    }
    public void CurlLegs()
    {
        rightLegSolver.localPosition = legsCurlPosRight;
        leftLegSolver.localPosition = legsCurlPosLeft;
    }
    #endregion

    void Update()
    {
        rightLegArrLength = Physics2D.Raycast(rightLeg.position, Vector2.down, walkableFilter, rightLegHit, maxLegLength);
        leftLegArrLength = Physics2D.Raycast(leftLeg.position, Vector2.down, walkableFilter, leftLegHit, maxLegLength);
        if (rightLegArrLength + leftLegArrLength > 0) 
        {
            grounded = true;
            jumps = 0;
            if (logic.currentSpeed != 0)
            {
                rightTargetParent.Rotate(Vector3.back * legsMoveRotationScaler * logic.currentSpeed * Time.deltaTime);
                leftTargetParent.Rotate(Vector3.back * legsMoveRotationScaler * logic.currentSpeed * Time.deltaTime);
                //leftHandTargetParent.Rotate(Vector3.back * handsTargetRotationScaler * logic.currentSpeed * Time.deltaTime);
                //rightHandTargetParent.Rotate(Vector3.back * handsTargetRotationScaler * logic.currentSpeed * Time.deltaTime);


                rightLegArrLength = Physics2D.Raycast(rightLeg.position, rightLegTarget.position - rightLeg.position, walkableFilter, rightLegHit, maxLegLength);
                leftLegArrLength = Physics2D.Raycast(leftLeg.position, leftLegTarget.position - leftLeg.position, walkableFilter, leftLegHit, maxLegLength);

                if (Mathf.Abs(logic.currentSpeed) < logic.speed)
                {
                    LegsKneelingCrouch();
                }
                else
                {
                    LegsKneelingWalk();
                }
            }
            else
            {
                StepOnGround();
            }
        }
        else
        {
            grounded = false;
            CurlLegs();
        }

        if (Mathf.Abs(recoilGrad) > 0.5f)
        {
            if (recoilGrad > 0)
            {
                recoilGrad -= 0.5f;
            }
            else
            {
                recoilGrad += 0.5f;
            }
        }
        else
        {
            recoilGrad = 0;
        }
    }
    public void Look(Vector2 mouseDelta)
    {
        //Mirror body
        if (mouseDelta.x - transform.position.x < 0)
        {
            transform.localScale = new Vector3(-1, 1, 1);
            mirrored = -1;
        }
        else
        {
            transform.localScale = new Vector3(1, 1, 1);
            mirrored = 1;
        }

        //Look Head
        float AngleRad = Mathf.Atan2((mouseDelta.y - head.position.y) * mirrored, Mathf.Abs(mouseDelta.x - head.position.x));
        float AngleDeg = (180 / Mathf.PI) * AngleRad + 90;
        if (AngleDeg > maxHeadDegree + 90) AngleDeg = maxHeadDegree + 90;
        if (AngleDeg < minHeadDegree + 90) AngleDeg = minHeadDegree + 90;
        if (mirrored < 0) AngleDeg -= 180;
        head.rotation = Quaternion.Euler(0, 0, AngleDeg);

        //Look Weapon
        AngleRad = Mathf.Atan2((mouseDelta.y - weaponParent.position.y) * mirrored, Mathf.Abs(mouseDelta.x - weaponParent.position.x));
        AngleDeg = (180 / Mathf.PI) * AngleRad;
        weaponParent.rotation = Quaternion.Euler(0, 0, AngleDeg + recoilGrad);

        MoveHands();
    }
    public void Recoil(float recoil)
    {
        recoilGrad = Random.Range(-recoil, recoil);
        weaponParent.localEulerAngles = new Vector3(0, 0, weaponParent.localEulerAngles.z + recoilGrad);
        MoveHands();
    }

    void MoveHands()
    {
        //Right Hand movement
        if (rightHandOccupied)
        {
            rightHandSolver.position = weapon.rightHandTarget.position;
        }
        else
        {
            //rightHandSolver.position = leftLegTarget.position;
            //rightHandSolver = rightHandIdleTrashTarget;
            //rightHandSolver.position = new Vector2(rightHandIdleTarget.position.x, rightHandSolverParent.position.y);
        }



        //Left Hand movement
        if (leftHandOccupied)
        {
            leftHandSolver.position = weapon.leftHandTarget.position;
        }
        else
        {
            //leftHandSolver.position = rightLegTarget.position;
            //leftHandSolver = leftHandIdleTrashTarget;
            //leftHandSolver.position = new Vector2(leftHandIdleTarget.position.x, leftHandSolverParent.position.y);
        }
    }
}
