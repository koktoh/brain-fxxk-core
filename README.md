# Brain Fxxk Core

Brain Fxxk Interpreter of .Net Core class library.

You can configurate ALL COMMAND TOKENS by JSON.

# Usage

You can just run after implement concrete `Executer` class.

Like this:

```csharp

var code = 
@"# Output ""Hello World!""
+++++++++[>++++++++>+++++++++++>+++<<<-]>. # H
>++.                                       # e
+++++++.                                   # l
.                                          # l
+++.                                       # o
>+++++.                                    #  
<<<+++++[>+++<-]>.                         # W
>.                                         # o
+++.                                       # r
------.                                    # l
--------.                                  # d
>+.                                        # !"

// Use default config
var config = new BFCommandConfig();
// Instantiate some concrete Executer class
var executer = new Executer(config);

executer.execute(code);

```


## ExecuterBase

This is **abstract** class. You should implemnt some methods.

1. `void Read()`
1. `void Write()`

## Configuration

### Common configuration

This configurate memory size. Default is `2048`.
You can use default config just instantiate `CommonConfig` class.

You can set value by JSON file.
(Also instatiating `CommonConfig` with properties.)

Like this:

```json
{
  "MemorySize": 2048
}
```

Save this JSON file somewhere you like. And, import this file using `ConfigManager.Import<CommonConfig>(path)` or `ConfigManager.Import<CommonConfig>(FileInfo)` or `ConfigManager.Import<CommonConfig>(Stream)`.
Also you can save configs in JSON format using `ConfigManager.Save<T>(source, path)` and so on.


### Command configuration

You can configurate command tokens by JSON file.

Default command tokens are here. 

|Token|Description|
|:-:|---|
|+|Increment the byte at the pointer.|
|-|Decrement the byte at the pointer.|
|>|Increment the pointer.|
|<|Decrement the pointer.|
|[|Jump forward past the matching `]` if the byte at the pointer is zero.|
|]|Jump backward to the matching `[` unless the byte at the pointer is zero.|
|#|Begin comment. Ignore following sentences until `;` or new line.|
|;|End comment.|

`BFCommandConfig` class is implemented these default tokens. You can use these tokens just instantiate `BFCommandConfig` class.

If you want to use custom command tokens, you CAN by JSON.
(Also instatiating `BFCommandConfig` with properties.)

Like this:

```json
{
  "Increment": {
    "Command": "inc",
    "CommandType": "Increment"
  },
  "Decrement": {
    "Command": "dec",
    "CommandType": "Decrement"
  },
  "MoveRight": {
    "Command": "moveR",
    "CommandType": "MoveRight"
  },
  "MoveLeft": {
    "Command": "moveL",
    "CommandType": "MoveLeft"
  },
  "RoopHead": {
    "Command": "roopH",
    "CommandType": "RoopHead"
  },
  "RoopTale": {
    "Command": "roopT",
    "CommandType": "RoopTale"
  },
  "Read": {
    "Command": "read",
    "CommandType": "Read"
  },
  "Write": {
    "Command": "write",
    "CommandType": "Write"
  },
  "BeginComment": {
    "Command": "beginC",
    "CommandType": "BeginComment"
  },
  "EndComment": {
    "Command": "endC",
    "CommandType": "EndComment"
  }
}
```

Save this JSON file somewhere you like. And, import this file using `ConfigManager.Import<BFCommandConfig>(path)` or `ConfigManager.Import<BFCommandConfig>(FileInfo)` or `ConfigManager.Import<BFCommandConfig>(Stream)`.
Also you can save configs in JSON format using `ConfigManager.Save<T>(source, path)` and so on.
