using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

[RequireComponent(typeof(CharacterAttributes))]
[RequireComponent(typeof(Inventory))]
public class GameCharacter : MonoBehaviour
{
    [Header("GameCharacter")]
    [Header("SurfaceRecognition")]
    public bool autoCheckSurfaceType;
    public LayerMask surfaceRecognitionLayer;
    GameObject currentTerrain;
    [SerializeField]
    protected float surfaceCheckFrequency = 1f;
    protected float nextSurfaceCheck;


    public SurfaceType currentSurface;
    public SurfaceType defaultSurfaceType;

    [SerializeField]
    public float normalSpeed = 2;
    [SerializeField]
    public float crouchSpeed = 1.5f;
    [SerializeField]
    public float runningSpeed = 4f;

    [HideInInspector]
    public float movementSpeed;
    [SerializeField]
    public bool isMoving;
    [SerializeField]
    protected bool isCrouching;
    [HideInInspector]
    public bool isSprinting;
    [SerializeField]
    protected Vector3 inventoryPosition;
    [SerializeField]
    protected Inventory inventory;
    protected CharacterAttributes charAttributes;
    [HideInInspector]
    public CharacterAnimationController charAnim;

   


    virtual public void Start()
    {
        inventory = GetComponent<Inventory>();
        charAttributes = GetComponent<CharacterAttributes>();
        charAttributes.RecalculateStats();
    }

    private void LateUpdate()
    {
        //if (autoCheckSurfaceType && Time.time > nextSurfaceCheck)
        //{
        //    CheckSurfaceType();
        //    nextSurfaceCheck = Time.time + surfaceCheckFrequency;
        //}
    }

    public IEnumerator MovementDetection()
    {
        Vector3 pos1 = transform.position;
        yield return new WaitForSeconds(0.03f);
        Vector3 pos2 = transform.position;

        if (Vector3.Distance(pos1, pos2) > 0.025f)
        {
            isMoving = true;
        }

        else isMoving = false;
    }

    public void ToggleInventory(bool toggle)
    {
        inventory.DrawInventory(toggle, inventoryPosition);
    }

    protected float GetDistance(Vector3 pos1, Vector3 pos2)
    {
        return Vector3.Distance(pos1, pos2);
    }

    public List<VirtualItem> GetInventoryItems()
    {
        return inventory.GetInventoryItems();
    }

    public void DealDamage(Transform target)
    {
        target.SendMessage("GetDamage", charAttributes.dmg);
    }
    public void GetDamage(int damage)
    {
        charAttributes.hp -= damage;

        if(charAttributes.hp <= 0)
        {
            if (Camera.main.GetComponent<CameraController>().cameraTarget == gameObject.transform)
            {
                //  UI_PlayerUI.instance.playerHp.text = "Dead";
                UI_PlayerUI.instance.UpdateHealthbar(0, charAttributes.maxHp);
            }

            charAttributes.hp = 0;
            Destroy(gameObject);


        }
    }

    public void CheckSurfaceType()
    {
        RaycastHit surfaceHit;
        if (Physics.Raycast(transform.position, Vector3.down, out surfaceHit, 1.1f, surfaceRecognitionLayer))
        {
            //if we hit object with defined surface
            if (surfaceHit.collider.GetComponent<SurfaceComponent>())
            {
                //some surface
                SurfaceComponent surface = surfaceHit.collider.GetComponent<SurfaceComponent>();
                if (surface.surfaceType != null && !surface.surfaceType.IsTerrain())
                {
                    currentSurface = surface.surfaceType;
                    Debug.DrawRay(surfaceHit.point, Vector3.up * 2, Color.green, 0.5f);
                    currentTerrain = null;
                }
                //terrain hit
                else if (surface.surfaceType != null && surface.surfaceType.IsTerrain())
                {
                    currentSurface = surface.surfaceType;
                    currentTerrain = surfaceHit.collider.gameObject;
                }
                //else set default surfaceType to this character
                else
                {
                    currentSurface = defaultSurfaceType;
                    Debug.DrawRay(surfaceHit.point, Vector3.up * 2, Color.red, 0.5f);
                    currentTerrain = null;
                }
            }
            /*
            //if we hit terrain but it has not SurfaceComponent for some reason. We want to save this terrain and use it.
            else if (gameObject.GetComponent<Terrain>())
            {
                Debug.Log("asdasd");
                currentSurface = defaultSurfaceType;
                currentTerrain = surfaceHit.collider.gameObject;
                currentTerrain = null;
            }
            */

            // else apply default surface type
            else
            {
                currentSurface = defaultSurfaceType;
                Debug.DrawRay(surfaceHit.point, Vector3.up * 2, Color.yellow, 0.5f);
                currentTerrain = null;
            }
        }

        // if we hit nothing apply default surface type
        else
        {
            currentSurface = defaultSurfaceType;
            currentTerrain = null;
        }
    }

    public GameObject GetCurrentTerrain()
    {
        return currentTerrain;
    }
}
