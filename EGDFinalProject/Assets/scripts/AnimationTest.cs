using UnityEngine;
using System.Collections;

public class AnimationTest : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {
        Animator anim = GetComponent<Animator>();
        anim.SetBool("IsMoving", true);
        anim.Play("Person_walking");
    }

    // Update is called once per frame
    void Update()
    {

    }
}
