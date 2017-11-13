using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;
using System.Windows.Threading;

namespace MapLoading.Controls
{
  public class TextWritingEffect : Control
  {
    public static readonly DependencyProperty TextProperty =
      DependencyProperty.Register("Text",
        typeof (string),
        typeof (TextWritingEffect),
        new PropertyMetadata(default(string), OnTextChanged));


    public static readonly DependencyProperty ChrunkLengthProperty =
      DependencyProperty.Register("ChrunkLength",
        typeof (int),
        typeof (TextWritingEffect),
        new PropertyMetadata(1, OnChrunkLengthChanged));


    public static readonly DependencyProperty DurationProperty =
      DependencyProperty.Register("Duration",
        typeof(int),
        typeof (TextWritingEffect),
        new PropertyMetadata(default(int), OnDurationChanged));

    private List<string> mChrunkList;
    private TextBlock mTextBlock;
    private DispatcherTimer mTimer;

    public TextWritingEffect()
    {
      mTimer = new DispatcherTimer();
      mTimer.Tick += TimerTick;
    }
    static TextWritingEffect()
    {
      DefaultStyleKeyProperty.OverrideMetadata(typeof (TextWritingEffect),
        new FrameworkPropertyMetadata(typeof (TextWritingEffect)));
    }

    public int Duration
    {
      get { return (int)GetValue(DurationProperty); }
      set { SetValue(DurationProperty, value); }
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


    private static void OnDurationChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
      var wSender = (TextWritingEffect) d;
      //wSender.UpdateDuration((int) e.NewValue);
    }

    private void UpdateDuration()
    {
      //var wNumberTick = Duration.TotalMilliseconds > 0.0 && Text.Length > 0 ? (Duration.TotalMilliseconds / Text.Length ): 0.0;
      //ChrunkLength = wNumberTick > 0 ? Math.Max(1, (int)Math.Floor(Text.Length / wNumberTick)) : Text.Length;
      //mTimer.Interval = wNumberTick > 0 ? TimeSpan.FromMilliseconds(Duration.TotalMilliseconds / wNumberTick) : Duration; // 20 Milliseconds pour un caractere
      mTimer.Interval = TimeSpan.FromMilliseconds(Duration); // 20 Milliseconds pour un caractere
    }

    private static void OnChrunkLengthChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
      var wSender = (TextWritingEffect) d;
      //wSender.UpdateTextList();
    }


    private static void OnTextChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
      var wSender = (TextWritingEffect) d;
      wSender.UpdateTextList();
    }

    private void UpdateTextList()
    {
      if (Text != null)
      {
        mTextBlock.Text = string.Empty;
        UpdateDuration();
        mChrunkList = SplitBy(Text, ChrunkLength).ToList();
        UpdateText();
        mTimer.Start();
      }
    }

    private void UpdateText()
    {
      string wText = GetNextChrunk();
      if (wText == null)
      {
        mTimer.Stop();
      }
      var wTime = new Stopwatch();
      wTime.Start();
      mTextBlock.Text = mTextBlock.Text + wText;
      wTime.Stop();
      Debug.WriteLine(wTime.Elapsed.TotalMilliseconds);
      //textBlock.Inlines.Add(wText);
    }

    private string ParseNewLine(string value)
    {
      if (value != null)
      {
        value = value;

        if (value.Contains("\\r\\n"))
          value = value.Replace("\\r\\n", Environment.NewLine);

        if (value.Contains("\\n"))
          value = value.Replace("\\n", Environment.NewLine);

        if (value.Contains("&#x0a;&#x0d;"))
          value = value.Replace("&#x0a;&#x0d;", Environment.NewLine);

        if (value.Contains("&#x0a;"))
          value = value.Replace("&#x0a;", Environment.NewLine);

        if (value.Contains("&#x0d;"))
          value = value.Replace("&#x0d;", Environment.NewLine);

        if (value.Contains("&#10;&#13;"))
          value = value.Replace("&#10;&#13;", Environment.NewLine);

        if (value.Contains("&#10;"))
          value = value.Replace("&#10;", Environment.NewLine);

        if (value.Contains("&#13;"))
          value = value.Replace("&#13;", Environment.NewLine);

        if (value.Contains("<br />"))
          value = value.Replace("<br />", Environment.NewLine);

        if (value.Contains("<LineBreak />"))
          value = value.Replace("<LineBreak />", Environment.NewLine);
      }

      return value;
    }

    private string GetNextChrunk()
    {
      if (mChrunkList != null)
      {
        if (!mChrunkList.Any()) return null;

        string wResult = mChrunkList.First();
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
      if (String.IsNullOrEmpty(text)) yield return text;
      if (chunkLength < 1) throw new ArgumentException();

      for (int wIndex = 0; wIndex < text.Length; wIndex += chunkLength)
      {
        if (chunkLength + wIndex > text.Length)
          chunkLength = text.Length - wIndex;

        yield return text.Substring(wIndex, chunkLength);
      }
    }

    public override void OnApplyTemplate()
    {
      base.OnApplyTemplate();
      mTextBlock = GetTemplateChild("PART_Text") as TextBlock;
      Debug.Assert(mTextBlock != null, "Unable to fetch template part 'PART_Text'");

      //UpdateDuration();
      //UpdateTextList();
    }
  }
}