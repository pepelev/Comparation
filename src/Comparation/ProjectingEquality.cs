﻿using System;
using System.Collections.Generic;

namespace Comparation
{
    public sealed class ProjectingEquality<Subject, Projection> : IEqualityComparer<Subject>
    {
        private readonly Func<Subject, Projection> projection;
        private readonly IEqualityComparer<Projection> equality;

        public ProjectingEquality(Func<Subject, Projection> projection, IEqualityComparer<Projection> equality)
        {
            this.projection = projection;
            this.equality = equality;
        }

        public bool Equals(Subject x, Subject y) => equality.Equals(
            projection(x),
            projection(y)
        );

        public int GetHashCode(Subject obj) => equality.GetHashCode(projection(obj));
    }
}