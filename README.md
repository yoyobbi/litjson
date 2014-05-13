LitJSON
=======

A *.Net* library to handle conversions from and to JSON (JavaScript Object
Notation) strings. This fork has been edited specificity for use with [Unity3D](http://unity3d.com/).

Originally created by [lbv](http://lbv.github.io/litjson/).

## Installation

Simply pull the repo and open the .unitypackage to import the library.

## Licence

This is free and unencumbered software released into the public domain.

For more information, please refer to http://unlicense.org/.

## Examples

For general examples, check out [/Docs/Quickstart/guide.md](https://github.com/VictorySquare/UnityLitJson/blob/master/Docs/quickstart/guide.md) 
in this repo, there is also a simple example included in the Unity package.

## Compiling

I've left the LitJson csproj intact so you can compile a dll and run the 
tests. When your ready to use the library in your project I recommend you import 
the uncompiled source as there are some #if directives in UnityPlatform.cs 
that help make the library work on WinRT (and possibly other platforms in the future).

## Tests

This library comes with a set of unit tests using the [NUnit](http://www.nunit.org/) framework.

## TODO

1. Remove the static writer and just create a new private one each time instead (eliminates need for global lock).
2. Add more error checks for type hinting.
3. Add full XMLDocs to public interfaces.
4. Try to get custom importers/exporters working recursively.
6. README.md needs more information and examples.
7. Fix unit tests that cause mono to crash (something with Enums I think).
