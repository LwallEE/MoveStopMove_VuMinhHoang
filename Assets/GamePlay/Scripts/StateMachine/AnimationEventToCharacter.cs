using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationEventToCharacter : MonoBehaviour
{
    private Character character;

    private void Awake()
    {
        character = GetComponentInParent<Character>();
    }

    public void TriggerAttack()
    {
        if(character != null)
            character.TriggerAttack();
    }

    public void AnimationFinishTrigger()
    {
        if(character != null)
            character.AnimationFinish();
    }
}
