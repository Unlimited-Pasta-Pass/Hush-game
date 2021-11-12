﻿using System;
using Common.Models;
using Enemies.Enums;

namespace Enemies
{
    [Serializable]
    public class EnemyModel
    {
        public EnemyState state;

        public SerializableTransform transform;
    }
}