/*
 * This file is part of Gnome Subtitles.
 * Copyright (C) 2009 Pedro Castro
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

namespace GnomeSubtitles.Dialog {

/// <summary>
/// Represents a dialog scope.
/// </summary>
public enum DialogScope {

	/// <summary>The dialog is active until it's closed.</summary>
	Singleton,
	
	/// <summary>The dialog is active until the document is closed.</summary>
	Document,

	/// <summary>The dialog is active until the video is closed.</summary>
	Video,
	
	/// <summary>The dialog is active until the application is closed.</summary>
	Application
}

}
