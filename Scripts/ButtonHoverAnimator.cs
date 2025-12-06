using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonHoverAnimator : MonoBehaviour
{
    public Animator anim;

    public void HoverEnter()
    {
        anim.SetBool("isHover", true);
    }

    public void HoverExit()
    {
        anim.SetBool("isHover", false);
    }
}