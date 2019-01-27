using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InsectScript : MonoBehaviour {

    public static Sprite rSprite;
    public static Sprite lSprite;

    float timer;

    SpriteRenderer sprite;

    bool rightSide;

    OldManScript oldMan;

    float speed;

    float size;

    float lastAttack;
    float attackCooldown;

    BrickScript targetBrick;

	// Use this for initialization
	void Start () {
        speed = 0.02f;
        attackCooldown = 1f;

        size = Random.Range(1f, 1.6f);

        speed /= size;
        transform.localScale = transform.localScale * size;

        if (rSprite == null)
            Load();

        sprite = GetComponent<SpriteRenderer>();
        sprite.sprite = lSprite;

        oldMan = FindObjectOfType<OldManScript>();
	}
	

	// Update is called once per frame
	void Update () {
        timer += Time.deltaTime;

        float changePeriod = speed * 10f;
        if(timer > changePeriod)
        {
            timer -= changePeriod;

            rightSide = !rightSide;
            if (rightSide)
                sprite.sprite = rSprite;
            else
                sprite.sprite = lSprite;

        }

	}

    private void FixedUpdate()
    {
        Vector2 target;

        if (targetBrick == null)
            target = oldMan.transform.position;
        else
            target = targetBrick.transform.position;

        Vector2 direction = target - (Vector2)transform.position;
        direction.Normalize();

        transform.position = transform.position + (Vector3)(direction * speed);
        transform.rotation = Quaternion.AngleAxis(Mathf.Atan2(direction.y, direction.x) * 360 / (Mathf.PI * 2), Vector3.forward);

        if((oldMan.transform.position - transform.position).magnitude <= 0.4f)
        {
            if(Time.time > lastAttack + attackCooldown)
            {
                oldMan.Damage(0.1f);
                lastAttack = Time.time;
            }
        }

        //Check Attack
        if (Time.time > lastAttack + attackCooldown)
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, 0.3f, LayerMask.GetMask(new string[] { "Bricks" }));
            if (hit.collider != null)
            {
                BrickScript brick = hit.collider.GetComponent<BrickScript>();
                if (brick == null)
                    Debug.LogWarning("BAD collision on layer");

                targetBrick = brick;
                brick.Damage(0.1f);
                lastAttack = Time.time;
            }
        }
    }

    public void Hit()
    {
        GameObject body = new GameObject("Body");
        SpriteRenderer bodySprite = body.AddComponent<SpriteRenderer>();
        float rdmV = Random.value;
        if (rdmV > 0.66f)
            bodySprite.sprite = Resources.Load<Sprite>("InsectBody");
        else if (rdmV > 0.33f)
            bodySprite.sprite = Resources.Load<Sprite>("InsectBody2");
        else
            bodySprite.sprite = Resources.Load<Sprite>("InsectBody3");
        bodySprite.sortingOrder = -2;
        bodySprite.color = new Color(1f, 1f, 1f, 0.8f);
        body.transform.position = transform.position;
        body.transform.rotation = transform.rotation;
        body.transform.localScale = transform.localScale * (1f + Random.value * 0.3f);

        oldMan.killCount++;
        Destroy(gameObject);
    }

    public void Load()
    {
        rSprite = Resources.Load<Sprite>("insectR");
        lSprite = Resources.Load<Sprite>("insectL");
    }

    public static void CreateInsect(Vector2 position)
    {
        GameObject insect = (GameObject)Instantiate(Resources.Load("InsectPrefab"));
        insect.transform.position = position;
    }
}
