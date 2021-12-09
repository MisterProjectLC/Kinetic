using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crystal : MonoBehaviour
{
    Health health;
    [SerializeField]
    Material unactiveMaterial;
    [SerializeField]
    Material activeMaterial;

    struct Shield
    {
        public CrystalShield shield;
        public LineRenderer laserGoingToThisShield;

        public Shield(CrystalShield shield, LineRenderer laserGoingToThisShield)
        {
            this.shield = shield;
            this.laserGoingToThisShield = laserGoingToThisShield;
        }
    }

    [SerializeField]
    Transform center;

    [SerializeField]
    GameObject LaserObject;

    List<Shield> shields = new List<Shield>();

    private void Start()
    {
        health = GetComponent<Health>();
        health.OnCriticalLevel += OnDeactivate;
    }


    public void Setup(CrystalShield newShield)
    {
        Debug.Log("Setup");
        health.OnCriticalLevel += newShield.DisableGenerator;

        GameObject newLaser = Instantiate(LaserObject);
        LineRenderer lineRenderer = newLaser.GetComponent<LineRenderer>();
        lineRenderer.SetPosition(0, center.transform.position);
        shields.Add(new Shield(newShield, lineRenderer));
    }

    private void Update()
    {
        foreach(Shield shield in shields)
            if (shield.shield)
                shield.laserGoingToThisShield.SetPosition(1, shield.shield.transform.position);
    }


    public void Activate()
    {
        health.Heal(1000);
        GetComponent<Renderer>().material = activeMaterial;
        foreach (Shield shield in shields)
            shield.laserGoingToThisShield.gameObject.SetActive(true);
    }

    void OnDeactivate()
    {
        GetComponent<Renderer>().material = unactiveMaterial;
        foreach (Shield shield in shields)
            shield.laserGoingToThisShield.gameObject.SetActive(false);
    }
}
