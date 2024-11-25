using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FinalProject
{
    public class PieceAnimation
    {
        private Timer animationTimer;
        private Button pieceButton;
        private Point startPosition;
        private Point endPosition;
        private int steps = 15; // Number of steps in animation
        private int currentStep = 0;
        private Action onAnimationComplete;

        public PieceAnimation(Button button)
        {
            pieceButton = button;
            animationTimer = new Timer
            {
                Interval = 20 // 20ms between frames
            };
            animationTimer.Tick += AnimationTimer_Tick;
        }

        public void AnimateMove(Point start, Point end, Action onComplete = null)
        {
            if (animationTimer.Enabled) return; // Don't start if already animating

            startPosition = start;
            endPosition = end;
            currentStep = 0;
            onAnimationComplete = onComplete;
            animationTimer.Start();
        }

        private void AnimationTimer_Tick(object sender, EventArgs e)
        {
            currentStep++;

            if (currentStep <= steps)
            {
                // Calculate new position using easing function
                double progress = (double)currentStep / steps;
                double easedProgress = EaseInOutQuad(progress);

                int newX = startPosition.X + (int)((endPosition.X - startPosition.X) * easedProgress);
                int newY = startPosition.Y + (int)((endPosition.Y - startPosition.Y) * easedProgress);

                pieceButton.Location = new Point(newX, newY);
            }
            else
            {
                animationTimer.Stop();
                pieceButton.Location = endPosition;
                currentStep = 0;
                onAnimationComplete?.Invoke();
            }
        }

        // Smooth easing function for better animation feel
        private double EaseInOutQuad(double t)
        {
            return t < 0.5 ? 2 * t * t : 1 - Math.Pow(-2 * t + 2, 2) / 2;
        }
    }
}
