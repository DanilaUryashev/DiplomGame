using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityPlayer : MonoBehaviour
{
    private Enemy enemy;
    [Header("Ability Active")]
    [SerializeField] private bool viewEnemyFov; 

    void Start()
    {
        enemy= GetComponentInParent<Enemy>();

    }

    // Update is called once per frame
    void Update()
    {

    }
    public void ViewEnemyFOV(bool activeAbil)
    {
        viewEnemyFov = activeAbil;
        enemy.FovActive(viewEnemyFov);
    }

}
