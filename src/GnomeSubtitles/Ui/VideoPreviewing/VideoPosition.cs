/*
 * This file is part of Gnome Subtitles.
 * Copyright (C) 2006-2008 Pedro Castro
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
using Mono.Unix;
using SubLib;
using System;

namespace GnomeSubtitles.Ui.Video {

public class VideoPosition {
	private Player player = null;
	private Label positionLabel = null;
	private Label positionValueLabel = null;
	private Label lengthValueLabel = null;	
	private TimeSpan position = TimeSpan.Zero;
	
	/* Slider related */
	private HScale slider = null;
	private uint userUpdateTimeoutId = 0; //the ID of the timeout after which the value will be updated by the user selection
	private bool isPlayerUpdate = false;

	/* Constants */
	private const int userUpdateTimeout = 100; //Milliseconds
	private TimeSpan seekIncrement = TimeSpan.FromMilliseconds(500);

	/* Handlers */
	public delegate void VideoPositionChangedHandler (TimeSpan position);

	/* Events */
	public event VideoPositionChangedHandler Changed;

	public VideoPosition (Player player) {
		slider = Global.GetWidget(WidgetNames.VideoSlider) as HScale;
		positionLabel = Global.GetWidget(WidgetNames.VideoPositionLabel) as Label;
		positionValueLabel = Global.GetWidget(WidgetNames.VideoPositionValueLabel) as Label;
		lengthValueLabel = Global.GetWidget(WidgetNames.VideoLengthValueLabel) as Label;

		this.player = player;
		player.OnPositionChanged = OnPlayerPositionChanged;
	}

	/* Public properties */

	public TimeSpan SeekIncrement {
		get { return seekIncrement; }
	}
	
	/// <summary>The current position, in seconds.</summary>
	public TimeSpan CurrentTime {
		get { return position; }
	}
	
	public int CurrentFrames {
		get { return Convert.ToInt32(SubLib.Synchronization.TimeToFrames(position, player.FrameRate)); }
	}
	
	/* Public methods */
	
	public void Disable () {
		DisconnectSliderSignals();
		RemoveUserUpdateTimeout();
		
		position = TimeSpan.Zero;
		UpdatePositionValues(TimeSpan.Zero);
		SetLength(TimeSpan.Zero);
		
		slider.Sensitive = false;	
	}
	
	public void Enable () {
		SetLength(player.Length);
		// TODO DELETE SetSliderLength(TimeSpan.FromMinutes(5)); //Setting the length to a somewhat random number, because we don't know the video length yet TODO
		slider.Sensitive = true;
		ConnectSliderSignals();
	}
	
	public void ToggleTimingMode (TimingMode newMode) {
		UpdatePositionLabel(newMode);
		UpdatePositionValueLabel(position);
		TimeSpan length = (player.IsLoaded ? player.Length : TimeSpan.Zero);
		UpdateLengthLabel(newMode, length);
	}

	/* Event members */
	
	private void OnSliderValueChanged (object o, EventArgs args) {
		if (isPlayerUpdate) {
			isPlayerUpdate = false;
			return;
		}
		RemoveUserUpdateTimeout();
		AddUserUpdateTimeout();
	}
	
	/// <summary>Handles changes in the player position.</summary>
	private void OnPlayerPositionChanged (TimeSpan newPosition) {
		position = newPosition;
	
		if (userUpdateTimeoutId == 0)  //There is not a manual positioning going on
			UpdateSlider(newPosition);

		UpdatePositionValueLabel(newPosition);
		EmitVideoPositionChanged(newPosition);
	}
	
	private bool UpdatePlayerPosition () {
		userUpdateTimeoutId = 0;
		player.Seek(slider.Value);
		return false;
	}
	
	private void RemoveUserUpdateTimeout () {
		if (userUpdateTimeoutId != 0) {
			GLib.Source.Remove(userUpdateTimeoutId);
			userUpdateTimeoutId = 0;
		}
	}
	
	private void AddUserUpdateTimeout () {
		userUpdateTimeoutId = GLib.Timeout.Add(userUpdateTimeout, UpdatePlayerPosition);
	}
	
	private void EmitVideoPositionChanged (TimeSpan newPosition) {
		if (Changed != null)
			Changed(newPosition);
	}
	
	private void ConnectSliderSignals () {
		slider.ValueChanged += OnSliderValueChanged;
	}
	
	private void DisconnectSliderSignals () {
		slider.ValueChanged -= OnSliderValueChanged;
	}

	/* Private members */
	
	private void UpdatePositionValues (TimeSpan newPosition) {
		UpdateSlider(newPosition);
		UpdatePositionValueLabel(newPosition);
	}
	
	private void UpdateSlider (TimeSpan newPosition) {
		isPlayerUpdate = true;
		slider.Value = newPosition.TotalMilliseconds;
	}

	private void UpdatePositionValueLabel (TimeSpan newPosition) {
		if (Global.TimingMode == TimingMode.Times)
			positionValueLabel.Text = Util.TimeSpanToText(newPosition);
		else {
			double frames = (newPosition == TimeSpan.Zero ? 0 : SubLib.Synchronization.TimeToFrames(newPosition, player.FrameRate));
			positionValueLabel.Text = Convert.ToInt32(frames).ToString();
		}
	}

	private void UpdatePositionLabel (TimingMode timingMode) {
		string mode = (timingMode == TimingMode.Times ? Catalog.GetString("Time") : Catalog.GetString("Frame"));
		positionLabel.Markup = "<b>" + mode + "</b>"; 
	}
	
	private void SetLength (TimeSpan length) {
		SetSliderLength(length);
		UpdateLengthLabel(Global.TimingMode, length);
	}
	
	private void UpdateLengthLabel (TimingMode timingMode, TimeSpan length) {
		if (timingMode == TimingMode.Times)
			lengthValueLabel.Text = Util.TimeSpanToText(length);
		else {
			double frames = (length == TimeSpan.Zero ? 0 : SubLib.Synchronization.TimeToFrames(length, player.FrameRate));
			lengthValueLabel.Text = Convert.ToInt32(frames).ToString();
		}
	}
	
	private void SetSliderLength (TimeSpan length) {
		slider.Adjustment.Upper = length.TotalMilliseconds;
	}

}

}
