// Dogukan Kaan Bozkurt
//      github.com/dkbozkurt

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace FreeDraw
{
    /// <summary>
    /// Drawing Settings
    ///
    /// WORKS ON PLAYABLE
    ///
    /// Ref : https://assetstore.unity.com/packages/tools/painting/free-draw-simple-drawing-on-sprites-2d-textures-113131
    /// </summary>
    public class DrawingSettings : MonoBehaviour
    {
        public static bool isCursorOverUI = false;
        public float Transparency = 1f;

        [Header("Default Color Setting")]
        [SerializeField] private Color initialColor = Color.black;
        [SerializeField] private int initialMarkerWidth = 15;

        private void Awake()
        {
            SetMarkerWidth(initialMarkerWidth);
            //SetMarkerAlpha();
            SetMarkerInputColor(initialColor);
        }

        // Changing pen settings is easy as changing the static properties Drawable.Pen_Colour and Drawable.Pen_Width
        public void SetMarkerColour(Color new_color)
        {
            Drawable.Pen_Colour = new_color;
        }
        // new_width is radius in pixels
        public void SetMarkerWidth(int new_width)
        {
            Drawable.Pen_Width = new_width;
        }
        public void SetMarkerWidth(float new_width)
        {
            SetMarkerWidth((int)new_width);
        }

        public void SetTransparency(float amount)
        {
            Transparency = amount;
            Color c = Drawable.Pen_Colour;
            c.a = amount;
            Drawable.Pen_Colour = c;
        }


        // // Call these these to change the pen settings
        // public void SetMarkerRed()
        // {
        //     Color c = Color.red;
        //     c.a = Transparency;
        //     SetMarkerColour(c);
        //     Drawable.drawable.SetPenBrush();
        // }
        // public void SetMarkerGreen()
        // {
        //     Color c = Color.green;
        //     c.a = Transparency;
        //     SetMarkerColour(c);
        //     Drawable.drawable.SetPenBrush();
        // }
        // public void SetMarkerBlue()
        // {
        //     Color c = Color.blue;
        //     c.a = Transparency;
        //     SetMarkerColour(c);
        //     Drawable.drawable.SetPenBrush();
        // }
        
        // Set marker Color
        public void SetMarkerInputColor(Color color)
        {
            color.a = Transparency;
            SetMarkerColour(color);
            Drawable.drawable.SetPenBrush();
        }

        // Set marker to Alpha
        public void SetMarkerAlpha()
        {
            Color c = new Color(1, 1, 1, 0);
            SetMarkerColour(c);
            Drawable.drawable.SetPenBrush();
        }
        public void SetEraser()
        {
            SetMarkerColour(new Color(255f, 255f, 255f, 0f));
        }

        public void PartialSetEraser()
        {
            SetMarkerColour(new Color(255f, 255f, 255f, 0.5f));
        }
    }
}