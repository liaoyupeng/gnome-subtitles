/*
 * This file is part of Gnome Subtitles, a subtitle editor for Gnome.
 * Copyright (C) 2006 Pedro Castro
 *
 * Gnome Subtitles is free software; you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation; either version 2 of the License, or
 * (at your option) any later version.
 *
 * Gnome Subtitles is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
 * GNU General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License
 * along with this program; if not, write to the Free Software
 * Foundation, Inc., 51 Franklin St, Fifth Floor, Boston, MA  02110-1301  USA
 */

using Gtk;
using System;
using System.Collections;
using SubLib;

namespace GnomeSubtitles {

public class SubtitleEdit : GladeWidget {
	private SpinButton startSpinButton = null;
	private SpinButton endSpinButton = null;
	private SpinButton durationSpinButton = null;
	private TextView textView = null;
	private Widget subtitleEditHBox = null;
	
	private TextTag scaleTag = new TextTag("scale");
	private TextTag boldTag = new TextTag("bold");
	private TextTag italicTag = new TextTag("italic");
	private TextTag underlineTag = new TextTag("underline");
	private ArrayList subtitleTags = new ArrayList(4); //4 not to resize with 3 items
	
	private Subtitle subtitle = null;

	public SubtitleEdit(GUI gui, Glade.XML glade) : base(gui, glade){
		startSpinButton = (SpinButton)GetWidget(WidgetNames.StartSpinButton);
		endSpinButton = (SpinButton)GetWidget(WidgetNames.EndSpinButton);
		durationSpinButton = (SpinButton)GetWidget(WidgetNames.DurationSpinButton);
		textView = (TextView)GetWidget(WidgetNames.SubtitleTextView);
		subtitleEditHBox = GetWidget(WidgetNames.SubtitleEditHBox);
		
   		startSpinButton.WidthRequest = SpinButtonWidth();
   		startSpinButton.Alignment = 0.5f;
   		endSpinButton.Alignment = 0.5f;
   		durationSpinButton.Alignment = 0.5f;   
    		 	
   		scaleTag.Scale = Pango.Scale.XLarge;
   		boldTag.Weight = Pango.Weight.Bold;
   		italicTag.Style = Pango.Style.Italic;
   		underlineTag.Underline = Pango.Underline.Single;
   		textView.Buffer.TagTable.Add(scaleTag);
   		textView.Buffer.TagTable.Add(boldTag);
   		textView.Buffer.TagTable.Add(italicTag);
   		textView.Buffer.TagTable.Add(underlineTag);
    }
    
    
	public bool Sensitive {
    	get { return subtitleEditHBox.Sensitive; }
    	set {
    		subtitleEditHBox.Sensitive = value;
    		if (value == false) {
				SetTextWithoutBufferChanged(String.Empty);
				ClearSpinButtonsValue();
			}
    	}
    }
    
    public void SetUp () {
    	subtitleEditHBox.Sensitive = true;
    	subtitleEditHBox.Visible = true;
    	SetTimingMode();
    }
    
    public void UpdateTimingMode () {
    	SetTimingMode();
	   	LoadTimings();
    }

    public void LoadSubtitle (Subtitle subtitle) {
    	this.subtitle = subtitle;
		LoadTimings();
		LoadTags();
    	LoadText();
    }
    
    public void ReLoadSubtitle () {
    	LoadSubtitle(subtitle);
    }
    
    public void LoadTimings () {
    	if (subtitle == null)
    		return;
    			
		DisconnectSpinButtonsChangedSignals();
    	if (GUI.Core.Subtitles.Properties.TimingMode == TimingMode.Frames) {
			startSpinButton.Value = subtitle.Frames.Start;
			endSpinButton.Value = subtitle.Frames.End;
			durationSpinButton.Value = subtitle.Frames.Duration;
		}
		else {
			startSpinButton.Value = subtitle.Times.Start.TotalMilliseconds;
			endSpinButton.Value = subtitle.Times.End.TotalMilliseconds;
			durationSpinButton.Value = subtitle.Times.Duration.TotalMilliseconds;
		}
		ConnectSpinButtonsChangedSignals();
    }
    
    public void LoadText () {
    	SetTextWithoutBufferChanged(subtitle.Text.Get());
    }
    
    public void LoadTags () {
    	subtitleTags.Clear();
    	if (subtitle.Style.Bold)
    		subtitleTags.Add(boldTag);
    	if (subtitle.Style.Italic)
    		subtitleTags.Add(italicTag);
    	if (subtitle.Style.Underline)
    		subtitleTags.Add(underlineTag);
    }    
    public void ApplyLoadedTags () {
    	TextBuffer buffer = textView.Buffer;
    	TextIter start = buffer.StartIter;
    	TextIter end = buffer.EndIter;
    	buffer.ApplyTag(scaleTag, start, end);
    	foreach (TextTag tag in subtitleTags)
		SetTag(tag, start, end, true);
    }
    
    public void SetBoldTag (bool activate) {
    	SetTag(boldTag, textView.Buffer.StartIter, textView.Buffer.EndIter, activate);
    }
    
    public void SetItalicTag (bool activate) {
    	SetTag(italicTag, textView.Buffer.StartIter, textView.Buffer.EndIter, activate);
    }
    
    public void SetUnderlineTag (bool activate) {
    	SetTag(underlineTag, textView.Buffer.StartIter, textView.Buffer.EndIter, activate);
    }
    
    public void TextGrabFocus () {
    	textView.GrabFocus();
    }
    
    public void StartSpinButtonGrabFocus () {
    	startSpinButton.GrabFocus();
    }
    
    public void EndSpinButtonGrabFocus () {
    	endSpinButton.GrabFocus();
    }
    
    public void DurationSpinButtonGrabFocus () {
    	durationSpinButton.GrabFocus();
    }
    
    
    /* Private Methods */    
    
	private void SetTag (TextTag tag, TextIter start, TextIter end, bool activate) {
    	if (activate)
	   		textView.Buffer.ApplyTag(tag, start, end);
	   	else
	   		textView.Buffer.RemoveTag(tag, start, end);
    }
    
    private void SetTimingMode () {
    	if (GUI.Core.Subtitles.Properties.TimingMode == TimingMode.Frames) {
	   		SetFramesMode(startSpinButton);
	   		SetFramesMode(endSpinButton);
	   		SetFramesMode(durationSpinButton);
	   	}
	   	else {
	   		SetTimesMode(startSpinButton);
	   		SetTimesMode(endSpinButton);
	   		SetTimesMode(durationSpinButton);
	   	}
    }
    
    private void SetTextWithoutBufferChanged (string text) {
    	TextBuffer buffer = textView.Buffer;
    	buffer.Changed -= OnBufferChanged; 
    	if (text == String.Empty)
    		buffer.Clear();
    	else {
    		buffer.Text = text;
    		ApplyLoadedTags();  	
    	}
	    buffer.Changed += OnBufferChanged; 
    }
    
    private void ClearSpinButtonsValue () {
    	DisconnectSpinButtonsChangedSignals();
    	startSpinButton.Text = String.Empty;
    	endSpinButton.Text = String.Empty;
    	durationSpinButton.Text = String.Empty;
    	ConnectSpinButtonsChangedSignals();
    }
    
    private void ConnectSpinButtonsChangedSignals () {
    	startSpinButton.ValueChanged += OnStartValueChanged;
    	endSpinButton.ValueChanged += OnEndValueChanged;
    	durationSpinButton.ValueChanged += OnDurationValueChanged;
    }
    
    private void DisconnectSpinButtonsChangedSignals () {
    	startSpinButton.ValueChanged -= OnStartValueChanged;
   		endSpinButton.ValueChanged -= OnEndValueChanged;
   		durationSpinButton.ValueChanged -= OnDurationValueChanged;
    }

    private int SpinButtonWidth () {
    	const int margins = 25;
		return Utility.TextWidth(startSpinButton, "00:00:00,000", margins);
    }
    
    private int TimeTextToMilliseconds (string text) {
		return (int)TimeSpan.Parse(text).TotalMilliseconds;	
	}
	
	private string MillisecondsToTimeText (int milliseconds) {
		return Utility.TimeSpanToText(TimeSpan.FromMilliseconds(milliseconds));
	}
    
    private void SetTimesMode (SpinButton spinButton) {
    	spinButton.Input += OnInput;
		spinButton.Output += OnOutput;
		spinButton.Adjustment.StepIncrement = 100;
		spinButton.Adjustment.Upper = 86399999;
	}
	
	private void SetFramesMode (SpinButton spinButton) {
    	spinButton.Input -= OnInput;
    	spinButton.Output -= OnOutput;
    	spinButton.Adjustment.StepIncrement = 1;
    	spinButton.Adjustment.Upper = 3000000;
	}
    
	private void OnInput (object o, InputArgs args) {
		args.NewValue = TimeTextToMilliseconds((o as SpinButton).Text);
		args.RetVal = 1;
	}
	
	private void OnOutput (object o, OutputArgs args) {
		SpinButton spinButton = (SpinButton)o;
		spinButton.Numeric = false;
		spinButton.Text = MillisecondsToTimeText((int)spinButton.Value);
		spinButton.Numeric = true;
		args.RetVal = 1;
	}
	
	private void OnBufferChanged (object o, EventArgs args) {
		GUI.Core.CommandManager.Execute(new ChangeTextCommand(GUI, subtitle, (o as TextBuffer).Text));
	}

	private void OnStartValueChanged (object o, EventArgs args) {
		if (GUI.Core.Subtitles.Properties.TimingMode == TimingMode.Frames)
			GUI.Core.CommandManager.Execute(new ChangeStartCommand(GUI, subtitle, (int)startSpinButton.Value));
		else
			GUI.Core.CommandManager.Execute(new ChangeStartCommand(GUI, subtitle, TimeSpan.FromMilliseconds(startSpinButton.Value)));
	}
	
	private void OnEndValueChanged (object o, EventArgs args) {
		if (GUI.Core.Subtitles.Properties.TimingMode == TimingMode.Frames)
			GUI.Core.CommandManager.Execute(new ChangeEndCommand(GUI, subtitle, (int)endSpinButton.Value));
		else
			GUI.Core.CommandManager.Execute(new ChangeEndCommand(GUI, subtitle, TimeSpan.FromMilliseconds(endSpinButton.Value)));
	}
	
	private void OnDurationValueChanged (object o, EventArgs args) {
		if (GUI.Core.Subtitles.Properties.TimingMode == TimingMode.Frames)
			GUI.Core.CommandManager.Execute(new ChangeDurationCommand(GUI, subtitle, (int)durationSpinButton.Value));
		else
			GUI.Core.CommandManager.Execute(new ChangeDurationCommand(GUI, subtitle, TimeSpan.FromMilliseconds(durationSpinButton.Value)));
	}

}

}
