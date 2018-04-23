using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAnimationController : MonoBehaviour
{
    public float blendSmoothing = 1f;
    public Vector3 movementDirection;
    public Vector3 forwardDirection;

    [SerializeField]
    GameCharacter gameCharacter;
    //[SerializeField]
    public Animator targetAnimator;


    Vector2 movingVector;

	// Use this for initialization
	void Awake ()
    {
		if(gameCharacter == null)
        {
            gameCharacter = GetComponent<GameCharacter>();
        }

        gameCharacter.charAnim = this;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateBooleans();
        CalculateNormalizedDirection();
    }

    public void PickUpAnimation()
    {
        targetAnimator.SetBool("PickUp", true);
    }

    public void UpdateBooleans()
    {
        targetAnimator.SetBool("Moving", gameCharacter.isMoving);
        targetAnimator.SetBool("Sprint", gameCharacter.isSprinting);
    }

    void CalculateNormalizedDirection()
    {
        forwardDirection = transform.TransformDirection(Vector3.forward).normalized;
        forwardDirection = transform.TransformDirection(Vector3.forward);

        Debug.DrawRay(transform.position, forwardDirection * 3);
        Debug.DrawRay(transform.position, movementDirection * 3);

        Debug.DrawRay(transform.position, Vector3.back * 2, Color.yellow);
        Debug.DrawRay(transform.position, Vector3.forward * 2, Color.blue);
        Debug.DrawRay(transform.position, Vector3.left * 2, Color.red);
        Debug.DrawRay(transform.position, Vector3.right * 2, Color.green);

        float dirAngle = Vector3.Angle(forwardDirection, movementDirection);

        float fwd = 0f;
        float side = 0f;

        if (dirAngle > 150f)
        {
            fwd = -1f;
            //targetAnimator.SetFloat("MovDirFwd", -1f);
        }
        else if(dirAngle < 120f &&
            dirAngle > 60f)
        {
            fwd = 1f;
            side = 1f;
           // targetAnimator.SetFloat("MovDirFwd", 1f);
           // targetAnimator.SetFloat("MovDirSide", 1f);
        }

        else
        {
            fwd = 1f;
            side = 0f;
            // targetAnimator.SetFloat("MovDirFwd", 1f);
            // targetAnimator.SetFloat("MovDirSide", 0f);
        }

        movingVector.x = Mathf.Lerp(movingVector.x, side, blendSmoothing * Time.deltaTime);
        movingVector.y = Mathf.Lerp(movingVector.y, fwd, blendSmoothing * Time.deltaTime);

        targetAnimator.SetFloat("MovDirFwd", movingVector.y);
        targetAnimator.SetFloat("MovDirSide", movingVector.x);
    }


    public void UpdateAimingState(bool aiming)
    {
        if (!aiming) targetAnimator.SetLayerWeight(1, 0);
        else targetAnimator.SetLayerWeight(1, 1);

        targetAnimator.SetBool("Aiming", aiming);
    }

    public void UseItem()
    {
        targetAnimator.SetBool("Use", true);
    }
}
