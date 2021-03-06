\ Simple debugging aids

\ Copyright (C) 1995,1997,1999,2002,2003,2004,2005,2006,2007,2009,2011,2012 Free Software Foundation, Inc.

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


\ They are meant to support a different style of debugging than the
\ tracing/stepping debuggers used in languages with long turn-around
\ times.

\ IMO, a much better (faster) way in fast-compilig languages is to add
\ printing code at well-selected places, let the program run, look at
\ the output, see where things went wrong, add more printing code, etc.,
\ until the bug is found.

\ We support fast insertion and removal of the printing code.

\ !!Warning: the default debugging actions will destroy the contents
\ of the pictured numeric output string (i.e., don't use ~~ between <#
\ and #>).

require source.fs

defer printdebugdata ( -- ) \ gforth print-debug-data
' .s IS printdebugdata
defer .debugline ( nfile nline -- ) \ gforth print-debug-line
\G Print the source code location indicated by @var{nfile nline}, and
\G additional debugging information; the default @code{.debugline}
\G prints the additional information with @code{printdebugdata}.

: (.debugline) ( nfile nline -- )
    cr .sourcepos ." :"
    \ it would be nice to print the name of the following word,
    \ but that's not easily possible for primitives
    printdebugdata
    cr ;

[IFUNDEF] debug-fid
stderr value debug-fid ( -- fid )
\G (value) Debugging output prints to this file
[THEN]

' (.debugline) IS .debugline

: .debugline-directed ( nfile nline -- )
    op-vector @ { oldout }
    try
	default-out op-vector !
	['] .debugline debug-fid outfile-execute
	0
    restore
	oldout op-vector !
    endtry
    throw ;

: ~~ ( -- ) \ gforth tilde-tilde
\G Prints the source code location of the @code{~~} and the stack
\G contents with @code{.debugline}.
    current-sourcepos .debugline-directed ;
comp: ( compilation  -- ; run-time  -- ) drop
    compile-sourcepos POSTPONE .debugline-directed ;

:noname ( -- )  stderr to debug-fid  defers 'cold ; IS 'cold

\ print a no-overhead backtrace

: once ( -- )
    \G do the following up to THEN only once
    here cell+ >r ]] true if [[ r> ]] Literal off [[ ;
    immediate compile-only
    
: ~~bt ( -- )
    \G print stackdump and backtrace
    ]] ~~ store-backtrace dobacktrace nothrow [[ ;
    immediate compile-only

: ~~1bt ( -- )
    \G print stackdump and backtrace once
    ]] once ~~bt then [[ ; immediate compile-only

\ launch a debug shell, quit with emtpy line

: ??? ( -- )
    \G Open a debuging shell
    create-input cr
    BEGIN  ." dbg> " refill  WHILE  source nip WHILE
		interpret ."  ok" cr  REPEAT  THEN
    0 pop-file drop ;
' ??? alias dbg-shell

: WTF?? ( -- )
    \G Open a debugging shell with backtrace and stack dump
    ]] ~~bt ??? [[ ; immediate compile-only

\ special exception for places that should never be reached

s" You've reached a !!FIXME!! marker" exception constant FIXME#

: !!FIXME!! ( -- )  FIXME# throw ;

\ replacing one word with another

: replace-word ( xt2 xt1 -- )
  \G make xt1 do xt2, both need to be colon definitions
  >body  here >r dp !  >r postpone AHEAD  r> >body dp !  postpone THEN
  r> dp ! ;

\ watching variables and values

: watch-does> ( -- ) DOES> dup @ ~~ drop ;
: watch-comp: ( xt -- ) comp: >body ]] Literal dup @ ~~ drop [[ ; 
: ~~Variable ( "name" -- )
  Create 0 , watch-does> watch-comp: ;

: ~~Value ( n "name" -- )
    Value [: ~~ >body ! ; comp: drop ]] Literal ~~ >body ! [[ ;] set-to ;

\ trace lines

: line-tracer ( -- )  ['] ~~ execute ;
\G print source position and stack on every source line start
: +ltrace ( -- ) ['] line-tracer is before-line ;
\G turn on line tracing
: -ltrace ['] noop is before-line ;
\G turn off line tracing