﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace GraphomatDrawingLibUwp
{
    class PixelPointsManager
    {
        private bool isDrawing, editingEndedDuringDrawing, isEditing;

        private PixelPoints pointsCur, pointsEdit;

        public PixelPoints Points { get { return pointsCur; } }

        public PixelPointsManager(ValuePoints valuePoints)
        {
            pointsCur = new PixelPoints(valuePoints);
            pointsEdit = new PixelPoints(valuePoints);

            isDrawing = editingEndedDuringDrawing = isEditing = false;
        }

        public void BeginUsing()
        {
            isDrawing = true;
        }

        public void EndUsing()
        {
            isDrawing = false;

            if (!editingEndedDuringDrawing || !isEditing) return;

            SwitchPoints();

            editingEndedDuringDrawing = false;
        }

        private void BeginEditing()
        {
            isEditing = true;
        }

        private void EndEditing()
        {
            isEditing = false;

            if (isDrawing) editingEndedDuringDrawing = true;
            else SwitchPoints();
        }

        public void Calculate(ViewArgs args)
        {
            BeginEditing();
            pointsEdit.Calculate(args);
            EndEditing();
        }

        public void Recalculate(ViewArgs args)
        {
            BeginEditing();
            pointsEdit.Recalculate(args);
            EndEditing();
        }

        private void SwitchPoints()
        {
            PixelPoints tmp = pointsCur;
            pointsCur = pointsEdit;
            pointsEdit = tmp;
        }
    }
}