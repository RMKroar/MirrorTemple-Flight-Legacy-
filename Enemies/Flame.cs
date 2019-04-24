using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flame : Enemy {

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
        if (GameController.rank <= 2) { }
        else if (GameController.rank <= 5)
        {
            ins = GetBullet("CIRCLE");
            if (ins != null) ins.GetComponent<Bullet>().SetIdentity(transform.localPosition, 8f, ToPlayerAngle(),
                0.25f, 0.25f, Color.white, Color.red);

            yield return new WaitForSeconds(1.5f);
            StartCoroutine("Launch");
        }
        else if (GameController.rank <= 8)
        {
            ins = GetBullet("CIRCLE");
            if (ins != null) ins.GetComponent<Bullet>().SetIdentity(transform.localPosition, 12f, ToPlayerAngle(),
                0.25f, 0.25f, Color.white, Color.red);

            yield return new WaitForSeconds(0.75f);
            StartCoroutine("Launch");
        }
        else // level INSANE
        {
            ins = GetBullet("CIRCLE");
            if (ins != null) ins.GetComponent<Bullet>().SetIdentity(transform.localPosition, 18f, ToPlayerAngle(),
                0.25f, 0.25f, Color.white, Color.red);

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
