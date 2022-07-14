using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class ShooterInfo : ObjectTileInfo
{
    public int maxAmmoCount = 10;
}

public class ShooterTile : ObjectTile
{
    public int maxAmmoCount = 10;
    public int curAmmoCount = 0;

    public override string ParseTileInfo()
    {
        return $"{{\\\"tileType\\\":" + myType + ", \\\"maxAmmoCount\\\":" + maxAmmoCount + "}";

    }

    public override void SettingTile(string info)
    {
        base.SettingTile(info);
        info = info.Substring(1, info.Length - 2);
        print(info);
        ShooterInfo shooterInfo = JsonUtility.FromJson<ShooterInfo>(info);
        myType = shooterInfo.tileType;
        maxAmmoCount = shooterInfo.maxAmmoCount;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Shoot();
        }
    }
    

    public void Shoot()
    {
        Debug.Log($"{this.name} Shooted");

        if (maxAmmoCount <= curAmmoCount)
            return;

        Ball ball = PoolManager.Instance.Pop("Ball") as Ball;
        ball.transform.position = this.transform.position;
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition); // ���콺 ��ġ �ް�
        Vector2 plusPos = (mousePos - (Vector2)ball.transform.position); // Ȥ�� ���� ��ġ �ٲ� �� ������ ��ġ ���ֱ�

        // �� �ǵ��� ����� ���� �����.
        if (Mathf.Abs(plusPos.x) > Mathf.Abs(plusPos.y)) plusPos.y = 0;
        else plusPos.x = 0;

        ball.Move(plusPos.normalized, 5f);

        curAmmoCount++;
    }
}
