using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InteractiveFan : InteractiveObj
{
    bool IsFanOn = false;
    bool IsBlowing = false;
    [SerializeField]
    float BlowSpeed = 15f;

    Transform FanTargetTransform;

    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();

        IsFanOn = false;
        IsBlowing = false;

        if(IsLinked)
        {
            FanTargetTransform = transform.GetChild(0);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if( IsBlowing && characterController != null && IsLinked )
        {
            if (Vector3.Distance(FanTargetTransform.position, characterController.transform.position) > 0.05f)
            {
                characterController.transform.position += Vector3.Normalize(FanTargetTransform.position - characterController.transform.position) * BlowSpeed * Time.deltaTime;
            }
            else
            {
                characterController.enabled = true;
                IsBlowing = false;
            }
        }
    }

    public override void TogglePosition(bool linkedObjOnly = false)
    {
        IsFanOn = !IsFanOn;

        if(!IsLinked)
        {
            LinkedObj.TogglePosition();
        }
                
        if(GetComponent<Animator>() != null )
        {
            GetComponent<Animator>().SetBool("fanRotate", IsFanOn);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && IsLinked && IsFanOn)
        {
            IsBlowing = true;
            other.gameObject.GetComponent<CharacterController>().enabled = false;
        }
    }
}
