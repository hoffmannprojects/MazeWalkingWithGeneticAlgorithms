using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Brain : MonoBehaviour {

    #region Properties
    public float TimeAlive
    {
        get { return timeAlive; }
        private set { timeAlive = value; }
    }

    public float TimeSpentWalking
    {
        get { return timeSpentWalking; }
        private set { timeSpentWalking = value; }
    }

    public Dna Dna { get; private set; }
    #endregion

    [SerializeField] private GameObject ethanPrefab;
    [SerializeField] private GameObject eyes;
    [SerializeField] private float timeAlive;
    [SerializeField] private float timeSpentWalking;
    [SerializeField] private float debugRaycastLifetime = 1f;

    private int DnaLength = 2;
    private bool isAlive = true;
    private bool canSeeGround = true;
    private MeshRenderer[] meshRenderers;
    private GameObject ethan;

    #region Public Methods
    public void Init ()
    {
        // Initialize Dna.
        // 0 = move forward.
        // 1 = turn left.
        // 2 = turn right.
        Dna = new Dna(DnaLength, 3);
        TimeAlive = 0f;
        timeSpentWalking = 0f;
        isAlive = true;
    }
    #endregion

    private void Start ()
    {
        meshRenderers = GetComponentsInChildren<MeshRenderer>();
    }
    // Update is called once per frame
    void Update ()
    {
        if (!isAlive)
        {
            foreach (var meshRenderer in meshRenderers)
            {
                meshRenderer.material.color = Color.gray;
            }
            return;
        }
        CheckIfCanSeeGround();
        MoveBasedOnDna();
    }

    private void CheckIfCanSeeGround ()
    {
        Debug.DrawRay(eyes.transform.position, eyes.transform.forward * 10, Color.red, debugRaycastLifetime);
        canSeeGround = false;

        RaycastHit hit;
        if (Physics.Raycast(eyes.transform.position, eyes.transform.forward * 10, out hit))
        {
            if (hit.collider.gameObject.tag == "platform")
            {
                canSeeGround = true;
            }
        }
        TimeAlive = PopulationManager.TimeElapsed;
    }

    private void MoveBasedOnDna ()
    {
        float turn = 0;
        float move = 0;

        if (canSeeGround)
        {
            if (Dna.Genes[0] == 0)
            {
                move = 1;
                timeSpentWalking += Time.deltaTime;
            }
            else if (Dna.Genes[0] == 1)
            {
                turn = -90;
            }
            else if (Dna.Genes[0] == 2)
            {
                turn = 90;
            }
        }
        else
        {
            if (Dna.Genes[1] == 0)
            {
                move = 1;
                timeSpentWalking += Time.deltaTime;
            }
            else if (Dna.Genes[1] == 1)
            {
                turn = -90;
            }
            else if (Dna.Genes[1] == 2)
            {
                turn = 90;
            }
        }

        this.transform.Translate(0, 0, move * 0.1f);
        this.transform.Rotate(0, turn, 0);
    }

    private void OnCollisionEnter (Collision collision)
    {
        if(collision.gameObject.tag == "dead")
        {
            isAlive = false;
        }
    }

    private void OnDestroy ()
    {
        Destroy(ethan);
    }
}
