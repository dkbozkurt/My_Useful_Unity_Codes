//Dogukan Kaan Bozkurt
//      github.com/dkbozkurt
//GEFEASOFT

//Animation Controls by getting key value from another script.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreAnimControl : MonoBehaviour
{
    [SerializeField] private Animator animator;

    void Start()
    {
        animator.SetInteger("Check", 0);
    }
    public void Check(int a)
    {
         animator.SetInteger("Check", a);    

    }  
    
}
