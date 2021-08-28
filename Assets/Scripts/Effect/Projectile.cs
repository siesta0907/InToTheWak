using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
	Entity owner;
	float damage;
	float speed;

	void Start()
	{
		Destroy(this.gameObject, 4.0f);
	}


	void Update()
	{
		transform.position += transform.right * speed * Time.deltaTime;
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
}
