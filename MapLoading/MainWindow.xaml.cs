using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using MapLoading.Controls;

namespace MapLoading
{
    //public partial class MainWindow : Window
    //{

    //    public MainWindow()
    //    {
    //        InitializeComponent();
    //    }


    //    private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
    //    {
    //      writingEffect.Text = "";
    //      writingEffect.Text = textBox.Text;
    //    }
    //}
  public partial class MainWindow : Window
  {
    /// Specifies the current state of the mouse handling logic.
    /// </summary>
    private MouseHandlingMode mouseHandlingMode = MouseHandlingMode.None;

    /// <summary>
    /// The point that was clicked relative to the ZoomAndPanControl.
    /// </summary>
    private Point origZoomAndPanControlMouseDownPoint;

    /// <summary>
    /// The point that was clicked relative to the content that is contained within the ZoomAndPanControl.
    /// </summary>
    private Point origContentMouseDownPoint;

    /// <summary>
    /// Records which mouse button clicked during mouse dragging.
    /// </summary>
    private MouseButton mouseButtonDown;

    public MainWindow()
    {
      InitializeComponent();
      
    }
    /// <summary>
    /// Event raised on mouse down in the ZoomAndPanControl.
    /// </summary>
    private void zoomAndPanControl_MouseDown(object sender, MouseButtonEventArgs e)
    {
      content.Focus();
      Keyboard.Focus(content);

      mouseButtonDown = e.ChangedButton;
      origZoomAndPanControlMouseDownPoint = e.GetPosition(zoomAndPanControl);
      origContentMouseDownPoint = e.GetPosition(content);

      if ((Keyboard.Modifiers & ModifierKeys.Shift) != 0 &&
          (e.ChangedButton == MouseButton.Left ||
           e.ChangedButton == MouseButton.Right))
      {
        // Shift + left- or right-down initiates zooming mode.
        mouseHandlingMode = MouseHandlingMode.Zooming;
      }
      else if (mouseButtonDown == MouseButton.Left)
      {
        // Just a plain old left-down initiates panning mode.
        mouseHandlingMode = MouseHandlingMode.Panning;
      }

      if (mouseHandlingMode != MouseHandlingMode.None)
      {
        // Capture the mouse so that we eventually receive the mouse up event.
        zoomAndPanControl.CaptureMouse();
        e.Handled = true;
      }
    }

    /// <summary>
    /// Event raised on mouse up in the ZoomAndPanControl.
    /// </summary>
    private void zoomAndPanControl_MouseUp(object sender, MouseButtonEventArgs e)
    {
      if (mouseHandlingMode != MouseHandlingMode.None)
      {
        if (mouseHandlingMode == MouseHandlingMode.Zooming)
        {
          if (mouseButtonDown == MouseButton.Left)
          {
            // Shift + left-click zooms in on the content.
            ZoomIn();
          }
          else if (mouseButtonDown == MouseButton.Right)
          {
            // Shift + left-click zooms out from the content.
            ZoomOut();
          }
        }

        zoomAndPanControl.ReleaseMouseCapture();
        mouseHandlingMode = MouseHandlingMode.None;
        e.Handled = true;
      }
    }

    /// <summary>
    /// Event raised on mouse move in the ZoomAndPanControl.
    /// </summary>
    private void zoomAndPanControl_MouseMove(object sender, MouseEventArgs e)
    {
      if (mouseHandlingMode == MouseHandlingMode.Panning)
      {
        //
        // The user is left-dragging the mouse.
        // Pan the viewport by the appropriate amount.
        //
        Point curContentMousePoint = e.GetPosition(content);
        Vector dragOffset = curContentMousePoint - origContentMouseDownPoint;

        zoomAndPanControl.ContentOffsetX -= dragOffset.X;
        zoomAndPanControl.ContentOffsetY -= dragOffset.Y;

        e.Handled = true;
      }
    }

    /// <summary>
    /// Event raised by rotating the mouse wheel
    /// </summary>
    private void zoomAndPanControl_MouseWheel(object sender, MouseWheelEventArgs e)
    {
      e.Handled = true;

      if (e.Delta > 0)
      {
        ZoomIn();
      }
      else if (e.Delta < 0)
      {
        ZoomOut();
      }
    }

    /// <summary>
    /// The 'ZoomIn' command (bound to the plus key) was executed.
    /// </summary>
    private void ZoomIn_Executed(object sender, ExecutedRoutedEventArgs e)
    {
      ZoomIn();
    }

    /// <summary>
    /// The 'ZoomOut' command (bound to the minus key) was executed.
    /// </summary>
    private void ZoomOut_Executed(object sender, ExecutedRoutedEventArgs e)
    {
      ZoomOut();
    }

    /// <summary>
    /// Zoom the viewport out by a small increment.
    /// </summary>
    private void ZoomOut()
    {
      zoomAndPanControl.ContentScale -= 1;
    }

    /// <summary>
    /// Zoom the viewport in by a small increment.
    /// </summary>
    private void ZoomIn()
    {
      zoomAndPanControl.ContentScale += 1;
    }

    /// <summary>
    /// Event raised when a mouse button is clicked down over a Rectangle.
    /// </summary>
    private void Rectangle_MouseDown(object sender, MouseButtonEventArgs e)
    {
      content.Focus();
      Keyboard.Focus(content);

      if ((Keyboard.Modifiers & ModifierKeys.Shift) != 0)
      {
        //
        // When the shift key is held down special zooming logic is executed in content_MouseDown,
        // so don't handle mouse input here.
        //
        return;
      }

      if (mouseHandlingMode != MouseHandlingMode.None)
      {
        //
        // We are in some other mouse handling mode, don't do anything.
        return;
      }

      mouseHandlingMode = MouseHandlingMode.DraggingRectangles;
      origContentMouseDownPoint = e.GetPosition(content);

      Rectangle rectangle = (Rectangle)sender;
      rectangle.CaptureMouse();

      e.Handled = true;
    }

    /// <summary>
    /// Event raised when a mouse button is released over a Rectangle.
    /// </summary>
    private void Rectangle_MouseUp(object sender, MouseButtonEventArgs e)
    {
      if (mouseHandlingMode != MouseHandlingMode.DraggingRectangles)
      {
        //
        // We are not in rectangle dragging mode.
        //
        return;
      }

      mouseHandlingMode = MouseHandlingMode.None;

      Rectangle rectangle = (Rectangle)sender;
      rectangle.ReleaseMouseCapture();

      e.Handled = true;
    }

    /// <summary>
    /// Event raised when the mouse cursor is moved when over a Rectangle.
    /// </summary>
    private void Rectangle_MouseMove(object sender, MouseEventArgs e)
    {
      if (mouseHandlingMode != MouseHandlingMode.DraggingRectangles)
      {
        //
        // We are not in rectangle dragging mode, so don't do anything.
        //
        return;
      }

      Point curContentPoint = e.GetPosition(content);
      Vector rectangleDragVector = curContentPoint - origContentMouseDownPoint;

      //
      // When in 'dragging rectangles' mode update the position of the rectangle as the user drags it.
      //

      origContentMouseDownPoint = curContentPoint;

      Rectangle rectangle = (Rectangle)sender;
      Canvas.SetLeft(rectangle, Canvas.GetLeft(rectangle) + rectangleDragVector.X);
      Canvas.SetTop(rectangle, Canvas.GetTop(rectangle) + rectangleDragVector.Y);

      e.Handled = true;
    }

    private void ButtonBase_OnClick1(object sender, RoutedEventArgs e)
    {
      zoomAndPanControl.AnimatedZoomTo(2, new Rect(200,200,200,200));
      //zoomAndPanControl.AnimatedZoomPointToViewportCenter(2, new Point(ActualWidth /2, ActualHeight / 2), null);
      //AnimatedZoomPointToViewportCenter()
    }
    private void ButtonBase_OnClick2(object sender, RoutedEventArgs e)
    {
      zoomAndPanControl.AnimatedZoomTo(30, new Rect(950,250,200,200));
      //zoomAndPanControl.AnimatedZoomPointToViewportCenter(2, new Point(ActualWidth /2, ActualHeight / 2), null);
      //AnimatedZoomPointToViewportCenter()
    }
  }
}
