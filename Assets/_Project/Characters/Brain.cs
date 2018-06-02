using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Brain : MonoBehaviour {

    #region Properties

    public float DistanceWalked
    {
        get { return distanceWalked; }
        private set { distanceWalked = value; }
    }

    public Dna Dna { get; private set; }
    #endregion

    [SerializeField] private GameObject eyes;
    [SerializeField] private float distanceWalked;
    [SerializeField] private float raycastLength = 5f;
    [SerializeField] private float debugRaycastLifetime = 1f;
    [SerializeField] private bool obstacleAhead = false;
    [SerializeField] private float moveSpeed = 10f;
    [SerializeField] private int gene0;
    [SerializeField] private int gene1;

    private int DnaLength = 2;
    private Vector3 startPosition;
    private Rigidbody rb;

    #region Public Methods
    public void Init ()
    {
        // Initialize Dna.
        // 0 = move forward.
        // 1 = turn left.
        // 2 = turn right.
        Dna = new Dna(DnaLength, 3);
        distanceWalked = 0f;
    }
    #endregion

    private void Start ()
    {
        GetComponent<MeshRenderer>().material.color = Color.red;
        startPosition = this.transform.position;
        rb = GetComponent<Rigidbody>();
    }
    // Update is called once per frame
    void Update ()
    {
        CheckForObstacle();
    }

    private void FixedUpdate ()
    {
        MoveBasedOnDna();
    }

    private void CheckForObstacle ()
    {
        Debug.DrawRay(eyes.transform.position, eyes.transform.forward * raycastLength, Color.red, debugRaycastLifetime);
        obstacleAhead = false;

        RaycastHit hit;
        if (Physics.Raycast(eyes.transform.position, eyes.transform.forward, out hit, raycastLength))
        {
            obstacleAhead = true;
        }
    }

    private void MoveBasedOnDna ()
    {
        float turn = 0;
        float move = 0;

        if (obstacleAhead)
        {
            if (Dna.Genes[0] == 0)
            {
                move = 1;
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
        gene0 = Dna.Genes[0];
        gene1 = Dna.Genes[1];
        
        Vector3 moveDirection = transform.forward * move * moveSpeed;
        rb.MovePosition(this.transform.position + moveDirection * Time.deltaTime);

        this.transform.Rotate(0, turn, 0);

        distanceWalked = Vector3.Distance(startPosition, this.transform.position);
    }
}
