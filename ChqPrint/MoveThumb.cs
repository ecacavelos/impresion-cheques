using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace ChqPrint
{
    public class MoveThumb : Thumb
    {
        public MoveThumb()
        {
            DragDelta += new DragDeltaEventHandler(this.MoveThumb_DragDelta);
        }

        private void MoveThumb_DragDelta(object sender, DragDeltaEventArgs e)
        {
            Control designerItem = this.DataContext as Control;

            if (designerItem != null)
            {
                double left = Canvas.GetLeft(designerItem);
                double top = Canvas.GetTop(designerItem);
                

                // Se establecen los limites horizontales, no puede ser menor a 150 ni mayor a 180. 
                if (left >= 150)
                    Canvas.SetLeft(designerItem, left + e.HorizontalChange);
                else
                    Canvas.SetLeft(designerItem, 150);

                    Canvas.SetTop(designerItem, top + e.VerticalChange);
            }
        }
    }
}
