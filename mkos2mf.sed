s% /bin/sh% bash%g
s% cp% copy%g
s% rm% del%g
s% ./gforth % gforth %g
s%@srcdir@%.%g
s%\"$(FORTHPATH)\"%\".\"%g
s%@CFLAGS@%%g
s%@CPPFLAGS@%%g
s%@CXXFLAGS@%%g
s%@DEFS@%-DHAVE_CONFIG_H%g
s%@LDFLAGS@%%g
s%@LIBS@%-lm %g
s%@exec_prefix@%${prefix}%g
s%@prefix@%/usr/local%g
s%@program_transform_name@%s,x,x,%g
s%@bindir@%${exec_prefix}/bin%g
s%@sbindir@%${exec_prefix}/sbin%g
s%@libexecdir@%${exec_prefix}/libexec%g
s%@datadir@%${prefix}/share%g
s%@sysconfdir@%${prefix}/etc%g
s%@sharedstatedir@%${prefix}/com%g
s%@localstatedir@%${prefix}/var%g
s%@libdir@%${exec_prefix}/lib%g
s%@includedir@%${prefix}/include%g
s%@oldincludedir@%/usr/include%g
s%@infodir@%${prefix}/info%g
s%@mandir@%${prefix}/man%g
s%@CC@%gcc%g
s%@GCCLDFLAGS@%%g
s%@DEBUGFLAG@%%g
s%@host@%%g
s%@host_alias@%i486-os2%g
s%@host_cpu@%i486%g
s%@host_vendor@%%g
s%@host_os@%os2%g
s%@ENGINE_FLAGS@%%g
s%gforth:%gforth.exe:%g
s%-$(CP) gforth gforth~%-$(CP) gforth.exe gforth.exe~%g
s%@MAKE_EXE@%\
gforth:		gforth.exe%g
s%@PATHSEP@%;%g
s%@LINK_KERNL16L@%%g
s%@LINK_KERNL16B@%%g
s%@LINK_KERNL32L@%%g
s%@LINK_KERNL32B@%%g
s%@LINK_KERNL64L@%%g
s%@LINK_KERNL64B@%%g
s%@KERNEL@%kernl16l.fi kernl16b.fi kernl32l.fi kernl32b.fi kernl64l.fi kernl64b.fi%g
s%@LN_S@%ln -s%g
s%@INSTALL@%install-sh -c%g
s%@INSTALL_PROGRAM@%${INSTALL}%g
s%@INSTALL_DATA@%${INSTALL} -m 644%g
s%@LIBOBJS@% pow10.o strsignal.o ecvt.o atanh.o%g
s%@getopt_long@%getopt.o getopt1.o%g
s%@kernel_fi@%kernl32l.fi%g
s%@PATHSEP@%;%g
s%-fforce-mem -fforce-addr %%g
s%": version-string s\\" $(VERSION)\\" ;\"%: version-string s" $(VERSION)" ;%g
s%"char gforth_version\[\]=\\"$(VERSION)\\" ;"%char gforth_version\[\]="$(VERSION)" ;%g
s%$(srcdir)/config.h.in:	stamp-h.in%#$(srcdir)/config.h.in:	stamp-h.in%g
s%config.h:	stamp-h%#config.h:	stamp-h%g
s%$(FORTHPATH)$(PATHSEP)%%g
