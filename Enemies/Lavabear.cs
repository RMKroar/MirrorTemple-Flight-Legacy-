using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lavabear : Enemy {

    public float mov_speed;

    IEnumerator Move()
    {
        transform.localPosition += new Vector3(-mov_speed, 0);
        yield return new WaitForSeconds(1 / 60f);
        StartCoroutine("Move");
    }

    IEnumerator Launch()
    {
        AudioSource sct = GameObject.Find("Sound_EnemyAttack").GetComponent<AudioSource>();
        sct.Play();

        GameObject ins = GetBullet("CIRCLE");
        if (ins != null)
        {
            float tmpSpd = 0;
            if (GameController.rank <= 3) tmpSpd = 1f + GameController.rank;
            else tmpSpd = 3f + (GameController.rank - 3) * 0.5f;
            ins.GetComponent<Bullet>().SetIdentityEx("LAVABALL", transform.localPosition, 1f + GameController.rank, ToPlayerAngle() + Random.Range(-30f, 30f), 0.5f, 0.5f, Color.white, Color.red);
            tmpSpd++;
        }
        yield return new WaitForSeconds(2.5f - GameController.rank * 0.15f);
        if (GameController.rank >= 3 && transform.localPosition.x >= 250f) {
            StartCoroutine(Launch());
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Main")
        {
            Destroy(gameObject);
        }
    }
}
