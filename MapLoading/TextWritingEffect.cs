using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using MapLoading.Converters;

namespace MapLoading
{
  
  public class TextWritingEffect : Control
  {
    public static readonly DependencyProperty TextProperty =
      DependencyProperty.Register("Text",
        typeof (string),
        typeof (TextWritingEffect),
        new PropertyMetadata(default(string),OnTextChanged));


    public static readonly DependencyProperty ChrunkLengthProperty =
      DependencyProperty.Register("ChrunkLength",
        typeof (int),
        typeof (TextWritingEffect),
        new PropertyMetadata(1, OnChrunkLengthChanged));


    public static readonly DependencyProperty SpeedProperty =
      DependencyProperty.Register("Speed",
        typeof (int),
        typeof (TextWritingEffect),
        new PropertyMetadata(30, OnDelayChanged));

    private List<string> mChrunkList;
    private TextBlock mTextBlock;
    private DispatcherTimer mTimer;

    static TextWritingEffect()
    {
      DefaultStyleKeyProperty.OverrideMetadata(typeof (TextWritingEffect),
        new FrameworkPropertyMetadata(typeof (TextWritingEffect)));
    }

    public int Speed
    {
      get { return (int) GetValue(SpeedProperty); }
      set { SetValue(SpeedProperty, value); }
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

   

    private static void OnDelayChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
      var wSender = (TextWritingEffect) d;
      //wSender.UpdateDelay((int) e.NewValue);
    }

    private void UpdateDelay(int value)
    {
      mTimer.Interval = TimeSpan.FromMilliseconds(Speed);
      UpdateTextList();
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
      mTextBlock.Text = mTextBlock.Text + wText;
      //textBlock.Inlines.Add(wText);
    }

    private string ParseNewLine(string value)
    {
      if (value != null)
      {
        value = value.ToString();

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
      if (String.IsNullOrEmpty(text)) throw new ArgumentException();
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

      mTimer = new DispatcherTimer();
      mTimer.Tick += TimerTick;
      UpdateDelay(Speed);
      //UpdateTextList();
    }
    
  }
}