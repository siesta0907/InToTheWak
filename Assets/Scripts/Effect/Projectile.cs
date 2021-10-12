using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/*
 * 투사체를 구현하는 스크립트입니다.
 * SetData를 통해 값을 설정하면 투사체가 됩니다.
 * lockRotation 옵션을 체크하면 설정된 dir의 방향으로 발사되고
 * 옵션을 해제하면 바라보는 회전방향으로 발사됩니다.
 */
public class Projectile : MonoBehaviour
{
	Entity owner;
	float damage;
	float speed;
	Vector3 dir;
	[SerializeField] private bool lockRotation = false;

	void Start()
	{
		Destroy(this.gameObject, 4.0f);
	}


	void Update()
	{
		if (!lockRotation)
			transform.position += transform.right * speed * Time.deltaTime;
		else
			transform.position += dir * speed * Time.deltaTime;
	}


	void OnTriggerEnter2D(Collider2D collision)
	{
		Entity entity = collision.gameObject.GetComponent<Entity>();
		if (entity)
		{
			entity.TakeDamage(damage, owner);
			GetComponent<BoxCollider2D>().enabled = false;
		}
	}


	public void SetData(Entity owner, float damage, float speed)
	{
		this.owner = owner;
		this.damage = damage;
		this.speed = speed;
	}

	public void SetData(Entity owner, float damage, float speed, Vector3 dir)
	{
		this.owner = owner;
		this.damage = damage;
		this.speed = speed;
		this.dir = dir.normalized;
	}
}
