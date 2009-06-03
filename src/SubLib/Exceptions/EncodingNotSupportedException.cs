/*
 * This file is part of SubLib.
 * Copyright (C) 2007-2008 Pedro Castro
 *
 * SubLib is free software; you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation; either version 2 of the License, or
 * (at your option) any later version.
 *
 * SubLib is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
 * GNU General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License
 * along with this program; if not, write to the Free Software
 * Foundation, Inc., 51 Franklin St, Fifth Floor, Boston, MA  02110-1301  USA
 */

using System;

namespace SubLib.Exceptions {

/// <summary>The exception that is thrown when an <see cref="System.Text.Encoding" /> is not supported by the platform.</summary>
public class EncodingNotSupportedException : ApplicationException {
	private static string defaultMessage = "The encoding is not supported by this platform.";

	public EncodingNotSupportedException (string message) : base(message) {
	}
	
	public EncodingNotSupportedException() : base(defaultMessage) {
	}

}

}
