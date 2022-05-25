using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CheesePointUI : MonoBehaviour
{
    [SerializeField] Image cheeseImage;
    [SerializeField] Text cheeseAmount;
    [SerializeField] Vector2 spawnPosRange = new Vector2(20f, 20f);
    [SerializeField] Vector2 moveSpeedRange = new Vector2(2f, 5f);
    float moveSpeed;
    [SerializeField] Vector2 moveDirRange = new Vector2(-5, 5);
    float moveDir;
    [SerializeField] float lifetime = 0.5f;
    float aliveTime = 0f;

    public void UpdateText(int amount)
    {
        cheeseAmount.text = "+" + amount.ToString();
        if (amount < 0)
        {
            cheeseAmount.color = Color.red;
            cheeseAmount.text = amount.ToString();
        }
        else if (amount > 1)
        {
            cheeseAmount.color = Color.yellow;
            lifetime *= 2f;
            transform.localScale *= 1.5f;
        }
        moveSpeed = Random.Range(moveSpeedRange.x, moveSpeedRange.y);
        moveDir = Random.Range(moveDirRange.x, moveDirRange.y);
        transform.localPosition += new Vector3(Random.Range(spawnPosRange.x, spawnPosRange.y), Random.Range(spawnPosRange.x, spawnPosRange.y), 0f);
    }

    void Update()
    {
        aliveTime += Time.deltaTime;
        if (aliveTime > lifetime)
        {
            Destroy(gameObject);
        }
        Vector3 dir = Vector3.up + Vector3.right * moveDir;
        transform.localPosition += dir.normalized * moveSpeed;

        Color imageColor = cheeseImage.color;
        imageColor.a = 1 - (aliveTime / lifetime);
        cheeseImage.color = imageColor;

        Color textColor = cheeseAmount.color;
        textColor.a = 1 - (aliveTime / lifetime);
        cheeseAmount.color = textColor;
    }
}
