using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blob : Enemy {

    public float mov_speed;
    float direction;
    bool flag = false;

    IEnumerator Move()
    {
        if (!flag)
        {
            if (transform.localPosition.y >= -100f) direction = Random.Range(190f, 210f);
            else direction = Random.Range(150f, 170f);
            flag = true;
        }
        float radiusDir = direction * Mathf.PI / 180f;
        transform.localPosition += new Vector3(mov_speed * Mathf.Cos(radiusDir), mov_speed * Mathf.Sin(radiusDir));
        yield return new WaitForSeconds(1 / 60f);
        StartCoroutine("Move");
    }

    IEnumerator Launch()
    {
        AudioSource cct = GameObject.Find("Sound_EnemyCluster").GetComponent<AudioSource>();
        GameObject ins;
        cct.Play();

        ins = GetBullet("CIRCLE");
        if (ins != null)
        {
            float randDist = Random.Range(0f, 50f);
            float randDirRad = Random.Range(0f, 2f) * Mathf.PI;
            ins.GetComponent<Bullet>().SetIdentityEx("BLOB", transform.localPosition + new Vector3(0f, 40f) + new Vector3(randDist * Mathf.Cos(randDirRad), randDist * Mathf.Sin(randDirRad))
                , 0.5f, Random.Range(0f, 360f), 0.35f, 0.35f, new Color(0f, 0.5f, 1f, 0.5f), new Color(0f, 0f, 1f, 0.5f));
        }

        yield return new WaitForSeconds(0.5f - 0.045f * GameController.rank);
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
