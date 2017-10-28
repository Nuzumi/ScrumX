using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Controls;
using System.Windows.Media.Animation;

namespace DragAndDrop
{
    class DragAndDrop
    {

        private Window _topWindow;
        private Point _initialMousePosition;
        private Panel _dragDropContainer;
        private UIElement _dropTarget;
        private bool _mouseCaptured;
        private DragAndDropPreviewBase _dragAndDropPreviewControl;
        private object _dragAndDropPreviewControlDataContext;
        private ICommand _itemDropCommand;
        private Point _delta;

        #region Instance
        private static readonly Lazy<DragAndDrop> _Instance = new Lazy<DragAndDrop>(() => new DragAndDrop());

        private static DragAndDrop Instance
        {
            get { return _Instance.Value; }
        }
        #endregion 

        #region Utilities
        public static FrameworkElement FindAncestor(Type ancestorType, Visual visual)
        {
            while(visual != null && !ancestorType.IsInstanceOfType(visual))
            {
                visual = (Visual)VisualTreeHelper.GetParent(visual);
            }
            return visual as FrameworkElement;
        } 

        public static bool IsMovementBigEnough(Point initialMousePosition, Point currentMousePosition)
        {
            return (Math.Abs(currentMousePosition.X - initialMousePosition.X) >= SystemParameters.MinimumHorizontalDragDistance ||
            Math.Abs(currentMousePosition.Y - initialMousePosition.Y) >= SystemParameters.MinimumVerticalDragDistance);
        }
        #endregion

        #region IsDragSource attached property

        #region Backing Dependency Property

        public static readonly DependencyProperty IsDragSourceProperty = DependencyProperty.RegisterAttached("IsDragSource",
            typeof(bool), typeof(DragAndDrop), new PropertyMetadata(false, IsDragSourceChanged));

        #endregion
        #region Get and set

        public static bool GetIsDragSource(DependencyObject element)
        {
            return (bool)element.GetValue(IsDragSourceProperty);
        }

        public static void SetIsDragSource(DependencyObject element, bool value)
        {
            element.SetValue(IsDragSourceProperty, value);
        }

        private static void IsDragSourceChanged(DependencyObject element, DependencyPropertyChangedEventArgs e)
        {
            var dragSource = element as UIElement;
            if (dragSource == null)
                return;

            if(object.Equals(e.NewValue, true))
            {
                dragSource.PreviewMouseLeftButtonDown += Instance.DragSource_PreviewMouseLeftButtonDown;
                dragSource.PreviewMouseLeftButtonUp += Instance.DragSource_PreviewMouseLeftButtonUp;
                dragSource.PreviewMouseMove += Instance.DragSource_PreviewMouseMove;
            }
            else
            {
                dragSource.PreviewMouseLeftButtonDown -= Instance.DragSource_PreviewMouseLeftButtonDown;
                dragSource.PreviewMouseLeftButtonUp -= Instance.DragSource_PreviewMouseLeftButtonUp;
                dragSource.PreviewMouseMove -= Instance.DragSource_PreviewMouseMove;
            }
        }

        #endregion
        #endregion

        #region Drag hendlers

        private void DragSource_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                var visual = e.OriginalSource as Visual;
                _topWindow = (Window)FindAncestor(typeof(Window), visual);
                _initialMousePosition = e.GetPosition(_topWindow);
                _dragDropContainer = GetDragDropContainer(sender as DependencyObject) as Canvas;

                if(_dragDropContainer == null)
                {
                    _dragDropContainer = (Canvas)FindAncestor(typeof(Canvas), visual);
                }

                _dropTarget = GetDropTarget(sender as DependencyObject);
                _dragAndDropPreviewControlDataContext = GetDragDropPreviewControlDataContext(sender as DependencyObject);

                if(_dragAndDropPreviewControlDataContext == null)
                {
                    _dragAndDropPreviewControlDataContext = (sender as FrameworkElement).DataContext;
                }

                _itemDropCommand = GetItemDropped(sender as DependencyObject);
            }
            catch (Exception exc)
            {
                Console.WriteLine("ExceptionInDragAndDropHelper: " + exc.InnerException.ToString());
            }
        }

        private void DragSource_PreviewMouseMove(object sender, MouseEventArgs e)
        {
            if( _mouseCaptured || _dragAndDropPreviewControlDataContext == null)
            {
                return;
            }

            if(IsMovementBigEnough(_initialMousePosition,e.GetPosition(_topWindow)) == false)
            {
                return;
            }

            _dragAndDropPreviewControl = GetDragDropPreviewControl(sender as DependencyObject);
            _dragAndDropPreviewControl.DataContext = _dragAndDropPreviewControlDataContext;
            _dragAndDropPreviewControl.Opacity = 0.7f;

            _dragDropContainer.Children.Add(_dragAndDropPreviewControl);
            _mouseCaptured = Mouse.Capture(_dragAndDropPreviewControl);

            Mouse.OverrideCursor = Cursors.Hand;

            Canvas.SetLeft(_dragAndDropPreviewControl, _initialMousePosition.X - 20);
            Canvas.SetTop(_dragAndDropPreviewControl, _initialMousePosition.Y - 15);

            _dragDropContainer.PreviewMouseMove += DragDropContainer_PreviewMouseMove;
            _dragDropContainer.PreviewMouseUp += DragDropContainer_PreviewMouseUp;
        }

        private void DragDropContainer_PreviewMouseMove(object sender, MouseEventArgs e)
        {
            var currentPoint = e.GetPosition(_topWindow);

            Mouse.OverrideCursor = Cursors.Hand;
            currentPoint.X -= 20;
            currentPoint.Y -= 15;

            _delta = new Point(_initialMousePosition.X - currentPoint.X, _initialMousePosition.Y - currentPoint.Y);
            var Target = new Point(_initialMousePosition.X - _delta.X, _initialMousePosition.Y - _delta.Y);

            Canvas.SetLeft(_dragAndDropPreviewControl, Target.X);
            Canvas.SetTop(_dragAndDropPreviewControl, Target.Y);

            _dragAndDropPreviewControl.DropState = DropState.CantDrop;

            if (_dropTarget == null)
            {
                AnimateDropState();
                return;
            }

            var transform = _dropTarget.TransformToVisual(_dragDropContainer);
            var dropBoundingBox = transform.TransformBounds(new Rect(0, 0, _dropTarget.RenderSize.Width, _dropTarget.RenderSize.Height));

            if (e.GetPosition(_dragDropContainer).X > dropBoundingBox.Left &&
                e.GetPosition(_dragDropContainer).X < dropBoundingBox.Right &&
                e.GetPosition(_dragDropContainer).Y > dropBoundingBox.Top &&
                e.GetPosition(_dragDropContainer).Y < dropBoundingBox.Bottom)
            {
                _dragAndDropPreviewControl.DropState = DropState.CanDrop;
            }

            if(_itemDropCommand != null && _itemDropCommand.CanExecute(_dragAndDropPreviewControlDataContext) == false)
            {
                _dragAndDropPreviewControl.DropState = DropState.CantDrop;
            }
        }

        private void AnimateDropState()
        {
            //determine if we need to animate states
            switch (_dragAndDropPreviewControl.DropState)
            {
                case DropState.CanDrop:

                    if (_dragAndDropPreviewControl.Resources.Contains("canDropChanged"))
                    {
                        ((Storyboard)_dragAndDropPreviewControl.Resources["canDropChanged"]).Begin(_dragAndDropPreviewControl);
                    }

                    break;
                case DropState.CantDrop:
                    if (_dragAndDropPreviewControl.Resources.Contains("cannotDropChanged"))
                    {
                        ((Storyboard)_dragAndDropPreviewControl.Resources["cannotDropChanged"]).Begin(_dragAndDropPreviewControl);
                    }
                    break;
                default:
                    break;
            }
        }

        private static DoubleAnimation CreateDoubleAnimation(Double to)
        {
            var anim = new DoubleAnimation();
            anim.To = to;
            anim.Duration = TimeSpan.FromMilliseconds(250);
            anim.AccelerationRatio = 0.1;
            anim.DecelerationRatio = 0.9;

            return anim;
        }

        private void DragDropContainer_PreviewMouseUp(object sender, MouseEventArgs e)
        {
            switch (_dragAndDropPreviewControl.DropState)
            {
                case DropState.CanDrop:
                    try
                    {
                        var scaleXAnim = CreateDoubleAnimation(0);
                        Storyboard.SetTargetProperty(scaleXAnim, new PropertyPath("(UIElement.RenderTransform).(TransformGroup.Children)[0].(ScaleTransform.ScaleX)"));

                        var scaleYAnim = CreateDoubleAnimation(0);
                        Storyboard.SetTargetProperty(scaleYAnim, new PropertyPath("(UIElement.RenderTransform).(TransformGroup.Children)[0].(ScaleTransform.ScaleY)"));

                        var opacityAnim = CreateDoubleAnimation(0);
                        Storyboard.SetTargetProperty(opacityAnim, new PropertyPath("(UIElement.Opacity)"));

                        var canDropSb = new Storyboard() { FillBehavior = FillBehavior.Stop };
                        canDropSb.Children.Add(scaleXAnim);
                        canDropSb.Children.Add(scaleYAnim);
                        canDropSb.Children.Add(opacityAnim);
                        canDropSb.Completed += (s, args) => { FinalizePreviewControlMouseUp(); };

                        canDropSb.Begin(_dragAndDropPreviewControl);

                        if (_itemDropCommand != null)
                        { _itemDropCommand.Execute(_dragAndDropPreviewControlDataContext); }
                    }
                    catch(Exception ex)
                    {

                    }
                    break;
                case DropState.CantDrop:
                    try
                    {
                        var translateXAnim = CreateDoubleAnimation(_delta.X);
                        Storyboard.SetTargetProperty(translateXAnim, new PropertyPath("(UIElement.RenderTransform).(TransformGroup.Children)[3].(TranslateTransform.X)"));

                        var translateYAnim = CreateDoubleAnimation(_delta.Y);
                        Storyboard.SetTargetProperty(translateYAnim, new PropertyPath("(UIElement.RenderTransform).(TransformGroup.Children)[3].(TranslateTransform.Y)"));

                        var opacityAnim = CreateDoubleAnimation(0);
                        opacityAnim.BeginTime = TimeSpan.FromMilliseconds(150);
                        Storyboard.SetTargetProperty(opacityAnim, new PropertyPath("(UIElement.Opacity)"));

                        var cannotDropSb = new Storyboard() { FillBehavior = FillBehavior.Stop };
                        cannotDropSb.Children.Add(translateXAnim);
                        cannotDropSb.Children.Add(translateYAnim);
                        cannotDropSb.Children.Add(opacityAnim);
                        cannotDropSb.Completed += (s, args) => { FinalizePreviewControlMouseUp(); };

                        cannotDropSb.Begin(_dragAndDropPreviewControl);
                    }
                    catch (Exception ex) { }
                    break;
            }
        }

        private void FinalizePreviewControlMouseUp()
        {
            _dragDropContainer.Children.Remove(_dragAndDropPreviewControl);
            _dragDropContainer.PreviewMouseMove -= DragDropContainer_PreviewMouseMove;
            _dragDropContainer.PreviewMouseUp -= DragDropContainer_PreviewMouseUp;

            if (_dragAndDropPreviewControl != null)
            {
                _dragAndDropPreviewControl.ReleaseMouseCapture();
            }
            _dragAndDropPreviewControl = null;
            Mouse.OverrideCursor = null;
        }

        private void DragSource_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            _dragAndDropPreviewControlDataContext = null;
            _mouseCaptured = false;

            if(_dragAndDropPreviewControl != null)
            {
                _dragAndDropPreviewControl.ReleaseMouseCapture();
            }
        }

        #endregion

        #region DragDropPreviewControl Attached Property

        #region Backing Dependency Property

        /// <summary>
        /// The backing <see cref="DependencyProperty"/> which enables animation, styling, binding, etc. for the control that is displayed (and moves with the mouse) during a drag drop operation
        /// </summary>
        public static readonly DependencyProperty DragDropPreviewControlProperty = DependencyProperty.RegisterAttached(
            "DragDropPreviewControl", typeof(DragAndDropPreviewBase), typeof(DragDrop), new PropertyMetadata(default(UIElement)));

        #endregion

        #region Get and set

        /// <summary>
        /// Gets the attached value of <see cref="DragDropPreviewControlProperty"/>
        /// </summary>
        /// <param name="element">The <see cref="DependencyObject"/> that the attached <see cref="DependencyProperty"/>, <see cref="DragDropPreviewControlProperty"/>, is attched to.</param>
        /// <returns>The attached value</returns>
        public static DragAndDropPreviewBase GetDragDropPreviewControl(DependencyObject element)
        {
            return (DragAndDropPreviewBase)element.GetValue(DragDropPreviewControlProperty);
        }

        /// <summary>
        /// Sets the attached value of <see cref="DragDropPreviewControlProperty"/>
        /// </summary>
        /// <param name="element">The <see cref="DependencyObject"/> that the attached <see cref="DependencyProperty"/>, <see cref="DragDropPreviewControlProperty"/>, is attched to.</param>
        /// <param name="value">the value to set</param>
        public static void SetDragDropPreviewControl(DependencyObject element, DragAndDropPreviewBase value)
        {
            element.SetValue(DragDropPreviewControlProperty, value);
        }

        #endregion

        #endregion

        #region DragDropContainer Attached Property

        #region Backing Dependency Property

        /// <summary>
        /// The backing <see cref="DependencyProperty"/> which enables animation, styling, binding, etc. for the outmost container of the <see cref="DragDropPreviewControlProperty"/>
        /// </summary>
        public static readonly DependencyProperty DragDropContainerProperty = DependencyProperty.RegisterAttached(
            "DragDropContainer", typeof(Panel), typeof(DragDrop), new PropertyMetadata(default(UIElement)));

        #endregion

        #region Get and set

        /// <summary>
        /// Gets the attached value of <see cref="DragDropContainerProperty"/>
        /// </summary>
        /// <param name="element">The <see cref="DependencyObject"/> that the attached <see cref="DependencyProperty"/>, <see cref="DragDropContainerProperty"/>, is attched to.</param>
        /// <returns>The attached value</returns>
        public static Panel GetDragDropContainer(DependencyObject element)
        {
            return (Panel)element.GetValue(DragDropContainerProperty);
        }

        /// <summary>
        /// Sets the attached value of <see cref="DragDropContainerProperty"/>
        /// </summary>
        /// <param name="element">The <see cref="DependencyObject"/> that the attached <see cref="DependencyProperty"/>, <see cref="DragDropContainerProperty"/>, is attched to.</param>
        /// <param name="value">the value to set</param>
        public static void SetDragDropContainer(DependencyObject element, Panel value)
        {
            element.SetValue(DragDropContainerProperty, value);
        }

        #endregion

        #endregion

        #region DropTarget Attached Property

        #region Backing Dependency Property

        /// <summary>
        /// The backing <see cref="DependencyProperty"/> which enables animation, styling, binding, etc. for <see cref="DropTarget"/>
        /// </summary>
        public static readonly DependencyProperty DropTargetProperty = DependencyProperty.RegisterAttached(
            "DropTarget", typeof(UIElement), typeof(DragDrop), new PropertyMetadata(default(String)));


        #endregion

        #region Get and set

        /// <summary>
        /// Gets the attached value of DropTarget
        /// </summary>
        /// <param name="element">The <see cref="DependencyObject"/> that the attached <see cref="DependencyProperty"/>, <see cref="DropTargetProperty"/>, is attched to.</param>
        /// <returns>The attached value</returns>
        public static UIElement GetDropTarget(DependencyObject element)
        {
            return (UIElement)element.GetValue(DropTargetProperty);
        }

        /// <summary>
        /// Sets the attached value of DropTarget
        /// </summary>
        /// <param name="element">The <see cref="DependencyObject"/> that the attached <see cref="DependencyProperty"/>, <see cref="DropTargetProperty"/>, is attched to.</param>
        /// <param name="value">the value to set</param>
        public static void SetDropTarget(DependencyObject element, UIElement value)
        {
            element.SetValue(DropTargetProperty, value);
        }

        #endregion

        #endregion

        #region DragDropPreviewControlDataContext Attached Property

        #region Backing Dependency Property

        /// <summary>
        /// The backing <see cref="DependencyProperty"/> which enables animation, styling, binding, etc. for DragDropPreviewControlDataContext
        /// </summary>
        public static readonly DependencyProperty DragDropPreviewControlDataContextProperty = DependencyProperty.RegisterAttached(
            "DragDropPreviewControlDataContext", typeof(Object), typeof(DragDrop), new PropertyMetadata(default(Object)));

        #endregion

        #region Get and set

        /// <summary>
        /// Gets the attached value of DragDropPreviewControlDataContext
        /// </summary>
        /// <param name="element">The <see cref="DependencyObject"/> that the attached <see cref="DependencyProperty"/>, <see cref="DragDropPreviewControlDataContextProperty"/>, is attched to.</param>
        /// <returns>The attached value</returns>
        public static Object GetDragDropPreviewControlDataContext(DependencyObject element)
        {
            return (Object)element.GetValue(DragDropPreviewControlDataContextProperty);
        }

        /// <summary>
        /// Sets the attached value of DragDropPreviewControlDataContext
        /// </summary>
        /// <param name="element">The <see cref="DependencyObject"/> that the attached <see cref="DependencyProperty"/>, <see cref="DragDropPreviewControlDataContextProperty"/>, is attched to.</param>
        /// <param name="value">the value to set</param>
        public static void SetDragDropPreviewControlDataContext(DependencyObject element, Object value)
        {
            element.SetValue(DragDropPreviewControlDataContextProperty, value);
        }

        #endregion

        #endregion

        #region ItemDropped Attached Property

        #region Backing Dependency Property

        /// <summary>
        /// The backing <see cref="DependencyProperty"/> which enables animation, styling, binding, etc. for ItemDropped
        /// </summary>
        public static readonly DependencyProperty ItemDroppedProperty = DependencyProperty.RegisterAttached(
            "ItemDropped", typeof(ICommand), typeof(DragDrop), new PropertyMetadata(new PropertyChangedCallback(AttachOrRemoveItemDroppedEvent)));



        #endregion

        #region Get and set

        /// <summary>
        /// Gets the attached value of ItemDropped
        /// </summary>
        /// <param name="element">The <see cref="DependencyObject"/> that the attached <see cref="DependencyProperty"/>, <see cref="ItemDroppedProperty"/>, is attched to.</param>
        /// <returns>The attached value</returns>
        public static ICommand GetItemDropped(DependencyObject element)
        {
            return (ICommand)element.GetValue(ItemDroppedProperty);
        }

        /// <summary>
        /// Sets the attached value of ItemDropped
        /// </summary>
        /// <param name="element">The <see cref="DependencyObject"/> that the attached <see cref="DependencyProperty"/>, <see cref="ItemDroppedProperty"/>, is attched to.</param>
        /// <param name="value">the value to set</param>
        public static void SetItemDropped(DependencyObject element, ICommand value)
        {
            element.SetValue(ItemDroppedProperty, value);
        }

        private static void AttachOrRemoveItemDroppedEvent(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
        {
        }

        #endregion

        #endregion


    }
}
