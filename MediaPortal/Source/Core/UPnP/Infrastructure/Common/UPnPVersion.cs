#region Copyright (C) 2007-2011 Team MediaPortal

/*
    Copyright (C) 2007-2011 Team MediaPortal
    http://www.team-mediaportal.com

    This file is part of MediaPortal 2

    MediaPortal 2 is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    MediaPortal 2 is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with MediaPortal 2. If not, see <http://www.gnu.org/licenses/>.
*/

#endregion

using System;
using System.Linq;

namespace UPnP.Infrastructure.Common
{
  public class UPnPVersion
  {
    public const string VERSION_PREFIX = "UPnP/";

    protected int _verMax;
    protected int _verMin;

    public UPnPVersion(int verMax, int verMin)
    {
      _verMax = verMax;
      _verMin = verMin;
    }

    public int VerMax
    {
      get { return _verMax; }
    }

    public int VerMin
    {
      get { return _verMin; }
    }

    public static bool TryParseFromServerOrUserAgent(string serverOrUserAgent, out UPnPVersion upnpVersion)
    {
      upnpVersion = null;
      if (string.IsNullOrEmpty(serverOrUserAgent))
        return false;
      // The specification says the userAgentStr should contain three entries, separated by ' '.
      // Unfortunately, some devices send entries separated by ", ". We try to handle both situations correctly here.
      string[] versionInfos = serverOrUserAgent.Split(new char[] {',', ' '}, StringSplitOptions.RemoveEmptyEntries);
      string upnpVersionInfo = versionInfos.FirstOrDefault(v => v.StartsWith(UPnPVersion.VERSION_PREFIX));
      if (upnpVersionInfo == null)
        return false;
      return TryParse(upnpVersionInfo, out upnpVersion);
    }

    public static bool TryParse(string versionStr, out UPnPVersion result)
    {
      result = null;
      int dotIndex = versionStr.IndexOf('.');
      if (!versionStr.StartsWith(VERSION_PREFIX) || dotIndex < VERSION_PREFIX.Length + 1)
        return false;
      int verMax;
      if (!int.TryParse(versionStr.Substring(VERSION_PREFIX.Length, dotIndex - VERSION_PREFIX.Length), out verMax))
        return false;
      int verMin;
      if (!int.TryParse(versionStr.Substring(dotIndex + 1), out verMin))
        return false;
      result = new UPnPVersion(verMax, verMin);
      return true;
    }

    public override string ToString()
    {
      return VERSION_PREFIX + _verMax + "." + _verMin;
    }
  }
}
