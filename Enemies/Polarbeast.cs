using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Polarbeast : Enemy {

    public float mov_speed;

    IEnumerator Move()
    {
        transform.localPosition += new Vector3(-mov_speed, 0);
        yield return new WaitForSeconds(1 / 60f);
        StartCoroutine("Move");
    }

    IEnumerator Launch()
    {
        AudioSource pct = GameObject.Find("Sound_EnemyPetal").GetComponent<AudioSource>();
        pct.volume = 0.2f; pct.Play();

        GameObject ins;

        for (float j = Random.Range(180f, 200f); j > 45; j -= (50f / (GameController.rank * 0.25f + 1))) {
            for (float i = Random.Range(0f, 60f); i < 360; i += 60)
            {
                ins = GetBullet("PETAL");
                ins.GetComponent<Bullet>().SetIdentityEx("SNOWFLOWER", transform.localPosition + new Vector3(200 * Mathf.Cos(j * Mathf.PI / 180f), 200 * Mathf.Sin(j * Mathf.PI / 180f)), 
                    0.5f, i, 0.4f, 0.4f, Color.white, BulletCode.COLOR_SKY);
            }
            yield return new WaitForSeconds(1 / 30f);
        }     
        
        yield return new WaitForSeconds(3f - 0.25f * GameController.rank);
        if (transform.localPosition.x >= 100) StartCoroutine(Launch());
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Main")
        {
            Destroy(gameObject);
        }
    }
}
