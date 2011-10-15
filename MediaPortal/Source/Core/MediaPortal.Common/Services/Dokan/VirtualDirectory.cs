﻿using System;
using System.Collections.Generic;
using MediaPortal.Common.Logging;
using MediaPortal.Common.ResourceAccess;

namespace MediaPortal.Common.Services.Dokan
{
  /// <summary>
  /// Handle for a virtual directory.
  /// </summary>
  public class VirtualDirectory : VirtualBaseDirectory
  {
    protected IDictionary<string, VirtualFileSystemResource> _children = null; // Lazily initialized

// ReSharper disable SuggestBaseTypeForParameter
    public VirtualDirectory(string name, IFileSystemResourceAccessor resourceAccessor) : base(name, resourceAccessor) { }
// ReSharper restore SuggestBaseTypeForParameter

    public override void Dispose()
    {
      if (_children != null)
        foreach (VirtualFileSystemResource resource in _children.Values)
          resource.Dispose();
      _children = null;
      base.Dispose();
    }

    public IFileSystemResourceAccessor Directory
    {
      get { return (IFileSystemResourceAccessor) _resourceAccessor; }
    }

    public override IDictionary<string, VirtualFileSystemResource> ChildResources
    {
      get
      {
        if (_children == null)
        {
          _children = new Dictionary<string, VirtualFileSystemResource>(StringComparer.InvariantCultureIgnoreCase);
          try
          {
            foreach (IFileSystemResourceAccessor childDirectoryAccessor in Directory.GetChildDirectories())
              _children[childDirectoryAccessor.ResourceName] = new VirtualDirectory(
                  childDirectoryAccessor.ResourceName, childDirectoryAccessor);
            foreach (IFileSystemResourceAccessor fileAccessor in Directory.GetFiles())
              _children[fileAccessor.ResourceName] = new VirtualFile(fileAccessor.ResourceName, fileAccessor);
          }
          catch (Exception e)
          {
            ServiceRegistration.Get<ILogger>().Warn("Dokan virtual directory: Error collecting child resources of directory '{0}'", e, _name);
          }
        }
        return _children;
      }
    }

    public override string ToString()
    {
      return string.Format("Virtual directory '{0}'", _name);
    }
  }
}