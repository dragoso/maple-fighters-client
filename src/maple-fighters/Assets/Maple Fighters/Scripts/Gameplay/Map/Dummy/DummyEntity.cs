﻿using System;
using Game.Common;
using UnityEngine;

namespace Scripts.Gameplay.Map.Dummy
{
    [Serializable]
    public class DummyEntity
    {
        public DummyObjects Type;
        public int Id;
        public Vector2 Position;
        public Directions Direction;
    }
}