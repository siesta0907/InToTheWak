using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowObject : MonoBehaviour
{
	[SerializeField] private float speed = 0.15f;
	[SerializeField] private float zPos = -10;
	Player target;

    void Awake()
    {
		target = FindObjectOfType<Player>();
    }

    void Update()
    {
		Vector3 targetPos = new Vector3(target.transform.position.x, target.transform.position.y, zPos);
		transform.position = Vector3.Lerp(transform.position, targetPos, speed * Time.deltaTime);
    }
}
