using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorUtil : MonoBehaviour {

    public void AnimatorTrigger(Animator animator, string triggerName, float delay) {
        StartCoroutine(TriggerVar(animator, triggerName, delay));
    }

    public void BlendTreeValue(Animator anim, string name, float to, float rate){
        float val = Mathf.Lerp(anim.GetFloat(name), to, rate * Time.deltaTime);

        if(to == 0f){
            if(val < 0.1f)
                val = 0f;
        }

        anim.SetFloat(name, val);
    }

    IEnumerator TriggerVar(Animator animator, string triggerName, float delay){
        animator.SetTrigger(triggerName);

        yield return new WaitForSeconds(delay);

        animator.ResetTrigger(triggerName);
    }
}