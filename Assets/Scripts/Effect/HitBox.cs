using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitBox : MonoBehaviour
{
	Entity owner;
	float damage;


	void OnTriggerEnter2D(Collider2D collision)
	{
		Entity entity = collision.gameObject.GetComponent<Entity>();
		if (entity)
		{
			entity.TakeDamage(damage, owner);
			GetComponent<BoxCollider2D>().enabled = false;
		}
	}


	public void SetData(Entity owner, float damage, float lifeTime)
	{
		this.owner = owner;
		this.damage = damage;

		Destroy(this.gameObject, lifeTime);
	}
}
