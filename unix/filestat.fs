\ file status and similar stuff                      04oct2013py

\ Copyright (C) 2012 Free Software Foundation, Inc.

\ This file is part of Gforth.

\ Gforth is free software; you can redistribute it and/or
\ modify it under the terms of the GNU General Public License
\ as published by the Free Software Foundation, either version 3
\ of the License, or (at your option) any later version.

\ This program is distributed in the hope that it will be useful,
\ but WITHOUT ANY WARRANTY; without even the implied warranty of
\ MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
\ GNU General Public License for more details.

\ You should have received a copy of the GNU General Public License
\ along with this program. If not, see http://www.gnu.org/licenses/.

c-library filestat
    \c #include <sys/types.h>
    \c #include <sys/stat.h>
    \c #include <sys/time.h>
    \c #include <unistd.h>
    e? os-type s" linux-android" str= [IF]
	\c #include <sys/syscall.h>
	\c #include <sys/linux-syscalls.h>
	\c #ifdef __arm__
	\c #define __NR_utimensat (__NR_SYSCALL_BASE+348)
	\c #endif
	\c int futimens(int fd, const struct timespec ts[2]) {
	\c   return syscall(__NR_utimensat, fd, NULL, ts, 0);
	\c }
	\c int utimensat(int fd, const char* path, const struct timespec ts[2], int flags) {
	\c   return syscall(__NR_utimensat, fd, path, ts, flags);
	\c }
    [THEN]
    
    c-function stat stat a a -- n ( path buf -- r )
    c-function fstat fstat n a -- n ( fd buf -- r )
    c-function lstat lstat a a -- n ( path buf -- r )
    c-function utimensat utimensat n a a n -- n ( fd path times flags -- r )
    c-function futimens futimens n a -- n ( fd times -- r )
    c-function chmod chmod a n -- n ( path mode -- r )
    c-function fchmod fchmod n n -- n ( fd mode -- r )
    c-function chown chown a n n -- n ( path uid git -- r )
    c-function fchown fchown n n n -- n ( fd uid git -- r )
    c-function lchown lchown a n n -- n ( path uid git -- r )
end-c-library

: utimens ( a a -- r )  -100 -rot 0 utimensat ;
: lutimens ( a a -- r )  -100 -rot $100 utimensat ;

begin-structure file-stat
cell 8 = [IF]
    drop 0 8 +field st_dev
    drop 8 field: st_ino
    drop 24 4 +field st_mode
    drop 28 4 +field st_uid
    drop 32 4 +field st_gid
    drop 40 8 +field st_rdev
    drop 48 field: st_size
    drop 56 field: st_blksize
    drop 64 field: st_blocks
    drop 72 16 +field st_atime
    drop 88 16 +field st_mtime
    drop 104 16 +field st_ctime
    drop 144
[ELSE]
    drop 0 8 +field st_dev
    drop 12 field: st_ino
    drop 16 4 +field st_mode
    drop 24 4 +field st_uid
    drop 28 4 +field st_gid
    drop 32 8 +field st_rdev
    drop 44 field: st_size
    drop 48 field: st_blksize
    drop 52 field: st_blocks
    drop 56 8 +field st_atime
    drop 64 8 +field st_mtime
    drop 72 8 +field st_ctime
    drop 88
[THEN]
end-structure

: ntime@ ( addr -- ud )  2@ 1000000000 um* rot 0 d+ ;
: utime@ ( addr -- ud )  2@ 1000000 um* rot 1000 / 0 d+ ;

: ntime! ( ud addr -- )  >r 1000000000 um/mod r> 2! ;
: utime! ( ud addr -- )  >r 1000000 um/mod >r 1000 * r> r> 2! ;
