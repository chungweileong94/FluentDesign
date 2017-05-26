using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Composition;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Hosting;
using Windows.UI.Xaml.Media;

namespace FluentDesign.Effects
{
    public class HoverLight : XamlLight
    {
        // Register an attached property that enables apps to set a UIElement or Brush as a target for this light type in markup.
        public static readonly DependencyProperty IsTargetProperty =
            DependencyProperty.RegisterAttached(
            "IsTarget",
            typeof(bool),
            typeof(HoverLight),
            new PropertyMetadata(null, OnIsTargetChanged)
        );
        public static void SetIsTarget(DependencyObject target, bool value)
        {
            target.SetValue(IsTargetProperty, value);
        }
        public static Boolean GetIsTarget(DependencyObject target)
        {
            return (bool)target.GetValue(IsTargetProperty);
        }

        // Handle attached property changed to automatically target and untarget UIElements and Brushes.
        private static void OnIsTargetChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            var isAdding = (bool)e.NewValue;

            if (isAdding)
            {
                if (obj is UIElement)
                {
                    XamlLight.AddTargetElement(GetIdStatic(), obj as UIElement);
                }
                else if (obj is Brush)
                {
                    XamlLight.AddTargetBrush(GetIdStatic(), obj as Brush);
                }
            }
            else
            {
                if (obj is UIElement)
                {
                    XamlLight.RemoveTargetElement(GetIdStatic(), obj as UIElement);
                }
                else if (obj is Brush)
                {
                    XamlLight.RemoveTargetBrush(GetIdStatic(), obj as Brush);
                }
            }
        }

        protected override void OnConnected(UIElement newElement)
        {
            if (CompositionLight == null)
            {
                // OnConnected is called when the first target UIElement is shown on the screen. This enables delaying composition object creation until it's actually necessary.
                var pointLight = Window.Current.Compositor.CreatePointLight();
                (pointLight as PointLight).Color = Colors.White;
                (pointLight as PointLight).CoordinateSpace = ElementCompositionPreview.GetElementChildVisual(newElement);
                (pointLight as PointLight).ConstantAttenuation = 1f;
                (pointLight as PointLight).Offset = new System.Numerics.Vector3(0, 0, 10);
                CompositionLight = pointLight;

                newElement.PointerMoved += (s, e) =>
                {
                    var pos = e.GetCurrentPoint(newElement).Position;
                    (CompositionLight as PointLight).Offset = new System.Numerics.Vector3((float)pos.X, (float)pos.Y, 45f);
                };
            }
        }

        protected override void OnDisconnected(UIElement oldElement)
        {
            // OnDisconnected is called when there are no more target UIElements on the screen. The CompositionLight should be disposed when no longer required.
            if (CompositionLight != null)
            {
                CompositionLight.Dispose();
                CompositionLight = null;
            }
        }

        protected override string GetId()
        {
            return GetIdStatic();
        }

        private static string GetIdStatic()
        {
            // This specifies the unique name of the light. In most cases you should use the type's FullName.
            return typeof(HoverLight).FullName;
        }
    }
}
