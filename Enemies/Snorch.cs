using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Snorch : Enemy {

    public float mov_speed;
    public float mov_amplitude;
    public float mov_frequency;

    float frequency = 0f;

    IEnumerator Move()
    {
        frequency += mov_frequency;
        transform.localPosition += new Vector3(-mov_speed, mov_amplitude * Mathf.Sin(frequency));
        yield return new WaitForSeconds(1 / 60f);
        StartCoroutine("Move");
    }

    IEnumerator Launch()
    {
        GameObject ins;

        if (GameController.rank <= 2)
        {
            for (float i = Random.Range(0, 180); i < 360; i += 180)
            {
                ins = GetBullet("CIRCLE");
                if (ins != null) ins.GetComponent<Bullet>().SetIdentity(transform.localPosition, 1f, i,
                        0.25f, 0.25f, Color.white, Color.yellow);
            }
            yield return new WaitForSeconds(0.65f);
            StartCoroutine("Launch");
        }
        else if (GameController.rank <= 4)
        {
            for (float i = Random.Range(0, 90); i < 360; i += 90)
            {
                ins = GetBullet("CIRCLE");
                if (ins != null) ins.GetComponent<Bullet>().SetIdentity(transform.localPosition, 1f, i,
                        0.25f, 0.25f, Color.white, Color.yellow);
            }
            yield return new WaitForSeconds(0.55f);
            StartCoroutine("Launch");
        }
        else if (GameController.rank <= 6)
        {
            for (float i = Random.Range(0, 45); i < 360; i += 45)
            {
                ins = GetBullet("CIRCLE");
                if (ins != null) ins.GetComponent<Bullet>().SetIdentity(transform.localPosition, 1f, i,
                        0.25f, 0.25f, Color.white, Color.yellow);
            }
            yield return new WaitForSeconds(0.45f);
            StartCoroutine("Launch");
        }
        else if (GameController.rank <= 8) {
            for (float i = Random.Range(0, 45); i < 360; i += 45)
            {
                ins = GetBullet("CIRCLE");
                if (ins != null) ins.GetComponent<Bullet>().SetIdentity(transform.localPosition, 1f, i,
                        0.25f, 0.25f, Color.white, Color.yellow);
            }
            yield return new WaitForSeconds(0.35f);
            StartCoroutine("Launch");
        }
        else // level INSANE
        {
            for (float i = Random.Range(0, 30); i < 360; i += 30)
            {
                ins = GetBullet("CIRCLE");
                if (ins != null) ins.GetComponent<Bullet>().SetIdentity(transform.localPosition, 1f, i,
                        0.25f, 0.25f, Color.white, Color.yellow);
            }
            yield return new WaitForSeconds(0.25f);
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
