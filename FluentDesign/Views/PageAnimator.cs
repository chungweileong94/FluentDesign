using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Hosting;

namespace FluentDesign.Views
{
    public static class PageAnimator
    {
        public static void SetupAnimation(this Page page)
        {
            var compositor = Window.Current.Compositor;
            var visual = ElementCompositionPreview.GetElementChildVisual(page);

            var showAnimation = compositor.CreateVector3KeyFrameAnimation();
            showAnimation.Target = "Offset";
            showAnimation.Duration = TimeSpan.FromMilliseconds(1000);
            showAnimation.InsertKeyFrame(0, new System.Numerics.Vector3(300, 0, 0));
            showAnimation.InsertKeyFrame(1, new System.Numerics.Vector3(0, 0, 0));

            var hideAnimation = compositor.CreateVector3KeyFrameAnimation();
            hideAnimation.Target = "Offset";
            hideAnimation.Duration = TimeSpan.FromMilliseconds(1000);
            hideAnimation.InsertKeyFrame(0, new System.Numerics.Vector3(0, 0, 0));
            hideAnimation.InsertKeyFrame(1, new System.Numerics.Vector3(-300, 0, 0));

            Canvas.SetZIndex(page, 1);
            ElementCompositionPreview.SetImplicitShowAnimation(page, showAnimation);
            ElementCompositionPreview.SetImplicitHideAnimation(page, hideAnimation);
        }
    }
}
