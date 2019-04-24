using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FrostWolf : Enemy {

    public float mov_speed;

    IEnumerator Move()
    {
        transform.localPosition += new Vector3(-mov_speed, 0);
        yield return new WaitForSeconds(1 / 60f);

        if (transform.localPosition.x < 250) {
            Image myImage = GetComponent<Image>();
            myImage.color -= new Color(0f, 0f, 0f, 1 / 15f);
            if (myImage.color.a <= 0) Destroy(gameObject);
        }
        StartCoroutine("Move");
    }

    IEnumerator Launch()
    {
        AudioSource cct = GameObject.Find("Sound_EnemyCluster").GetComponent<AudioSource>();
        GameObject ins;

        for (int j = 0; j < 1 + GameController.rank / 2; j++)
        {
            cct.Play();

            ins = GetBullet("CIRCLE");
            if (ins != null) ins.GetComponent<Bullet>().SetIdentityEx("CRYSTAL", transform.localPosition, 13 + 0.5f * GameController.rank, ToPlayerAngle(), 0.4f, 0.4f,
             new Color(0, Random.Range(0.2f, 1f), 1f), Color.white);

            yield return new WaitForSeconds(1 / 8f);
        }
        yield return new WaitForSeconds(1.5f);
        if (transform.localPosition.x >= 0) StartCoroutine(Launch());
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Main")
        {
            Destroy(gameObject);
        }
    }
}
