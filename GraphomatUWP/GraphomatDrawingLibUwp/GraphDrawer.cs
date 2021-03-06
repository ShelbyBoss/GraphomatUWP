﻿using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace GraphomatDrawingLibUwp
{
    internal abstract class GraphDrawer
    {
        private const float nearDistance = 20;
        protected const int movingSkipPoints = 0;
        public const float Thickness = 2.5F;

        public Graph Graph { get; }

        private ViewArgs viewArgs;

        public ViewArgs ViewArgs
        {
            get => viewArgs;
            set
            {
                if (value == viewArgs) return;

                viewArgs = value;
                MoveScrollView();
            }
        }


        internal GraphDrawer(Graph graph, ViewArgs args)
        {
            Graph = graph;
            viewArgs = args;
        }

        public void Move(Vector2 deltaValue, ViewArgs viewArgs)
        {
            this.viewArgs = viewArgs;

            Move(deltaValue);
        }

        protected abstract void Move(Vector2 deltaValue);

        protected abstract void MoveScrollView();

        public bool IsNearCurve(Vector2 refPoint, out float minDistance)
        {
            minDistance = float.MaxValue;
            Vector2[] points = GetPoints().Where(IsRelevantInView).Select(ToViewPoint).ToArray();
            Vector2 prePoint = points.FirstOrDefault();

            foreach (Vector2 viewPoint in points.Skip(1))
            {
                float distance = GetDistance(refPoint, prePoint, viewPoint);

                if (distance < minDistance) minDistance = distance;

                prePoint = viewPoint;
            }

            return minDistance <= nearDistance;
        }

        public void GetMinAndMaxValue(out float min, out float max)
        {
            bool anyPoints = false;

            min = float.MaxValue;
            max = float.MinValue;

            foreach (Vector2 point in GetPoints().Where(IsRelevantInView))
            {
                anyPoints = true;

                if (max < point.Y) max = point.Y;
                if (min > point.Y) min = point.Y;
            }

            if (anyPoints && min != max) return;

            min = -1;
            max = 1;
        }

        protected abstract IEnumerable<Vector2> GetPoints();

        protected Vector2 ToViewPoint(Vector2 valuePoint)
        {
            return float.IsNaN(valuePoint.Y) ? valuePoint : ViewArgs.ToView(valuePoint);
        }

        private bool IsRelevantInView(Vector2 valuePoint)
        {
            return valuePoint.X >= ViewArgs.ValueDimensions.Left &&
                valuePoint.X <= ViewArgs.ValueDimensions.Right && !float.IsNaN(valuePoint.Y);
        }

        private static float GetDistance(Vector2 point, Vector2 v1, Vector2 v2)
        {
            float k12 = (v2.Y - v1.Y) / (v2.X - v1.X);
            float kp = -(v2.X - v1.X) / (v2.Y - v1.Y);

            float crossX = (k12 * v1.X - v1.Y - kp * point.X + point.Y) / (k12 - kp);
            float crossY = k12 * (crossX - v1.X) + v1.Y;

            if (crossX <= Math.Max(v1.X, v2.X) && crossX >= Math.Min(v1.X, v2.X))
            {
                Vector2 cross = new Vector2(crossX, crossY);
                return (point - cross).Length();
            }

            float dist1 = (point - v1).Length();
            float dist2 = (point - v2).Length();

            return Math.Min(dist1, dist2);
        }

        public abstract CanvasGeometry GetGeometry(ICanvasResourceCreator iCreator, bool isMoving);

        public override string ToString()
        {
            return Graph.Name;
        }
    }
}
