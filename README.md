# false-net

## Code examples 

### Hello world

```
"Hello world"
```

In False language every character between double quotes marks (```"```) is printed without any stack side effects.

### Recursive increment until value is greater than 10

```
[1+$10>~[f;!]?]f:

2f;!. 
```

First open bracket ```[``` begins new function (lambda). What is happening inside?

1. ```1``` puts number (value = 1) on stack.
2. ```+``` takes from stack two values. We put only one value at point 1., right? Second one is provided before function call.
3. ```$``` duplicates top stack value.
4. ```>``` removes from stacks two top values (num2 and num1), then compares if num1 is greater than num2. As a result we get False boolean value (0 is false, -1 is true). 
5. ```~``` negates value on stack. Please note, that ```>~``` operation is equivalent to ```<=``` operation, but False language doesn't provide such useless things :)
6. Then we define nested anonymous function, which will be called if ```?``` operator finds true (-1) value on stack.

At last we close function definition with close bracket ```]```. After than we define variable ```f``` and assign to it our newly created function with ```:``` operator.

### Takes text from input and prints reversed version:

```
""Please enter text to reverse: "" 0[$10=~][^]#%
""Reversed text: "" [$0>][,]#
```

### Factorial function:

```
{ Code from False language documentation }

[$1=$[\%1\]?~[$1-f;!*]?]f:

6f;!.
```