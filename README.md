# Script Merger for The Witcher 3

I threw together this tool because I got tired of manually merging script files.

- Checks your Mods folder for mod conflicts.  Uses [QuickBMS](http://aluigi.altervista.org/quickbms.htm) to scan .bundle packages.
- Merges .ws scripts or .xml files inside bundle packages using the powerful open-source merge tool [KDiff3](http://kdiff3.sourceforge.net/).
- Packages new .bundle packages using the official mod tool [wcc_lite](http://www.nexusmods.com/witcher3/news/12625/?).
- Detects updated merge source files using the [xxHash](https://github.com/Cyan4973/xxHash) algorithm by Yann Collet, [implemented in .NET](https://github.com/wilhelmliao/xxHash.NET) by Wilhelm Liao.

**KDiff3 & other external binary dependencies aren't included in this source code.**
