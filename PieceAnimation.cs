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
        private int steps = 15; //num of steps
        private int currentStep = 0;
        private Action onAnimationComplete;

        public PieceAnimation(Button button)
        {
            pieceButton = button;
            animationTimer = new Timer
            {
                Interval = 20 // times bwtween frames (made it 20 because of stuttering)
            };
            animationTimer.Tick += AnimationTimer_Tick;
        }

        public void AnimateMove(Point start, Point end, Action onComplete = null)
        {
            if (animationTimer.Enabled) return; //if animating - dont redo it

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
                // calculate the new positiomnm
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

        //smoothing the animation - we couldnt do it properly, so we tried using gpt for it, it works somehow
        private double EaseInOutQuad(double t)
        {
            return t < 0.5 ? 2 * t * t : 1 - Math.Pow(-2 * t + 2, 2) / 2;
        }
    }
}
