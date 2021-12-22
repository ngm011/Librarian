using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Media;

namespace Librarian.KioskClient.Controls
{
    public class CircledButton : Button
    {
        static CircledButton()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(CircledButton), new FrameworkPropertyMetadata(typeof(CircledButton)));
        }

        #region Dependency properties

        #region Geometry property

        [Description("Icon in the circle button")]
        public static DependencyProperty IconProperty = DependencyProperty.Register("Icon", typeof(Geometry), typeof(CircledButton), new PropertyMetadata(default(Geometry)));

        public Geometry Icon
        {
            get
            {
                return (Geometry)GetValue(IconProperty);
            }
            set
            {
                SetValue(IconProperty, value);
            }
        }

        #endregion

        #region Icon margin

        [Description("Margins for the icon in the circle button")]
        public static DependencyProperty IconMarginProperty = DependencyProperty.Register("IconMargin", typeof(Thickness), typeof(CircledButton), new PropertyMetadata(default(Thickness)));

        /// <summary> Gets or sets the margins of the icon in the circle button. </summary>
        public Thickness IconMargin
        {
            get
            {
                return (Thickness)GetValue(IconMarginProperty);
            }
            set
            {
                SetValue(IconMarginProperty, value);
            }
        }

        #endregion

        #region ForegroundBrush property

        public static DependencyProperty ForegroundBrushProperty = DependencyProperty.Register("ForegroundBrush", typeof(Brush), typeof(CircledButton), new PropertyMetadata(Brushes.White));

        [Description("Foreground brush which is used for the circle color, and icon color.")]
        public Brush ForegroundBrush
        {
            get
            {
                return (Brush)GetValue(ForegroundBrushProperty);
            }
            set
            {
                SetValue(ForegroundBrushProperty, value);
            }
        }

        #endregion

        #region BackgroundBrush property

        public static DependencyProperty BackgroundBrushProperty = DependencyProperty.Register("BackgroundBrush", typeof(Brush), typeof(CircledButton), new PropertyMetadata(Brushes.White));

        [Description("Background brush of the button.")]
        public Brush BackgroundBrush
        {
            get
            {
                return (Brush)GetValue(BackgroundBrushProperty);
            }
            set
            {
                SetValue(BackgroundBrushProperty, value);
            }
        }

        #endregion

        #region Text property

        public static DependencyProperty TextProperty = DependencyProperty.Register("Text", typeof(string), typeof(CircledButton), new PropertyMetadata(default(string)));

        [Description("Caption for the button")]
        public string Text
        {
            get
            {
                string returnValue = (string)GetValue(TextProperty);

                return String.IsNullOrEmpty(returnValue) ? "Not defined" : returnValue;
            }
            set
            {
                SetValue(TextProperty, value);
            }
        }

        #endregion

        #endregion
    }
}
