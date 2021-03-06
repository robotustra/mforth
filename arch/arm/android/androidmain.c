/* Android main() for Gforth on Android

  Copyright (C) 2012 Free Software Foundation, Inc.

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

#include <stdio.h>
#include <stdlib.h>
#include <string.h>
#include <unistd.h>
#include <sys/types.h> 
#include <sys/stat.h>
#include <fcntl.h>
#include <stdarg.h>

#include "forth.h"

#ifdef __ANDROID__
#include <jni.h>
#include <android/log.h>
#include <android/native_activity.h>
#include <android/looper.h>
#include "android_native_app_glue.h"

static Xt ainput=0;
static Xt acmd=0;
static Xt akey=0;

int ke_fd[2]={ 0, 0 };

typedef struct { jobject event; } sendKeyEvent;

JNIEXPORT void Java_gnu_gforth_Gforth_onKeyEventNative(JNIEnv * env, jobject  obj, jobject event)
{
  sendKeyEvent ke = { event };
  if(ke_fd[1])
    write(ke_fd[1], &ke, sizeof(ke));
}

static JNINativeMethod GforthMethods[] = {
  {"onKeyEventNative", "(Landroid/view/KeyEvent;)V",
   (void*) Java_gnu_gforth_Gforth_onKeyEventNative},
};

int android_kb_callback(int fd, int events, void* data)
{
  sendKeyEvent ke;
  read(fd, &ke, sizeof(ke));
  if(akey && gforth_SP) {
    *--gforth_SP=(Cell)ke.event;
    gforth_execute(akey);
  }
  return 1;
}

void init_key_event()
{
  pipe(ke_fd);
  ALooper_addFd(ALooper_forThread(), ke_fd[0],
		ALOOPER_POLL_CALLBACK, ALOOPER_EVENT_INPUT,
		android_kb_callback, 0);
}

static int32_t engine_handle_input(struct android_app* app, AInputEvent* event) 
{
  if(ainput) {
    *--gforth_SP=(Cell)event;
    gforth_execute(ainput);
    return 1;
  }
  fprintf(stderr, "Input event of type %d\n", AInputEvent_getType(event));
  return 0;
}

static void engine_handle_cmd(struct android_app* app, int32_t cmd)
{
  if(acmd) {
    *--gforth_SP=cmd;
    gforth_execute(acmd);
    return;
  }
  fprintf(stderr, "App cmd %d\n", cmd);
}

const char sha256sum[]="sha256sum-sha256sum-sha256sum-sha256sum-sha256sum-sha256sum-sha2";

int checksha256sum(void)
{
  int checkdir;
  char sha256buffer[64];
  int checkread;

  checkdir=open("/sdcard/gforth/" PACKAGE_VERSION, O_RDONLY);
  if(checkdir==-1) return 0; // directory not there
  close(checkdir);
  checkdir=open("/sdcard/gforth/" PACKAGE_VERSION "/sha256sum", O_RDONLY);
  if(checkdir==-1) return 0; // sha256sum not there
  checkread=read(checkdir, sha256buffer, 64);
  close(checkdir);
  if(checkread!=64) return 0;
  if(memcmp(sha256buffer, sha256sum, 64)) return 0;
  return 1;
}

void register_natives(JavaVM* vm, jobject class,
		      JNINativeMethod * list, int n)
{
  jmethodID jid;
  int i;
  jint val;
  jclass clazz;
  JNIEnv* env;

  val=(*vm)->AttachCurrentThread(vm, &env, NULL);

  clazz=(*env)->GetObjectClass(env, class);
  
  for(i=0; i<n; i++) {
    jid=(*env)->GetMethodID(env, clazz, list[i].name, list[i].signature);
    if(jid==0) fprintf(stderr, "Can't find method %s %s\n",
		       list[i].name, list[i].signature);
  }

  if((*env)->RegisterNatives(env, clazz, list, n)<0) {
    fprintf(stderr, "Register Natives failed\n");
    fflush(stderr);
  }

  (*vm)->DetachCurrentThread(vm);
}

void android_main(struct android_app* state)
{
  char statepointer[2*sizeof(char*)+3]; // 0x+hex digits+trailing 0
  char *argv[] = { "gforth", "starta.fs" };
  const int argc = sizeof(argv)/sizeof(char*);
  int retvalue;
  int checkdir;
  int epipe[2];
  JavaVM* vm=state->activity->vm;
  jobject clazz=state->activity->clazz;

  if((checkdir=open("/sdcard/gforth/", O_RDONLY))==-1) {
    mkdir("/sdcard/gforth/", S_IRWXU | S_IRWXG | S_IROTH | S_IXOTH);
  } else { close(checkdir); }
  if((checkdir=open("/sdcard/gforth/home", O_RDONLY))==-1) {
    mkdir("/sdcard/gforth/home", S_IRWXU | S_IRWXG | S_IROTH | S_IXOTH);
  } else { close(checkdir); }

  freopen("/sdcard/gforth/home/aout.log", "w+", stdout);
  freopen("/sdcard/gforth/home/aerr.log", "w+", stderr);
  pipe(epipe);
  fileno(stdin)=epipe[0];

  if(!checksha256sum()) {
    chdir("/sdcard");
    zexpand("/data/data/gnu.gforth/lib/libgforthgz.so");
    checkdir=creat("/sdcard/gforth/" PACKAGE_VERSION "/sha256sum", O_WRONLY);
    write(checkdir, sha256sum, 64);
    close(checkdir);
  }

  state->onAppCmd = engine_handle_cmd;
  state->onInputEvent = engine_handle_input;
  
  register_natives(vm, clazz,
		   GforthMethods,
		   sizeof(GforthMethods)/sizeof(GforthMethods[0]));
  init_key_event();

  snprintf(statepointer, sizeof(statepointer), "%p", state);
  setenv("HOME", "/sdcard/gforth/home", 1);
  setenv("SHELL", "/system/bin/sh", 1);
  setenv("libccdir", "/data/data/gnu.gforth/lib", 1);
  setenv("LANG", "en_US.UTF-8", 1);
  setenv("APP_STATE", statepointer, 1);
  
  app_dummy();

#ifdef DOUBLY_INDIRECT
  checkdir=open("/sdcard/gforth/" PACKAGE_VERSION "/gforth.fi", O_RDONLY);
  if(checkdir==-1) {
    chdir("/sdcard/gforth/" PACKAGE_VERSION);
    retvalue=gforth_make_image(0);
    exit(retvalue);
  } else {
    close(checkdir);
  }
#endif

  chdir("/sdcard/gforth/home");

  retvalue=gforth_start(argc, argv);

  ainput=gforth_find("ainput");
  acmd=gforth_find("acmd");
  akey=gforth_find("akey");
  
  if(retvalue == -56) {
    Xt bootmessage=gforth_find((Char*)"bootmessage");
    if(bootmessage != 0)
      gforth_execute(bootmessage);
    retvalue = gforth_quit();
  }
  exit(retvalue);
}
#else
int main(int argc, char ** argv, char ** env)
{
  return gforth_main(argc, argv, env);
}
#endif
