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
    [SerializeField] private float debugRaycastLifetime = 1f;

    private int DnaLength = 2;
    private bool canSeeGround = true;
    private Vector3 startPosition;

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
    }
    // Update is called once per frame
    void Update ()
    {
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
            if (hit.collider.gameObject.tag == "Ground")
            {
                canSeeGround = true;
            }
        }
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

        this.transform.Translate(0, 0, move * 0.1f);
        this.transform.Rotate(0, turn, 0);

        distanceWalked = Vector3.Distance(startPosition, this.transform.position);
    }
}
