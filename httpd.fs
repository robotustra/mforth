#! /usr/local/bin/gforth

warnings off

include string.fs

Variable url
Variable protocol
Variable data
Variable command?

: get ( addr -- )  name rot $! ;
: get-rest ( addr -- )  source >in @ /string dup >in +! rot $! ;

Table constant values
Table constant commands

: value:  ( -- )  name
  Forth definitions 2dup 1- nextname Variable
  values set-current nextname here cell - Create ,
  DOES> @ get-rest ;
: >values  values 1 set-order command? off ;

\ HTTP protocol commands                               26mar00py

: rework-% ( -- )  base @ >r hex
    0 url $@len 0 ?DO
	url $@ drop I + c@ dup '% = IF
	    drop 0. url $@ I 1+ /string
	    2 min dup >r >number r> swap - >r 2drop
	ELSE  0 >r  THEN  over url $@ drop + c!  1+
    r> 1+ +LOOP  url $!len
    r> base ! ;

commands set-current

: GET   url get rework-% protocol get-rest >values data on  ;
: HEAD  url get rework-% protocol get-rest >values data off ;

\ HTTP protocol values                                 26mar00py

values set-current

value: User-Agent:
value: Pragma:
value: Host:
value: Accept:
value: Accept-Encoding:
value: Accept-Language:
value: Accept-Charset:
value: Via:
value: X-Forwarded-For:
value: Cache-Control:
value: Connection:
value: Referer:

definitions


Variable maxnum

: ?cr ( -- )
  #tib @ 1 >= IF  source 1- + c@ #cr = #tib +!  THEN ;
: refill-loop ( -- flag )
  BEGIN  refill ?cr  WHILE  interpret  >in @ 0=  UNTIL
  true  ELSE  maxnum off false  THEN ;
: get-input ( -- flag ior )
  s" /nosuchfile" url $!  s" HTTP/1.0" protocol $!
  s" close" connection $!
  infile-id push-file loadfile !  loadline off  blk off
  commands 1 set-order  command? on  ['] refill-loop catch
  only forth also  pop-file ;

\ Keep-Alive handling                                  26mar00py

: .connection ( -- )
  ." Connection: "
  connection $@ s" Keep-Alive" compare 0= maxnum @ 0> and
  IF  connection $@ type cr
      ." Keep-Alive: timeout=15, max=" maxnum @ 0 .r cr
      -1 maxnum +!  ELSE  ." close" cr maxnum off  THEN ;

\ Use Forth as server-side script language             26mar00py

: $> ( -- )
    BEGIN  source >in @ /string s" <$" search  0= WHILE
        type cr refill  0= UNTIL  EXIT  THEN
    nip source >in @ /string rot - dup 2 + >in +! type ;
: <HTML> ( -- )  ." <HTML>" $> ;

\ Rework HTML directory                                26mar00py

Variable htmldir

: rework-htmldir ( addr u -- addr' u' / ior )
  htmldir $!
  htmldir $@ 1 min s" ~" compare 0=
  IF    s" /.html-data" htmldir dup $@ 2dup '/ scan
        nip - nip $ins
  ELSE  s" /usr/local/httpd/htdocs/" htmldir 0 $ins  THEN
  htmldir $@ 1- 0 max + c@ '/ = htmldir $@len 0= or
  IF  s" index.html" htmldir dup $@len $ins  THEN
  htmldir $@ file-status nip ?dup ?EXIT
  htmldir $@ ;

\ MIME type handling                                   26mar00py

: >mime ( addr u -- mime u' )  2dup tuck over + 1- ?DO
  I c@ '. = ?LEAVE  1-  -1 +LOOP  /string ;

: >file ( addr u -- size fd )
  r/o bin open-file throw >r
  r@ file-size throw drop
  ." Accept-Ranges: bytes" cr
  ." Content-Length: " dup 0 .r cr r> ;
: transparent ( size fd -- ) >r
  dup allocate throw swap
  over swap r@ read-file throw over swap type
  free r> close-file throw throw ;

: transparent: ( addr u -- ) Create  here over 1+ allot place
  DOES>  >r  >file
  .connection
  ." Content-Type: "  r> count type cr cr
  data @ IF  transparent  ELSE  nip close-file throw  THEN ;

\ mime types                                           26mar00py

: mime-read ( addr u -- )  r/o open-file throw
    push-file loadfile !  0 loadline ! blk off
    BEGIN  refill  WHILE  name
	BEGIN  >in @ >r name nip  WHILE
	    r> >in ! 2dup transparent:  REPEAT
	2drop rdrop
    REPEAT  loadfile @ close-file pop-file throw ;

: lastrequest
  ." Connection: close" cr maxnum off
  ." Content-Type: text/html" cr cr ;

wordlist constant mime
mime set-current

: shtml ( addr u -- )  lastrequest
    data @ IF  included  ELSE  2drop  THEN ;

s" application/pgp-signature" transparent: sig
s" application/x-bzip2" transparent: bz2
s" application/x-gzip" transparent: gz
s" /etc/mime.types" mime-read

definitions

s" text/plain" transparent: txt

\ http errors                                          26mar00py

: .server ." Server: Gforth httpd/0.1 ("
    s" os-class" environment? IF  type  THEN  ." )" cr ;
: .ok   ." HTTP/1.1 200 OK" cr .server ;
: html-error ( n addr u -- )
    ." HTTP/1.1 " 2 pick . 2dup type cr .server
    2 pick &405 = IF ." Allow: GET, HEAD" cr  THEN  lastrequest
    ." <HTML><HEAD><TITLE>" 2 pick . 2dup type ." </TITLE></HEAD>" cr
    ." <BODY><H1>" type drop ." </H1>" cr ;
: .trailer ( -- )
    ." <HR><ADDRESS>Gforth httpd 0.1</ADDRESS>" cr
    ." </BODY></HTML>" cr ;
: .nok  command? @ IF  &405 s" Method Not Allowed"
    ELSE  &400 s" Bad Request"  THEN  html-error
    ." <P>Your browser sent a request that this server could not understand.</P>" cr
    ." <P>Invalid request in: <CODE>" error-stack cell+ 2@ swap type
    ." </CODE></P>" cr .trailer ;
: .nofile  &404 s" Not Found" html-error
    ." <P>The requested URL <CODE>" url $@ type
    ." </CODE> was not found on this server</P>" cr .trailer ;

\ http server                                          26mar00py

: http  get-input  IF  .nok  ELSE
    IF  url $@ 1 /string rework-htmldir
	dup 0< IF  drop .nofile
	ELSE  .ok  2dup >mime mime search-wordlist
	    0= IF  ['] txt  THEN  catch IF  maxnum off THEN
	THEN  THEN  THEN  outfile-id flush-file throw ;

: httpd  ( n -- )  maxnum !
  BEGIN  ['] http catch  maxnum @ 0= or  UNTIL ;

script? [IF]  &100 httpd bye  [THEN]