/*
 * This file is part of Gnome Subtitles.
 * Copyright (C) 2008 Pedro Castro
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

namespace GnomeSubtitles.Dialog {


public class TimingsSynchronizeDialog : GladeDialog {
	
	/* Constant strings */
	private const string gladeFilename = "TimingsSynchronizeDialog.glade";
	
	/* Widgets */

	public TimingsSynchronizeDialog () : base(gladeFilename){
	
	}
	
	
	private void OnResponse (object o, ResponseArgs args) {
		/*if (args.ResponseId == ResponseType.Ok) {
			SelectionIntended selectionIntended = (allSubtitlesRadioButton.Active ? SelectionIntended.All : SelectionIntended.Range);
			
			if (timingMode == TimingMode.Times) {
				TimeSpan firstTime = TimeSpan.Parse(firstSubtitleNewStartSpinButton.Text);
				TimeSpan lastTime = TimeSpan.Parse(lastSubtitleNewStartSpinButton.Text);
				Base.CommandManager.Execute(new AdjustTimingsCommand(firstTime, lastTime, selectionIntended));
			}
			else {
				int firstFrame = (int)firstSubtitleNewStartSpinButton.Value;
				int lastFrame = (int)lastSubtitleNewStartSpinButton.Value;
				Base.CommandManager.Execute(new AdjustTimingsCommand(firstFrame, lastFrame, selectionIntended));
			}
		}*/
		Close();
	}

}

}