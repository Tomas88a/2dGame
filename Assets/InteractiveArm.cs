using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractiveArm : InteractiveObj
{
    private Animator animator;

    private int CurrentStep;
    const int ARM_ANIM_MAX_STEP = 3;
    bool Hooked = false;
    [SerializeField]
    InteractiveObj BoxToHook;
    [SerializeField]
    GameObject ArmCollision;

    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
        animator = GetComponent<Animator>();
        CurrentStep = 0;
        Hooked = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void TogglePosition(bool linkedObjOnly = false)
    {
        ++ CurrentStep;

        if (CurrentStep > ARM_ANIM_MAX_STEP)
        {
            CurrentStep = 2;
            ArmCollision.SetActive(false);
        }
        else if(CurrentStep == ARM_ANIM_MAX_STEP)
        {
            ArmCollision.SetActive(true);
        }

        animator.SetInteger("CurrentStep", CurrentStep);

        if (CurrentStep == 2 
            && BoxToHook != null 
            && transform.Find("hook") != null 
            && !Hooked)
        {
            BoxToHook.transform.SetParent(transform.Find("hook"), true);
            Hooked = true;
        }
    }
}
