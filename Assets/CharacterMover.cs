using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMover : MonoBehaviour
{
    public float moveSpeedf = 1.0f;

    public void MoveCharacter(float x, float z)
    {
        Vector3 dirVector = new Vector3(x, 0, z).normalized * moveSpeedf;
        GetComponent<Rigidbody>().MovePosition(transform.position + dirVector * Time.deltaTime);
    }
}
