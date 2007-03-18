/*
 * This file is part of Gnome Subtitles.
 * Copyright (C) 2007 Pedro Castro
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

using Glade;
using Gtk;
using System;

namespace GnomeSubtitles {

public class PreferencesDialog : GladeDialog {

	/* Constant strings */
	private const string gladeFilename = "PreferencesDialog.glade";

	/* Widgets */
	
	[WidgetAttribute]
	private CheckButton videoAutoSelectFileCheckButton;


	public PreferencesDialog () : base(gladeFilename, false, false) {
		LoadValues();
		Autoconnect();
	}

	/* Private members */
	
	private void LoadValues () {
		videoAutoSelectFileCheckButton.Active = Global.Config.PrefsVideoAutoSelectFile;
	}
	
	/* Event handlers */

	#pragma warning disable 169		//Disables warning about handlers not being used
	
	private void OnResponse (object o, ResponseArgs args) {
		CloseDialog();
	}
	
	private void OnVideoAutoSelectFileToggled (object o, EventArgs args) {
		Global.Config.PrefsVideoAutoSelectFile = videoAutoSelectFileCheckButton.Active;
	}

}

}
