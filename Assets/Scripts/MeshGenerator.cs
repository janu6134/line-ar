﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.Experimental.PlayerLoop;
using UnityEngine.Serialization;


namespace LineAR {
    public class MeshGenerator : MonoBehaviour {
        [SerializeField] private bool startGrow = false;
        [SerializeField] private GameObject unitCircle;

        [SerializeField] private GameObject rbCirclePrefab;
        
        [SerializeField] private GameObject last;

        [SerializeField] private float TIMESTEP = 0.01f;
        [SerializeField] private float timer = 0.5f;

        [SerializeField] private float horizontal;

        [SerializeField] private PlayerInput input;

        [SerializeField] private Vector3 turnRot = new Vector3(0,10,0);

        [SerializeField] private GameObject rbCircle;
        [SerializeField] private bool isPlayer;
        
        private float currentAngle = 0;
        private float angle = 10;
        private const float turnAngle = 2;

        public bool StartGrow
        {
            get => startGrow;
            set => startGrow = value;
        }

        public GameObject Last
        {
            get => last;
            set => last = value;
        }

        private void Awake() {
            last = this.gameObject;
            
            input = GetComponent<PlayerInput>();
            
            // instantiate RB circle

            rbCircle = Instantiate(rbCirclePrefab, last.transform.position + (last.transform.forward * (0.005f * 2)),
                last.transform.rotation, gameObject.transform);

            Spawn(Quaternion.identity);
        }

        private void Start() {
            if (GetComponentInChildren<PlayerInput>() != null) {
                isPlayer = true;
            }

        }

        private void Spawn(Quaternion rot) {
            //spawn a small cylinder
            
            var obj = Instantiate(unitCircle, last.transform.position + (last.transform.forward * 0.005f),
                 rot, gameObject.transform);
            

            
            // Spawn that single rigidbody infront of this.
            rbCircle.transform.position = last.transform.position + (last.transform.forward * (0.005f * 2f));
            rbCircle.transform.rotation = last.transform.rotation;
            
            if (obj != null) {
                last = obj;
            }
            

        }

        private void Update() {
            if (!startGrow) return;
            if (!isPlayer) return;
            
            // get input from the PlayerInput class
            horizontal = input.Horizontal;
            angle = 0;
            if (horizontal < 0) {
                // turn left
                angle = -turnAngle;
            }else if (horizontal > 0) {
                // turn right
                angle = turnAngle;
            }
        }

        private void FixedUpdate() {
            // keep spawning
            if (!startGrow) return;

            timer -= Time.fixedDeltaTime;
            if (timer <= 0) {
                currentAngle += angle;
                Spawn(Quaternion.Euler(0, currentAngle,0));
                timer = TIMESTEP;
            }
        }
    }
}