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
    [SerializeField] private float raycastMaxDistance = 5f;
    [SerializeField] private float debugRaycastLifetime = 1f;
    [SerializeField] private bool obstacleAhead = false;
    [SerializeField] private float moveSpeed = 0.01f;
    [SerializeField] private int gene0;
    [SerializeField] private int gene1;

    private int DnaLength = 2;
    private Vector3 startPosition;
    private Rigidbody rb;

    #region Public Methods
    public void Init ()
    {
        // Initialize Dna. Both genes store values between 0 and 360.
        // Gene 0 = move forward.
        // Gene 1 = turn angle.
        Dna = new Dna(DnaLength, 360);
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

        Vector3 origin = eyes.transform.position;
        float radius = 0.1f;
        Vector3 direction = eyes.transform.forward;
        RaycastHit hit;

        Debug.DrawRay(origin, direction * raycastMaxDistance, Color.red, debugRaycastLifetime);
        obstacleAhead = false;

        if (Physics.SphereCast(origin, radius, direction, out hit, raycastMaxDistance))
        {
            obstacleAhead = true;
        }
    }

    private void MoveBasedOnDna ()
    {
        float turn = 0;
        float move = Dna.Genes[0];

        if (obstacleAhead)
        {
            turn = Dna.Genes[1];
        }

        // Only used for debugging in the inspector.
        gene0 = Dna.Genes[0];
        gene1 = Dna.Genes[1];
        
        Vector3 moveDirection = transform.forward * move * moveSpeed;
        rb.MovePosition(this.transform.position + moveDirection * Time.deltaTime);

        this.transform.Rotate(0, turn, 0);

        distanceWalked = Vector3.Distance(startPosition, this.transform.position);
    }
}
