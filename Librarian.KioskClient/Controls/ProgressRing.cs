using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;

namespace Librarian.KioskClient.Controls
{
    [TemplateVisualState(Name = "Large", GroupName = "SizeStates")]
    [TemplateVisualState(Name = "Small", GroupName = "SizeStates")]
    [TemplateVisualState(Name = "Inactive", GroupName = "ActiveStates")]
    [TemplateVisualState(Name = "Active", GroupName = "ActiveStates")]
    public class ProgressRing : Control
    {
        public static readonly DependencyProperty BindableWidthProperty = DependencyProperty.Register("BindableWidth", typeof(double), typeof(ProgressRing), new PropertyMetadata(default(double), BindableWidthCallback));

        public static readonly DependencyProperty IsActiveProperty = DependencyProperty.Register("IsActive", typeof(bool), typeof(ProgressRing), new FrameworkPropertyMetadata(default(bool), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, IsActiveChanged));

        public static readonly DependencyProperty IsLargeProperty = DependencyProperty.Register("IsLarge", typeof(bool), typeof(ProgressRing), new PropertyMetadata(true, IsLargeChangedCallback));

        public static readonly DependencyProperty MaxSideLengthProperty = DependencyProperty.Register("MaxSideLength", typeof(double), typeof(ProgressRing), new PropertyMetadata(default(double)));

        public static readonly DependencyProperty EllipseDiameterProperty = DependencyProperty.Register("EllipseDiameter", typeof(double), typeof(ProgressRing), new PropertyMetadata(default(double)));

        public static readonly DependencyProperty EllipseOffsetProperty = DependencyProperty.Register("EllipseOffset", typeof(Thickness), typeof(ProgressRing), new PropertyMetadata(default(Thickness)));

        private List<Action> _deferredActions = new List<Action>();

        static ProgressRing()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ProgressRing), new FrameworkPropertyMetadata(typeof(ProgressRing)));
        }

        public ProgressRing()
        {
            this.SizeChanged += this.OnSizeChanged;
        }

        [Description("Maximum side length.")]
        public double MaxSideLength
        {
            get
            {
                return (double)this.GetValue(MaxSideLengthProperty);
            }
            private set
            {
                this.SetValue(MaxSideLengthProperty, value);
            }
        }

        [Description("Diameter of the ProgressRing ellipse.")]
        public double EllipseDiameter
        {
            get
            {
                return (double)this.GetValue(EllipseDiameterProperty);
            }
            private set
            {
                this.SetValue(EllipseDiameterProperty, value);
            }
        }

        [Description("Offset for the ProgressRing ellipse.")]
        public Thickness EllipseOffset
        {
            get
            {
                return (Thickness)this.GetValue(EllipseOffsetProperty);
            }
            private set
            {
                this.SetValue(EllipseOffsetProperty, value);
            }
        }

        [Description("Width of ProgressRing.")]
        public double BindableWidth
        {
            get
            {
                return (double)this.GetValue(BindableWidthProperty);
            }
            private set
            {
                this.SetValue(BindableWidthProperty, value);
            }
        }

        [Description("Is progress ring active (running).")]
        public bool IsActive
        {
            get
            {
                return (bool)this.GetValue(IsActiveProperty);
            }
            set
            {
                this.SetValue(IsActiveProperty, value);
            }
        }

        [Description("Is ProgressRing large.")]
        public bool IsLarge
        {
            get
            {
                return (bool)this.GetValue(IsLargeProperty);
            }
            set
            {
                this.SetValue(IsLargeProperty, value);
            }
        }

        private static void BindableWidthCallback(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
        {
            ProgressRing ring = dependencyObject as ProgressRing;
            if (ring == null)
                return;

            Action action = () =>
            {
                ring.SetEllipseDiameter(
                                        (double)dependencyPropertyChangedEventArgs.NewValue);
                ring.SetEllipseOffset(
                                      (double)dependencyPropertyChangedEventArgs.NewValue);
                ring.SetMaxSideLength(
                                      (double)dependencyPropertyChangedEventArgs.NewValue);
            };

            if (ring._deferredActions != null)
                ring._deferredActions.Add(action);
            else
                action();
        }

        private void SetMaxSideLength(double width)
        {
            this.MaxSideLength = width <= 60 ? 60.0 : width;
        }

        private void SetEllipseDiameter(double width)
        {
            if (width <= 60)
            {
                this.EllipseDiameter = 6.0;
            }
            else
            {
                this.EllipseDiameter = width * 0.1 + 6;
            }
        }


        private void SetEllipseOffset(double width)
        {
            if (width <= 60)
            {
                this.EllipseOffset = new Thickness(0, 24, 0, 0);
            }
            else
            {
                this.EllipseOffset = new Thickness(0, width * 0.4 + 24, 0, 0);
            }
        }

        private static void IsLargeChangedCallback(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
        {
            ProgressRing ring = dependencyObject as ProgressRing;
            if (ring == null)
                return;

            ring.UpdateLargeState();
        }

        private void UpdateLargeState()
        {
            Action action;

            if (this.IsLarge)
                action = () => VisualStateManager.GoToState(this, "Large", true);
            else
                action = () => VisualStateManager.GoToState(this, "Small", true);

            if (this._deferredActions != null)
                this._deferredActions.Add(action);

            else
                action();
        }

        private void OnSizeChanged(object sender, SizeChangedEventArgs sizeChangedEventArgs)
        {
            this.BindableWidth = this.ActualWidth;
        }

        private static void IsActiveChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
        {
            ProgressRing ring = dependencyObject as ProgressRing;
            if (ring == null)
                return;

            ring.UpdateActiveState();
        }

        private void UpdateActiveState()
        {
            Action action;

            if (this.IsActive)
                action = () => VisualStateManager.GoToState(this, "Active", true);
            else
                action = () => VisualStateManager.GoToState(this, "Inactive", true);

            if (this._deferredActions != null)
                this._deferredActions.Add(action);

            else
                action();
        }

        public override void OnApplyTemplate()
        {
            this.UpdateLargeState();
            this.UpdateActiveState();
            base.OnApplyTemplate();
            if (this._deferredActions != null)
                foreach (Action action in this._deferredActions)
                    action();
            this._deferredActions = null;
        }
    }
}
