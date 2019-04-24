using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour {

    public Sprite[] img;
    public int imgIndex = 0;
    public float startDelay = 0f;

    public float health;
    public Sprite collapseImage;

    public bool immortal = false;
    public Color collapseColor;

	// Use this for initialization
	void Start () {
        StartCoroutine("Animate");
        StartCoroutine("StartLaunch");
        StartCoroutine("Move");

        float[] healthDelta = new float[] { 1f, 1.2f, 1.5f, 1.8f, 2.1f, 2.8f, 2.8f };
        if(GameController.stageNum >= 0) health *= healthDelta[GameController.stageNum];
    }

    public IEnumerator Animate() {
        gameObject.GetComponent<Image>().sprite = img[imgIndex];
        yield return new WaitForSeconds(1 / 4f);
        imgIndex = (imgIndex == img.Length - 1) ? 0 : imgIndex + 1;
        StartCoroutine("Animate");
    }

    public IEnumerator StartLaunch() {
        if (startDelay >= 0)
        {
            yield return new WaitForSeconds(startDelay);
            StartCoroutine("Launch");
        }
        else yield return null;
    }

    // for overriding
    IEnumerator Move()
    {
        yield return null;
    }

    // for overriding
    IEnumerator Launch() {
        yield return null;
    }

    public GameObject GetBullet(string code) {
        return GameController.BulletPool.GetComponent<BulletPool>().GetChildMin(code);
    }

    public GameObject GetPlayer() {
        GameObject returnObj = null;
        foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Player"))
        {
            returnObj = obj;
        }

        return returnObj;
    }

    public float ToPlayerAngle() {
        GameObject player = GetPlayer();
        Vector3 moveDirection = player.transform.localPosition - transform.localPosition;
        if (moveDirection != Vector3.zero)
        {
            float angle = Mathf.Atan2(moveDirection.y, moveDirection.x) * Mathf.Rad2Deg;
            return angle;
        }
        else return 0;
    }

    // Overrided by 'boss'
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Shot" && !immortal)
        {
            Shot s = collision.GetComponent<Shot>();

            health -= s.damage;
            if ("KUNAI".Equals(s.moveCode)) {
                GameObject tmp = GameController.ShotPool.GetComponent<ShotPool>().GetChildMin("SUPPORT");
                if (tmp != null) tmp.GetComponent<Shot>().SetIdentity(transform.localPosition, 30f, Random.Range(175f, 185f), 1f, 1f, 0);
            }
            if (s.moveCode != null) {
                if (!s.moveCode.Contains("#")) s.Unable();
            }
           
            CheckHealth();
        }
    }

    public void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Shot" && !immortal)
        {
            Shot s = collision.GetComponent<Shot>();

            if (s.moveCode != null) {
                if(s.moveCode.Contains("#")) health -= collision.GetComponent<Shot>().damage;
            }
            
            CheckHealth();
        }
    }

    private void OnDestroy()
    {
        Collapse();
    }

    public virtual void CheckHealth() {
        if (health <= 0) Destroy(gameObject);
    }

    public void Collapse() {
        if (health <= 0 || gameObject.GetComponent<Boss>() != null) {
            GameObject ins = null;
            GameObject CollapsedEnemy = GameObject.Find("GameController").GetComponent<GameController>().CollapsedEnemy;
            if (CollapsedEnemy != null) ins = Instantiate(CollapsedEnemy, GameObject.Find("TriggerPanel").transform);
            if (ins != null)
            {
                Vector2 face = GetComponent<RectTransform>().sizeDelta;

                ins.transform.localPosition = transform.localPosition;
                ins.GetComponent<CollapsedEnemy>().SetIdentity(collapseImage, face.x, face.y, transform.localScale.x, transform.localScale.y, collapseColor);
            }
        }             
    }
}
