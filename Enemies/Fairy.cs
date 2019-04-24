using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fairy : Enemy {

    public float mov_startSpeed;
    public float mov_friction;
    public float mov_endSpeed;

    int launchCnt = 0;

    IEnumerator Move()
    {
        transform.localPosition += new Vector3(-mov_startSpeed, 0, 0);
        if(mov_startSpeed >= mov_friction)
        {
            mov_startSpeed -= mov_friction;
            yield return new WaitForSeconds(1 / 60f);
            StartCoroutine("Move");
        }
        else
        {
            yield return new WaitForSeconds(2.5f);
            StartCoroutine("EndMove");
        }
    }

    IEnumerator EndMove()
    {
        mov_endSpeed += 0.1f;
        if (transform.localPosition.y > 0) transform.localPosition += new Vector3(mov_endSpeed * (-0.7f), mov_endSpeed * 0.7f, 0f);
        else transform.localPosition += new Vector3(mov_endSpeed * (-0.7f), mov_endSpeed * (-0.7f), 0f);

        yield return new WaitForSeconds(1 / 60f);
        StartCoroutine("EndMove");
    }

    IEnumerator Launch() {
        GameObject ins;
        AudioSource sct = GameObject.Find("Sound_EnemyAttack").GetComponent<AudioSource>();
        sct.Play();

        if (GameController.rank <= 1)
        {
            ins = GetBullet("CIRCLE");
            if (ins != null) ins.GetComponent<Bullet>().SetIdentity(transform.localPosition, 10f, ToPlayerAngle(), 
                0.25f, 0.25f, Color.white, BulletCode.COLOR_TEAL);

            yield return new WaitForSeconds(1f);
        }
        else if(GameController.rank <= 4)
        {       
            ins = GetBullet("CIRCLE");
            if (ins != null) ins.GetComponent<Bullet>().SetIdentity(transform.localPosition, 10f, ToPlayerAngle(),
                    0.25f, 0.25f, Color.white, BulletCode.COLOR_TEAL);
            if (launchCnt == 0)
            {
                for(float i = Random.Range(0, 60); i < 360; i+=60)
                {
                    ins = GetBullet("CIRCLE");
                    if (ins != null) ins.GetComponent<Bullet>().SetIdentity(transform.localPosition, 6f, i,
                            0.25f, 0.25f, Color.white, Color.green);
                }
            }

            yield return new WaitForSeconds(1f);
            launchCnt++; StartCoroutine("Launch");            
        }
        else if(GameController.rank <= 6)
        {
            ins = GetBullet("CIRCLE");
            if (ins != null) ins.GetComponent<Bullet>().SetIdentity(transform.localPosition, 10f, ToPlayerAngle(),
                    0.25f, 0.25f, Color.white, BulletCode.COLOR_TEAL);

            for (float i = 3; i < 10; i += 3)
            {
                ins = GetBullet("CIRCLE");
                if (ins != null) ins.GetComponent<Bullet>().SetIdentity(transform.localPosition, 10f - (i/3), ToPlayerAngle() + i,
                    0.25f, 0.25f, Color.white, BulletCode.COLOR_TEAL);

                ins = GetBullet("CIRCLE");
                if (ins != null) ins.GetComponent<Bullet>().SetIdentity(transform.localPosition, 10f - (i/3), ToPlayerAngle() - i,
                    0.25f, 0.25f, Color.white, BulletCode.COLOR_TEAL);
            }

            if (launchCnt == 0)
            {
                for (float i = Random.Range(0, 60); i < 360; i += 60)
                {
                    ins = GetBullet("CIRCLE");
                    if (ins != null) ins.GetComponent<Bullet>().SetIdentity(transform.localPosition, 6f, i,
                     0.25f, 0.25f, Color.white, Color.green);
                }
            }

            yield return new WaitForSeconds(2f);
            launchCnt++; StartCoroutine("Launch");
        }
        else if(GameController.rank <= 8)
        {
            ins = GetBullet("CIRCLE");
            if (ins != null) ins.GetComponent<Bullet>().SetIdentity(transform.localPosition, 10f, ToPlayerAngle(),
                    0.25f, 0.25f, Color.white, BulletCode.COLOR_TEAL);

            for (float i = 3; i < 10; i += 3)
            {
                ins = GetBullet("CIRCLE");
                if (ins != null) ins.GetComponent<Bullet>().SetIdentity(transform.localPosition, 10f - (i / 3), ToPlayerAngle() + i,
                    0.25f, 0.25f, Color.white, BulletCode.COLOR_TEAL);

                ins = GetBullet("CIRCLE");
                if (ins != null) ins.GetComponent<Bullet>().SetIdentity(transform.localPosition, 10f - (i / 3), ToPlayerAngle() - i,
                    0.25f, 0.25f, Color.white, BulletCode.COLOR_TEAL);
            }

            if (launchCnt == 0)
            {
                for (float i = Random.Range(0, 30); i < 360; i += 30)
                {
                    ins = GetBullet("CIRCLE");
                    if (ins != null) ins.GetComponent<Bullet>().SetIdentity(transform.localPosition, 6f, i,
                     0.25f, 0.25f, Color.white, Color.green);
                }
            }

            yield return new WaitForSeconds(1.5f);
            launchCnt++; StartCoroutine("Launch");
        }
        else // level INSANE
        {
            ins = GetBullet("CIRCLE");
            if (ins != null) ins.GetComponent<Bullet>().SetIdentity(transform.localPosition, 10f, ToPlayerAngle(),
                    0.25f, 0.25f, Color.white, BulletCode.COLOR_TEAL);

            for (float i = 3; i < 30; i += 3)
            {
                ins = GetBullet("CIRCLE");
                if (ins != null) ins.GetComponent<Bullet>().SetIdentity(transform.localPosition, 10f - (i / 5), ToPlayerAngle() + i,
                    0.25f, 0.25f, Color.white, BulletCode.COLOR_TEAL);

                ins = GetBullet("CIRCLE");
                if (ins != null) ins.GetComponent<Bullet>().SetIdentity(transform.localPosition, 10f - (i / 5), ToPlayerAngle() - i,
                    0.25f, 0.25f, Color.white, BulletCode.COLOR_TEAL);
            }

            if (launchCnt == 0)
            {
                for (float i = Random.Range(0, 30); i < 360; i += 30)
                {
                    ins = GetBullet("CIRCLE");
                    if (ins != null) ins.GetComponent<Bullet>().SetIdentity(transform.localPosition, 6f, i,
                     0.25f, 0.25f, Color.white, Color.green);
                }
            }

            yield return new WaitForSeconds(2.5f);
            launchCnt++; StartCoroutine("Launch");
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
