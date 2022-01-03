#call with LANG=en_US.ISO8859-1 make
SOURCE=tree.fs Makefile postings HEADER.html .htaccess CVS .cvsignore

all: tree.eps tree.pdf timeline.eps timeline-paged.pdf timeline.gif tree.gif tree.map README.html family-tree.zip

clean:
	rm *~ *.dot *.neato *.eps *.pdf *.gif *.map *.svg timeline.fs README.html

tree.svg: tree.dot
	dot -Tsvg tree.dot >tree.svg

tree.eps: tree.dot
	dot -Grotate=90 -Tps tree.dot >tree.eps

timeline-paged.eps: timeline.neato
	neato -s -n -Grotate=90 -Gsplines=true -Tps timeline.neato >timeline-paged.eps

timeline.eps: timeline.neato
	grep -v '^page=".*"$$' timeline.neato |neato -s -n -Grotate=90 -Gsplines=true -Tps >timeline.eps

timeline.gif: timeline.neato
	grep -v '^page=".*"$$' timeline.neato |neato -s -n -Gsplines=true -Tgif >$@

timeline.map: timeline.neato
	grep -v '^page=".*"$$' timeline.neato |sed 's/digraph tree/digraph timeline/'|neato -s -n -Gsplines=true -Tcmapx >$@


tree.dot: tree.fs
	gforth -e true tree.fs -e "drop bye" >tree.dot

timeline.fs: tree.dot
	dot tree.dot|awk '/pos="[0-9]+,[0-9]+"/ {sub(/[[].*pos="[0-9]+,/,"",$$0); sub(/".*$$/,"",$$0); print $$2" dup constant "$$1" max"}' >timeline.fs

timeline.neato: tree.fs timeline.fs
	gforth -e false tree.fs -e "drop bye" >timeline.neato

%.pdf: %.eps
	echo "0.65 0.65 scale"|cat - $<|ps2pdf - $@

# %.gif: %.eps
# 	echo "0.5 0.5 scale"|cat - $<|gs -r110 -dTextAlphaBits=4 -dGraphicsAlphaBits=4 -sPAPERSIZE=a0 -sDEVICE=ppmraw -sOutputFile=- -sNOPAUSE -q - -c showpage -c quit | pnmcrop| pnmflip -r270|pnmmargin -white 10 | ppmtogif >$@

#server-side image maps
%.map: %.dot
	dot -Timap $< >$@

#client-side image maps
%.map: %.dot
	grep -v '^page=".*"$$' $<|dot -Tcmapx >$@

%.gif: %.dot
	grep -v '^page=".*"$$' $<|dot -Tgif >$@

README.html: tree.map timeline.map
	echo "<a href="/anton/">Anton Ertl</a>"|cat tree.map timeline.map - >$@

family-tree.zip: $(SOURCE)
	rm -rf family-tree $@
	mkdir family-tree
	cp -rp $(SOURCE) family-tree
	zip -r9 $@ family-tree
	rm -rf family-tree
