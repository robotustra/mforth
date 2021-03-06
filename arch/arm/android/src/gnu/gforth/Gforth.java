/* Android activity for Gforth on Android

  Copyright (C) 2013 Free Software Foundation, Inc.

  This file is part of Gforth.

  Gforth is free software; you can redistribute it and/or
  modify it under the terms of the GNU General Public License
  as published by the Free Software Foundation, either version 3
  of the License, or (at your option) any later version.

  This program is distributed in the hope that it will be useful,
  but WITHOUT ANY WARRANTY; without even the implied warranty of
  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
  GNU General Public License for more details.

  You should have received a copy of the GNU General Public License
  along with this program; if not, see http://www.gnu.org/licenses/.
*/

package gnu.gforth;

import android.view.KeyEvent;
import android.os.Bundle;
import android.media.MediaPlayer;
import android.media.MediaPlayer.OnVideoSizeChangedListener;

public class Gforth
    extends android.app.NativeActivity
    implements KeyEvent.Callback, OnVideoSizeChangedListener {
    public native void onKeyEventNative(KeyEvent event);
    @Override
    public boolean dispatchKeyEvent (KeyEvent event) {
	onKeyEventNative(event);
	return true;
    }
    @Override
    public boolean onKeyDown(int keyCode, KeyEvent event) {
	onKeyEventNative(event);
	return true;
    }
    @Override
    public boolean onKeyUp(int keyCode, KeyEvent event) {
	onKeyEventNative(event);
	return true;
    }
    @Override
    public boolean onKeyLongPress(int keyCode, KeyEvent event) {
	onKeyEventNative(event);
	return true;
    }
    @Override
    public boolean onKeyMultiple(int keyCode, int count, KeyEvent event) {
	onKeyEventNative(event);
	return true;
    }
    @Override
    public void onVideoSizeChanged(MediaPlayer mp, int width, int height) {
    }
}
