﻿using UnityEngine;

namespace InterestManagement.Scripts
{
    public class Entity : MonoBehaviour, ISceneObject
    {
        public int Id { get; set; }
        public GameObject GetGameObject() => gameObject;

        private static int id;
        private IScene scene;

        private void Awake()
        {
            Id = ++id;
            name = $"{name} (Id: {Id})";
        }

        private void Start()
        {
            scene = GameObject.FindGameObjectWithTag("Scene").GetComponent<IScene>();
            scene?.AddSceneObject(this);
        }

        private void OnDestroy()
        {
            scene?.RemoveSceneObject(this);
        }
    }
}