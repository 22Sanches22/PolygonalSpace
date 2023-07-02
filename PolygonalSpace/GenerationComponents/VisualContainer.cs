using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;

namespace PolygonalSpace.GenerationComponents
{
    internal class VisualContainer : FrameworkElement
    {
        private List<Visual> visuals = new();

        protected override int VisualChildrenCount
        {
            get { return visuals.Count; }
        }

        protected override Visual GetVisualChild(int index) => visuals[index];

        public void AddElement(Visual element)
        {
            visuals.Add(element);
            base.AddVisualChild(element);
            base.AddLogicalChild(element);
        }

        public void RemoveElement(Visual element)
        {
            visuals.Remove(element);
            base.RemoveVisualChild(element);
            base.RemoveLogicalChild(element);
        }

        public void Clear()
        {
            for (int i = 0; i < visuals.Count; i++)
                RemoveElement(visuals[i]);
        }
    }
}