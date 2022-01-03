\ gforth program for printing the Forth family tree

\ helper words

: year->x ( year -- x )
    1970 - 100 *
;

dup [if]
    \ tree
: get-y 0 ;

: year-node ( n -- )
	drop ;
[else]
    \ timeline
    get-current
    wordlist constant y-coordinates y-coordinates set-current
    0
    include timeline.fs
\ some manual redefinitions to avoid layout conflicts
\ these have to be revised whenever the layout changes
\ 315 constant whitelightning
\ 390 constant vicforth
\ 210 constant WorkForth
y-coordinates >order
\ UNIFORTH 10 - constant UNIFORTH
\ workforth 20 + constant workforth
previous

    swap set-current

    20 + constant year-y

: scale-y ( y1 -- y2 )
    \     4 3 */ ;
    \ 2 * ;
    ;


: get-y ( -- y )
    \ get the y-coordinate of the latest node
    latest name>string y-coordinates search-wordlist
    0= -13 and throw execute scale-y ;

: year-node ( n -- )
    ." y" dup 0 .r .\" [label=\"" dup 0 .r .\" \" fontsize=20 pos=\"" 
    year->x 0 .r ." ," year-y scale-y 0 .r .\" \"]\n" ;
    
[endif]

: edge ."  -> " ;

: point. ( year -- )
    '" emit year->x 0 .r ." ," get-y 0 .r .\" \"" ;

2variable url  \ url for next node
2variable info \ tooltip info for next node

\ \U and \I comments will be used by the next node
: \U ( "url<eol>" -- )
    -1 parse save-mem url 2! ;

: \I ( "info<eol>" -- )
    -1 parse save-mem info 2! ;

: url-info. ( -- )
    url 2@ over if
	.\"  URL=\"" 2dup type '" emit
    endif
    2drop
    info 2@ over if
	.\"  tooltip=\"" 2dup type '" emit
    endif
    2drop
    0 0 url 2! 0 0 info 2! ;

\ node names have to conform to typical C name conventions (e.g. no spaces)
: standard ( year addr u "nodename" -- )
    \ addr u is the labelname
    create
    latest id.
    .\" [label=\"" type
    .\" \" fontname=\"Helvetica\" fontcolor=\"red\" shape=plaintext height=0 pos=" point. url-info. ." ];" cr
;

: implementation ( year "nodename" -- )
    create
    latest id.
    .\" [fontname=\"Helvetica\" shape=plaintext height=0 pos=" point. url-info.  ." ];" cr
;

: name-implementation ( year addr u "nodename" -- )
    \ addr u is the labelname
    create
    latest id.
    .\" [label=\"" save-mem 2dup 2, type
    .\" \" fontname=\"Helvetica\" shape=plaintext height=0 pos=" point. url-info. ." ];" cr
;

: node. ( addr -- )
    body> >name id. ;

: parse-node. ( "node" -- )
    parse-word evaluate node. ;

: conforms-to ( implementation "standard" -- )
\    parse-node. edge node. ." [weight=1 style=dashed]" cr ;
    parse-node. edge node. .\" [weight=1 color=\"red\"]" cr ;

: forward-edge ( node "node" -- )
    node. edge parse-node. ;

: begot ( node "node" -- )
    forward-edge ." [weight=100]" cr ;

: begat begot ; \ King James bible: Matthew

: inspired ( node "node" -- )
\    forward-edge ." [weight=10 style=dotted] " cr ;
    forward-edge .\" [weight=10 color=\"green\"] " cr ;

\ Sources: the postings in this directory; the "Pedigree" section of
\ the Gforth manual;
\ www.complang.tuwien.ac.at/forth/ftp.dei.isep.ipp.pt/pub/forth/pygmy/forths.dbf
\ http://www.eforth.com.tw/academy/library/insidef83%5Crootsf83.htm

.\" digraph tree { rankdir=LR\npage=\"12,17\"\noverlap=false\n"
." graph [fontsize=8]; edge [fontsize=8]; node [shape=plaintext fontsize=8];" cr
." ranksep = 0.2; nodesep = .04;" cr
1975 year-node
1980 year-node
1985 year-node
1990 year-node
1995 year-node
2000 year-node
2005 year-node
2010 year-node
2015 year-node
2020 year-node

\ standards
\ source: http://soton.mpeforth.com/flag/jfar/vol5/no2/article5.pdf
\         Usenet <jc4kp5$drc$1@speranza.aioe.org>
\U http://stackosaurus.com/misc/Forth_AST.01.pdf
\I Authors: Forth Astronomical Users Group
1977 s" AST.01" standard AST01

\U http://www.complang.tuwien.ac.at/forth/forth-77.txt
\I Authors: European FORTH Users Group
1977 s" Forth-77" standard Forth77
AST01 begot Forth77

\U http://forth.org/OffeteStore/1003_InsideF83.pdf
\I Authors: Forth Standards Team
1978 s" Forth-78" standard Forth78
Forth77 begot Forth78

\ no longer valid: \U http://www.eforth.com.tw/academy/library/insidef83%5Crootsf83.htm
\U http://www.complang.tuwien.ac.at/forth/standards/Forth-79.pdf
\I Authors: Forth Standards Team
1979 s" Forth-79" standard Forth79
Forth78 begot Forth79

\U http://forth.sourceforge.net/standard/fst83/index.html
\I Authors: Forth Standards Team
1983 s" Forth-83" standard Forth83
Forth79 begot Forth83

\U http://www.complang.tuwien.ac.at/forth/dpans-html/dpans.htm
\I Authors: X3J14 TC
1994 s" ANS Forth" standard Forth94
Forth83 begot Forth94

\U http://forth-standard.org
\I Authors: Forth 200x Committee
2014 s" Forth-2012" standard Forth2012
Forth94 begot Forth2012

\ implementations

\ Info from HOPL-II paper
\I SAO Pre-Forth
1958 s" SAO Pre-Forth" name-implementation saoPreForth \ SAO Special Report #11, late 50s; SAO, 1958

\I SLAC Pre-Forth
1961 s" SLAC Pre-Forth" name-implementation slacPreForth
saoPreForth inspired slacPreForth \ precise info would be nice

\I Mohasco B5500 Pre-Forth
1968 s" Mohasco Pre-Forth" name-implementation mohascoPreForth
slacPreForth inspired mohascoPreForth \ precise info would be nice

\I Chuck Moore's Language
\U http://www.forth.com/resources/evolution/evolve_1.html
1971 Implementation Forth \ Chuck Moore's language, early 70s; NRAO, 1972
mohascoPreForth inspired Forth \ precise info would be nice
Forth inspired AST01 \ precise info would be nice

\ Implementation Forth                         \ NRAO 1972
\ Forth implements microFORTH         \ FORTH, Inc. 1977
\I Chuck Moore's Language as Forth Inc. product
\U http://www.forth.com/resources/evolution/evolve_2.html#2.2
1973 Implementation miniFORTH
Forth begot miniFORTH            \ FORTH, Inc. 1973-5

\I Author: Dean Sanderson @ Forth, Inc.
\U http://www.forth.com/resources/evolution/evolve_2.html#2.3
1977 Implementation microFORTH
miniForth begot microForth

\I Authors: Robert Selzer, Bill Ragsdale, and others
\U http://www.eforth.com.tw/academy/library/figforth.htm
1978 s" fig-Forth" name-implementation figForth
microForth begot figForth
figForth conforms-to Forth78

\I Authors: Henry Laxen and Mike Perry
\U http://www.eforth.com.tw/academy/library/insidef83%5Crootsf83.htm
1983 Implementation F83
F83 conforms-to Forth83
figForth inspired F83

\ from Elizabeth Rather
\  As far as standards conformance is concerned, Forth-79 was such an
\  incomplete and contradictory "standard" that I doubt anyone can fully claim
\  conformance to it.  polyFORTH was very close to Forth-83, and had an
\  optional compatibility suite for the non-conforming bits.  This unhappy
\  state of affairs came about because Forth-83 was done in two meetings; the
\  design of pF was finalized between the first and second, and conformed to
\  all the "firm decisions" taken in the first meeting.  But some of those were
\  reversed in the 2nd meeting, and that created too many incompatibilities for
\  us to follow.  SwiftForth conforms to ANS Forth.

\I FORTH, Inc.'s PC offering
\U http://www.forth.com/resources/evolution/evolve_3.html#3.2
1982 Implementation polyFORTH
miniFORTH begot polyFORTH        \ FORTH, Inc., approx. 1980

\I Cross-development environment by Forth, Inc.
\U http://www.computer-solutions.co.uk/chipdev/cf.htm
1985 Implementation chipFORTH
microFORTH inspired chipFORTH  \ FORTH, Inc. & Computer Solutions, mid-80's
polyFORTH begot chipFORTH

\I Native-code, ANS Forth from Forth, Inc.
\U http://www.forth.com/swiftforth/index.html
1996 Implementation SwiftForth
SwiftForth conforms-to Forth94
polyFORTH begot SwiftForth          \ 1996-7

\I Native-code, cross-development environment
\U http://www.forth.com/embedded/index.html
1997 Implementation SwiftX
chipFORTH inspired SwiftX
SwiftForth begot SwiftX          \ 1997

\  Implementation: STOIC
\  Implements: STOIC
\  Project-start: 1975?
\  Inspired-by: Moore's early 70's Forth
\  Hardware: Data General Nova/Eclipse
\  Author: Jonathan Sachs
\  Note: Execution of compiled code at line end instead of word by word 
\  interpretation.
\  Note: An early version was called "FIFTH"
\I by Jonathan Sachs for DG Nova
\U http://hopl.murdoch.edu.au/showlanguage2.prx?exp=2537
1975 Implementation STOIC
Forth inspired STOIC

\  Implementation: LSE
\  Implements: LSE
\  Project-start: 1978?
\  Inspired-by: STOIC
\  Hardware: RCA 1802
\  Author: Robert Goeke
\  Note: Early versions were called "STOIC", although the language is much 
\  simpler than Sachs' STOIC.
1978 Implementation LSE
STOIC inspired LSE

\  Implementation: MAGIC/L
\  Implements: MAGIC/L
\  Project-start: 1978?
\  Inspired-by: STOIC
\  Author: Arnold Epstein
\  Note: Incrementally-compiled PASCAL-like language based on a STOIC-like 
\  threaded code VM, and bootstrapped via an RPN layer.
1978 s" MAGIC/L" name-implementation magicl
stoic inspired magicl

\I public domain implementation of Forth-79, Roy Martin et al.
\U http://theforthsource.com/history.html
1980 s" MVP-Forth" name-implementation MVPForth \ !!date? 1987 forths.dbf
MVPForth conforms-to Forth79
\ date: "developed a Forth implementation based on the 79 Standard
\ when that became available"

\  VIC FORTH [VIC cart] (Datatronic) (Tom Zimmer 1982?? HES?)
\  FIG FORTH implementation with editor, compiler and interpreter.
\  Includes disk, I/O and cassette support and an assembler. VIC version
\  of C64 FORTH.
\  http://ftp.funet.fi/pub/cbm/vic20/programming/VIC-Forth/Manual.txt
\  http://project64.c64.org/misc/datforth.zip
\I by Tom Zimmer
\U http://ftp.funet.fi/pub/cbm/vic20/programming/VIC-Forth/index.html
1982 s" VIC Forth" name-implementation vicforth
figforth begot vicforth

\  64 FORTH [C64 cart] (1983, Tom Zimmer, HES)
\  Full FIG FORTH implementation with editor, compiler and interpreter.
\  Expanded version of VIC FORTH for the C64 which includes disk support,
\  trace, decompile and assembler tools, plus audio and sprite support.
\  http://project64.c64.org/misc/64forthm.zip
\  ftp://ftp.funet.fi/pub/cbm/c64/programming/64FORTH.zip
\  http://project64.ath.cx/misc/64forthm.zip
\I by Tom Zimmer
\ \U http://project64.c64.org/misc/
\U ftp://ftp.forth.org/pub/Forth/Compilers/native/misc/commodore64/    
1983 s" 64 Forth" name-implementation hes64forth
vicforth begot hes64forth

\  C64 FORTH/79 [C64] (1983, Greg Harris, Performance Micro Products)
\  "Conforms to FORTH-79 standard with double-number extensions." Nice
\  commercial package, with plenty of C64-dependent niceties like
\  graphics, disk access, screen editing included. 
1983 s" C64 FORTH/79" name-implementation c64forth79
c64forth79 conforms-to forth79

\  Blazin' Forth [C64,264] (© 1985 Scott Ballantyne)
\  Complete implementation of FORTH-83 with support for sound, turtle
\  graphics, sprites and strings. A Plus/4 version was also created,
\  lacking only the sound and sprite words.
\  (the following are claimed to be doc files, but I was not able to connect)
\  ftp://ftp.forth.org/pub/Forth/Compilers/native/misc/commodore64/blazinforth.doc
\  ftp://ftp.forth.org/pub/Forth/Compilers/native/misc/commodore64/blazinforth.arc
\  ftp://ftp.forth.org/pub/Forth/Compilers/native/misc/commodore64/blazin64.zip
\  ftp://ftp.forth.org/pub/Forth/Compilers/native/misc/commodore64/blazinp4.zip
\ ftp://ftp.forth.org/pub/Forth/Compilers/native/misc/commodore64/
\I (C) Scott Ballantyne    
\U ftp://ftp.forth.org/pub/Forth/Compilers/native/misc/commodore64/
1985 s" Blazin' Forth" name-implementation blazinforth
blazinforth conforms-to forth83

\  FORTH [C64,PET] (1982, LC. Cargile and Michael Riley, A B Computers)
\  FIG FORTH with FORTH 79 standard extensions. Full screen editor.
\  Metacompiler available separately for producing a compact run-time
\  version of a program. Floating point support available separately.
1982 s" Forth (Cargile&Riley)" name-implementation forthcargile
figforth begot forthcargile
forthcargile conforms-to forth79

\  Super FORTH 64 [C64] (1983, Elliot B. Schneider, Parsec Research)
\  MVP-FORTH/FORTH 79 superset. Includes full screen editor, split screen
\  display, interpreter and compiler, sound, graphics, turtle graphics,
\  sprite, disk and kernel routines, floating point math, interrupt
\  routines, conditional macro assembler. Includes sprite editor.
\  Artificial intelligence expert systems development module available
\  separately.
1983 s" Super Forth 64" name-implementation superforth64
MVPForth begot superforth64

\  Tiny FORTH-64 [C64] (1983, Abacus Software)
\  FIG FORTH implementation.
1983 s" Tiny FORTH-64" name-implementation tinyforth64
figforth begot tinyforth64

\  Tri FORTH [C64] (1983, International Tri Micro)
\  FIG FORTH implementation with extensions.
1983 s" Tri FORTH" name-implementation triforth
figforth begot triforth

\  UNIFORTH [VIC] (1984, Unified Software Systems)
\  Enhanced FORTH-79 with strings, arrays, etc., full macro assembler,
\  video editor, complete DOS interface, primitive disk utility,
\  IEEE-compatible floating point with all trig functions, single-step
\  tracing, decompiler, text file support, vectored I/O.
1984 implementation UNIFORTH
UNIFORTH conforms-to Forth79

\  Sixty FORTH [C64] (1984, Elcomp Publishing Inc.)
\  Extended FIG FORTH.
1984 s" Sixty FORTH" name-implementation sixtyforth
figforth begot sixtyforth

\  White Lightning [C64] (1984, Oasis Software)
\  "White Lightning is a complete games writing system comprising of a
\  high level, FORTH based, multi-tasking games writing language and a
\  powerful Sprite Generator Program. Without doubt, White Lightning's
\  most innovative feature is its use of interrupts to effectively run
\  two program at once." (description of the Sinclair version).
\  "Ideal" subset of FIG-FORTH, but allowed intermixing of BASIC
\  Lightning and FORTH commands to produce fast hybrid code. Support for
\  scrolling, raster effects (256 sprites), etc. Includes a sprite
\  designer, sprite library, BASIC Lightning, and a compiler that can
\  produce standalone executables. Adds over 250 commands to BASIC.
\  Popular in the UK and spawned several user groups.
1984 s" White Lightning" name-implementation whitelightning
figforth begot whitelightning

\  FORTH-64 Language [C64] (1985, Abacus Software)
\  FORTH 79 with some FORTH 83 extensions. Includes full-screen editor,
\  complete FORTH-style assembler, programming tools. Includes hires and
\  lores graphics and sound words.
1985 s" Forth-64" name-implementation forth64
forth64 conforms-to forth79
forth83 inspired forth64

\  Master FORTH [C64] (1986, MicroMotion)
\  Follows the FORTH-83 standard; includes graphics system.
1986 s" Master FORTH" name-implementation masterforth
masterforth conforms-to forth83

\  Enhanced FORTH [C64] (1986, Accelerated Software)
\  Follows FIG FORTH standard.
1986 s" Enhanced FORTH" name-implementation enhancedforth
figforth begot enhancedforth

\  geoFORTH [C64 GEOS] (© 1989 Nicholas Vrtis)
\  Version of FIG FORTH which runs under GEOS.
\  (the following are claimed to be doc files, but I was not able to connect)
\  ftp://ftp.forth.org/pub/Forth/Archive/commodore64/geoforth.doc
\  ftp://ftp.forth.org/pub/Forth/Compilers/native/misc/commodore64/geoforth.doc
\  ftp://ftp.forth.org/pub/Forth/Compilers/native/misc/commodore64/geoforth.arc
\U ftp://ftp.forth.org/pub/Forth/Compilers/native/misc/commodore64/    
1989 implementation geoFORTH
figforth begot geoFORTH

\  Laser Genius [C64] (Date unknown - before 1986)
\  (David Hunter, Martin Lewis and Andrew Foord, Oasis Software)
\  Assembler with syntax-checking editor. Monitor with over 70 commands
\  included. Supports "the analyser" with intelligent breakpoints to slow
\  or stop the execution of a program, controlled by a tiny subset of
\  FORTH language commands.
\  http://home.freeuk.net/markk/CBM/Laser_Genius.readme
\  http://home.freeuk.net/markk/CBM/Laser_Genius.tar.gz
\ s" Laser Genius" name-implementation lasergenious

\ cmForth
1984 Implementation cmForth
miniForth begot cmForth

\ Pygmy
\U http://www.eskimo.com/~pygmy/forth.html
\I by Frank Sergeant
1992 Implementation Pygmy \ 1992 forths.dbf
cmForth inspired Pygmy \ forths.dbf

\  Implementation: 4IM
\  Implements: itself
\  Project-start: 2002-10
\  Contains-code-from: (nihil)
\  Inspired-by: Pygmy
\  Inspired-by: cmForth
\  Reference: http://membres.lycos.fr/astrobe
\  Release: 2002-10
\  Release: 2003-04
\  Release: 2003-06
\  Release: 2003-11
\  Release: 2004-01
\  Release: 2004-06
\U http://membres.lycos.fr/astrobe    
2002 s" 4IM" name-implementation fourIM
Pygmy inspired fourIM
cmForth inspired fourIM

\ VolksForth and TurboForth
\U http://www.forth-ev.de/filemgmt/singlefile.php?lid=17
\I for MSDOS, 6502, Z80, Atari ST by Forth Gesellschaft
1985 Implementation VolksForth
figForth begot VolksForth
VolksForth conforms-to Forth83

1986 Implementation deltatFORTH
VolksForth begot deltatFORTH
deltatFORTH conforms-to Forth83
    
1988 Implementation TurboForth
TurboForth conforms-to Forth83
VolksForth begot TurboForth

\ BigForth
\U http://www.jwdt.com/~paysan/bigforth.html
\I Free, native code, by Bernd Paysan    
1990 Implementation BigForth
BigForth conforms-to Forth94
TurboForth begot BigForth

\ Implementation: Gforth
\ Implements: ANS Forth
\ Project-start: 1992-07
\ Contains-code-from: bigForth 1993
\ Inspired-by: fig-Forth
\ Inspired-by: F83
\ Reference: http://www.complang.tuwien.ac.at/forth/gforth/Docs-html/Origin.html
\ Release: 0.2 1996-12
\ Release: 0.3 1997-04
\ Release: 0.4 1998-12
\ Release: 0.5 2000-09
\ Release: 0.6 2003-03
\ Reference: http://www.complang.tuwien.ac.at/forth/gforth/old/
\ \U http://www.complang.tuwien.ac.at/forth/gforth
\U http://gforth.org
\I Free, portable and fast
1996 Implementation Gforth
Gforth conforms-to Forth94
BigForth begot Gforth
figForth inspired Gforth
F83 inspired Gforth

\ version 3.2 in 1987
\U DOS Forth by Ray Duncan
1984 s" LMI PC/Forth" name-implementation lmi_pcf
lmi_pcf conforms-to forth83    

\ UR/Forth
1986 s" LMI UR-Forth" name-implementation urforth \ !!date?
urforth conforms-to forth83 \ forths.dbf
lmi_pcf begot urforth

\ version 3.0 manual copyright 1985-1989
1985 s" LMI Metacompilers" name-implementation lmi_meta
lmi_pcf begot lmi_meta

\  Implementation: kForth
\  Implements: ANS Forth subset + special features (dynamic type checking, 
\  dynamically allocated dictionary, deferred execution)
\  Project-start: 1995, as an embedded interpreter for the program xyplot
\  Contains-code-from: No prior program.
\  Inspired-by: LMI UR/Forth
\  Reference: http://www.ccreweb.org/software/kforth/kforth.html
\  Release: see http://www.ccreweb.org/software/kforth/revisions.html
\  Reference: Forthwrite article, "Special Features of kForth",
\              issues 116 and 117, 2002.
\I Free, by Krishna Myneni
\U http://ccreweb.org/software/kforth/kforth.html
1995 Implementation kForth
kForth conforms-to Forth94
urforth inspired kForth

\ from http://www.forth.com/resources/evolution/evolve_3.html#3.2
\I for 68000 from Don Colburn
\U http://www.forth.com/resources/evolution/evolve_3.html#3.2
1980 implementation MultiForth
figForth begot MultiForth

\I Author: Miller Microcomputer Services
\U https://www.millermicro.com/mmsforth.html
1979 implementation MMSForth
MMSForth conforms-to Forth79
microForth inspired MMSForth \ see the paper "Editors and Editing Technique: A Review of Fancy and Practical Features in the Evolution of the MMSFORTH Screen Editor"

\ from Doug Hoffman
\ confirmed by Ward McFarland as follows:
\ Multiforth was the basis for MacForth in early 1984.
\ 
\ Don Colburn at CSI also spun it off into Atari & Amiga versions 
\ shortly thereafter.  I have asked him to remind me or you of the 
\ actula names and dates.
\ 
\ Power MacForth evolved from MacForth and first surfaced in late 94 or early 95.
\ 
\ Carbon MacForth evolved from Power MacForth in late 2000 to add Mac 
\ OSX support.

\I Creative Solutions (Don Colburn)
\U http://www.macforth.com/
1984 Implementation MacForth \ Creative Solutions 1984, First Forth for Mac
MultiForth begot MacForth \ probably, according to HOPL-II
\ date 1984 according to HOPL-II, Doug Hoffman; 1985 according to
\ www.macforth.com

\I Creative Solutions (Don Colburn)
\U http://www.macforth.com/
1987 s" MacForth+" name-implementation MacForthplus \ Creative Solutions 1987?  Supported new Roms
MacForth begot MacForthplus \ Creative Solutions 1987?  Supported new Roms

\I Creative Solutions/Forth, Inc./Megawolf
\U http://www.macforth.com/
1995 Implementation PowerMacForth \ !!date? 
MacForthplus begot PowerMacForth \ MegaWolf Date?
PowerMacForth conforms-to Forth94
\ PowerMacs came out in 1996 or so; but Ward McFarland writes that he
\ supervised the transition to PowerMacForth while he was at CSI
\ (1990-1995).

\I Megawolf, MacOS X support
\U http://www.macforth.com/
2000 s" Carbon MacForth" name-implementation CarbonMacForth
PowerMacForth begot CarbonMacForth
CarbonMacForth conforms-to Forth94

\I by Charles Duff
\U http://foldoc.doc.ic.ac.uk/foldoc/foldoc.cgi?Neon
1985 Implementation Neon
1980 Implementation Smalltalk \ !! not a Forth system \ Kriya Systems 1985  Charles Duff OOP
Forth inspired Neon
Smalltalk inspired Neon
1990 Implementation Yerk \ Bob Loewenstein Date?  Controlling Observatory telescope
Neon begot Yerk
\I by Mike Hore
\U http://www.powermops.org/
1990 Implementation Mops \ Mike Hore Date? Subroutine Threading, Optimizing Compiler
Neon inspired Mops
\I by Mike Hore
\U http://www.powermops.org/
1996 Implementation PowerMops
Mops begot PowerMops \ Mike Hore Date? PowerPC native code, OSX Native Code Date?
\ 1986 Implementation MachForth \ Company?  !! Date? Subroutine Threading
1989 Implementation Flint \ !! no date information; by G. Yates Fletchner
1993 Implementation PocketForth \ date from http://hopl.murdoch.edu.au/showlanguage.prx?exp=3693
Flint inspired PocketForth \ Chris Heilman Date?  Extreme Minimalism
figForth inspired PocketForth \ from PocketForth introduction

\ from http://www.eforth.com.tw/academy/library/fpcusersmanual/0-4.htm
1984 implementation F83Y \ !! date? by Wil Baden
F83 begot F83Y

\ from email by Friederich Prinz
\I (aka DF) by Tom Zimmer
1985 implementation ZF
F83Y begot ZF
    
\ from Alex McDonald
\I by Tom Zimmer
\U http://www.eforth.com.tw/academy/library/f-pctech.htm
1988 s" F-PC" name-implementation FPC \ !! date? nothing known by me; TJZ public domain
\ F83Y begot FPC \ according to http://www.eforth.com.tw/academy/library/fpcusersmanual/0-4.htm;
    \ there was also a zForth first.
ZF begot FPC    

1990 Implementation TCOM \ !!date ? nothing known by me; TJZ
FPC begot TCOM

\I public domain, by Andrew McKewan and Tom Zimmer
\U http://win32forth.sourceforge.net/
1994 Implementation Win32Forth \ Andrew McKewan, Robert Smith, Y.T. Lin, Andy Corsack, Jim Schneider; mid 1994; WinNT
\ unknown inspired Win32Forth \ Andrew McKewan; 1994; ? anyone know? F-83? F-PC?
Win32Forth conforms-to Forth94
FPC begot Win32Forth \ Tom Zimmer; 1994
F83 inspired Win32Forth \ Andrew McKewan; 1996; meta compilation
Yerk inspired Win32Forth \ Andrew McKewan; 1996; OO support
figForth inspired Win32Forth \ Tom Zimmer's additions

\ from Stephen Pelc:

\ Although it was no formal standard, FIG Forth was
\ influential enough to deserve categorisation as a
\ standard
\ 1978 0 s" FIG Forth" standard ForthFIG

1983 implementation WorkForth  \ circa 1983
\ The first hosted MPE Forth for CPM, MSDOS and so on.
\ These were 16 bit ITC Forths as far as I can remember.
\  WorkForth conforms-to ForthFIG
  figForth inspired WorkForth

\I by MPE
\U http://www.mpeltd.demon.co.uk/forthsta.htm
1985 implementation PowerForth
\ Our first Forth 83 product and the basis of a chassis
\ which lasted until ANS Forth arrived. 16 bit DTC DOS system
\ with TOS cached.
  PowerForth conforms-to Forth83
\  WorkForth inspired PowerForth
  WorkForth begot PowerForth

\I by MPE
\U http://www.mpeltd.demon.co.uk/forthsta.htm
1987 implementation ModularForth
\ A derivative of PowerForth with multiple 64k modules.
  ModularForth conforms-to Forth83
\  PowerForth inspired ModularForth
  PowerForth begot ModularForth

1988 implementation GemForth
\ 32 bit version for the Atari ST
  GemForth conforms-to Forth83
\  PowerForth inspired GemForth
  PowerForth begot GemForth

\I 32-bit by MPE
\U http://www.mpeltd.demon.co.uk/pfd.htm
1988 s" ProForth for DOS" name-implementation PFD
\ 32 bit version for DOS
  PFD conforms-to Forth83
\  PowerForth inspired PFD
  PowerForth begot PFD

\I by MPE
\U http://www.mpeltd.demon.co.uk/pfw2.htm
1991 s" ProForth for Windows" name-implementation PFW
\ 32 bit version for Windows. Released almost simultaneously
\ with Windows NT v3.1. Version 1.x was soon replaced by
\ version 2.x.
  PFW conforms-to Forth83
\  PFD inspired PFW
  PFD begot PFW

PFW inspired Win32Forth \ <4110a43d.591140234@192.168.0.1>


1978 s" UCSD Pascal" name-implementation Pcode \ for reference
1990 s" ANSI C" standard AnsiC    \ for reference

1993 implementation SENDIT
\ A tokenised Forth VM for byte code interpreters on embedded
\ network systems. Produced as a deliverable for the EU SENDIT
\ project, it had both C and Forth compilers.
\  Forth83 inspired SENDIT
  AnsiC inspired SENDIT
  Pcode inspired SENDIT
  SENDIT conforms-to Forth83

1996 implementation OTA
\ A SENDIT derivative for payment terminals with C and Forth
\ compilers. The project is owned by Europay International
\ (now part of MasterCard). The tool chains were mostly 
\ developedby MPE. The run-time kernels were mostly developed
\ by Forth Inc (in Forth), a few by MPE (in Forth) and the C
\ kernel was written by Rowley Associates as an MPE
\ subcontractor.
  OTA conforms-to Forth94
  SENDIT inspired OTA

1997 implementation Practical
\ Another EU research project in which MPE worked on
\ JIT compilers for tokenised systems. These formed the
\ basis of the VFX code generator.
\  SENDIT inspired Practical
  SENDIT begot Practical

\I sophisticated native-code compiler by MPE
\U http://www.mpeltd.demon.co.uk/pfwvfx.htm
1998 s" VFX Forth for Windows" name-implementation VFXW
\ 32 bit version for Windows. Only the third Forth chassis 
\ that MPE has written. A native code ANS system with a
\ very aggressive code generator. Developed simultaneously
\ with the VFX cross compilers below.
  VFXW conforms-to Forth94
\  PFW inspired VFXW
  PFW begot VFXW
  SENDIT begot VFXW

\I Cross-development environment by MPE
\U http://www.mpeltd.demon.co.uk/forth6.htm
1998 s" VFX Cross Compilers" name-implementation XVFX
\ The VFX code generator has been ported to produce code
\ for seven different CPU architectures: i32, H8S, ARM, 68xxx,
\ Coldfire, 68HC12/9S12, MSP430. Several other CPUs have been
\ prototyped.
  XVFX conforms-to Forth94
  Practical inspired XVFX
\  PFW inspired XVFX
  PFW begot XVFX

\I Author: Bill Muench
\U https://web.archive.org/web/20080720002954/http://www.baymoon.com/~bimu/forth/
1990 Implementation bForth \ metacompiled system

\I Author: C. H. Ting (Bill Muench for the more complete, less popular version)
\U http://forth.org/library/eforth_SOC/eforth_SOC_source/eForth1/
1991 Implementation eForth \ actually 1990, but this leads to a bad placement in the timeline
bForth begot eForth

\I Author: Wonyong Koh, Ph.D. (ANS-ified version of eForth)
\U https://www.taygeta.com/hforth.html
1997 Implementation hForth
eForth inspired hForth

\ RetroForth is a public_domain implementation of the Forth
\ programming language. Originally written in 1998 by Tom Novelli, and
\ maintained by Charles Childers since 2002
\ RetroForth [since version 3] is modeled after CMforth, Colorforth, 
\ Eforth and Pygmy. It uses some, but not all, of Chuck Moore's 
\ newer ideas. It's clean, elegant, and tiny - only about 20k 
\ source/10k binary for Linux. 
\I public domain, by Tom Novelli
\U http://www.retroforth.org/ 
1998 Implementation RetroForth
cmForth inspired RetroForth
\ ColorForth inspired RetroForth
Eforth inspired RetroForth
Pygmy inspired RetroForth

\I for PalmOS by Neal Bridges, Quartus Handheld Software
\  PilotFORTH [PalmOS]
\  Prototype implementation of an ANS 94 Standard Forth for
\  PalmPilot handhelds.  Freeware.
1997 Implementation PilotFORTH
cmForth inspired PilotFORTH
PilotFORTH conforms-to Forth94

\I for PalmOS by Neal Bridges, Quartus Handheld Software
\U http://www.quartus.net/products/forth
\  Quartus Forth [PalmOS]
\  Full commercial implementation of an ANS 94 Standard Forth, for
\  Palm Powered handhelds, with a wide range of extensions for the
\  Palm OS.
1998 s" Quartus Forth" name-implementation QuartusForth
PilotFORTH begot QuartusForth
QuartusForth conforms-to Forth94

\I small, cross-platform (x86 Linux and Windows) by Ron Aaron
\U http://ronware.org/reva/
\ Announcement: <slrndfeup4.8d9.ronaaron@newsgroups.comcast.net> 2005-08-08
2005 Implementation Reva
RetroForth begot Reva

\ 
\I by Jörg Völker presented at Forth-Tagung 2006
2005 Implementation T4
SwiftX inspired T4
\ Holon11 inspired T4

\ native Forth for Z80
1982 Implementation popFORTH
figFORTH begot popFORTH

\I by Egmont Woitzel (FORTecH)
1986 Implementation comFORTH
popFORTH begot comFORTH
comFORTH conforms-to Forth83

\ \I by Klaus Kohl
\ originally reported as 1990, changed to 1991 to ease layout
1991 Implementation kkForth
kkForth conforms-to Forth83

\ \I on CP/M with separate headers
\ 1979 Implementation SL5

\I on Telefunken TEMIC MARC4 (a very successful 4-bit Forth chip)
1994 s" MARC4 qForth" name-implementation Marc4qForth
Marc4qForth conforms-to Forth83

\I by Talbot Microsystems, CA for 6809 and 6800
1982 s" Talbot tFORTH" name-implementation TalbotForth
figForth begot TalbotForth

\U http://sourceforge.net/projects/dragonforth/
\I for PalmOS by Dimitry Yakimov?
2001 s" Dragon FORTH" name-implementation DragonForth
\ Martin Bitter writes:
\ Jahreszahl leider nicht bekannt. Meine =E4lteste Version ist von 6.2001
DragonForth conforms-to Forth94


\ 32FORTH von Rainer Aumiller und Denise Luda (Buch mit Diskette) 1988
\ (Buchtitel: ATARI ST 32FORTH-Compiler erschienen bei Markt&Technik)
\ Ein 32-bittiges Forth für den Atari-ST, GEM-VDI, LINE-A, 
\ Im Vorwort wird FIG und F83 erwähnt.
1988 s" 32FORTH" name-implementation _32FORTH
figForth inspired _32FORTH
F83 inspired _32FORTH

\I Minimal ANS Forth by Chris Jakeman written in Forth
\U ftp://ftp.taygeta.com/pub/Forth/Applications/ANS/maf1v02.zip
\ http://www.figuk.plus.com/codeindex/all_by_date.html says:
\ tutorial        distribution    Jakeman Chris   1996-02 MAF - Minimal ANS Forth
\ Written as an educational tool for anyone planning to implement a Forth
\ and includes extensive documentation. Used by Ralph Hempel to make
\ pbForth portable        FIG Archive     author  ANS Forth
1996 Implementation MAF
MAF conforms-to Forth94

\ From Marcel Hendrix:

\ iForth is derived from tForth, which conforms to ANS Forth.

\ tForth is strongly influenced by Bill Muench's eForth.

\ The optimizers for tForth and iForth are independent developments.

\ The root of both Forths is Aackosoft FIG-Forth (for the Sinclair
\ ZX-81, released in 1982 http://tzxvault.retrogames.com/f.htm). I
\ disassembled this Forth (by hand) and rewrote it in assembler for a
\ homebrew Z80 system.  This had to be done in this way because
\ floppy/harddisks were frightfully expensive at this point in
\ time. The Z80 Forth was ported to a 68000 kit from the now defunct
\ German magazine MC. Originally the Forth had 16 bit data and 32 bit
\ addressing, but I modified this to 32/32 bits to become more
\ compatible with existing Forths of the time. This must have been
\ around 1986.  Forth was also my OS because I didn't want to spend
\ money for CP/M.  These 100% hardware specific Forths had no names
\ because it was impossible that other people could ever use them
\ unchanged.

\ After the 68K I acquired 16-bit FysForth for the IBM AT. The origin of 
\ FysForth lies on the Apple-II (~1983, University of Utrecht), but it was 
\ released for MS-DOS in 1985. FysForth was derived from Forth-78 but had 
\ an enormous number of quite innovative extensions (TO concept ONERROR ..)
\ I didn't like Intel hardware at first, but in the end wrote a good 
\ optimizing Forth for it, F-4TH 1.45 (1987, see Vijgeblad 
\ #28 http://sikkepitje.no-ip.org/~csadmin/vijgebladarchief/28/thumb.html).
\ Although I was initially very impressed by FysForth, only its TO concept
\ has survived to the present day.

\ F-4TH 1.45 was used to build tForth (1992 pre-ANS, 1994 official ANS, 
\ http://home.iae.nl/users/mhx/t4artic.html). 

\ When the 32-bit iForth finally was brought up with tForth, its
\ optimizer went through numerous stages. Published experiments are a
\ 32-bit eforth with peephole optimizer (November 1995), and mxForth
\ (1997, no floating-point and no tasking, and therefore more free
\ registers for the optimizer to use).

\U http://tzxvault.retrogames.com/f.htm
\I for the Sinclair ZX-81
1982 s" Aackosoft FIG-Forth" name-implementation aackosoft_fig
figForth begot aackosoft_fig

1983 implementation FysForth
FysForth conforms-to Forth78

\ \U http://sikkepitje.no-ip.org/~csadmin/vijgebladarchief/28/thumb.html
\ \I Optimizing Forth for MS-DOS by Marcel Hendrix
\ 1987 s" F-4TH 1.45" name-implementation f_4th

\U http://home.iae.nl/users/mhx/t4artic.html
\I for the Transputer by the Dutch Forth Workshop
1992 implementation tForth
aackosoft_fig inspired tForth
FysForth inspired tForth
eForth inspired tForth

\U http://users.bart.nl/users/mhx/i4faq.html
\I Featureful optimizing Forth for IA-32, various OSs
1995 implementation iForth
iForth conforms-to Forth94
tForth begot iForth

\U http://home.vianetworks.nl/users/mhx/mxforth.html
\I Optimizing Forth for IA-32 by Marcel Hendrix
1997 implementation mxForth
iForth begot mxForth

\U http://www.jupiter-ace.co.uk
\I Home computer with Forth in ROM
1982 s" Jupiter ACE" name-implementation jupiter_ace
Forth79 inspired jupiter_ace

\U https://github.com/mitra42/webForth
\I Author: Mitra Ardron
\ webForth started out with the Eforth code fixed bugs then adapted
\ significantly to confirm to Forth2012
2020 implementation webForth
eForth begot webForth
webForth conforms-to Forth2012

.( }) cr


