using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/*
 * 닿으면 데미지를 입는 히트박스입니다.
 * 이 컴포넌트가 붙어있는 프리팹을 소환하고, SetData로 정보를 설정해주면 됩니다.
 * 
 * EX) 풍신의 Lightning
 */
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
