# false-net

## Code examples 

Hello world:

```
"Hello world"
```

Call recursive function 'f' until value on stack is greater than 10, then print result:

```
[1+$10>~[f;!]?]f:

2f;!. 
```

Factorial function:

```
{ Code from False language documentation }

[$1=$[\%1\]?~[$1-f;!*]?]f:

6f;!.
```