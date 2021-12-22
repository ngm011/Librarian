using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace Librarian.KioskClient.Controls
{
    [TemplateVisualState(GroupName = "Transitions", Name = "Entrance"),
    TemplateVisualState(GroupName = "Transitions", Name = "Exit")]
    public class DynamicContentControl : ContentControl
    {
        #region Private State

        private readonly DispatcherTimer _timer = new DispatcherTimer();

        #endregion

        #region Private Static

        private const int Duration = 30;

        private static readonly Dictionary<string, List<WeakReference>> Asymmetrics =
            new Dictionary<string, List<WeakReference>>();

        #endregion

        #region Constructors

        static DynamicContentControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(
                typeof(DynamicContentControl),
                new FrameworkPropertyMetadata(typeof(DynamicContentControl)));
        }

        public DynamicContentControl()
        {
            this.Loaded += this.OnLoaded;
            this.Unloaded += this.OnUnloaded;
            this.IsVisibleChanged += this.OnIsVisibleChanged;
        }

        #endregion

        #region OnApplyTemplate

        public override void OnApplyTemplate()
        {
            VisualStateManager.GoToState(this, "Exit", false);
        }

        #endregion

        #region OnLoaded, OnUnloaded, OnVisibilityChanged

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            this.Enter();
        }

        private void OnUnloaded(object sender, RoutedEventArgs e)
        {
            this.Exit();

            if (this.IsAsymmetric) this.AsymmetricGroup = String.Empty;
        }

        private void OnIsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (!this.IsLoaded) return;

            if (this.IsVisible)
                this.Enter();
            else
                this.Exit();
        }

        #endregion

        #region AsymmetricGroup, IsAsymmetric

        public static readonly DependencyProperty AsymmetricGroupProperty =
                DependencyProperty.Register("AsymmetricGroup", typeof(string), typeof(DynamicContentControl), new PropertyMetadata(String.Empty, OnAsymmetricGroupChanged));

        [Description("Asymmetric group for animation.")]
        public string AsymmetricGroup
        {
            get { return (string)this.GetValue(AsymmetricGroupProperty); }
            set { this.SetValue(AsymmetricGroupProperty, value); }
        }

        [Description("Is asymmetric group defined.")]
        public bool IsAsymmetric
        {
            get { return !String.IsNullOrEmpty(this.AsymmetricGroup); }
        }

        private static void OnAsymmetricGroupChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            string oldValue = (string)e.OldValue;
            string newValue = (string)e.NewValue;

            if (!String.IsNullOrEmpty(oldValue))
            {
                string group = oldValue.ToLower();
                List<WeakReference> groupMembers = Asymmetrics[group];

                groupMembers.RemoveAll(m => ReferenceEquals(m.Target, d) || m.Target == null);

                if (groupMembers.Count == 0) Asymmetrics.Remove(group);
            }

            if (!String.IsNullOrEmpty(newValue))
            {
                string group = newValue.ToLower();
                List<WeakReference> groupMembers;

                if (Asymmetrics.ContainsKey(group))
                {
                    groupMembers = Asymmetrics[group];
                }
                else
                {
                    groupMembers = new List<WeakReference>();

                    Asymmetrics.Add(group, groupMembers);
                }


                groupMembers.Add(new WeakReference(d, false));
            }
        }

        #endregion

        #region Interval

        private int Interval
        {
            get
            {
                if (!this.IsAsymmetric) return 0;

                string group = this.AsymmetricGroup.ToLower();
                List<WeakReference> groupMembers = Asymmetrics[group];

                var member = groupMembers
                    .Select((m, i) => new { m.Target, Index = i })
                    .FirstOrDefault(m => ReferenceEquals(m.Target, this));

                return member != null ? member.Index * Duration : 0;
            }
        }

        #endregion

        #region Enter, Exit

        private void Enter()
        {
            Action goToStateAction = () => VisualStateManager.GoToState(this, "Entrance", true);

            if (this.IsAsymmetric)
                GoToDelayedState(goToStateAction);
            else
                goToStateAction();
        }

        private void Exit()
        {
            Action goToStateAction = () => VisualStateManager.GoToState(this, "Exit", true);

            if (this.IsAsymmetric)
                GoToDelayedState(goToStateAction);
            else
                goToStateAction();
        }

        #endregion

        #region Helper Method

        private EventHandler _onTicked;

        private void GoToDelayedState(Action goToStateAction)
        {
            _timer.Stop();

            _timer.Tick -= _onTicked;
            _timer.Interval = TimeSpan.FromMilliseconds(this.Interval);
            _onTicked = (s, args) =>
            {
                goToStateAction();

                _timer.Stop();
            };
            _timer.Tick += _onTicked;

            _timer.Start();
        }

        #endregion
    }
}
