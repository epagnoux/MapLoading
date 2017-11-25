using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace MapLoading.Controls.Manipulation3D
{
  public enum PerspectiveRotationAxeOrigin
  {
    X,
    XM,
    X1,
    Y,
    YM,
    Y1
  }

  [ContentProperty("Child")]
  public class Perspective : FrameworkElement
  {
    #region Public API

    #region Dependency Properties

    public static readonly DependencyProperty RotationXProperty =
      DependencyProperty.Register("RotationX", typeof (double), typeof (Perspective),
        new UIPropertyMetadata(0.0, (d, args) => ((Perspective) d).UpdateRotation()));

    public static readonly DependencyProperty RotationYProperty =
      DependencyProperty.Register("RotationY", typeof (double), typeof (Perspective),
        new UIPropertyMetadata(0.0, (d, args) => ((Perspective) d).UpdateRotation()));

    public static readonly DependencyProperty RotationZProperty =
      DependencyProperty.Register("RotationZ", typeof (double), typeof (Perspective),
        new UIPropertyMetadata(0.0, (d, args) => ((Perspective) d).UpdateRotation()));

    public static readonly DependencyProperty FieldOfViewProperty =
      DependencyProperty.Register("FieldOfView", typeof (double), typeof (Perspective),
        new UIPropertyMetadata(45.0, (d, args) => ((Perspective) d).Update3D(),
          (d, val) => Math.Min(Math.Max((double) val, 0.5), 179.9))); // clamp to a meaningful range

    public static readonly DependencyProperty RotationAxeOriginProperty =
      DependencyProperty.Register("RotationAxeOrigin", typeof(PerspectiveRotationAxeOrigin),
        typeof (Perspective),
        new PropertyMetadata(default(PerspectiveRotationAxeOrigin)));

    public PerspectiveRotationAxeOrigin RotationAxeOrigin
    {
      get { return (PerspectiveRotationAxeOrigin) GetValue(RotationAxeOriginProperty); }
      set { SetValue(RotationAxeOriginProperty, value); }
    }

    public double RotationX
    {
      get { return (double) GetValue(RotationXProperty); }
      set { SetValue(RotationXProperty, value); }
    }

    public double RotationY
    {
      get { return (double) GetValue(RotationYProperty); }
      set { SetValue(RotationYProperty, value); }
    }

    public double RotationZ
    {
      get { return (double) GetValue(RotationZProperty); }
      set { SetValue(RotationZProperty, value); }
    }

    public double FieldOfView
    {
      get { return (double) GetValue(FieldOfViewProperty); }
      set { SetValue(FieldOfViewProperty, value); }
    }

    #endregion

    public FrameworkElement Child
    {
      get { return mOriginalChild; }
      set
      {
        if (mOriginalChild != value)
        {
          RemoveVisualChild(mVisualChild);
          RemoveLogicalChild(mLogicalChild);

          // Wrap child with special decorator that catches layout invalidations. 
          mOriginalChild = value;
          mLogicalChild = new LayoutInvalidationCatcher {Child = mOriginalChild};
          mVisualChild = CreateVisualChild();

          AddVisualChild(mVisualChild);

          // Need to use a logical child here to make sure databinding operations get down to it,
          // since otherwise the child appears only as the Visual to a Viewport2DVisual3D, which 
          // doesn't have databinding operations pass into it from above.
          AddLogicalChild(mLogicalChild);
          InvalidateMeasure();
        }
      }
    }

    #endregion

    #region Layout Stuff

    protected override int VisualChildrenCount
    {
      get { return mVisualChild == null ? 0 : 1; }
    }

    protected override Size MeasureOverride(Size availableSize)
    {
      Size wResult;
      if (mLogicalChild != null)
      {
        // Measure based on the size of the logical child, since we want to align with it.
        mLogicalChild.Measure(availableSize);
        wResult = mLogicalChild.DesiredSize;
        mVisualChild.Measure(wResult);
      }
      else
      {
        wResult = new Size(0, 0);
      }
      return wResult;
    }

    protected override Size ArrangeOverride(Size finalSize)
    {
      if (mLogicalChild != null)
      {
        mLogicalChild.Arrange(new Rect(finalSize));
        mVisualChild.Arrange(new Rect(finalSize));
        Update3D();
      }
      return base.ArrangeOverride(finalSize);
    }

    protected override Visual GetVisualChild(int index)
    {
      return mVisualChild;
    }

    #endregion

    #region 3D Stuff

    private FrameworkElement CreateVisualChild()
    {
      var wSimpleQuad = new MeshGeometry3D
      {
        Positions = new Point3DCollection(Mesh),
        TextureCoordinates = new PointCollection(TexCoords),
        TriangleIndices = new Int32Collection(Indices)
      };

      // Front material is interactive, back material is not.
      Material wFrontMaterial = new DiffuseMaterial(Brushes.White);
      wFrontMaterial.SetValue(Viewport2DVisual3D.IsVisualHostMaterialProperty, true);

      var wVisualBrush = new VisualBrush(mLogicalChild);
      SetCachingForObject(wVisualBrush); // big perf wins by caching!!
      Material wBackMaterial = new DiffuseMaterial(wVisualBrush);

      mRotationTransform.Rotation = mQuaternionRotation;
      var wTransform3DGroup = new Transform3DGroup {Children = {mScaleTransform, mRotationTransform}};

      var wGeometryModel3D = new GeometryModel3D {Geometry = wSimpleQuad, Transform = wTransform3DGroup, BackMaterial = wBackMaterial};
      var wModel3DGroup = new Model3DGroup
      {
        Children =
        {
          new DirectionalLight(Colors.White, new Vector3D(0, 0, -1)),
          new DirectionalLight(Colors.White, new Vector3D(0.1, -0.1, 1)),
          wGeometryModel3D
        }
      };

      // Non-interactive Visual3D consisting of the backside, and two lights.
      var wModelVisual3D = new ModelVisual3D {Content = wModel3DGroup};

      // Interactive frontside Visual3D
      var wViewport2DVisual3D = new Viewport2DVisual3D
      {
        Geometry = wSimpleQuad,
        Visual = mLogicalChild,
        Material = wFrontMaterial,
        Transform = wTransform3DGroup
      };

      // Cache the brush in the VP2V3 by setting caching on it.  Big perf wins.
      SetCachingForObject(wViewport2DVisual3D);

      // Scene consists of both the above Visual3D's.
      mViewport3D = new Viewport3D {ClipToBounds = false, Children = {wModelVisual3D, wViewport2DVisual3D}};

      UpdateRotation();

      return mViewport3D;
    }

    private void SetCachingForObject(DependencyObject d)
    {
      RenderOptions.SetCachingHint(d, CachingHint.Cache);
      RenderOptions.SetCacheInvalidationThresholdMinimum(d, 0.5);
      RenderOptions.SetCacheInvalidationThresholdMaximum(d, 2.0);
    }

    private void UpdateRotation()
    {
      var wQx = new Quaternion(XAxis, RotationX);
      var wQy = new Quaternion(YAxis, RotationY);
      var wQz = new Quaternion(ZAxis, RotationZ);

      mQuaternionRotation.Quaternion = wQx*wQy*wQz;
    }

    private void Update3D()
    {
      // Use GetDescendantBounds for sizing and centering since DesiredSize includes layout whitespace, whereas GetDescendantBounds 
      // is tighter
      var wLogicalBounds = VisualTreeHelper.GetDescendantBounds(mLogicalChild);
      var wWidth = wLogicalBounds.Width;
      var wHeight = wLogicalBounds.Height;

      // Create a camera that looks down -Z, with up as Y, and positioned right halfway in X and Y on the element, 
      // and back along Z the right distance based on the field-of-view is the same projected size as the 2D content
      // that it's looking at.  See http://blogs.msdn.com/greg_schechter/archive/2007/04/03/camera-construction-in-parallaxui.aspx
      // for derivation of this camera.
      var wFovInRadians = FieldOfView*(Math.PI/180);
      var wZValue = wWidth/Math.Tan(wFovInRadians/2)/2;
      mViewport3D.Camera = new PerspectiveCamera(new Point3D(wWidth/2, wHeight/2, wZValue),
        - ZAxis,
        YAxis,
        FieldOfView);


      mScaleTransform.ScaleX = wWidth;
      mScaleTransform.ScaleY = wHeight;
      var wRotationAxeOriginX = 0.0;
      var wRotationAxeOriginY = 0.0;
      switch (RotationAxeOrigin)
      {
        case PerspectiveRotationAxeOrigin.X:
          wRotationAxeOriginX = 0.0;
          break;
        case PerspectiveRotationAxeOrigin.XM:
          wRotationAxeOriginX = wHeight/2;
          break;
        case PerspectiveRotationAxeOrigin.X1:
          wRotationAxeOriginX = wHeight;
          break;
        case PerspectiveRotationAxeOrigin.Y:
          wRotationAxeOriginY = 0;
          break;
        case PerspectiveRotationAxeOrigin.YM:
          wRotationAxeOriginY = wWidth/2;
          break;
        case PerspectiveRotationAxeOrigin.Y1:
          wRotationAxeOriginY = wWidth;
          break;
      }
      mRotationTransform.CenterX = wRotationAxeOriginX;
      mRotationTransform.CenterY = wRotationAxeOriginY;
      //mRotationTransform.CenterX = w / 2;
      //mRotationTransform.CenterY = h / 2;
    }

    #endregion

    #region Private Classes

    /// <summary>
    ///   Wrap this around a class that we want to catch the measure and arrange
    ///   processes occuring on, and propagate to the parent Perspective, if any.
    ///   Do this because layout invalidations don't flow up out of a
    ///   Viewport2DVisual3D object.
    /// </summary>
    private class LayoutInvalidationCatcher : Decorator
    {
      protected override Size MeasureOverride(Size constraint)
      {
        var wPl = Parent as Perspective;
        if (wPl != null)
        {
          wPl.InvalidateMeasure();
        }
        return base.MeasureOverride(constraint);
      }

      protected override Size ArrangeOverride(Size arrangeSize)
      {
        var wPl = Parent as Perspective;
        if (wPl != null)
        {
          wPl.InvalidateArrange();
        }
        return base.ArrangeOverride(arrangeSize);
      }
    }

    #endregion

    #region Private data

    // Instance data

    // Static data
    private static readonly Point3D[] Mesh =
    {
      new Point3D(0, 0, 0), new Point3D(0, 1, 0), new Point3D(1, 1, 0),
      new Point3D(1, 0, 0)
    };

    private static readonly Point[] TexCoords = {new Point(0, 1), new Point(0, 0), new Point(1, 0), new Point(1, 1)};
    private static readonly int[] Indices = {0, 2, 1, 0, 3, 2};
    private static readonly Vector3D XAxis = new Vector3D(1, 0, 0);
    private static readonly Vector3D YAxis = new Vector3D(0, 1, 0);
    private static readonly Vector3D ZAxis = new Vector3D(0, 0, 1);
    private readonly QuaternionRotation3D mQuaternionRotation = new QuaternionRotation3D();
    private readonly RotateTransform3D mRotationTransform = new RotateTransform3D();
    private readonly ScaleTransform3D mScaleTransform = new ScaleTransform3D();
    private FrameworkElement mLogicalChild;
    private FrameworkElement mOriginalChild;
    private Viewport3D mViewport3D;
    private FrameworkElement mVisualChild;

    #endregion
  }
}