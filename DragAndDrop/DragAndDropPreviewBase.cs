using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace DragAndDrop
{
    class DragAndDropPreviewBase : UserControl
    {
        public DragAndDropPreviewBase()
        {
            ScaleTransform scale = new ScaleTransform(1f, 1f);
            SkewTransform skew = new SkewTransform(0f, 0f);
            RotateTransform rotate = new RotateTransform(0f);
            TranslateTransform translate = new TranslateTransform(0f, 0f);
            TransformGroup transformGroup = new TransformGroup();
            transformGroup.Children.Add(scale);
            transformGroup.Children.Add(skew);
            transformGroup.Children.Add(rotate);
            transformGroup.Children.Add(translate);

            RenderTransform = transformGroup;
        }

        #region DropState Dependency Property
        
        #region Binding Property
        public DropState DropState
        {
            get { return (DropState)GetValue(DropStateProperty); }
            set { SetValue(DropStateProperty, value); }
        }
        #endregion

        #region Dependency Property

        public static readonly DependencyProperty DropStateProperty =
            DependencyProperty.Register("DropState", typeof(DropState),
                typeof(DragAndDropPreviewBase), new UIPropertyMetadata(DropStateChanged));

        public static void DropStateChanged(DependencyObject element,DependencyPropertyChangedEventArgs e)
        {
            var instance = (DragAndDropPreviewBase)element;
            instance.StateChangedHandler(element, e);
        }

        public virtual void StateChangedHandler(DependencyObject d, DependencyPropertyChangedEventArgs e) { }
        #endregion
        #endregion
    }
}
