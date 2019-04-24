using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slime : Enemy {

    public float mov_x;
    public float mov_y;
    public float mov_deltaForce;
    public float rotateSpeed;

    float rot = 0f;

    IEnumerator Move()
    {
        mov_y -= mov_deltaForce;
        rot += rotateSpeed;
        transform.localPosition += new Vector3(-mov_x * (0.5f + 0.1f * GameController.rank), mov_y);
        transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, rot));
        yield return new WaitForSeconds(1 / 60f);
        StartCoroutine("Move");
    }

    float dir = 0;

    IEnumerator Launch()
    {
        GameObject ins;
        AudioSource cct = GameObject.Find("Sound_EnemyCluster").GetComponent<AudioSource>();
        cct.Play();

        if (GameController.rank <= 2) { }
        else if (GameController.rank <= 5)
        {
            dir += 37.7f / GameController.rank + Random.Range(-1.1f, 1.1f);
            ins = GetBullet("CIRCLE");
            ins.GetComponent<Bullet>().SetIdentity(transform.localPosition, 6f, dir, 0.25f, 0.25f, Color.white, new Color(0f, 0.5f, 1f));
            yield return new WaitForSeconds(0.2f / GameController.rank);
            StartCoroutine("Launch");
        }
        else if (GameController.rank <= 8)
        {
            dir += 150.7f / GameController.rank + Random.Range(-2.1f, 2.1f);
            for (float i = Random.Range(0f, 1.8f); i < 360f; i += 120f)
            {
                ins = GetBullet("CIRCLE");
                ins.GetComponent<Bullet>().SetIdentity(transform.localPosition, 6.5f, dir + i, 0.25f, 0.25f, Color.white, new Color(0f, 0.3f, 1f));
            }
            yield return new WaitForSeconds(0.3f / GameController.rank);
            StartCoroutine("Launch");
        }
        // level INSANE
        else {
            dir += 127.7f / GameController.rank + Random.Range(-1.1f, 1.1f);
            for (float i = Random.Range(0f, 0.9f); i < 360f; i += 90f)
            {
                ins = GetBullet("CIRCLE");
                ins.GetComponent<Bullet>().SetIdentity(transform.localPosition, 5.5f, dir + i, 0.3f, 0.3f, Color.white, new Color(0f, 0.1f, 1f));
            }
            yield return new WaitForSeconds(0.4f / GameController.rank);
            StartCoroutine("Launch");
        }
        yield return null;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Main")
        {
            Destroy(gameObject);
        }
    }
}
