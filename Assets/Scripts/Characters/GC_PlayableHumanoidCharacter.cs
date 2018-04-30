using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(CapsuleCollider))]

public class GC_PlayableHumanoidCharacter : GC_HumanoidCharacter
{
    public enum movStates { Walking, Sprinting, Crouching, Standing}

    [Header("Player Control")]
    public bool playerControl = false;
    public movStates movementState;


    [Header("Movement")]
    public float dashForce = 200f;
    public float dashRate = 1f;
    public float gravity = 10f;
    public float maxVelocityChange = 10f;
    public float rotationSpeed = 10f;
    public Vector3 targetVelocity;

    float nextDash;
    bool lookAtMouse;
    bool grounded = true;
    Rigidbody rb;
    RaycastHit mouseRaycastHit;
    public LayerMask mouseRaycastLayer;

    [Header("Interaction")]
    public LayerMask detectLayer;
    List<Transform> nearbyEntities;

    // Reference to audio and animation script
    AudioCharacter sound;

    // Use this for initialization
    public new void Start ()
    {
        sound = GetComponent<AudioCharacter>();
        animationController = GetComponent<CharacterAnimationController>();
        charAttributes = GetComponent<CharacterAttributes>();

        rb = GetComponent<Rigidbody>();
        inventory = GetComponent<Inventory>();

        if (playerControl) ToggleInventory(true);
    }

    // Update is called once per frame
    void Update()
    {
        if (playerControl)
        {

            StartCoroutine("MovementDetection");
            PlayerInputReader();

            /*
            UI_PlayerUI.instance.playerHp.text = "HP: " + charAttributes.hp + " / " + charAttributes.maxHp;
            if(charAttributes.hp <= 0) UI_PlayerUI.instance.playerHp.text = "DEAD";
            */
            UI_PlayerUI.instance.UpdateHealthbar(charAttributes.hp, charAttributes.maxHp);

            if (lootInv != null && isMoving)
            {
                // But we have Loot Inventory stored, Don't draw it and remove from our variable
                lootInv.DrawInventory(false, Vector3.zero);
                lootInv = null;
            }

            nearbyEntities = DetectNearbyEntities();
        }

        else
        {
            if (lootInv != null)
            {
                // But we have Loot Inventory stored, Don't draw it and remove from our variable
                lootInv.DrawInventory(false, Vector3.zero);
                lootInv = null;
            }
        }

    }

    void FixedUpdate ()
    {
        if (playerControl)
        {
            animationController.UpdateAimingState(aiming);
            HumanoidCharacterMovement();
            CharacterRotation();
            PlayerMovementStates();
        }
    }

    void PlayerInputReader()
    {
        VirtualItem selectedItem = inventory.GetInventoryItems()[inventory.GetSelectedSlot()];

        // Item selection
        if (Input.GetButtonDown("SelectSlot0")) inventory.SetSelectedSlot(0);
        else if (Input.GetButtonDown("SelectSlot1") && inventory.GetInventoryItems().Count >= 1) inventory.SetSelectedSlot(1);
        else if (Input.GetButtonDown("SelectSlot2") && inventory.GetInventoryItems().Count >= 2) inventory.SetSelectedSlot(2);
        else if (Input.GetButtonDown("SelectSlot3") && inventory.GetInventoryItems().Count >= 3) inventory.SetSelectedSlot(3);
        else if (Input.GetButtonDown("SelectSlot4") && inventory.GetInventoryItems().Count >= 4) inventory.SetSelectedSlot(4);
        else if (Input.GetButtonDown("SelectSlot5") && inventory.GetInventoryItems().Count >= 5) inventory.SetSelectedSlot(5);
        else if (Input.GetButtonDown("SelectSlot6") && inventory.GetInventoryItems().Count >= 6) inventory.SetSelectedSlot(6);
        else if (Input.GetButtonDown("SelectSlot7") && inventory.GetInventoryItems().Count >= 7) inventory.SetSelectedSlot(7);
        else if (Input.GetButtonDown("SelectSlot8") && inventory.GetInventoryItems().Count >= 8) inventory.SetSelectedSlot(8);
        else if (Input.GetButtonDown("SelectSlot9") && inventory.GetInventoryItems().Count >= 9) inventory.SetSelectedSlot(9);
        //Item Selection via scrolling
        if (Input.GetAxis("Mouse ScrollWheel") < 0) inventory.SetSelectedSlot(inventory.GetSelectedSlot() + 1);
        if (Input.GetAxis("Mouse ScrollWheel") > 0) inventory.SetSelectedSlot(inventory.GetSelectedSlot() - 1);




        if (Input.GetButtonDown("Fire1") && !aiming)
        {
            GameObject clickedObject = mouseRaycastHit.collider.gameObject;

            if (GetDistance(transform.position, clickedObject.transform.position) < lootingDistance)
            {
                if (clickedObject.CompareTag("ItemPickUpContainer"))
                {
                    TakeItem(clickedObject, true);
                }

                /*
                   else if (clickedObject.CompareTag("Interaction"))
                  {
                      if (clickedObcject.GetComponent<InteractibleDoor>())
                      {
                          clickedObject.GetComponent<InteractibleDoor>().DoorToggleOpen();
                      }
                  }
                 */

                else if (clickedObject.gameObject.GetComponent<LootInventory>()
               && lootInv != clickedObject.gameObject.GetComponent<LootInventory>())
                {
                    // Whether this is the LootInventory stored or not
                    if (clickedObject.GetComponent<LootInventory>() && lootInv != null)
                    {
                        lootInv.DrawInventory(false, Vector3.zero);
                        lootInv = clickedObject.GetComponent<LootInventory>();
                        lootInv.DrawInventory(true, Vector3.zero);
                    }

                    else if (clickedObject.GetComponent<LootInventory>() && lootInv == null)
                    {
                        lootInv = clickedObject.GetComponent<LootInventory>();
                        lootInv.DrawInventory(true, Vector3.zero);
                    }
                }
            }
        }

        //Use slectedItem
        else if (Input.GetButtonDown("Fire1") && aiming)
        {
            Vector3 directionToMouse =  mouseRaycastHit.point - transform.position;
            if (handsPoint.GetComponentInChildren<Usable>().CanUse())
            {
                handsPoint.GetComponentInChildren<Usable>().Use(transform, directionToMouse.normalized);
                animationController.UseItem();
            }
            /* whole itemAbility system is obsolete after ItemPrefab update (JF)  30.03.2018
            ItemAbility ability = selectedItem.GetItemAbility();

            switch (ability.GetAbility())
            {
                case abilities.Heal:
                    if (selectedItem.GetItemWeaponAimed()) ability.Heal();
                    break;
                case abilities.ShootProjectile:
                    if (selectedItem.GetItemWeaponAimed()) ability.ShootProjectile(transform);
                    break;
                case abilities.MeleeAttack:
                    if (selectedItem.GetItemWeaponAimed()) ability.MeleeAttack();
                    break;
            }
            */
        }

        //Aiming
        if (Input.GetButton("Fire2"))
        {
        selectedItem.SetItemWeaponAimed(true);
        aiming = true;
        }

        else
          {
            selectedItem.SetItemWeaponAimed(false);
            aiming = false;
          }

        //Dash
        if (Input.GetButtonDown("Dash"))
        {
            Dash();
        }
    }
        
    public void PlayerMovementStates()
    {       
            if (Input.GetButtonDown("ToggleCrouch"))
            {
                isCrouching = !isCrouching;

                CapsuleCollider col = GetComponent<CapsuleCollider>();

                if (isCrouching)
                {
                    col.height = 1;
                    movementState = movStates.Crouching;
                }

                else
                {
                    col.height = 1.7f;
                }
            }

            else if (Input.GetButton("Sprint") && isMoving && !isCrouching && !aiming)
            {
                movementState = movStates.Sprinting;
            }

            else if(!isCrouching)
            {
                if (!isMoving) movementState = movStates.Standing;
                else movementState = movStates.Walking;
            }

        charAnim.movementDirection = targetVelocity.normalized;

           

        switch (movementState)
        {

            case movStates.Standing:
                isSprinting = false;

                lookAtMouse = true;
                movementSpeed = normalSpeed;
                break;

            case movStates.Walking:
                isSprinting = false;

                lookAtMouse = true;
                movementSpeed = normalSpeed;
                break;

            case movStates.Sprinting:
                isSprinting = true;

                lookAtMouse = false;

                movementSpeed = runningSpeed;
                break;

            case movStates.Crouching:
                isSprinting = false;

                lookAtMouse = true;
                movementSpeed = crouchSpeed;
                break;
        }

        movementSpeed = movementSpeed * currentSurface.GetHumanoidMovementMultiplier();
    }

    public void Dash()
    {
        if (Time.time > nextDash)
        {
            sound.PlayDash();

            if (isMoving)
            {
                Vector3 dashVector = targetVelocity.normalized * dashForce;
                rb.AddForce(dashVector * dashForce);
            }

            else
            {
                Vector3 dashVector = transform.TransformDirection(-Vector3.forward * dashForce);
                rb.AddForce(dashVector * dashForce);
            }

            nextDash = Time.time + dashRate;
        }

    }

      public override void HumanoidCharacterMovement()
      {      

          if (grounded)
          {
              // Input
              targetVelocity = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));

              // Normalize vector, affection of momevemnt speed in seconds
              targetVelocity = targetVelocity.normalized * movementSpeed * Time.deltaTime;

              rb.MovePosition(transform.position + targetVelocity);
          }
            if(!Physics.Raycast(transform.position, Vector3.down, 0.6f))rb.AddForce(new Vector3(0, -gravity * 1000, 0));

    }
      void CharacterRotation()
      {
         // Rotate to mouse position
          if(lookAtMouse)
          {
              // Calculate direction from character to mouse position
              Vector3 direction = (new Vector3(MousePosition().x, transform.position.y, MousePosition().z) - transform.position);

              // Direction to rotation
              Quaternion targetRotation = Quaternion.LookRotation(direction, transform.position);
              targetRotation.x = 0;
              targetRotation.z = 0;
              // Spherical interpolation to desired rotation
              transform.rotation = Quaternion.SlerpUnclamped(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

          }

          //Rotate character to velocity vector
          else
          {
              // Set previous character rotation as new rotation
              Quaternion targetRotation = transform.rotation;
              // Calculate direction from character velocity vector
              Vector3 direction = new Vector3(targetVelocity.x, 0.0f, targetVelocity.z);

              // Direction to rotation
              if (direction != Vector3.zero)
                  targetRotation = Quaternion.LookRotation(direction);

              // Spherical interpolation to desired rotation
              transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
          }

      }

      List<Transform> DetectNearbyEntities()
      {

          Collider[] colliders = Physics.OverlapSphere(transform.position, lootingDistance, detectLayer);
          List<Transform> relevant = new List<Transform>();

          foreach (Collider col in colliders)
          {
              relevant.Add(col.transform);
          }
          return relevant;
      }

      public RaycastHit GetMouseRaycastHit()
      {
          return mouseRaycastHit;
      }

      public Vector3 MousePosition()
      {
          // Get mouse position on Main Camera screen
          Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
          // If Raycast hit something, return hit position
          if (Physics.Raycast(ray, out mouseRaycastHit, Mathf.Infinity, mouseRaycastLayer))
          {
              return mouseRaycastHit.point;
          }

          // If not calculate direction from Character position to mouse position on screen
          else
          {
              Vector3 mousePos = ray.origin + ray.direction * Vector3.Distance(ray.origin, transform.position);
              return mousePos;
          }

          /*
          // If Raycast hit something, return hit position
          if (Physics.Raycast(ray, out mouseRaycastHit))
          {
              Vector3 mousePos = ray.origin + ray.direction * Vector3.Distance(ray.origin, transform.position);
              return mousePos;
          }

          // If not calculate direction from Character position to mouse position on screen
          else
          {
              Vector3 mousePos = ray.origin + ray.direction * Vector3.Distance(ray.origin, transform.position);
              return mousePos;
          }
          */
            }
#if UNITY_EDITOR
    public override void OnDrawGizmos()
    {
        Vector3 pos = transform.position;
        pos.y = transform.position.y - 1f;

        Handles.color = Color.yellow;
        Handles.DrawWireDisc(pos, Vector3.up, lootingDistance);

        if (playerControl)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(MousePosition(), 0.1f);
            Gizmos.DrawLine(pos, MousePosition());

            Gizmos.color = Color.yellow;
            Gizmos.DrawRay(pos, targetVelocity * 10);
            /*
            for(int i = 0; i < nearbyEntities.Count; i++)
            {
                Gizmos.DrawLine(transform.position, nearbyEntities[i].position);
            }
            */

            Handles.Label(transform.position, movementState + "(aiming:" + aiming + ")");
        }

        if (handsPoint) Gizmos.DrawSphere(handsPoint.transform.position, 0.1f);
    }
#endif
}
