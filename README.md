# Script Merger for The Witcher 3

I threw together this tool because I got tired of manually merging script files.  It checks your Mods folder for duplicate versions of .ws script files, then merges them using the powerful open-source merge tool [KDiff3](http://kdiff3.sourceforge.net/) by Joachim Eibl.  If you try to merge mods that modify the same part of the same script, KDiff3 will appear so you can resolve the conflict manually.

**Doesn't merge any other mod files, such as .xml, .bundle, .store, or .cache.**

**KDiff3 not included in source code.**
