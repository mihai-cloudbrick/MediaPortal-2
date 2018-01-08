#region Copyright (C) 2007-2017 Team MediaPortal

/*
    Copyright (C) 2007-2017 Team MediaPortal
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
using MediaPortal.UI.SkinEngine.DirectX11;
using SharpDX;

namespace MediaPortal.UI.SkinEngine.DirectX.RenderPipelines
{
  /// <summary>
  /// Abstract render pipeline that implementes a generic 1-pass 2D rendering.
  /// </summary>
  internal abstract class AbstractRenderPipeline : IRenderPipeline, IDisposable
  {
    public virtual void BeginRender()
    {
      GraphicsDevice11.Instance.RenderPass = RenderPassType.SingleOrFirstPass;
      GraphicsDevice11.Instance.Context2D1.BeginDraw();
      GraphicsDevice11.Instance.Context2D1.Clear(Color.Black);
    }

    public virtual void Render()
    {
      GraphicsDevice11.Instance.ScreenManager.Render();
    }

    public virtual void EndRender()
    {
      GraphicsDevice11.Instance.Context2D1.EndDraw();
    }

    public virtual void GetVideoClip(RectangleF fullVideoClip, out RectangleF tranformedRect)
    {
      tranformedRect = fullVideoClip;
    }

    public virtual Matrix3x2 GetRenderPassTransform(Matrix3x2 initialScreenTransform)
    {
      return initialScreenTransform;
    }

    public virtual void Dispose() { }
  }
}
