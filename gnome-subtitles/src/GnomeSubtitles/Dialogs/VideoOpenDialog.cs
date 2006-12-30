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

namespace GnomeSubtitles {

public class VideoOpenDialog : GladeDialog {
	private string chosenFilename = String.Empty;

	/* Constant strings */
	private const string gladeFilename = "VideoOpenDialog.glade";
	
	public VideoOpenDialog () : base(gladeFilename) {
	}
	
	/* Public properties */
	
	public string Filename {
		get { return chosenFilename; }
	}
	
	#pragma warning disable 169		//Disables warning about handlers not being used
	
	private void OnResponse (object o, ResponseArgs args) {
		if (args.ResponseId == ResponseType.Ok) {
			chosenFilename = (dialog as FileChooserDialog).Filename;
			actionDone = true;
		}
		CloseDialog();
	}

}

}