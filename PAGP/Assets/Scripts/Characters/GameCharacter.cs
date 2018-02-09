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
    [SerializeField]
    protected float normalSpeed = 2;
    [SerializeField]
    protected float crouchSpeed = 1.5f;
    [SerializeField]
    protected float runningSpeed = 4f;

    protected float movementSpeed;
    [SerializeField]
    protected bool isMoving;
    [SerializeField]
    protected bool isCrouching;

    [SerializeField]
    protected Vector3 inventoryPosition;
    [SerializeField]
    protected Inventory inventory;
    protected CharacterAttributes charAttributes;

    virtual public void Start()
    {
        inventory = GetComponent<Inventory>();
        charAttributes = GetComponent<CharacterAttributes>();
    }

    public IEnumerator MovementDetection()
    {
        Vector3 pos1 = transform.position;
        yield return new WaitForSeconds(0.03f);
        Vector3 pos2 = transform.position;

        if (pos1 != pos2)
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
                UI_PlayerUI.UI_PlayerHp.text = "Dead";
            }

            charAttributes.hp = 0;
            Destroy(gameObject);


        }
    }
}
