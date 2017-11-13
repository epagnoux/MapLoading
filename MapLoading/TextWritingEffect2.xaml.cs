using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace MapLoading
{
  public partial class TextWritingEffect2 : UserControl
  {
    public static readonly DependencyProperty TextProperty =
      DependencyProperty.Register("Text",
        typeof (string),
        typeof (TextWritingEffect2),
        new PropertyMetadata(OnTextChanged));


    public static readonly DependencyProperty ChrunkLengthProperty =
      DependencyProperty.Register("ChrunkLength",
        typeof (int),
        typeof (TextWritingEffect2),
        new PropertyMetadata(1, OnChrunkLengthChanged));


    public int Delay
    {
      get { return (int) GetValue(DelayProperty); }
      set { SetValue(DelayProperty, value); }
    }

    public static readonly DependencyProperty DelayProperty =
      DependencyProperty.Register("Duration",
        typeof (int),
        typeof (TextWritingEffect2),
        new PropertyMetadata(30,OnDelayChanged));

    private static void OnDelayChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
      var wSender = (TextWritingEffect2) d;
      wSender.UpdateDelay((int) e.NewValue);
    }

    private void UpdateDelay(int value)
    {
      mTimer.Interval = TimeSpan.FromMilliseconds(Delay);
      UpdateTextList();
    }


    private readonly DispatcherTimer mTimer;
    private List<string> mChrunkList;


    public TextWritingEffect2()
    {
      InitializeComponent();
      mTimer = new DispatcherTimer();
      mTimer.Tick += TimerTick;
      //mTimer.Interval = TimeSpan.FromMilliseconds(Duration);
      //UpdateTextList();
    }

    public string Text
    {
      get { return (string) GetValue(TextProperty); }
      set { SetValue(TextProperty, value); }
    }

    public int ChrunkLength
    {
      get { return (int) GetValue(ChrunkLengthProperty); }
      set { SetValue(ChrunkLengthProperty, value); }
    }

    private static void OnChrunkLengthChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
      var wSender = (TextWritingEffect2) d;
      wSender.UpdateTextList();
    }


    private static void OnTextChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
      var wSender = (TextWritingEffect2) d;
      wSender.UpdateTextList();
    }

    private void UpdateTextList()
    {
      if (Text != null)
      {
        mChrunkList = SplitBy(Text, ChrunkLength).ToList();
        UpdateText();
        mTimer.Start();
      }
    }

    private void UpdateText()
    {
      var wText = GetNextChrunk();
      if (wText == null)
      {
        mTimer.Stop();
      }
      textBlock.Text = textBlock.Text + wText;
      //textBlock.Inlines.Add(wText);
    }

    private string GetNextChrunk()
    {
      if (mChrunkList != null)
      {
        if (!mChrunkList.Any()) return null;

        var wResult = mChrunkList.First();
        mChrunkList.RemoveAt(0);
        return wResult;
      }
      
      return null;
    }

    private void TimerTick(object sender, EventArgs e)
    {
      UpdateText();
    }

    private IEnumerable<string> SplitBy(string text, int chunkLength)
    {
      if (String.IsNullOrEmpty(text)) throw new ArgumentException();
      if (chunkLength < 1) throw new ArgumentException();

      for (int wIndex = 0; wIndex < text.Length; wIndex += chunkLength)
      {
        if (chunkLength + wIndex > text.Length)
          chunkLength = text.Length - wIndex;

        yield return text.Substring(wIndex, chunkLength);
      }
    }
  }
}