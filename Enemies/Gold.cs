using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gold : Enemy {

    public float mov_speed;
    public float mov_amplitude;
    public float mov_frequency;

    float frequency = 0f;

    IEnumerator Move()
    {
        frequency += mov_frequency;
        transform.localPosition += new Vector3(-mov_speed, mov_amplitude * Mathf.Cos(frequency));
        yield return new WaitForSeconds(1 / 60f);
        StartCoroutine("Move");
    }

    IEnumerator Launch()
    {
        GameObject ins;
        AudioSource lct = GameObject.Find("Sound_EnemyLazer").GetComponent<AudioSource>();

        float randValue = Random.Range(0f, 360f);
        for (float i = 0; i < 360; i += 360f / (1 + GameController.rank / 2))
        {
            lct.volume = 0.35f; lct.Play();
            ins = GetBullet("PETAL");
            if (ins != null) ins.GetComponent<Bullet>().SetIdentityEx("ALCADROLAZZOSHORT", transform.localPosition, 8f + GameController.rank, randValue + i,
                0.3f, 0.3f, Color.white, Color.yellow);
        }

        yield return new WaitForSeconds(1.1f - 0.06f * GameController.rank);
        StartCoroutine(Launch());
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Main")
        {
            Destroy(gameObject);
        }
    }
}
