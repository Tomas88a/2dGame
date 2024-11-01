using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class InteractiveObj : MonoBehaviour
{
    public const float togglingTime = 1.5f;

    public InteractiveObj LinkedObj = null;
    public bool IsLinked = false;

    private MeshRenderer m_MeshRenderer;
    private Material originalMaterial;
    public Material highlightMaterial = null;

    protected CharacterController characterController;

    private bool IsInPosB = false;
    private Vector3 Position_A;
    [SerializeField] private Vector3 Position_B;
    private Vector3 targetPosition;
    private Vector3 togglingVelocity;

    private static Quaternion DEFAULT_ROT_B = new Quaternion(-1f, -1f, -1f, -1f);
    private bool IsInRotB = false;
    private Quaternion Rotation_A;
    [SerializeField] private Quaternion Rotation_B = DEFAULT_ROT_B;
    [SerializeField] private GameObject blocker;
    private float togglingTimer = 0;

    // Start is called before the first frame update
    public virtual void Start()
    {
        targetPosition = transform.position;
        Position_A = transform.position;
        IsInPosB = false;

        Rotation_A = transform.rotation;
        IsInRotB = false;
        if(blocker != null) blocker.SetActive(false);

        characterController = FindAnyObjectByType<CharacterController>();
        
        m_MeshRenderer = GetComponent<MeshRenderer>();
        originalMaterial = m_MeshRenderer.material;
    }

    // Update is called once per frame
    void Update()
    {
        if (togglingTimer < togglingTime && togglingTimer >= 0f)
        {
            float DistToTarget = Vector3.Magnitude(targetPosition - transform.position);
            if (DistToTarget > 0.05f )
            {
                transform.position += togglingVelocity * Time.deltaTime;
            }

            togglingTimer += Time.deltaTime;
            if (togglingTimer >= togglingTime)
            {
                if (blocker != null && blocker.activeSelf)
                {
                    blocker.SetActive(false);
                }
                togglingTimer = -1f;
            }
        }
    }

    public virtual void OnMouseEnter()
    {
        SetHighlight(true);
    }

    public virtual void OnMouseExit()
    {
        SetHighlight(false);
    }

    public virtual void OnMouseDown()
    {
        if(IsLinked) return; 

        TogglePosition();
    }

    public virtual void TogglePosition(bool linkedObjOnly = false)
    {
        if (!linkedObjOnly)
        {
            if (Position_B != Vector3.zero)
            {
                targetPosition = IsInPosB ? Position_A : Position_B;
                togglingVelocity = (targetPosition - transform.position) / togglingTime;
                IsInPosB = !IsInPosB;
                togglingTimer = 0f;
                if (blocker != null) blocker.SetActive(true);
            }

            if (Rotation_B != DEFAULT_ROT_B)
            {
                transform.rotation = IsInRotB ? Rotation_A : Rotation_B;
                IsInRotB = !IsInRotB;
                togglingTimer = 0f;
                if (blocker != null) blocker.SetActive(true);
            }
        }

        if (LinkedObj != null)
        {
            LinkedObj.TogglePosition();
        }

        if (characterController != null && !IsLinked)
        {
            characterController.CastSuperPower();
        }

    }

    public virtual void SetHighlight(bool highlight)
    {
        if (IsLinked) return;

        if (highlight)
        {
            m_MeshRenderer.material = highlightMaterial;
        }
        else
        {
            m_MeshRenderer.material = originalMaterial;
        }
    }
}
