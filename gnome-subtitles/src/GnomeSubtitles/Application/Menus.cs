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
using SubLib;

namespace GnomeSubtitles {

public class Menus : GladeWidget {
	ApplicationCore core = null;

	public Menus (GUI gui, Glade.XML glade) : base(gui, glade) {
		core = gui.Core;
	}
	
	public void BlankStartUp () {
		SetBlankSensitivity();
	}

	public void NewDocument (bool wasLoaded) {
		SetNewDocumentSensitivity(wasLoaded);
		SetActiveTimingMode();
		SetFrameRateMenus();	
	}
	
	public void SetActiveTimingMode () {
		if (core.Subtitles.Properties.TimingMode == TimingMode.Frames)
	    	SetActivity(WidgetNames.FramesMenuItem, true);
	    else
	    	SetActivity(WidgetNames.TimesMenuItem, true);
	}
	
	public void SetActiveStyles (bool bold, bool italic, bool underline) {
		SetActivity(WidgetNames.BoldMenuItem, bold, core.Handlers.OnBold);
		SetActivity(WidgetNames.ItalicMenuItem, italic, core.Handlers.OnItalic);
		SetActivity(WidgetNames.UnderlineMenuItem, underline, core.Handlers.OnUnderline);
	}
	
	/* Static members */
	
	static public float FrameRateFromMenuItem (string menuItem) {
		return (float)Convert.ToDouble(menuItem.Split(' ')[0]);
	}

	/* Private members */
	
	private void SetBlankSensitivity () {
		/* File Menu */
		SetSensitivity(WidgetNames.SaveMenuItem, false);
		SetSensitivity(WidgetNames.SaveAsMenuItem, false);
		/* Edit Menu */
		SetSensitivity(WidgetNames.UndoMenuItem, false);
		SetSensitivity(WidgetNames.RedoMenuItem, false);	
		SetSensitivity(WidgetNames.CutMenuItem, false);
		SetSensitivity(WidgetNames.CopyMenuItem, false);
		SetSensitivity(WidgetNames.PasteMenuItem, false);
	}
	
	private void SetNewDocumentSensitivity (bool wasLoaded) {
		if (!wasLoaded) {	
			/* File Menu */
			SetSensitivity(WidgetNames.SaveMenuItem, true);
			SetSensitivity(WidgetNames.SaveAsMenuItem, true);
			/* Edit Menu */
			SetSensitivity(WidgetNames.CutMenuItem, true);
			SetSensitivity(WidgetNames.CopyMenuItem, true);
			SetSensitivity(WidgetNames.PasteMenuItem, true);
			SetMenuSensitivity(WidgetNames.InsertSubtitleMenuItem, true);
			SetSensitivity(WidgetNames.DeleteSubtitlesMenuItem, true);
			/* View Menu */
			SetSensitivity(WidgetNames.TimesMenuItem, true);
			SetSensitivity(WidgetNames.FramesMenuItem, true);
			/* Format Menu */
			SetSensitivity(WidgetNames.BoldMenuItem, true);
			SetSensitivity(WidgetNames.ItalicMenuItem, true);
			SetSensitivity(WidgetNames.UnderlineMenuItem, true);
			/* Toolbar */
			SetSensitivity(WidgetNames.SaveButton, true);
		}
		else {
			/* Edit Menu */
			SetSensitivity(WidgetNames.UndoMenuItem, false);
			SetSensitivity(WidgetNames.RedoMenuItem, false);
			/* Toolbar */
			SetSensitivity(WidgetNames.UndoButton, false);
			SetSensitivity(WidgetNames.RedoButton, false);
		}	
	}
	
	private void SetFrameRateMenus () {
		if (core.Subtitles.Properties.TimingMode == TimingMode.Frames) {
			SetMenuSensitivity(WidgetNames.InputFrameRateMenuItem, true);
			SetMenuSensitivity(WidgetNames.MovieFrameRateMenuItem, true);
		}
		else {
			SetMenuSensitivity(WidgetNames.InputFrameRateMenuItem, false);
			SetMenuSensitivity(WidgetNames.MovieFrameRateMenuItem, true);
		}
		
		SetActivity(WidgetNames.InputFrameRateMenuItem25, true, core.Handlers.OnInputFrameRate);
		SetActivity(WidgetNames.MovieFrameRateMenuItem25, true, core.Handlers.OnMovieFrameRate);
	}
	
	private void SetActivity (string menuItemName, bool isActive) {
		(GetWidget(menuItemName) as CheckMenuItem).Active = isActive;
	}
	
	private void SetActivity (string menuItemName, bool isActive, EventHandler handler) {
		CheckMenuItem menuItem = GetWidget(menuItemName) as CheckMenuItem;
		menuItem.Toggled -= handler;
		menuItem.Active = isActive;
		menuItem.Toggled += handler;		
	}
	
	private void SetSensitivity (string widgetName, bool isSensitive) {
		GetWidget(widgetName).Sensitive = isSensitive;
	}
	
	private void SetMenuSensitivity (string menuItemName, bool sensitivity) {
		MenuItem menuItem = GetWidget(menuItemName) as MenuItem;
		Menu menu = menuItem.Submenu as Menu;
		foreach (Widget widget in menu)
			widget.Sensitive = sensitivity;	
	}



}

}