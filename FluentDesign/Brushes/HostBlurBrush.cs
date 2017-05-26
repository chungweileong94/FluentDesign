using Windows.UI.Composition;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;
using Microsoft.Graphics.Canvas.Effects;
using Windows.UI;

namespace FluentDesign.Brushes
{
    public class HostBlurBrush : XamlCompositionBrushBase
    {
        protected override void OnConnected()
        {
            Compositor compositor = Window.Current.Compositor;

            GaussianBlurEffect blur = new GaussianBlurEffect()
            {
                BlurAmount = 60f,
                BorderMode = EffectBorderMode.Hard,
                Optimization = EffectOptimization.Balanced,
                Source = new ArithmeticCompositeEffect()
                {
                    Source1 = new CompositionEffectSourceParameter("backdrop"),
                    Source2 = new ColorSourceEffect()
                    {
                        Color = Color.FromArgb(255, 45, 45, 45)
                    },

                    Source1Amount = .2f,
                    Source2Amount = .8f
                }
            };

            var effectFactory = compositor.CreateEffectFactory(blur);

            var brush = effectFactory.CreateBrush();
            brush.SetSourceParameter("backdrop", compositor.CreateHostBackdropBrush());

            CompositionBrush = brush;
        }

        protected override void OnDisconnected()
        {
            CompositionBrush.Dispose();
            CompositionBrush = null;
        }
    }
}
