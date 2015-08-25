# Script Merger for The Witcher 3

I threw together this tool because I got tired of manually merging script files.  It checks your Mods folder for duplicate versions of .ws script files, then tries to merge them using [Google's text-merging library](https://code.google.com/p/google-diff-match-patch/).  If you try to merge mods that modify the same part of the same script, it'll prompt you to choose which change to keep.

**Doesn't merge any other mod files, such as .xml, .bundle, .store, or .cache.**
