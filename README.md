# false-net

False language interpreter, created with .NET stuff.

Usage:

```./FalseNet script.f```

## Cheat Sheet

| True value | False value |
|:----------:|:-----------:|
|     -1     |      0      |

| Syntax      | Pops (--> top) | Pushes (--> top) | Example                         |
|-------------|:--------------:|:----------------:|---------------------------------|
| `{comment}` |       -        |        -         | `{ this is a comment }`         |        
| `[code]`    |       -        |     function     | `[1+]` { (lambda (x) (+ x 1)) } |
| `a .. z`    |       -        |      varadr      | `abc` { use abc: or abc; }      |
| `1 .. 9`    |       -        |      value       | `123`                           |
| `'char`     |       -        |      value       | `'A` { 65 }                     | 
| `:`         |   n, varadr    |        -         | `1a:` { a:=1 }                  |
| `;`         |     varadr     |     varvalue     | `a;` { a }                      |
| `!`         |    function    |        -         | `f;!` { f() }                   |
| `+`         |     n1, n1     |      n1+n2       | `1 2+` { 1+2 }                  |
| `-`         |     n1, n2     |      n1-n2       | `1 2-`                          |
| `*`         |     n1, n2     |      n1\*n2      | `1 2\*`                         |
| `/`         |     n1, n2     |      n1/n2       | `1 2/`                          |
| `_`         |       n        |        -n        | `1_` { -1 }                     |
| `=`         |     n1, n1     |      n1=n2       | `1 2=~` { 1<>2 }                |
| `>`         |     n1, n2     |      n1>n2       | `1 2>`                          |
| `&`         |     n1, n2     |    n1 and n2     | `1 2&` { 1 and 2 }              |
| `\|`        |     n1, n2     |     n1 or n2     | `1 2 \|`                        |
| `~`         |       n        |      not n       | `0~` { -1,TRUE }                |
| `$`         |       n        |       n,n        | `1$` { dupl. top stack }        |
| `%`         |       n        |        -         | `1%` { del. top stack }         |
| `\`         |     n1, n2     |      n2,n1       | `1 2\` { swap }                 |
| `@`         |   n, n1, n2    |     n1,n2,n      | `1 2 3@` { rot }                |
| `ø`         |       n        |        v         | `1 2 1ø` { pick }               |
| `?`         |   bool, fun    |        -         | `a;2=[1f;!]?`                   |
| `#`         |   boolf, fun   |        -         | `1[$100<][1+]#`                 |
| `.`         |       n        |        -         | `1.` { printnum(1) }            | 
| `"string"`  |       -        |        -         | `"hi!"` { printstr("hi!") }     |
| `^`         |       -        |        ch        | `^` { getc() }                  |

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
"Please enter your name: " 1$m:[$10=~][^]#%0[m;ø$1>][m;2+m:]#
"Hello, " [$0>][,]# "!"
```

Version for decent False programmers - no variable use, just stack operations :yum:

```
"Please enter your name: " 0[$10=~][^]#%0
"Hello, " 2[$ø$1>][\2+]#%%[$0>][,]# "!"
```

### Factorial function:

```
{ Code from False language specification }

[$1=$[\%1\]?~[$1-f;!*]?]f:

6f;!.
```