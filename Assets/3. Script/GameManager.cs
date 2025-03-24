using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private const int towerCount = 40;

    [SerializeField]private Transform towerParent;
    [SerializeField]private GameObject[] towers;

    private void Start()
    {
        for (int i = 0; i < towerCount; i++)
        {
            int rand = Random.Range(0, towers.Length);
            Vector3 createPos = new Vector3(Random.Range(1, 60), 3, Random.Range(1, 60));
            Instantiate(towers[rand], createPos, Quaternion.identity, towerParent);
        }
    }
}
