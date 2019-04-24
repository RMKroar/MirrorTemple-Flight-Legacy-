using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class General : Boss {

    IEnumerator Move()
    {
        yield return null; 
    }

    IEnumerator MoveSupport()
    {
        yield return null;
    }

    IEnumerator Launch()
    {
        StartCoroutine("Pattern_" + (maxPattern - pattern));
        immortal = false;
        yield return null;
    }

    int moveDirection = 1;

    IEnumerator Pattern_0()
    {
        GameObject ins;
        AudioSource sct = GameObject.Find("Sound_EnemyAttack").GetComponent<AudioSource>();

        float dir = 180f - moveDirection * 40f;
        float sp = 10f;
        float _direction = Random.Range(165f, 195f);

        StopCoroutine("Animate");
        for (float _speed = moveSpeed, cnt = 0; _speed > 0; _speed -= 0.8f, cnt++) {
            transform.localPosition += new Vector3(_speed * Mathf.Cos(_direction * Mathf.PI / 180f), _speed * Mathf.Sin(_direction * Mathf.PI / 180f));

            PatternSupport1(cnt, sp, dir);
            dir -= moveDirection * 4f;
            sp -= 0.24f;
            yield return new WaitForSeconds(1 / 60f);
        }
        StartCoroutine("Animate");
        yield return new WaitForSeconds(1f);
        StopCoroutine("Animate");
        for (float _speed = moveSpeed, cnt = 0; _speed > 0; _speed -= 0.8f, cnt++)
        {
            transform.localPosition -= new Vector3(_speed * Mathf.Cos(_direction * Mathf.PI / 180f), _speed * Mathf.Sin(_direction * Mathf.PI / 180f));

            if (GameController.rank <= 3) {
                if (cnt % 24 == 0)
                {
                    sct.Play();
                    ins = GetBullet("CIRCLE");
                    if (ins != null) ins.GetComponent<Bullet>().SetIdentityEx("CANNON_GENERAL", transform.localPosition, 8, Random.Range(0f, 360f), 0.7f, 0.7f, Color.black, Color.black);
                }
            }  
            else if (GameController.rank <= 6)
            {
                if (cnt % 20 == 0)
                {
                    sct.Play();
                    ins = GetBullet("CIRCLE");
                    if (ins != null) ins.GetComponent<Bullet>().SetIdentityEx("CANNON_GENERAL", transform.localPosition, 8, Random.Range(0f, 360f), 0.7f, 0.7f, Color.black, Color.black);
                }
            }
            else if (GameController.rank <= 8)
            {
                if (cnt % 16 == 0)
                {
                    sct.Play();
                    ins = GetBullet("CIRCLE");
                    if (ins != null) ins.GetComponent<Bullet>().SetIdentityEx("CANNON_GENERAL", transform.localPosition, 8, Random.Range(0f, 360f), 0.7f, 0.7f, Color.black, Color.black);
                }
            }
            else if (GameController.rank <= 9)
            {
                if (cnt % 12 == 0)
                {
                    sct.Play();
                    ins = GetBullet("CIRCLE");
                    if (ins != null) ins.GetComponent<Bullet>().SetIdentityEx("CANNON_GENERAL", transform.localPosition, 8, Random.Range(0f, 360f), 0.7f, 0.7f, Color.black, Color.black);
                }
            }
            else
            {
                if (cnt % 8 == 0)
                {
                    sct.Play();
                    ins = GetBullet("CIRCLE");
                    if (ins != null) ins.GetComponent<Bullet>().SetIdentityEx("CANNON_GENERAL", transform.localPosition, 8, Random.Range(0f, 360f), 0.7f, 0.7f, Color.black, Color.black);
                }
            }
            yield return new WaitForSeconds(1 / 60f);
        }
        StartCoroutine("Animate");
        yield return new WaitForSeconds(3f);       
        StartCoroutine("Launch");
        moveDirection *= -1;
    }

    private void PatternSupport1(float cnt, float sp, float dir) {
        GameObject ins;
        AudioSource cct = GameObject.Find("Sound_EnemyCluster").GetComponent<AudioSource>();

        if (GameController.rank <= 1)
        {
            if (cnt % 7 == 0)
            {
                cct.Play();
                ins = GetBullet("THORN");
                if (ins != null) ins.GetComponent<Bullet>().SetIdentity(transform.localPosition, sp + 5, dir, 0.35f, 0.35f, Color.white, Color.red);
                ins = GetBullet("THORN");
                if (ins != null) ins.GetComponent<Bullet>().SetIdentity(transform.localPosition, sp + 2, dir, 0.25f, 0.25f, Color.white, BulletCode.COLOR_ORANGE);
            }
        }
        else if (GameController.rank <= 4)
        {
            if (cnt % 5 == 0)
            {
                cct.Play();
                ins = GetBullet("THORN");
                if (ins != null) ins.GetComponent<Bullet>().SetIdentity(transform.localPosition, sp + 5, dir, 0.35f, 0.35f, Color.white, Color.red);
                ins = GetBullet("THORN");
                if (ins != null) ins.GetComponent<Bullet>().SetIdentity(transform.localPosition, sp + 2, dir + 18.3f, 0.25f, 0.25f, Color.white, BulletCode.COLOR_ORANGE);
                ins = GetBullet("THORN");
                if (ins != null) ins.GetComponent<Bullet>().SetIdentity(transform.localPosition, sp + 2, dir - 18.3f, 0.25f, 0.25f, Color.white, BulletCode.COLOR_ORANGE);
            }
        }
        else if (GameController.rank <= 7) {
            if (cnt % 3 == 0)
            {
                cct.Play();
                ins = GetBullet("THORN");
                if (ins != null) ins.GetComponent<Bullet>().SetIdentity(transform.localPosition, sp + 5, dir, 0.35f, 0.35f, Color.white, Color.red);
                ins = GetBullet("THORN");
                if (ins != null) ins.GetComponent<Bullet>().SetIdentity(transform.localPosition, sp + 1, dir + 12.7f, 0.25f, 0.25f, Color.white, BulletCode.COLOR_ORANGE);
                ins = GetBullet("THORN");
                if (ins != null) ins.GetComponent<Bullet>().SetIdentity(transform.localPosition, sp + 1, dir - 12.7f, 0.25f, 0.25f, Color.white, BulletCode.COLOR_ORANGE);
                ins = GetBullet("THORN");
                if (ins != null) ins.GetComponent<Bullet>().SetIdentity(transform.localPosition, sp + 3, dir + 25.4f, 0.25f, 0.25f, Color.white, Color.yellow);
                ins = GetBullet("THORN");
                if (ins != null) ins.GetComponent<Bullet>().SetIdentity(transform.localPosition, sp + 3, dir - 25.4f, 0.25f, 0.25f, Color.white, Color.yellow);
            }
        }
        else
        {
            if (cnt % 2 == 0)
            {
                cct.Play();
                ins = GetBullet("THORN");
                if (ins != null) ins.GetComponent<Bullet>().SetIdentity(transform.localPosition, sp + 5, dir, 0.35f, 0.35f, Color.white, Color.red);
                ins = GetBullet("THORN");
                if (ins != null) ins.GetComponent<Bullet>().SetIdentity(transform.localPosition, sp + 1, dir + 11.7f, 0.25f, 0.25f, Color.white, BulletCode.COLOR_ORANGE);
                ins = GetBullet("THORN");
                if (ins != null) ins.GetComponent<Bullet>().SetIdentity(transform.localPosition, sp + 1, dir - 11.7f, 0.25f, 0.25f, Color.white, BulletCode.COLOR_ORANGE);
                ins = GetBullet("THORN");
                if (ins != null) ins.GetComponent<Bullet>().SetIdentity(transform.localPosition, sp + 3f, dir + 23.4f, 0.25f, 0.25f, Color.white, Color.yellow);
                ins = GetBullet("THORN");
                if (ins != null) ins.GetComponent<Bullet>().SetIdentity(transform.localPosition, sp + 3f, dir - 23.4f, 0.25f, 0.25f, Color.white, Color.yellow);
                ins = GetBullet("THORN");
                if (ins != null) ins.GetComponent<Bullet>().SetIdentity(transform.localPosition, sp - 1f, dir + 35.1f, 0.25f, 0.25f, Color.white, Color.green);
                ins = GetBullet("THORN");
                if (ins != null) ins.GetComponent<Bullet>().SetIdentity(transform.localPosition, sp - 1f, dir - 35.1f, 0.25f, 0.25f, Color.white, Color.green);
                ins = GetBullet("THORN");
                if (ins != null) ins.GetComponent<Bullet>().SetIdentity(transform.localPosition, sp + 1, dir + 46.8f, 0.25f, 0.25f, Color.white, Color.green);
                ins = GetBullet("THORN");
                if (ins != null) ins.GetComponent<Bullet>().SetIdentity(transform.localPosition, sp + 1, dir - 46.8f, 0.25f, 0.25f, Color.white, Color.green);
            }
        }
    } 
}
