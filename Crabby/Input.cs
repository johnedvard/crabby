using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;

namespace Crabby
{
    public static class Input
    {
        private static TouchCollection touches, previousTouches;
        static Input()
        {
            
        }

        
        public static void ProcessTouchInput(out Vector2 player1Velocity, out bool isTappingGesture, out bool isFlickingGesture)
        {
            
            player1Velocity = new Vector2(0, 0);
            isTappingGesture = false;
            isFlickingGesture = false;

            previousTouches = touches;
            touches = TouchPanel.GetState();
            

            while (TouchPanel.IsGestureAvailable)
            {
                //take care of multitouches
                for (int i = 0; i < touches.Count; i++)
                {
                    if (touches[i].State != TouchLocationState.Pressed)
                        continue;
                    if (touches.Count >= 1)
                        isTappingGesture = true;
                }

                // take care of gestures
                GestureSample gestureSample = TouchPanel.ReadGesture();
                if (gestureSample.GestureType == GestureType.FreeDrag && gestureSample.Position.Y > 0)
                    player1Velocity += gestureSample.Delta;
                if (gestureSample.GestureType == GestureType.Flick && gestureSample.GestureType != GestureType.HorizontalDrag)
                    isFlickingGesture = true;
                if (gestureSample.GestureType == GestureType.Tap)
                    isTappingGesture = true;
            }

        }
    }
}
