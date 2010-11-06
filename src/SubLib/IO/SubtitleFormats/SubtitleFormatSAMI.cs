/*
 * This file is part of SubLib.
 * Copyright (C) 2007 Pedro Castro
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
using System.Text;
using System.Text.RegularExpressions;
using SubLib.Core.Domain;

namespace SubLib.IO.SubtitleFormats {

internal class SubtitleFormatSAMI : SubtitleFormat {

	internal SubtitleFormatSAMI () {		
		name = "SAMI";
		type = SubtitleType.SAMI;
		mode = SubtitleMode.Times;
		extensions = new string[] { "smi" };
		lineBreak = "|";
		
		format = @"\s*\<sami\>\s*([^<]|<(?!body>))*<body>";
		headers=new string[2]{@"<SAMIParam[^>]*>([^<]|<(?!/SAMIParam>))*</SAMIParam>",@"<STYLE[^>]*>([^<]|<(?!/STYLE>))*</STYLE>" };
		bodyEndIn = @"</BODY>\s*(</SAMI>)?"; //IGNORE CASE
		bodyBeginOut = "<SAMI><HEAD></HEAD>\n<BODY>"; 
		bodyEndOut = "</BODY>\n</SAMI>";
		
		subtitleIn = @"<SYNC\s*start=[^0-9>]*(?<StartMilliseconds>\d*)[^0-9>]*>(?<Text>([^<]|<(?!/?SYNC))*)(</SYNC\s*>)?\s*<SYNC\s*start=[^0-9>]*(?<EndMilliseconds>\d*)[^0-9>]*>([^<]|<(?!/?SYNC))*(</SYNC\s*>)?";
		subtitleOut = "<SYNC Start=\"<<StartTotalMilliseconds>>\"><P class=\"ENUSCC\"><<Text>></P></SYNC>\n<SYNC Start=\"<<EndTotalMilliseconds>>\"><P class=\"ENUSCC\">&#160;</P></SYNC>";
	}
	
	internal override void SubtitleInputPostProcess (Subtitle subtitle) {
		Regex regPara = new Regex(@"\s*<P[^>]*>", RegexOptions.IgnoreCase);
		Regex regParaClose = new Regex(@"</P\s*>\s*", RegexOptions.IgnoreCase | RegexOptions.RightToLeft);
		string oriText = subtitle.Text.GetTrimLines();
		Match para = regPara.Match(oriText);
		Match paraClose = regParaClose.Match(oriText);            
	//	subtitle.endOfLine = @"<br\s*/?>[\r\n]*";
		if(para.Length>0 && paraClose.Length>0){
		    string newText = oriText.Substring(para.Value.Length, oriText.Length-para.Value.Length -paraClose.Value.Length);
	//	    subtitle.videoStart = para.Value;
	//	    Subtitle.PossibleVideoStart = para.Value;
	//	    subtitle.videoEnd = paraClose.Value;
		    subtitle.Text = new SubtitleText(newText);
		}
	}
	
}

}
