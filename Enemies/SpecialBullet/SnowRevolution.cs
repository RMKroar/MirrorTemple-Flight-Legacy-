using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnowRevolution : MonoBehaviour {

    public float speed;
    public float direction;
    public float range;
    public float dirDelta;
    public ArrayList bullets_one;
    public ArrayList bullets_two;
    public int moveCode;

    bool destroy = false;

    public void SetIdentity(float _speed, float _direction)
    {
        speed = _speed; direction = _direction;
    }

    // Use this for initialization
    void Start () {
        bullets_one = new ArrayList();
        bullets_two = new ArrayList();
        GameObject ins;
        float bulletDir = 0;

        if (moveCode == 0) bulletDir = 360f / (2 + GameController.rank);
        else if (moveCode == 1) bulletDir = 360f / (4 + GameController.rank * 2);

        for (float i = Random.Range(0f, bulletDir); i < 360; i += bulletDir) {
            ins = GetBullet("CIRCLE");
            if (ins != null) ins.GetComponent<Bullet>().SetIdentityEx("NOT_UNABLE", transform.localPosition, 0, i, 0.3f, 0.3f, Color.white, BulletCode.COLOR_SKY);
            bullets_one.Add(ins);

            ins = GetBullet("CIRCLE");
            if (ins != null) ins.GetComponent<Bullet>().SetIdentityEx("NOT_UNABLE", transform.localPosition, 0, i, 0.3f, 0.3f, Color.white, BulletCode.COLOR_SKY);
            bullets_two.Add(ins);
        }
        
        StartCoroutine(Move());
	}

    IEnumerator Move() {    

        transform.localPosition += new Vector3(speed * Mathf.Cos(direction * Mathf.PI / 180f), speed * Mathf.Sin(direction * Mathf.PI / 180f));

        if (moveCode == 0)
        {
            if (range < 150 + 60 * GameController.rank) range += (2f + GameController.rank * 0.7f);
            dirDelta += 1f - 0.06f * GameController.rank;
        }
        else if (moveCode == 1) {
            if (range < 600) range += (6f + GameController.rank);
            dirDelta += 0.5f;
        }
        
        ArrayList TempDestroy = new ArrayList();

        foreach (GameObject bul in bullets_one)
        {
            if (bul.activeInHierarchy)
            {
                float i = bul.GetComponent<Bullet>().direction;
                bul.transform.localPosition = transform.localPosition + new Vector3(range * Mathf.Cos((i + dirDelta) * Mathf.PI / 180f), range * Mathf.Sin((i + dirDelta) * Mathf.PI / 180f));
            }
            else
            {
                TempDestroy.Add(bul);
            }

            if (destroy) break;
        }

        if (!destroy) {
            foreach (GameObject bul in TempDestroy) {
                bullets_one.Remove(bul);
            }
            TempDestroy.Clear();
        }

        foreach (GameObject bul in bullets_two)
        {
            if (bul.activeInHierarchy)
            {
                float i = bul.GetComponent<Bullet>().direction;
                bul.transform.localPosition = transform.localPosition + new Vector3(range * Mathf.Cos((i - dirDelta) * Mathf.PI / 180f), range * Mathf.Sin((i - dirDelta) * Mathf.PI / 180f));
            }
            else
            {
                TempDestroy.Add(bul);
            }

            if (destroy) break;
        }

        if (!destroy)
        {
            foreach (GameObject bul in TempDestroy)
            {
                bullets_two.Remove(bul);
            }
            TempDestroy.Clear();
        }
        else {
            foreach (GameObject bul in bullets_one)
            {
                if (bul != null) bul.GetComponent<Bullet>().UnableWithDebris();
            }
            foreach (GameObject bul in bullets_two)
            {
                if (bul != null) bul.GetComponent<Bullet>().UnableWithDebris();
            }
            bullets_one.Clear();
            bullets_two.Clear();
            Destroy(gameObject);
        }

        yield return new WaitForSeconds(1 / 60f);
        StartCoroutine(Move());
    }

    public GameObject GetBullet(string code)
    {
        return GameController.BulletPool.GetComponent<BulletPool>().GetChildMin(code);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Main")
        {
            if (moveCode == 0) StartCoroutine(DelayDestroy(0.5f));
            else if (moveCode == 1) StartCoroutine(DelayDestroy(4f));
        }
    }

    IEnumerator DelayDestroy(float sec) {
        yield return new WaitForSeconds(sec);
        destroy = true;
    }
}
