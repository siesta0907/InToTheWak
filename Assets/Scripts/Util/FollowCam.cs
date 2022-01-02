using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCam : MonoBehaviour
{
	[SerializeField] private float speed = 0.15f;
	Player target;

    void Awake()
    {
		target = FindObjectOfType<Player>();
    }

    void Update()
    {
		Vector3 targetPos = new Vector3(target.transform.position.x, target.transform.position.y, -10);
		transform.position = Vector3.Lerp(transform.position, targetPos, speed * Time.deltaTime);
    }
}
