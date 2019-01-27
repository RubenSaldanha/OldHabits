using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrickScript : MonoBehaviour {

    public static Sprite brick;
    public static Sprite brickd1;
    public static Sprite brickd2;
    public static Sprite brickd3;

    SpriteRenderer renderer;

    public float health;

    static int bricks;

    // Use this for initialization
    void Start () {
        health = 1f;
        if (brick == null)
            Load();

        renderer = gameObject.AddComponent<SpriteRenderer>();
        BoxCollider2D box = gameObject.AddComponent<BoxCollider2D>();
        box.size = new Vector2(1f, 1f);

        int rr = (int)(Random.Range(0, 4) * 0.99f);
        transform.rotation = Quaternion.AngleAxis(rr * 90, Vector3.forward);

        gameObject.layer = LayerMask.NameToLayer("Bricks");

        GameObject bedRock = new GameObject("BedRock");
        SpriteRenderer bedSprite = bedRock.AddComponent<SpriteRenderer>();
        bedSprite.sprite = Resources.Load<Sprite>("BedRock");
        bedSprite.sortingOrder = -8;
        //bedRock.transform.SetParent(transform, false);
        bedRock.transform.position = transform.position;
        bedRock.transform.localScale = transform.localScale;
        bedRock.transform.rotation = transform.rotation;
	}
	
	// Update is called once per frame
	void Update () {
        if (health > 0.75f)
            renderer.sprite = brick;
        else if (health > 0.5f)
            renderer.sprite = brickd1;
        else if (health > 0.25f)
            renderer.sprite = brickd2;
        else
            renderer.sprite = brickd3;

        if (health <= 0f)
            Destroy(gameObject);
	}

    public void Damage(float value)
    {
        health -= value;
    }

    public static BrickScript CreateBrick(Vector2 position)
    {
        //Debug.Log("Brick insta");
        bricks++;
        GameObject gObj = new GameObject("Brickdsa" + bricks);
        gObj.transform.position = position;
        BrickScript brick = gObj.AddComponent<BrickScript>();

        return brick;
    }

    public void Load()
    {
        brick = Resources.Load<Sprite>("brick");
        brickd1 = Resources.Load<Sprite>("brickd1");
        brickd2 = Resources.Load<Sprite>("brickd2");
        brickd3 = Resources.Load<Sprite>("brickd3");
    }
}
