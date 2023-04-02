# false-net

False language interpreter, created with .NET stuff. 

Usage:

```./FalseNet script.f```

## Code examples 

### Hello world

```
"Hello world"
```

### Recursive incrementation until value is not greater than 10

```
[1+$10>~[f;!]?]f:

2f;!. 
```

### Takes text from input and prints reversed version

```
"Please enter text to reverse: " 0[$10=~][^]#%
"Reversed text: " [$0>][,]#
```

### Says hello

```
"Please enter your name: " 1$m:[$10=~][^]#%00[m;ø$1>][m;2+m:]#

"Hello, " [$0>][,]# "!"
```

### Factorial function:

```
{ Code from False language specification }

[$1=$[\%1\]?~[$1-f;!*]?]f:

6f;!.
```