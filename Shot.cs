using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Shot : MonoBehaviour {

    public float speed;
    public float direction;
    public string moveCode;
    public string additionalCode; // unlike moveCode, additionalCode becomes null when Unable() invoked
    public float damage;
    public GameObject triggerObject;

    public Sprite[] animatedSprites;
    int animIterator = 0;

    IEnumerator Move()
    {
        float radDir = direction * Mathf.PI / 180f;
        transform.localPosition += new Vector3(speed * Mathf.Cos(radDir), speed * Mathf.Sin(radDir), 0f);
        if(!"#ANCHOR".Equals(moveCode)) transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, direction));

        switch (additionalCode)
        {
            case "MARKON":
                direction += (ToEnemyAngle() > direction) ? 1.5f : -1.5f;
                break;
            case "MARS":
                transform.localPosition += new Vector3(25f, 0f);
                break;
            default:
                break;
        }

        yield return new WaitForSeconds(1 / 60f);
        StartCoroutine("Move");
    }

    float value = 0;
    IEnumerator SpecialMove()
    {
        Image myImage = GetComponent<Image>();

        switch (moveCode) {
            case "ROTATE":
                transform.rotation = Quaternion.Euler(new Vector3(0, 0, value));
                yield return new WaitForSeconds(1 / 60f);
                value -= 12; StartCoroutine("SpecialMove");
                break;
            case "KUNAI_RETURN":
                if (myImage.color.a < 0.5f) myImage.color += new Color(0f, 0f, 0f, 1 / 30f);
                else myImage.color = Color.white;
                transform.rotation = Quaternion.Euler(new Vector3(0, 0, value));
                yield return new WaitForSeconds(1 / 60f);
                value += 18; StartCoroutine("SpecialMove");
                break;
            case "TONE":
                direction += (ToEnemyAngle() > direction) ? 0.8f : -0.8f;
                yield return new WaitForSeconds(1 / 60f);
                StartCoroutine("SpecialMove");
                break;
            case "SNOWFLAKE":
                transform.rotation = Quaternion.Euler(new Vector3(0, 0, value));
                yield return new WaitForSeconds(1 / 60f);
                value -= 20; StartCoroutine("SpecialMove");
                break;
            case "#ENCELADUS":
                transform.rotation = Quaternion.Euler(new Vector3(0, 0, value));
                yield return new WaitForSeconds(1 / 60f);
                value -= 3; StartCoroutine("SpecialMove");
                break;
            case "#SLASH":
                if (animIterator >= animatedSprites.Length) Destroy(gameObject);
                else GetComponent<Image>().sprite = animatedSprites[animIterator++];

                transform.localPosition += new Vector3(value, 0f);
                value += 2.5f;
                damage = (damage > 0) ? damage - 0.1f : 0;
                yield return new WaitForSeconds(1 / 60f);
                StartCoroutine("SpecialMove");
                break;
            case "#EXPLOSION":
                if (animIterator >= animatedSprites.Length) Destroy(gameObject);
                else {
                    GetComponent<Image>().sprite = animatedSprites[animIterator++];
                    if (animIterator == 18) tag = "Untagged";
                }
                yield return new WaitForSeconds(1 / 30f);
                StartCoroutine("SpecialMove");
                break;
            case "#ECLAIRE":
                while (true) {
                    if (animIterator >= animatedSprites.Length) Destroy(gameObject);
                    else
                    {
                        Sprite tempSpr = animatedSprites[animIterator++];
                        if (tempSpr != null) GetComponent<Image>().sprite = tempSpr;
                        else GetComponent<Image>().color = Color.clear;
                    }
                    yield return new WaitForSeconds(1 / 24f);
                }               
            case "#POISON":
                if (animIterator >= animatedSprites.Length) Destroy(gameObject);
                else
                {
                    Sprite tempSpr = animatedSprites[animIterator++];
                    if (tempSpr != null) GetComponent<Image>().sprite = tempSpr;
                    else GetComponent<Image>().color = Color.clear;
                }
                yield return new WaitForSeconds(1 / 24f);
                StartCoroutine("SpecialMove");
                break;
            case "#BAT":
                if (triggerObject == null) {
                    triggerObject = GameObject.Find("NoxOrb");
                    
                }

                if (triggerObject != null) {
                    Vector3 orbVector = triggerObject.transform.localPosition - transform.localPosition;
                    if (orbVector != Vector3.zero)
                    {
                        direction = Mathf.Atan2(orbVector.y, orbVector.x) * Mathf.Rad2Deg;
                    }
                    else direction = 0;
                    speed += 0.3f;
                    yield return new WaitForSeconds(1 / 24f);
                    StartCoroutine(SpecialMove());
                }
                else {
                    yield return new WaitForSeconds(1 / 24f);
                    StartCoroutine(SpecialMove());
                }
                break;
            case "#NOX":
                while (true) {
                    if (animIterator >= animatedSprites.Length) Destroy(gameObject);
                    else
                    {                    
                        Sprite tempSpr = animatedSprites[animIterator++];
                        if (tempSpr != null)
                        {
                            GetComponent<Image>().sprite = tempSpr;
                            myImage.color -= new Color(0f, 0f, 0f, 1 / 15f);
                        }
                        else GetComponent<Image>().color = Color.clear;
                    }
                    yield return new WaitForSeconds(1 / 24f);
                }
            case "#MAPLE":
                for (myImage.color = new Color(1f, 1f, 1f, 0.5f); myImage.color.a > 0; myImage.color -= new Color(0f, 0f, 0f, 1 / 60f))
                {
                    transform.localScale += new Vector3(1 / 15f, 1 / 15f);
                    yield return new WaitForSeconds(1 / 60f);
                }
                Destroy(gameObject);
                break;
            case "#SUN":
                for (myImage.color = new Color(1f, 1f, 1f, 0.5f); myImage.color.a > 0; myImage.color -= new Color(0f, 0f, 0f, 1 / 600f))
                {
                    yield return new WaitForSeconds(1 / 60f);
                }
                Destroy(gameObject);
                break;
            case "#ANCHOR":
                while (true) {
                    if (transform.localPosition.y < -800f) Destroy(gameObject);
                    yield return new WaitForSeconds(1 / 60f);
                }
            default:
                break;
        }
    }

    // Direction uses degree (not radian)
    public void SetIdentity(Vector3 pos, float speed, float direction, float scaleX, float scaleY, float damage)
    {
        gameObject.SetActive(true);

        this.speed = speed;
        this.direction = direction;
        this.damage = damage;

        transform.localPosition = pos;
        transform.localScale = new Vector3(scaleX, scaleY, 1f);

        if (speed != 0)
        {
            StartCoroutine("Move");
        }
        if (moveCode != null)
        {
            if ("TONE".Equals(moveCode)) GetComponent<Image>().color = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f), 1f);
            StartCoroutine("SpecialMove");
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Main")
        {
            Unable();
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if ("KUNAI_RETURN".Equals(moveCode) && GetComponent<Image>().color.a == 1) {
            if (collision.tag == "Player") {
                GameObject tmp = GameController.ShotPool.GetComponent<ShotPool>().GetChildMin("MAIN");
                if (tmp != null) tmp.GetComponent<Shot>().SetIdentity(transform.localPosition, 30f, 0f, 1f, 1f,
                    collision.gameObject.GetComponent<Player>().mainShotDamage);

                Unable();
            }
        }

        if ("SNOWFLAKE".Equals(moveCode) || "MASTERPIANO".Equals(additionalCode) || "MARS".Equals(additionalCode)
            || "#ENCELADUS".Equals(moveCode) || "#NOX".Equals(moveCode) || "#ANCHOR".Equals(moveCode)) {
            if (collision.tag == "Bullet") collision.gameObject.GetComponent<Bullet>().UnableWithDebris();
        }

        if ("POISON".Equals(moveCode) || "#POISON".Equals(moveCode)) {
            if (collision.tag == "Enemy")
            {
                bool flag = true;
                Transform enemyTr = collision.transform;
                for (int i = enemyTr.childCount - 1; i >= 0; i--) {
                    if ("PoisonGenerator(Clone)".Equals(enemyTr.GetChild(i).name)) {
                        flag = !flag;
                        break;
                    }
                }
                if (flag) {
                    Instantiate(triggerObject, enemyTr);
                }
            }
        }
    }

    private void Update()
    {
        if (Mathf.Abs(transform.localPosition.x) > 1000f || Mathf.Abs(transform.localPosition.y) > 1000f)
        {
            moveCode = null;
            gameObject.SetActive(false);
        }
    }

    public void Unable()
    {
        if ("KUNAI_RETURN".Equals(moveCode)) GetComponent<Image>().color = Color.clear;
        additionalCode = null;
        gameObject.SetActive(false);
    }

    float min = 10000;
    public GameObject GetNearestEnemy()
    {
        GameObject returnObj = null;
        foreach (GameObject en in GameObject.FindGameObjectsWithTag("Enemy"))
        {
            float dist = Vector3.Distance(en.transform.localPosition, transform.localPosition);
            if (min > dist)
            {
                min = dist;
                returnObj = en;
            }
        }

        min = 10000;
        return returnObj;
    }

    public float ToEnemyAngle()
    {
        GameObject enemy = GetNearestEnemy();
        if (enemy != null)
        {
            Vector3 moveDirection = enemy.transform.localPosition - transform.localPosition;
            if (moveDirection != Vector3.zero)
            {
                float angle = Mathf.Atan2(moveDirection.y, moveDirection.x) * Mathf.Rad2Deg;
                return angle;
            }
            else return 0;
        }
        else return 0;
    }
}
