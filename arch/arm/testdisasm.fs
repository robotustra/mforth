\ testdisasm.fs: ARM dis/assembler testcases

\ Copyright (C) 2009 Free Software Foundation, Inc.

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

\ Contributed by Andreas Bolka.

." \ --- automatically generated by testdisasm.fs ---" cr

also disassembler

\ --

: code:
    code ; immediate
: ;;
    end-code latestxt xt-see ; immediate

\ condition codes

$00000000 dis-CC drop
$10000000 dis-CC drop
$20000000 dis-CC drop
$30000000 dis-CC drop
$40000000 dis-CC drop
$50000000 dis-CC drop
$60000000 dis-CC drop
$70000000 dis-CC drop
$80000000 dis-CC drop
$90000000 dis-CC drop
$A0000000 dis-CC drop
$B0000000 dis-CC drop
$C0000000 dis-CC drop
$D0000000 dis-CC drop
$E0000000 dis-CC drop
$F0000000 dis-CC drop
cr

\ data processing opcodes

code: dp-imm/rot-and r1 r0 42 # and, ;; \ AND r1, r0, #42
code: dp-imm/rot-eor r1 r0 42 # eor, ;;
code: dp-imm/rot-sub r1 r0 42 # sub, ;;
code: dp-imm/rot-rsb r1 r0 42 # rsb, ;;
code: dp-imm/rot-add r1 r0 42 # add, ;;
code: dp-imm/rot-adc r1 r0 42 # adc, ;;
code: dp-imm/rot-sbc r1 r0 42 # sbc, ;;
code: dp-imm/rot-rsc r1 r0 42 # rsc, ;;
code: dp-imm/rot-orr r1 r0 42 # orr, ;;
code: dp-imm/rot-bic r1 r0 42 # bic, ;;

\ data processing immediate shift

code: dp-imm/sh-add0 r12 r11 r10 9 #lsl add, ;; \ ADD r12, r11, r10, LSL #9
code: dp-imm/sh-add1 r12 r11 r10 9 #lsr add, ;;
code: dp-imm/sh-add2 r12 r11 r10 9 #asr add, ;;
code: dp-imm/sh-add3 r12 r11 r10 9 #ror add, ;;

code: dp-imm/sh-add-lsl0 r12 r11 r10 add, ;;
cr \ @@ fix asm.fs for the following two tests
0 $E08CA02B disasm-inst cr \ code: dp-imm/sh-add-lsr0 r12 r11 r10 32 #lsr add, ;;
0 $E08CA04B disasm-inst cr \ code: dp-imm/sh-add-asr0 r12 r11 r10 32 #asr add, ;;
code: dp-imm/sh-add-ror0 r12 r11 r10 rrx add, ;; \ ADD r12, r11, r10, RRX

code: dp-imm/sh-mov0 r11 r10 mov, ;;
code: dp-imm/sh-mov1 r11 r10 9 #lsl mov, ;;
code: dp-imm/sh-mov2 r11 r10 9 #lsl movs, ;;
code: dp-imm/sh-mov3 r11 r10 9 #lsl ne movs, ;;

code: dp-imm/sh-cmp0 r11 r10 cmp, ;;
code: dp-imm/sh-cmp1 r11 r10 9 #lsl cmp, ;;
code: dp-imm/sh-cmp2 r11 r10 9 #lsl cmpp, ;;

\ data processing register shift

code: dp-reg/sh-sub0 r6 r5 r4 r3 lsl sub, ;; \ SUB r6, r5, r4, LSL r3
code: dp-reg/sh-sub1 r6 r5 r4 r3 lsr sub, ;;
code: dp-reg/sh-sub2 r6 r5 r4 r3 asr sub, ;;
code: dp-reg/sh-sub3 r6 r5 r4 r3 ror sub, ;;

code: dp-reg/sh-mvn0 r5 r4 r3 lsl mvn, ;; \ MVN r5, r4, LSL r3
code: dp-reg/sh-mvn1 r5 r4 r3 lsl mvns, ;;

code: dp-reg/sh-teq0 r6 r5 r4 lsl teq, ;;
code: dp-reg/sh-teq1 r6 r5 r4 lsl teqp, ;;

code: dp-imm/rot-add0 r1 r0 $3F0 # add, ;; \ ADD r1, r0, #0x03F0
code: dp-imm/rot-add1 r1 r0 $3F0 # adds, ;;

\ data processing immediate

code: dp-imm/rot-mov r0 42 # mov, ;; \ MOV r0, #42
code: dp-imm/rot-mvn r0 42 # mvn, ;;

code: dp-imm/rot-movs r0 42 # movs, ;;
code: dp-imm/rot-mvns r0 42 # mvns, ;;

code: dp-imm/rot-tst r1 42 # tst, ;;
code: dp-imm/rot-teq r1 42 # teq, ;;
code: dp-imm/rot-cmp r1 42 # cmp, ;;
code: dp-imm/rot-cmn r1 42 # cmn, ;;

code: dp-imm/rot-tstp r1 42 # tstp, ;;
code: dp-imm/rot-teqp r1 42 # teqp, ;;
code: dp-imm/rot-cmpp r1 42 # cmpp, ;;
code: dp-imm/rot-cmnp r1 42 # cmnp, ;;

\ load/store immediate offset

code: ls-imm-ldr0 r7 r8       ]  ldr, ;; \ LDR r7, [r8]
code: ls-imm-ldr1 r7 r8    0 #]  ldr, ;; \ LDR r7, [r8, #0]
code: ls-imm-ldr2 r7 r8   42 #]  ldr, ;;
code: ls-imm-ldr3 r7 r8  -42 #]  ldr, ;;
code: ls-imm-ldr4 r7 r8   42 #]! ldr, ;; \ LDR r7, [r8, #42]!
code: ls-imm-ldr5 r7 r8  -42 #]! ldr, ;;
code: ls-imm-ldr6 r7 r8   42 ]#  ldr, ;; \ LDR r7, [r8], #42
code: ls-imm-ldr7 r7 r8  -42 ]#  ldr, ;;
code: ls-imm-ldr8 r7 r8 $FFF ]#  ldr, ;;

code: ls-imm0 r7 r8 ] str, ;; \ STR r7, [r8]
code: ls-imm1 r7 r8 ] strb, ;;
\ @@ T forms depend on fix to asm.fs
code: ls-imm2 r7 r8 0 ]# ldrt, ;;   \ T forms require explicit
code: ls-imm3 r7 r8 0 ]# ldrbt, ;;  \ post-indexed addressing

\ load/store register offset

code: ls-reg-str0 r13 r15 r14 9 #lsl +] str, ;; \ STR r13, [r15, r14, LSL #9]
code: ls-reg-str1 r13 r15 r14 9 #lsl -] str, ;; \ STR r13, [r15, -r14, LSL #9]
code: ls-reg-str2 r13 r15 r14 9 #lsl +]! str, ;;
code: ls-reg-str3 r13 r15 r14 9 #lsl -]! str, ;;
code: ls-reg-str4 r13 r15 r14 9 #lsl ]+ str, ;;
code: ls-reg-str5 r13 r15 r14 9 #lsl ]- str, ;;

code: ls-reg0 r0 r3 r2 1 #lsl +] ldr, ;;
code: ls-reg1 r0 r3 r2 1 #lsl -] ldrb, ;;
code: ls-reg2 r0 r3 r2 1 #lsl ]+ ldrt, ;;
code: ls-reg3 r0 r3 r2 1 #lsl ]- ldrbt, ;;

\ load/store multiple

code: ldm0 r0 da  { r1 r2 r3 }  ldm, ;; \ LDMDA r0, {r1, r2, r3}
code: ldm1 r0 ia  { r1 r2 r3 }  ldm, ;;
code: ldm2 r0 db  { r1 r2 r3 }  ldm, ;;
code: ldm3 r0 ib  { r1 r2 r3 }  ldm, ;;
code: ldm4 r0 da!  { r1 r2 r3 } ldm, ;; \ LDMDA r0!, {r1, r2, r3}
code: ldm5 r0 ia!  { r1 r2 r3 } ldm, ;;
code: ldm6 r0 db!  { r1 r2 r3 } ldm, ;;
code: ldm7 r0 ib!  { r1 r2 r3 } ldm, ;;

code: stm0  r0 da  { r1 r2 r3 }  stm, ;;
code: ^ldm0 r0 da  { r2 r3 r4 } ^ldm, ;;
code: ^stm0 r0 da  { r2 r3 r4 } ^stm, ;;

code: stm-all
    r0 da  { r0 r1 r2 r3 r4 r5 r6 r7 r8 r9 r10 r11 r12 r13 r14 r15 } stm, ;;

\ software interrupt

code: swi0 $0 swi, ;;
code: swi1 $FFFFFF swi, ;;

\ multiply instructions

code: mul0 r3 r2 r1 mul, ;;
code: mul1 r3 r2 r1 muls, ;;

code: mla0 r4 r3 r2 r1 mla, ;;
code: mla1 r4 r3 r2 r1 mlas, ;;

code: smull0 r4 r3 r2 r1 smull, ;;
code: smull1 r4 r3 r2 r1 smulls, ;;
code: umull2 r4 r3 r2 r1 umull, ;;
code: umull3 r4 r3 r2 r1 umulls, ;;

code: smlal0 r4 r3 r2 r1 smlal, ;;
code: smlal1 r4 r3 r2 r1 smlals, ;;
code: umlal2 r4 r3 r2 r1 umlal, ;;
code: umlal3 r4 r3 r2 r1 umlals, ;;

\ branch, branch with link, branch with link and change to thumb

cr
$CAFE $EA000000 disasm-inst space \ +8 b
$CAFE $EAFFFFFF disasm-inst space \ +4 b
$CAFE $EAFFFFFE disasm-inst space \  0 b
$CAFE $EAFFFFFC disasm-inst space \ -8 b
cr

cr
$CAFE $0AFFFFFE disasm-inst space \  0 eq b
$CAFE $1AFFFFFE disasm-inst space \  0 ne b
$CAFE $BAFFFFFE disasm-inst space \  0 lt b
cr

cr
$CAFE $EB000000 disasm-inst space \ +8 bl
$CAFE $EBFFFFFE disasm-inst space \  0 bl
$CAFE $EBFFFFFC disasm-inst space \ -8 bl
cr

cr
$CAFE $FA000000 disasm-inst space \ +8 blx (h=0)
$CAFE $FAFFFFFE disasm-inst space \  0 blx (h=0)
$CAFE $FAFFFFFC disasm-inst space \ -8 blx (h=0)
cr

cr
$CAFE $FB000000 disasm-inst space \ +A blx (h=1)
$CAFE $FBFFFFFE disasm-inst space \  0 blx (h=1)
$CAFE $FBFFFFFC disasm-inst space \ -A blx (h=1)
cr

code: blx0  here $10 +  #blx, ;;   \ +8 blx (h=0)
code: blx1  here $8  +  #blx, ;;   \  0 blx (h=0)
code: blx2  here 0   +  #blx, ;;   \ -8 blx (h=0)

code: blx3  here $12 +  #blx, ;;   \ +A blx (h=1)
code: blx4  here $A  +  #blx, ;;   \  0 blx (h=1)
code: blx5  here 2   +  #blx, ;;   \ -A blx (h=1)

\ generic co-processor instructions

$0 $EC198700 disasm-inst cr \ LDC p7, c8, [r9]
$0 $EE2AC6E3 disasm-inst cr \ CDP p6, 2, c12, c10, c3, 7
$0 $EEE84512 disasm-inst cr \ MCR p5, 7, r4, c8, c2, 0

\ FPA extension

cr
$0 $ED949100 disasm-inst cr \ f1 r4 ]     ldfd,     \ LDFD f1, r4
$0 $ED948102 disasm-inst cr \ f0 r4 8 #]  ldfd,     \ LDFD f0, [r4, #8]
$0 $EDB48102 disasm-inst cr \ f0 r4 8 #]! ldfd,     \ LDFD f0, [r4, #8]!
$0 $EC948102 disasm-inst cr \ f0 r4 8 ]#  ldfd,     \ LDFD f0, [r4], #8

$0 $ED848102 disasm-inst cr \ f0 r4 8 #]  stfd,     \ STFD f0, [r4, #8]

$0 $EE010182 disasm-inst cr \ f0 f1 f2     adfd,    \ ADFD f0, f1, f2
$0 $EE01018D disasm-inst cr \ f0 f1 $5+0 # adfd,    \ ADFD f0, f1, #5.0
$0 $EE01018E disasm-inst cr \ f0 f1 $5-1 # adfd,    \ ADFD f0, f1, #0.5
$0 $EE01018F disasm-inst cr \ f0 f1 $A+0 # adfd,    \ ADFD f0, f1, #10.0

$0 $4E008181 disasm-inst cr \ f0 f1 mi mvfd,        \ MVFMID f0, f1

$0 $EE90F110 disasm-inst cr \ f0 f0 cmf,            \ CMF f0, f0
$0 $EEB0F118 disasm-inst cr \ f0 0.0 # cnf,         \ CNF f0, #0.0
$0 $EED4F118 disasm-inst cr \ f4 0.0 # cmfe,        \ CNFE f4, #0.0

$0 $ECBDC203 disasm-inst cr \ lfm not impl'd -> ldc \ LFM f4, 1, [r14], #12
$0 $ED948F02 disasm-inst cr \ no ldfd, but ldc      \ LDC p2, c12, [R13, #3]
$0 $EE000F81 disasm-inst cr \ no adfd, but cdp      \ CDP p15, 0, c0, c0, c1, 2

\ instructions supported by the assembler, but not the disassembler

code: misc0 r0 r1 ] ldrh, ;;
code: misc1 r0 r1 ] ldrsh, ;;
code: misc2 r0 r1 ] ldrsb, ;;
code: misc3 r0 r2 1 #] strh, ;;
code: misc4 r0 r2 r1 +] strh, ;;

code: misc5 r0 r1 r2 ] swp, ;; \ SWP r0, r1, [r2]
code: misc6 r0 r1 r2 ] swpb, ;;

code: misc7 r0 cpsr mrs, ;;
code: misc8 r0 spsr mrs, ;;

code: misc9   cpsr c x s f  r0 msr, ;;
code: misc10  spsr c x s f  $0ff # msr, ;;

\ --

previous

cr .s cr
